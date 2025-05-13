using System;
using System.Collections.Generic;
using System.Linq;
using DPG;
using JigglePhysics;
using Naelstrof.Inflatable;
using UnityEngine;
using UnityEngine.AI;
using Naelstrof.Easing;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public abstract partial class CharacterBase : MonoBehaviour, ITasable, IChurnable {
    protected Rigidbody body;
    protected static RaycastHitComparer raycastComparer = new RaycastHitComparer();
    private static Collider[] overlapSphereColliders = new Collider[1];
    private static RaycastHit[] raycastHits = new RaycastHit[32];
    protected static LayerMask groundMask;
    protected static LayerMask characterMask;
    public static LayerMask visibilityMask;
    protected LayerMask interactableMask;
    public static LayerMask solidWorldMask;
    protected static LayerMask obscurityLevelMask;
    protected static LayerMask limbMask;

    [Serializable]
    public enum PenetrableType {
        Hip,
        Face,
    }
    [Serializable]
    public struct PenetrableData {
        public Penetrable penetrable;
        public PenetrableType penetrableType;
    }

    [SerializeField] private PenetrableData[] penetrables;
    [SerializeField] private float volumeSolid = 8f;
    [SerializeField] private float volumeChurned = 1f;
    //private AudioPack landingPack;
    [SerializeField] private DialogueTheme dialogueTheme;

    protected InputGenerator inputGenerator;
    [SerializeField, SerializeReference, SubclassSelector]
    public VoreContainer voreContainer;
    public bool IsPlayer() => inputGenerator is InputGeneratorPlayerPossession;
    private new CapsuleCollider collider;
    private IInteractable grabbedInteractable;
    private Quaternion facingDirection;
    [SerializeField]
    private Sprite headSprite;
    public Quaternion GetFacingDirection() => facingDirection;
    public Vector3 GetLookDirection() => inputGenerator.GetLookDirection();
    public void SetFacingDirection(Quaternion facingDirection) => this.facingDirection = facingDirection;
    private Vector3 oldVelocity;
    private bool wasGrounded;
    public delegate void MovementChangeAction(Vector3 wishDirection, Vector3 acceleration, Quaternion facingDirection);

    public delegate void GrabbedOtherAction(IInteractable other);

    public event GrabbedOtherAction grabbedOther;
    public event MovementChangeAction movementChanged;
    public delegate void VelocityChangeAction(Vector3 velocity);
    public event VelocityChangeAction velocityChanged;
    private float lastLeftGround;
    public bool IsSprinting() => inputGenerator.GetSprint() && GetCrouchAmount() < 0.5f;
    protected bool grounded;

    private Dictionary<PenetrableType, Penetrable> backupPenetrables;

    public Penetrable GetPenetrable(PenetrableType type) {
        List<PenetrableData> validData = penetrables.Where(penetrableData => penetrableData.penetrableType == type).ToList();
        if (validData.Count != 0) {
            int randomSelection = UnityEngine.Random.Range(0, validData.Count);
            return validData[randomSelection].penetrable;
        }

        backupPenetrables ??= new Dictionary<PenetrableType, Penetrable>();
        if (backupPenetrables.TryGetValue(type, out var penetrable)) {
            return penetrable;
        }
        
        var backupPenetrable = gameObject.AddComponent<PenetrableBasic>();
        var animator = GetDisplayAnimator();
        var head = animator.GetBoneTransform(HumanBodyBones.Head);
        GameObject outsideMouth = new GameObject("Nose");
        outsideMouth.transform.SetParent(head);
        outsideMouth.transform.localPosition = noseOffset + head.InverseTransformVector(-animator.transform.up*0.1f);
        backupPenetrable.SetShouldTruncate(false);
        backupPenetrable.SetClippingRange(new PenetrableBasic.ClippingRange {
            allowAllTheWayThrough = true,
            startNormalizedDistance = 0.1f,
            endNormalizedDistance = 0.9f,
        });
        backupPenetrable.SetKnotForceSampleLocations(new[] { new PenetrableBasic.KnotForceSampleLocation { normalizedDistance = 0.05f } });
        switch (type) {
            case PenetrableType.Hip:
                backupPenetrable.SetTransforms(new [] {
                    animator.GetBoneTransform(HumanBodyBones.Hips),
                    animator.GetBoneTransform(HumanBodyBones.Chest),
                    animator.GetBoneTransform(HumanBodyBones.Neck),
                    animator.GetBoneTransform(HumanBodyBones.Head),
                    outsideMouth.transform,
                });
                backupPenetrables.Add(type, backupPenetrable);
                return backupPenetrable;
            case PenetrableType.Face:
                backupPenetrable.SetTransforms(new [] {
                    outsideMouth.transform,
                    animator.GetBoneTransform(HumanBodyBones.Head),
                    animator.GetBoneTransform(HumanBodyBones.Neck),
                    animator.GetBoneTransform(HumanBodyBones.Chest),
                    animator.GetBoneTransform(HumanBodyBones.Hips),
                });
                backupPenetrables.Add(type, backupPenetrable);
                return backupPenetrable;
            default:
                throw new UnityException($"Failed to create a penetrable for unknown type {type}");
        }
    }

    public void SetInputGenerator(InputGenerator newInputGenerator) {
        if (enabled) {
            OnDisable();
        }
        inputGenerator = newInputGenerator;
        OnEnable();
        if (!enabled) {
            OnDisable();
        }
    }

    private Animator displayAnimator;
    private float crouchAmount = 0f;
    private float previousCrouchAmount = 0f;
    private float originalColliderHeight;
    private Vector3 originalColliderOffset;
    //[SerializeField, SerializeReference, SerializeReferenceButton] protected InputGenerator inputGenerator;
    
    private PID floatingPID = new PID();
    private float hoverAmount = 1f;
    
    private List<CharacterGroup> groups; // Groups the character is in
    private List<CharacterGroup> usedByGroups; // Groups the character can be interacted from
    
    [SerializeField] protected float speed = 8f;
    private List<Transform> limbs;

    [SerializeField] private List<CharacterGroup> animationGroup;
    public IReadOnlyCollection<CharacterGroup> GetDisplayGroups() => animationGroup;

    public void SetGroups(ICollection<CharacterGroup> groups) {
        this.groups = new List<CharacterGroup>(groups);
    }
    
    public void SetUseByGroups(ICollection<CharacterGroup> groups) {
        usedByGroups = new List<CharacterGroup>(groups);
    }

    public DialogueTheme GetDialogueTheme() => dialogueTheme;

    public delegate void TaseAction(CharacterBase from, bool tased);
    public event TaseAction tased;

    private const float minimumCrouchHeight = 0.5f;
    private List<Modifier> speedModifiers = new();

    public Animator GetDisplayAnimator() {
#if UNITY_EDITOR
        if (displayAnimator != null) return displayAnimator;
        foreach (var animator in GetComponentsInChildren<Animator>(true)) {
            if (!animator.isHuman) continue;
            displayAnimator = animator;
            displayAnimator.applyRootMotion = false;
            break;
        }
#endif
        return displayAnimator;
    }

    public ICollection<CharacterGroup> GetCharacterGroups() => groups.AsReadOnly();

    private float speedMultiplier {
        get {
            float multi = 1f;
            foreach (var modifier in speedModifiers) {
                multi *= modifier.GetMultiplier();
            }

            return multi;
            //float encumbrance = Mathf.Clamp01(balls.GetSize() / (balls.GetSize() + 2f));
            //return multi * (1f-(encumbrance*encumbrance));
        }
    }

    private int taseCount = 0;
    protected virtual int GetMaxTaseCount() => 1;
    public TicketLock ticketLock { private set; get; }
    protected class RaycastHitComparer : IComparer<RaycastHit> {
        public int Compare(RaycastHit x, RaycastHit y) {
            return x.distance.CompareTo(y.distance);
        }
    }

    public float GetCrouchAmount() => crouchAmount;

    public Transform GetLimb(HumanBodyBones target) {
        return displayAnimator.GetBoneTransform(target);
    }

    public Transform GetRandomLimb() {
        return limbs[Random.Range(0, limbs.Count)];
    }
    
    protected virtual void OnEnable() {
        inputGenerator?.Initialize(gameObject);
        OnEnableInteractor();
        OnEnableGrabbable();
        InteractableLibrary.AddInteractable(this);
        if (IsPlayer()) {
            OnEnablePlayer();
            playerInstance = this;
        }
    }

    protected virtual void OnDisable() {
        if (IsPlayer()) {
            OnDisablePlayer();
        }
        inputGenerator?.CleanUp();
        OnDisableInteractor();
        OnEnableGrabbable();
        InteractableLibrary.RemoveInteractable(this);
    }


    #if UNITY_EDITOR
    private static void CreateLayer(string name, int layerID) {
        int layer = LayerMask.NameToLayer(name);
        if (layer == layerID) return;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        SerializedProperty layerProp = layers.GetArrayElementAtIndex(layerID);
        layerProp.stringValue = name;
        tagManager.ApplyModifiedPropertiesWithoutUndo();
    }
    [InitializeOnLoadMethod]
    private static void EnsureLayersAreCorrect() {
        CreateLayer("Penetrables", 6);
        CreateLayer("Characters", 7);
        CreateLayer("World", 3);
        CreateLayer("HitBoxes", 9);
        CreateLayer("BlocksVision", 10);
        CreateLayer("Glass", 11);
        CreateLayer("WorldButIgnoredByCamera", 12);
    }
    #endif

    protected virtual void Awake() {
        usedByGroups = new List<CharacterGroup>();
        groups = new List<CharacterGroup>();
        collider = gameObject.AddComponent<CapsuleCollider>();
        collider.height = 1.25f;
        collider.center = Vector3.zero.With(y: (collider.height - 1f) * 0.5f);
        collider.radius = 0.25f;
        collider.sharedMaterial = GameManager.GetLibrary().spaceLube;
        
        body = gameObject.AddComponent<Rigidbody>();
        body.mass = 15f;
        body.drag = 0f;
        body.angularDrag = 0.05f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        body.automaticCenterOfMass = false;
        body.automaticInertiaTensor = false;
        body.centerOfMass = Vector3.zero;
        body.inertiaTensorRotation = Quaternion.identity;

        gameObject.AddComponent<PhysicsAudio>();

        
        characterMask = LayerMask.GetMask("Characters");
        visibilityMask = LayerMask.GetMask("World", "BlocksVision", "WorldButIgnoredByCamera");
        solidWorldMask = LayerMask.GetMask("World", "BlocksVision", "Glass", "WorldButIgnoredByCamera");
        interactableMask = ~LayerMask.GetMask("HitBoxes");
        obscurityLevelMask = LayerMask.GetMask("Glass");
        collider = GetComponent<CapsuleCollider>();
        foreach (var animator in GetComponentsInChildren<Animator>(true)) {
            if (!animator.isHuman) continue;
            displayAnimator = animator;
            displayAnimator.applyRootMotion = false;
            break;
        }

        limbs = new List<Transform>();
        for (int i = 0; i < (int)HumanBodyBones.LastBone; i++) {
            Transform targetBone = displayAnimator.GetBoneTransform((HumanBodyBones)i);
            if (targetBone == null) {
                continue;
            }
            limbs.Add(targetBone);
        }

        if (limbs.Count == 0) {
            Debug.LogError("Character " + gameObject.name + " is missing all limbs, this makes visibility checks fail.");
        }

        originalColliderHeight = collider.height;
        originalColliderOffset = collider.center;
        ticketLock = new TicketLock();
        ticketLock.locksChanged += OnLocksChanged;
        body = GetComponent<Rigidbody>();
        groundMask = LayerMask.GetMask("World", "WorldButIgnoredByCamera");
        colliderSorter = new ColliderSorter(transform, this);
        
        voreContainer ??= new GenericVoreContainer();
        voreContainer?.Initialize(this);

        if (voreMachine == null) {
            var genericVoreMachine = new GenericVoreMachine();
            List<Transform> genericSplinePath = new List<Transform>();
            var animator = GetDisplayAnimator();
            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            GameObject outsideMouth = new GameObject("Nose");
            outsideMouth.transform.SetParent(head);
            outsideMouth.transform.localPosition = noseOffset + head.InverseTransformVector(-animator.transform.up*0.1f);
            genericSplinePath.Add(outsideMouth.transform);
            genericSplinePath.Add(animator.GetBoneTransform(HumanBodyBones.Head));
            genericSplinePath.Add(animator.GetBoneTransform(HumanBodyBones.Neck));
            genericSplinePath.Add(animator.GetBoneTransform(HumanBodyBones.Hips));
            genericVoreMachine.SetTransformPath(genericSplinePath);
            voreMachine = genericVoreMachine;
        }
        AwakeCockVore();
    }

    protected virtual void Start() {
        Pauser.pauseChanged += OnPauseChangedBasic;
        OnPauseChangedBasic(Pauser.GetPaused());
    }

    protected virtual void OnDestroy() {
        Pauser.pauseChanged -= OnPauseChangedBasic;
    }

    private void OnPauseChangedBasic(bool paused) {
        if (enabled != !paused) {
            enabled = !paused;
        }
    }

    public virtual float GetMaxSpeed() {
        return (speed * speedMultiplier) * (IsSprinting() ? 2f : 1f);
    }

    public virtual void AddChurnable(IChurnable target) {
        voreContainer.AddChurnable(target);
    }
    public float GetBallVolume() => voreContainer?.GetStorage().GetVolume() ?? 0f;

    public void BeginEmission(CumStorage.ChurnedAction startEvent = null, CumStorage.EmitCumAction emitEvent = null, CumStorage.ChurnedAction endEvent = null) {
        voreContainer.GetStorage().BeginEmission(1.2f, this, startEvent, emitEvent, endEvent);
    }

    public virtual void DoFootStep(PhysicsMaterialExtension.ImpactInfo impact, Vector3 position) {
        if (impact == null || !grounded || IsGrabbed()) {
            return;
        }

        if (IsPlayer()) {
            float volume = IsSprinting() ? 1f : Mathf.Lerp(0.6f, 0.2f, GetCrouchAmount());
            CharacterDetector.PlayInvestigativeAudioPackAtPoint(this, impact.soundEffect, position, 8f * volume,
                volume);
        } else {
            AudioPack.PlayClipAtPoint(impact.soundEffect, position, Mathf.Lerp(1f, 0.4f, crouchAmount));
        }

        if (impact.visualEffects.Count != 0) {
            GameObject visualEffectGameObject = new GameObject("TemporaryVFX", typeof(VisualEffect));
            visualEffectGameObject.transform.SetPositionAndRotation(position,
                Quaternion.FromToRotation(Vector3.up, Vector3.up));
            VisualEffect visualEffect = visualEffectGameObject.GetComponent<VisualEffect>();
            visualEffect.visualEffectAsset = impact.visualEffects[Random.Range(0, impact.visualEffects.Count)];
            visualEffect.Play();
            Destroy(visualEffectGameObject, 3f);
        }
    }

    [Serializable]
    private class PID {
        [SerializeField]
        private float proportionalGain = 5f;
        [SerializeField]
        private float derivativeGain = 0.1f;
        [SerializeField]
        private float integralGain = 2f;
        [SerializeField]
        private float maxIntegrationRange = 0.1f;
        [SerializeField]
        private DerivativeMeasurement derivativeMeasurement = DerivativeMeasurement.Velocity;
        
        private float errorLast;
        private float valueLast;
        private bool derivativeInitialized = false;
        private float integrationStored;

        private enum DerivativeMeasurement {
            Velocity,
            ErrorRateOfChange // Experiences "derivative kick" when the target value teleports.
        }

        public void Reset() {
            derivativeInitialized = false;
        }
        
        
        public float GetCorrection(float currentValue, float targetValue) {
            float error = targetValue - currentValue;
            float P = proportionalGain * error;
            
            float errorRateOfChange = (error - errorLast) / Time.deltaTime;
            errorLast = error;
            
            float valueRateOfChange = (currentValue - valueLast) / Time.deltaTime;
            valueLast = currentValue;
            
            float D = derivativeGain * (derivativeMeasurement == DerivativeMeasurement.Velocity ? -valueRateOfChange : errorRateOfChange);
            
            if (!derivativeInitialized) {
                D = 0f;
                derivativeInitialized = true;
            }

            integrationStored = Mathf.Clamp(integrationStored + (error * Time.deltaTime), -maxIntegrationRange, maxIntegrationRange);
            
            float I = integralGain * integrationStored;

            return P + I + D;
        }
    }

    private IEnumerable<Vector2> GetUniformPointsInCircle(int numPoints) {
        numPoints = Mathf.Max(numPoints, 2);
        const float turnFraction = 1.61803398875f; // (1 + sqrt(5))/2
        for (int i = 0; i < numPoints; i++) {
            float dst = Mathf.Sqrt(i / (numPoints - 1f));
            float angle = 2f * Mathf.PI * turnFraction * i;
            float x = dst * Mathf.Cos(angle);
            float y = dst * Mathf.Sin(angle);
            yield return new Vector2(x,y);
        }
    }

    private bool IsGrounded(out float groundDistance, out Vector3 groundNormal, bool alreadyGrounded, int quality=8) {
        float footLength = alreadyGrounded ? 1.5f : 1f;
        groundDistance = footLength+1f;
        groundNormal = Vector3.up;
        bool found = false;
        foreach (Vector2 point in GetUniformPointsInCircle(quality)) {
            var characterTransform = transform;
            var radius = collider.radius;
            int rayHits = Physics.RaycastNonAlloc(characterTransform.position + Vector3.forward * (point.y * radius) + Vector3.right * (point.x * radius), -characterTransform.up, raycastHits, footLength+0.1f, groundMask);
            for (int i = 0; i < rayHits; i++) {
                if (raycastHits[i].normal.y > 0.7 && raycastHits[i].distance <= footLength+0.001f) {
                    if (raycastHits[i].distance < groundDistance) {
                        Debug.DrawLine(raycastHits[i].point + Vector3.up*raycastHits[i].distance, raycastHits[i].point, Color.green, Time.fixedDeltaTime);
                        groundNormal = raycastHits[i].normal;
                        groundDistance = raycastHits[i].distance;
                        found = true;
                    }
                } else {
                    Debug.DrawLine(raycastHits[i].point + Vector3.up*raycastHits[i].distance, raycastHits[i].point, Color.yellow, Time.fixedDeltaTime);
                }
            }
        }
        //Debug.DrawLine(transform.position - transform.up * (footLength * 0.5f), transform.position, Color.red, Time.fixedDeltaTime);
        return found;
    }

    protected virtual void Update() {
        if (!ticketLock.GetLocked(TicketLock.LockFlags.FacingDirectionLock)) {
            Quaternion desiredRotation = QuaternionExtensions.LookRotationUpPriority(inputGenerator.GetLookDirection(), transform.up);
            facingDirection = Quaternion.RotateTowards(facingDirection, desiredRotation, Time.deltaTime * (90f + Quaternion.Angle(facingDirection, desiredRotation) * 10f));
        }

        if (ticketLock.GetLocked(TicketLock.LockFlags.Kinematic)) {
            movementChanged?.Invoke(Vector3.zero, Vector3.zero, facingDirection);
        } else {
            movementChanged?.Invoke(inputGenerator.GetWishDirection(), accelerationThisFrame, facingDirection);
        }

        if (grabbedInteractable != null) {
            Vector3 diff = body.position - grabbedInteractable.transform.position;
            Vector3 dir = diff.normalized;
            if (grabbedInteractable is CharacterBase grabbedCharacter) {
                if (grabbedCharacter.GetGrabDirection() == DragDirection.Back) dir *= -1;
                grabbedCharacter.SetFacingDirection(QuaternionExtensions.LookRotationUpPriority(dir, Vector3.up));
            }
        }

        UpdateGrabbable();
        UpdateCockVore();
        if (IsPlayer()) {
            UpdatePlayer();
        }
    }

    protected virtual void LateUpdate() {
        voreContainer?.LateUpdate();
        LateUpdateCockVore();
    }

    private void OnLocksChanged(TicketLock.LockFlags flags) {
        body.isKinematic = (flags & TicketLock.LockFlags.Kinematic) != 0;
        body.interpolation = (flags & TicketLock.LockFlags.Kinematic) != 0 ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;
        body.detectCollisions = (flags & TicketLock.LockFlags.IgnoreCollisions) == 0;
        if ((flags & TicketLock.LockFlags.Constraints) != 0) {
            body.constraints = RigidbodyConstraints.None;
        } else {
            body.rotation = QuaternionExtensions.LookRotationUpPriority(GetFacingDirection() * Vector3.forward, Vector3.up);
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    // Check if we've clipped our capsule into something.
    private bool Stuck() {
        var colliderTransform = collider.transform;
        Vector3 topPoint = colliderTransform.TransformPoint(collider.center + new Vector3(0, collider.height / 2f, 0));
        Vector3 botPoint = colliderTransform.TransformPoint(collider.center);
        int capsuleHits = Physics.OverlapCapsuleNonAlloc(topPoint, botPoint, collider.radius * colliderTransform.lossyScale.x, overlapSphereColliders, groundMask);
        for (int i = 0; i < capsuleHits; i++) {
            if (overlapSphereColliders[i].transform.root == transform.root) {
                continue;
            }

            return true;
        }
        return false;
    }

    private Vector3 accelerationThisFrame;
    protected virtual void FixedUpdate() {
        if(inputGenerator == null) { return; } // Early terminate as the character has not been initialised yet
        if (grabbedInteractable != null) {
            Vector3 diff = body.position - grabbedInteractable.transform.position;
            Vector3 dir = diff.normalized;
            float dist = diff.magnitude;
            var interactableBody = grabbedInteractable.transform.GetComponent<Rigidbody>();
            if (interactableBody != null && !interactableBody.isKinematic) {
                interactableBody.AddForce(dir * (Mathf.Max((dist - 1f) * 2f, 0f)), ForceMode.VelocityChange);
            }
        }


        if (!ticketLock.GetLocked()) { // Only mess with crouching when we're not locked in someway (being grabbed manipulates the collider).
            if (Vector3.Angle(body.transform.up, Vector3.up) > 0.1f) {
                body.MoveRotation(QuaternionExtensions.LookRotationUpPriority(body.transform.forward, Vector3.up));
            }
            if (inputGenerator.GetCrouchInput() > 0.5f && !inputGenerator.GetSprint()) {
                crouchAmount = Mathf.MoveTowards(crouchAmount, inputGenerator.GetCrouchInput(), Time.deltaTime * 3f);
                collider.height = Mathf.Lerp(originalColliderHeight, minimumCrouchHeight, crouchAmount);
                collider.center = Vector3.Lerp(originalColliderOffset, originalColliderOffset - Vector3.up * ((originalColliderHeight - minimumCrouchHeight) * 0.5f), crouchAmount);
            } else {
                previousCrouchAmount = crouchAmount;
                crouchAmount = Mathf.MoveTowards(crouchAmount, inputGenerator.GetCrouchInput(), Time.deltaTime * 3f);
                collider.height = Mathf.Lerp(originalColliderHeight, minimumCrouchHeight, crouchAmount);
                collider.center = Vector3.Lerp(originalColliderOffset, originalColliderOffset - Vector3.up * ((originalColliderHeight - minimumCrouchHeight) * 0.5f), crouchAmount);
                if (Stuck()) {
                    crouchAmount = previousCrouchAmount;
                    collider.height = Mathf.Lerp(originalColliderHeight, minimumCrouchHeight, crouchAmount);
                    collider.center = Vector3.Lerp(originalColliderOffset, originalColliderOffset - Vector3.up * ((originalColliderHeight - minimumCrouchHeight) * 0.5f), crouchAmount);
                }
            }            
        }

        if (ticketLock.GetLocked(TicketLock.LockFlags.Kinematic)) {
            velocityChanged?.Invoke(Vector3.zero);
            return;
        }
        // Rotate towards facingDirection
        Vector3 rotationNeeded = Vector3.Cross(body.rotation * Vector3.forward, GetFacingDirection() * Vector3.forward);
        body.angularVelocity = new Vector3(rotationNeeded.x, rotationNeeded.y, rotationNeeded.z)*20f;
        
        Vector3 velocity = body.velocity;
        accelerationThisFrame = velocity-oldVelocity;
        oldVelocity = velocity;
        grounded = IsGrounded(out float groundDistance, out Vector3 groundNormal, wasGrounded, GetRaytraceQuality()) && (Vector3.Dot(velocity, Physics.gravity.normalized) > 0f || wasGrounded);

        if (!wasGrounded && grounded)
        {
            var jumpStrength = Easing.Sinusoidal.InOut(Mathf.Clamp01(MathF.Abs(velocity.y / 10f)));
            var landingPack = GameManager.GetLibrary().landingPack;
            if (IsPlayer()) {
                CharacterDetector.PlayInvestigativeAudioPackAtPoint(this, landingPack, transform.position, 8f*jumpStrength, jumpStrength);
            } else {
                AudioPack.PlayClipAtPoint(landingPack, transform.position, jumpStrength);
            }
        }

        if (grounded && inputGenerator.GetJumpInput() && taseCount == 0) {
            // TODO: Probably replace this with an easier to execute jump (pressing jump while grounded, crouched, and not moving maybe gives you the bonus and uncrouches you?)
            if (previousCrouchAmount <= crouchAmount) {
                velocity.y = 2f + (1f - crouchAmount) * 4f;
            } else {
                velocity.y = 2f + (1f + crouchAmount) * 4f;
            }
            
            voreContainer?.OnJump(velocity.y);
            grounded = false;

        }

        if (grounded) {
            velocity = Friction(velocity, 9f);
        }

        if (taseCount == 0) {
            //what if wishdir itself was lerping/smooth? would that not solve things down the line?
            Vector3 wishDirection = Vector3.ProjectOnPlane(inputGenerator.GetWishDirection(), Vector3.up).normalized;
            velocity = Accelerate(velocity, wishDirection, GetMaxSpeed(), 10f, grounded, 1f);
        }

        if (!wasGrounded && grounded) {
            floatingPID.Reset();
        }
        wasGrounded = grounded;
        
        if (grounded) {

            float correction = Mathf.Clamp(floatingPID.GetCorrection(groundDistance, hoverAmount), -1f, 1f);
            velocity += correction * transform.up;
        }

        body.velocity = velocity;
        velocityChanged?.Invoke(body.velocity);
    }

    private Vector3 Friction(Vector3 velocity, float effectiveFriction) {
        float velocityMagnitude = velocity.magnitude;
        if ( velocityMagnitude < 0.1f ) {
            return
             Vector3.zero;
        }
        float stopSpeed = 1f;
        float control = velocityMagnitude < stopSpeed ? stopSpeed : velocityMagnitude;
        float drop = 0;
        drop += control * effectiveFriction * Time.fixedDeltaTime;
        float newspeed = velocityMagnitude - drop;
        if (newspeed < 0) {
            newspeed = 0;
        }
        newspeed /= velocityMagnitude;
        return velocity * newspeed;
    }
    
    private Vector3 Accelerate(Vector3 velocity, Vector3 wishdir, float wishspeed, float accel, bool grounded, float airCap) {
        float wishspd = wishspeed;
        if (!grounded) {
            wishspd = Mathf.Min(wishspd, airCap);
        }
        float currentspeed = Vector3.Dot(velocity, wishdir);

        float addspeed = wishspd - currentspeed;
        if (addspeed <= 0) {
            return velocity;
        }
        float accelspeed = accel * wishspeed * Time.deltaTime;

        accelspeed = Mathf.Min(accelspeed, addspeed);

        return velocity + accelspeed * wishdir;
    }

    public void Grab(IInteractable target) {
        grabbedInteractable = target;
        grabbedOther?.Invoke(target);
    }

    public IInteractable GetGrabbed() => grabbedInteractable;
    protected virtual void OnCharacterImpact(Collider by, ImpactAnalysis impactAnalysis) {
    }

    protected void OnCollisionEnter(Collision collision) {
        ImpactAnalysis analysis = new ImpactAnalysis(body, collision);
        OnCharacterImpact(collision.collider, analysis);
    }

    public virtual void GotSeen(KnowledgeDatabase.Knowledge knowledge, CharacterBase by) {
        seen?.Invoke(knowledge, by);
    }

    public virtual void AddSpeedModifier(Modifier speedMod) {
        speedModifiers.Add(speedMod);
    }

    public virtual void RemoveSpeedModifier(Modifier speedMod) {
        speedModifiers.Remove(speedMod);
    }
    
    public virtual void OnTaseStart(CharacterBase by) {
        taseCount++;
        if (taseCount >= GetMaxTaseCount()) {
            OnTaseCaptured();
        }
        if (activeInteractable != null) {
            StopInteractionWith(activeInteractable);
        }
        tased?.Invoke(by,true);
    }

    public virtual void OnTaseEnd(CharacterBase by) {
        tased?.Invoke(by,false);
        taseCount--;
    }

    public int GetTaseCount() => taseCount;

    protected virtual void OnTaseCaptured() {
        if (IsPlayer()) {
            LevelManager.TriggerGameOver();
        }
    }

    public Rigidbody GetBody() => body;
    public bool GetGrounded() => wasGrounded;
    public virtual float GetVolumeSolid() {
        return volumeSolid;
    }

    public virtual float GetVolumeChurned() {
        return volumeChurned;
    }

    public virtual float GetChurnDuration() {
        return 30f;
    }

    public void AddChurnVolumeAndSolidVolume(float churned, float solid) {
        volumeChurned += churned;
        volumeSolid += solid;
    }
    
    public Sprite GetHeadSprite() => headSprite;

    protected virtual int GetRaytraceQuality() {
        return (IsGrabbed() || IsPlayer()) ? 8 : 1;
    }
}
