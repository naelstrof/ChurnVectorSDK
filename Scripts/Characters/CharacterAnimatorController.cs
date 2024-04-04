using System;
using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using Naelstrof.Inflatable;
using DPG;
using JigglePhysics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CharacterAnimatorController : MonoBehaviour {
    private Animator animator;
    private bool hasClothes = true;
    private CharacterBase character;
    private Vector3 wishDirection;
    private Vector3 velocity;
    private Vector3 localBallsPosition;
    private Quaternion localBallsRootRotation;
    private Coroutine removeClothesRoutine;
    private Coroutine emotionRoutine;
    private bool grimaced = false;
    private static readonly string[] recognizedEmotions = {"Surprised", "Curious", "Grimace"};

    [SerializeField]
    private Inflatable boner;

    private float lastBonerTime;
    
    [SerializeField]
    private Inflatable cockVoreSizeChange;
    
    [FormerlySerializedAs("balls")] [SerializeField] private Transform ballsCenter;
    [FormerlySerializedAs("ballRoot")] [SerializeField] private Transform ballsRoot;
    [SerializeField] private float ballStorageScale = 1f;
    
    [FormerlySerializedAs("xrayOriginalRenderers")] [SerializeField] private List<Renderer> xrayBodyRenderers;
    private List<Renderer> xrayRenderers;

    [Header("Clothes settings")]
    [SerializeField] private AudioPack clothRipPack;
    [SerializeField] private VisualEffectAsset clothRipAsset;
    [SerializeField] private Texture2D clothTexture;
    [SerializeField] private Color clothColor;
    
    [FormerlySerializedAs("bodyRenderers")] [SerializeField] private List<SkinnedMeshRenderer> nakedBodyRenderers;
    [FormerlySerializedAs("clothesRenderers")] [SerializeField] private List<SkinnedMeshRenderer> clothedBodyRenderers;
    [SerializeField] private List<Transform> boobOomphEffect;
    [SerializeField] private InflatableCurve bounceCurve;
    [Header("Penetrable settings")]
    [SerializeField] private Inflatable cumInflation;
    [SerializeField] private List<string> dickTipOpenBlendshapes;
    private float totalGrabMovement;
    
    private List<int> dickTipIndicies = new();
    private List<Material> dickMaterials = new();
    private static RaycastHit[] hits = new RaycastHit[32];
    private static LayerMask groundMask;
    private VisualEffect clothRip;
    private float cumInflateAmount;
    private bool grabbed;
    private Vector3 lastPosition;
    private List<Tuple<JiggleRigBuilder, JiggleRigBuilder.JiggleRig>> ballJiggleRigs;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int Churning = Animator.StringToHash("Churning");
    private static readonly int BulgeOffset = Shader.PropertyToID("_BulgeOffset");
    private static readonly int BulgeAmount = Shader.PropertyToID("_BulgeAmount");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int GrabbedDirection = Animator.StringToHash("GrabbedDirection");
    private static readonly int Grabbed = Animator.StringToHash("Grabbed");
    private static readonly int Awareness = Animator.StringToHash("Awareness");
    private static readonly int Knowledge = Animator.StringToHash("Knowledge");
    private static readonly int Tased = Animator.StringToHash("Tased");
    private static readonly int SpherizeAmount = Shader.PropertyToID("_SpherizeAmount");
    private static readonly int SphereRadius = Shader.PropertyToID("_SphereRadius");
    private static readonly int SpherePosition = Shader.PropertyToID("_SpherePosition");
    private static readonly int BulgeProgress = Shader.PropertyToID("_BulgeProgress");
    private static readonly int VoringProgress = Animator.StringToHash("VoringProgress");
    private static readonly int VoreAnimation = Animator.StringToHash("VoreAnimation");
    private static readonly int CrouchAmount = Animator.StringToHash("CrouchAmount");
    private static readonly int Sprinting = Animator.StringToHash("Sprinting");
    private static readonly int XDirection = Animator.StringToHash("XDirection");
    private static readonly int ZDirection = Animator.StringToHash("ZDirection");
    private static readonly int BeingVored = Animator.StringToHash("BeingVored");
    private static readonly int TipRadius = Shader.PropertyToID("_TipRadius");
    private static readonly int Aiming = Animator.StringToHash("Aiming");

    public void SetClothes(bool hasClothes) {
        if (this.hasClothes == hasClothes) {
            return;
        }

        this.hasClothes = hasClothes;
        foreach (var cloth in clothedBodyRenderers) {
            cloth.gameObject.SetActive(hasClothes);
        }
        foreach (var body in nakedBodyRenderers) {
            body.gameObject.SetActive(!hasClothes);
        }

        if (!hasClothes) {
            StartCoroutine(ClothesRemoveRoutine());
        } else {
            StopAllCoroutines();
        }
    }
    
    private List<Tuple<JiggleRigBuilder, JiggleRigBuilder.JiggleRig>> FindJiggleRigsForBone(Transform target) {
        List<Tuple<JiggleRigBuilder,JiggleRigBuilder.JiggleRig>> rigs = new ();
        foreach (var jiggleRigBuilder in character.GetComponentsInChildren<JiggleRigBuilder>(true)) {
            foreach (var rig in jiggleRigBuilder.jiggleRigs) {
                if (target.IsChildOf(rig.GetRootTransform())) {
                    rigs.Add(new Tuple<JiggleRigBuilder, JiggleRigBuilder.JiggleRig>(jiggleRigBuilder, rig));
                }
            }
        }
        return rigs;
    }

    private void Awake() {
        character = GetComponentInParent<CharacterBase>();
        if (ballsCenter != null) {
            if (ballsRoot == null) {
                ballsRoot = ballsCenter.parent;
            }
            localBallsRootRotation = ballsRoot.localRotation;
            ballJiggleRigs = FindJiggleRigsForBone(ballsCenter);
            localBallsPosition = ballsCenter.localPosition;
        }

        dickMaterials = new List<Material>();
        if (character.voreMachine is CockVoreMachine cockVoreMachine) {
            List<Renderer> dickRenderers = new List<Renderer>();
            cockVoreMachine.GetPenetrator().GetOutputRenderers(dickRenderers);
            foreach (var dickRenderer in dickRenderers) {
                foreach (var dickMat in dickRenderer.materials) {
                    dickMaterials.Add(dickMat);
                }
            }
        }

        groundMask = LayerMask.GetMask("World");
        boner.OnEnable();
        boner.SetSizeInstant(0f);
        cockVoreSizeChange.OnEnable();
        cockVoreSizeChange.SetSizeInstant(0f);
        cumInflation.OnEnable();
        Pauser.pauseChanged += OnPauseChanged;
    }

    private void OnPauseChanged(bool paused) {
        enabled = !paused;
    }

    private void OnDestroy() {
        Pauser.pauseChanged -= OnPauseChanged;
    }

    private void OnEnable() {
        animator = GetComponent<Animator>();
        character = GetComponentInParent<CharacterBase>();
        character.grabbedOther += OnGrabOther;
        character.usedInteractable += OnUsedInteractable;
        character.movementChanged += OnMovementChanged;
        character.velocityChanged += OnVelocityChanged;
        character.tased += OnTased;

        character.grabChanged += OnGrabChanged;
        character.startCockVoreAsPrey += OnCockVoreStartAsPrey;
        character.updateCockVoreAsPrey += OnCockVoreUpdateAsPrey;
        character.endCockVoreAsPrey += OnCockVoreEndAsPrey;
        character.cancelCockVoreAsPrey += OnCockVoreEndAsPrey;
        if (character.voreContainer != null) {
            if (character.voreContainer is Balls balls) {
                balls.ballsChanged += OnBallSizeChanged;
            }
        }

        if (character.voreMachine != null) {
            character.voreMachine.voreStart += OnCockCockVoreStart;
            character.voreMachine.voreUpdate += OnCockCockVoreProgressChanged;
            character.voreMachine.voreEnd += OnCockCockVoreEnd;
        }

        if (character is Civilian civilian) {
            civilian.reloaded += OnReloadWeapon;
        }

        if (character is CharacterDetector detector && detector.knowledgeDatabase != null) {
            detector.knowledgeDatabase.knowledgeLevelChanged += OnKnowledgeLevelChanged;
        }

        XRayHandler.xrayChanged += OnXRayChanged;

        if (clothRip == null) {
            var clothObj = new GameObject("ClothRipEffect", typeof(VisualEffect));
            clothObj.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.Chest));
            clothObj.transform.localScale = Vector3.one;
            clothObj.transform.localPosition = Vector3.zero;

            clothRip = clothObj.GetComponent<VisualEffect>();
            if (clothRipAsset != null) {
                clothRip.visualEffectAsset = clothRipAsset;
                clothRip.SetVector4("ThreadColor", clothColor);
                clothRip.SetTexture("ClothingTexture", clothTexture);
            }

            clothRip.Stop();
        }

        SetUpXRay();
        StartCoroutine(BlinkRoutine());
        hasClothes = true;
        OnPauseChanged(Pauser.GetPaused());
    }

    private void OnGrabOther(IInteractable other) {
        if (other is CharacterBase) {
            boner.SetSize(1f, this);
            lastBonerTime = Time.time;
        }
    }

    private void OnStartChurn(CumStorage.CumSource churnable) {
        StartCoroutine(ChurningRoutine());
    }

    private IEnumerator ChurningRoutine() {
        animator.SetBool(Churning, true);
        float bulgeAmount = 0f;
        if (character.voreContainer != null) {
            while (!character.voreContainer.GetStorage().GetDoneChurning()) {
                float movementFreq = 0.25f;
                float bulgeFreq = 1f;
                float perlinX = Mathf.PerlinNoise(Time.time * movementFreq, 0f).Remap(0f, 1f, -0.25f, 0.25f);
                float perlinY = Mathf.PerlinNoise(0f, Time.time * movementFreq).Remap(0f, 1f, -0.25f, 0.25f);
                foreach (var material in dickMaterials) {
                    material.SetVector(BulgeOffset, new Vector4(perlinX, perlinY, 0f, 0f));
                    bulgeAmount = Mathf.Clamp01(Mathf.Sin(Time.time * bulgeFreq) * 3f);
                    material.SetFloat(BulgeAmount, bulgeAmount);
                }

                yield return null;
            }

            while (bulgeAmount != 0f) {
                bulgeAmount = Mathf.MoveTowards(bulgeAmount, 0f, Time.deltaTime);
                foreach (var material in dickMaterials) {
                    material.SetFloat(BulgeAmount, bulgeAmount);
                }

                yield return null;
            }
        }

        animator.SetBool(Churning, false);
    }

    private void OnLocksChanged(TicketLock.LockFlags flags) {
        if ((flags & TicketLock.LockFlags.Kinematic) != 0) {
            animator.SetFloat(Speed, 0f);
        }
    }

    private void OnXRayChanged(float xrayAmount) {
        foreach (var r in xrayRenderers) {
            r.enabled = xrayAmount != 0f;
            foreach (var mat in r.materials) {
                mat.SetColor(BaseColor, mat.GetColor(BaseColor).With(a: xrayAmount));
            }
        }
    }
    
    private void SetEmotion(string name, float duration) {
        if (emotionRoutine != null) {
            StopCoroutine(emotionRoutine);
            emotionRoutine = null;
        }
        if (!isActiveAndEnabled) {
            return;
        }
        emotionRoutine = StartCoroutine(EmotionRoutine(name, duration));
    }

    private List<SkinnedMeshRenderer> GetBodies() {
        List<SkinnedMeshRenderer> bodies = new List<SkinnedMeshRenderer>();
        foreach (var r in xrayBodyRenderers) {
            if (r is SkinnedMeshRenderer skinnedMeshRenderer && r.name.Contains("Body")) {
                bodies.Add(skinnedMeshRenderer);
            }
        }
        foreach (var r in xrayRenderers) {
            if (r is SkinnedMeshRenderer skinnedMeshRenderer && r.name.Contains("Body")) {
                bodies.Add(skinnedMeshRenderer);
            }
        }
        return bodies;
    }

    private IEnumerator EmotionRoutine(string name, float duration) {
        var bodies = GetBodies();
        if (bodies.Count == 0) {
            Debug.LogWarning("Tried to play emotion on character with no recognizable Body!");
            yield break;
        }

        List<int> validIndices = new List<int>();
        for (int i = 0; i < bodies[0].sharedMesh.blendShapeCount; i++) {
            foreach (var emotion in recognizedEmotions) {
                if (bodies[0].sharedMesh.GetBlendShapeName(i) == emotion) {
                    validIndices.Add(i);
                    break;
                }
            }
        }

        if (validIndices.Count == 0) {
            yield break;
        }

        List<float> initialState = new List<float>();
        List<float> targetState = new List<float>();
        foreach(var i in validIndices) {
            initialState.Add(bodies[0].GetBlendShapeWeight(i));
            if (bodies[0].sharedMesh.GetBlendShapeName(i) == name) {
                targetState.Add(100f);
            } else {
                targetState.Add(0f);
            }
        }

        float startTime = Time.time;
        float transitionDuration = 1f;
        while (Time.time - startTime < transitionDuration) {
            float t = (Time.time-startTime)/transitionDuration;
            for (int i = 0; i < validIndices.Count; i++) {
                foreach (var body in bodies) {
                    body.SetBlendShapeWeight(validIndices[i], Mathf.LerpUnclamped(initialState[i], targetState[i], bounceCurve.EvaluateCurve(t)));
                }
            }
            yield return null;
        }
        for (int i = 0; i < validIndices.Count; i++) {
            foreach (var body in bodies) {
                body.SetBlendShapeWeight(validIndices[i], targetState[i]);
            }
        }
        yield return new WaitForSeconds(duration);
        if (name != "None") {
            SetEmotion("None", 0f);
        }
    }

    private void SetUpXRay() {
        if (xrayRenderers != null) {
            return;
        }
        xrayRenderers = new List<Renderer>();

        List<Renderer> decalableAndXRayableRenderers = new List<Renderer>();
        
        foreach (var r in xrayBodyRenderers) {
            var copyr = Instantiate(r, r.transform.parent, true);
            List<Material> copyrMaterials = new List<Material>(copyr.sharedMaterials);
            for (int i = 0; i < copyrMaterials.Count; i++) {
                copyrMaterials[i] = XRayHandler.GetXRayMaterial(copyrMaterials[i].shader);
            }
            copyr.materials = copyrMaterials.ToArray();
            copyr.enabled = XRayHandler.GetXRayAmount() != 0f;
            copyr.gameObject.SetActive(true);
            foreach (var p in transform.parent.GetComponentsInChildren<PenetrableProcedural>()) {
                p.AddTargetRenderer(copyr);
            }

            xrayRenderers.Add(copyr);
        }

        if (character.voreMachine is CockVoreMachine cockVoreMachine) {
            List<Renderer> xrayDickRenderers = new List<Renderer>();
            cockVoreMachine.GetPenetrator().GetOutputRenderers(xrayDickRenderers);
            foreach (var r in xrayDickRenderers) {
                var copyr = Instantiate(r, r.transform.parent, true);
                List<Material> copyrMaterials = new List<Material>(copyr.sharedMaterials);
                for (int i = 0; i < copyrMaterials.Count; i++) {
                    copyrMaterials[i] = XRayHandler.GetXRayMaterial(copyrMaterials[i].shader);
                }

                copyr.materials = copyrMaterials.ToArray();
                dickMaterials.AddRange(copyr.materials);
                copyr.enabled = XRayHandler.GetXRayAmount() != 0f;
                copyr.gameObject.SetActive(true);
                cockVoreMachine.GetPenetrator().AddOutputRenderer(copyr);

                foreach (var listener in boner.listeners) {
                    if (listener is InflatableBlendShape blendshape) {
                        if (blendshape.ContainsTargetRenderer(r as SkinnedMeshRenderer)) {
                            blendshape.AddTargetRenderer(copyr as SkinnedMeshRenderer);
                        }
                    }
                }

                xrayRenderers.Add(copyr);
            }
            decalableAndXRayableRenderers.AddRange(xrayDickRenderers);
        }

        decalableAndXRayableRenderers.AddRange(xrayRenderers);
        decalableAndXRayableRenderers.AddRange(xrayBodyRenderers);
        
        foreach (var col in GetComponentsInChildren<Collider>()) {
            if (!col.TryGetComponent(out DecalableCollider decalableCollider)) {
                decalableCollider = col.gameObject.AddComponent<DecalableCollider>();
            }
            decalableCollider.SetDecalableRenderers(decalableAndXRayableRenderers.ToArray());
        }
    }

    private void Start() {
        if (character is CharacterDetector detector) {
            detector.knowledgeDatabase.knowledgeLevelChanged += OnKnowledgeLevelChanged;
        }

        if (character.voreContainer != null) {
            character.voreContainer.GetStorage().startChurn += OnStartChurn;
        }

        character.ticketLock.locksChanged += OnLocksChanged;
    }

    private void OnDisable() {
        character.grabbedOther -= OnGrabOther;
        character.usedInteractable -= OnUsedInteractable;
        character.movementChanged -= OnMovementChanged;
        character.velocityChanged -= OnVelocityChanged;
        if (character.voreContainer != null) {
            if (character.voreContainer is Balls balls) {
                balls.ballsChanged -= OnBallSizeChanged;
            }
            character.voreContainer.GetStorage().startChurn -= OnStartChurn;
        }
        character.grabChanged -= OnGrabChanged;
        character.startCockVoreAsPrey -= OnCockVoreStartAsPrey;
        character.updateCockVoreAsPrey -= OnCockVoreUpdateAsPrey;
        character.endCockVoreAsPrey -= OnCockVoreEndAsPrey;
        character.cancelCockVoreAsPrey -= OnCockVoreEndAsPrey;
        if (character is CharacterDetector detector) {
            detector.knowledgeDatabase.knowledgeLevelChanged -= OnKnowledgeLevelChanged;
        }

        if (character is Civilian civilian) {
            civilian.reloaded -= OnReloadWeapon;
        }

        if (character.voreMachine != null) {
            character.voreMachine.voreStart -= OnCockCockVoreStart;
            character.voreMachine.voreUpdate -= OnCockCockVoreProgressChanged;
            character.voreMachine.voreEnd -= OnCockCockVoreEnd;
        }

        if (ballsCenter != null) {
            if (ballsRoot != null) {
                ballsRoot.localRotation = localBallsRootRotation;
            }

            ballsCenter.localPosition = localBallsPosition;
        }

        if (clothRip != null) {
            Destroy(clothRip.gameObject);
        }

        XRayHandler.xrayChanged -= OnXRayChanged;
    }

    private void OnReloadWeapon() {
        animator.SetTrigger("Reload");
    }

    private void OnUsedInteractable(IInteractable interactable) {
        if (interactable is BreedingStand) {
            boner.SetSize(1f, this);
            lastBonerTime = Time.time;
        }
    }

    private void OnCockVoreUpdateAsPrey(CockVoreMachine.VoreStatus status) {
        if (status.progress > 0.5f && !grimaced) {
            SetEmotion("Grimace", 2f);
            grimaced = true;
        }

        foreach (var ren in nakedBodyRenderers) {
            foreach (var mat in ren.materials) {
                float maxRadiusFromDickBlendshapes = 0.3f;
                mat.SetFloat(TipRadius, Mathf.Min(status.dickTipRadius, maxRadiusFromDickBlendshapes)*0.5f);
            }
        }
    }

    private void OnCockVoreEndAsPrey(CharacterBase other) {
        animator.SetBool(BeingVored, false);
    }

    private void OnCockVoreStartAsPrey(CharacterBase other) {
        grimaced = false;
        animator.SetBool(BeingVored, true);
        SetEmotion("Surprised", 2f);
        if (removeClothesRoutine != null) {
            StopCoroutine(removeClothesRoutine);
            removeClothesRoutine = null;
        }
        if (removeClothesRoutine == null) {
            removeClothesRoutine = StartCoroutine(RemoveClothesAfterDelay());
        }
    }

    private void FixedUpdate() {
        if (character.ticketLock.GetLocked(TicketLock.LockFlags.Kinematic)) {
            return;
        }
        float speed = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z)/Time.deltaTime;
        animator.SetFloat(Speed, speed / 100f);
    }
    private void Update() {
        if (hasClothes && grabbed) {
            totalGrabMovement += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
            const float movementUntilClothesPopOff = 3f;
            if (totalGrabMovement > movementUntilClothesPopOff) {
                SetClothes(false);
            }
        }

        // Duration of a churn + duration of CV + 15 seconds
        if (Time.time - lastBonerTime > 48f) {
            boner.SetSize(0f, this);
            lastBonerTime = Time.time;
        }

        if (character.ticketLock.GetLocked()) {
            return;
        }
        Vector3 localAnimatorSpace = Quaternion.Inverse(transform.rotation) * Vector3.ProjectOnPlane(velocity, Vector3.up);
        animator.SetFloat(ZDirection, localAnimatorSpace.z);
        animator.SetFloat(XDirection, localAnimatorSpace.x);
        animator.SetBool(Sprinting, character.IsSprinting());
        animator.SetFloat(CrouchAmount, character.GetCrouchAmount());
        var civ = (Civilian)character;
        animator.SetBool(Aiming, (civ.GetAimingWeapon() || civ.GetTaseTarget() != null) && civ.IsCop());
    }

    private float aimLerp;
    private static readonly int BulgeRadius = Shader.PropertyToID("_BulgeRadius");

    private void OnAnimatorIK(int layerIndex) {
        var civ = (Civilian)character;
        bool shouldAim = civ.GetAimingWeapon() && civ.IsCop() && !civ.IsInteracting() && !civ.IsGrabbed();
        float targetAim = shouldAim ? 1f : 0f;
        aimLerp = Mathf.MoveTowards(aimLerp, targetAim, Time.deltaTime * 8f);
        if (aimLerp != 0f) {
            Vector3 aimPosition = character.transform.position + character.GetLookDirection() * 5f;
            animator.SetLookAtPosition(aimPosition);
            animator.SetLookAtWeight(aimLerp*0.75f, aimLerp*0.75f, aimLerp*0.25f, aimLerp*0.25f);
        }
    }

    private void OnMovementChanged(Vector3 wishDirection, Quaternion facingDirection) {
        if (character.IsBeingVored()) {
            return;
        }
        transform.rotation = facingDirection;
    }

    private void OnCockCockVoreStart(CockVoreMachine.VoreStatus status) {
        boner.SetSize(1f, this);
        lastBonerTime = Time.time;
        animator.SetInteger(VoreAnimation, Random.Range(0, 3));
        animator.SetBool("Voring", true);
        SetClothes(false);
        cockVoreSizeChange.SetSize(1f, this);
        
        if (status.dick != null) {
            List<Renderer> outputRenderers = new List<Renderer>();
            status.dick.GetOutputRenderers(outputRenderers);
            dickTipIndicies.Clear();
            if (outputRenderers.Count != 0 && outputRenderers[0] is SkinnedMeshRenderer dickRenderer) {
                foreach (var blendshapeName in dickTipOpenBlendshapes) {
                    var index = dickRenderer.sharedMesh.GetBlendShapeIndex(blendshapeName);
                    if (index == -1) {
                        Debug.LogWarning($"Couldn't find blendshape {blendshapeName} on mesh {dickRenderer}", dickRenderer.gameObject);
                        continue;
                    }
                    dickTipIndicies.Add(index);
                }
                dickTipIndicies.RemoveAll((a) => a == -1);
            }
        }
    }
    private void OnCockCockVoreEnd(CockVoreMachine.VoreStatus status) {
        status.dickTipRadius = 0f;
        if (status.dick != null) {
            SetDickTipOpenAmount(status.dick, 0f);
        }
        cockVoreSizeChange.SetSize(0f, this);
        animator.SetBool("Voring", false);
    }

    private void OnCockCockVoreProgressChanged(CockVoreMachine.VoreStatus status) {
        animator.SetFloat(VoringProgress, status.progress);
        float bulgeAdjust = GameManager.GetLibrary().bulgeCurve.EvaluateCurve(status.progress);
        status.dickTipRadius = GameManager.GetLibrary().tipOpenCurve.EvaluateCurve(status.progress);
        
        if (status.dick != null) {
            int iterations = 8;
            float length = status.dick.GetSquashStretchedWorldLength();
            float averageGirth = 0f;
            for (int i = 0; i < iterations; i++) {
                float t = (float)i / (iterations-1);
                averageGirth += status.dick.GetWorldGirthRadius(length * t);
            }
            averageGirth /= iterations;
            
            dickRenderers ??= new List<Renderer>();
            status.dick.GetOutputRenderers(dickRenderers);
            foreach (var dickRenderer in dickRenderers) {
                foreach (var mat in dickRenderer.materials) {
                    mat.SetVector(DickOffset, status.dick.GetRootTransform().TransformPoint(status.dick.GetRootPositionOffset()));
                    mat.SetVector(DickForward, status.dick.GetRootTransform().TransformDirection(status.dick.GetRootForward()).normalized);
                    float dickBulgeRadius = averageGirth*2f+0.5f;
                    mat.SetFloat(BulgeRadius, dickBulgeRadius);
                    float dickBulgeStart = status.dick.GetSquashStretchedWorldLength() + dickBulgeRadius;
                    float dickBulgeEnd = -dickBulgeRadius;
                    float dist = Mathf.Abs(bulgeAdjust - 0.5f)*2f;
                    mat.SetFloat(BulgeBlend, (1f-(dist*dist))*0.5f);
                    mat.SetFloat(BulgeProgress, Mathf.Lerp(dickBulgeEnd, dickBulgeStart, bulgeAdjust));
                }
            }
            SetDickTipOpenAmount(status.dick, status.dickTipRadius);
        }

    }

    private void OnVelocityChanged(Vector3 velocity) {
        this.velocity = velocity;
    }

    private void OnBallSizeChanged(bool active, float colliderSize, Vector3 position) {
        colliderSize *= ballStorageScale;
        if (ballsCenter == null) {
            return;
        }

        if (!active) {
            foreach (var pair in ballJiggleRigs) {
                if (pair.Item1.jiggleRigs.Contains(pair.Item2)) {
                    continue;
                }
                pair.Item1.jiggleRigs.Add(pair.Item2);
            }
            ballsCenter.localPosition = localBallsPosition;
            ballsRoot.localRotation = localBallsRootRotation;
            foreach (var mat in dickMaterials) {
                mat.DisableKeyword("_SPHERIZE_ON");
            }
            return;
        }
        foreach (var pair in ballJiggleRigs) {
            if (!pair.Item1.jiggleRigs.Contains(pair.Item2)) {
                continue;
            }
            pair.Item1.jiggleRigs.Remove(pair.Item2);
        }
        foreach (var mat in dickMaterials) {
            mat.EnableKeyword("_SPHERIZE_ON");
            mat.SetVector(SpherePosition, position);
            mat.SetFloat(SphereRadius, colliderSize+.1f);
            mat.SetFloat(SpherizeAmount, Mathf.Clamp01(colliderSize*2f));
        }

        ballsRoot.localRotation = localBallsRootRotation;
        ballsCenter.localPosition = localBallsPosition;
        
        Vector3 rootToLocalBalls = (ballsCenter.position - ballsRoot.position).normalized;
        
        Vector3 hipToBalls = position - ballsRoot.position;
        Vector3 hipToBallsDir = hipToBalls.normalized;
        
        Quaternion fromTo = Quaternion.FromToRotation(rootToLocalBalls, hipToBallsDir);
        
        ballsRoot.rotation = fromTo * ballsRoot.rotation;
        
        ballsCenter.position = position;
    }

    private void OnTased(CharacterBase by, bool tased) {
        animator.SetBool(Tased, tased);
        SetEmotion("Grimace", 10f);
        SetClothes(false);
    }

    private void OnKnowledgeLevelChanged(KnowledgeDatabase.KnowledgeLevel lastLevel, KnowledgeDatabase.Knowledge knowledge) {
        if (knowledge.target != CharacterBase.GetPlayer().gameObject) {
            return;
        }

        if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Investigative) {
            SetEmotion("Curious", 8f);
        } else if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Alert) {
            SetEmotion("Surprised", 3f);
        }

        animator.SetFloat(Awareness, knowledge.awareness);
        animator.SetInteger(Knowledge, (int)knowledge.GetKnowledgeLevel());
    }
    private IEnumerator RemoveClothesAfterDelay() {
        yield return new WaitForSeconds(0.5f);
        SetClothes(false);
    }

    private void OnGrabChanged(CharacterBase target, CharacterBase.DragDirection direction) {
        grabbed = target != null;
        totalGrabMovement = 0f;
        lastPosition = transform.position;
        SetEmotion("Surprised", 3f);
        animator.SetBool(Grabbed, grabbed);
        if (grabbed) {
            animator.SetInteger(GrabbedDirection, (int)direction);
        }
        boner.SetSize(1f, this);
        lastBonerTime = Time.time;
    }

    private void DoFootStep(AnimationEvent e) {
        if (e.animatorClipInfo.weight < 0.4f) {
            return;
        }
        if (!character.GetGrounded()) {
            return;
        }
        Transform foot = e.stringParameter == "left" ? animator.GetBoneTransform(HumanBodyBones.LeftFoot) : animator.GetBoneTransform(HumanBodyBones.RightFoot);
        int hitCount = Physics.RaycastNonAlloc(new Ray(foot.position+transform.up*0.5f, -transform.up), hits, 1f, groundMask);
        float min = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < hitCount; i++) {
            if (hits[i].distance < min) {
                min = hits[i].distance;
                minIndex = i;
            }
        }
        if (minIndex != -1) {
            if (GameManager.GetPhysicsMaterialExtensionDatabase().TryGetImpactInfo(
                    hits[minIndex].collider.sharedMaterial,
                    PhysicsMaterialExtension.PhysicMaterialInfoType.Soft |
                    PhysicsMaterialExtension.PhysicMaterialInfoType.Hard,
                    PhysicsMaterialExtension.PhysicsResponseType.Footstep,
                    out PhysicsMaterialExtension.ImpactInfo impactInfo)) {
                character.DoFootStep(impactInfo, hits[minIndex].point);
            }
        }
    }

    IEnumerator ClothesRemoveRoutine() {
        if (clothRipPack != null && Time.timeSinceLevelLoad > 1f) {
            AudioPack.PlayClipAtPoint(clothRipPack, transform.position);
        }

        if (clothRipAsset != null && clothRip != null) {
            clothRip.transform.SetParent(null, true);
            clothRip.transform.rotation = Quaternion.identity;
            clothRip.Play();
        }

        float startTime = Time.time;
        const float duration = 1.5f;
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            foreach (var boob in boobOomphEffect) {
                boob.localScale = Vector3.one * bounceCurve.EvaluateCurve(t);
            }
            yield return null;
        }
        foreach (var boob in boobOomphEffect) {
            boob.localScale = Vector3.one;
        }
    }

    private IEnumerator BlinkRoutine() {
        var bodies = GetBodies();
        while (bodies.Count != 0) {
            int blinkIndex = bodies[0].sharedMesh.GetBlendShapeIndex("Blink");
            if (blinkIndex == -1) {
                yield break;
            }

            float startTime = Time.time;
            float duration = 0.25f;
            while (Time.time - startTime < duration) {
                float t = (Time.time - startTime) / duration;
                foreach (var body in bodies) {
                    body.SetBlendShapeWeight(blinkIndex, Mathf.LerpUnclamped(0f, 100f, Easing.Elastic.Out(t)));
                }
                yield return null;
            }
            foreach (var body in bodies) {
                body.SetBlendShapeWeight(blinkIndex, 100f);
            }
            startTime = Time.time;
            duration = 0.25f;
            while (Time.time - startTime < duration) {
                float t = (Time.time - startTime) / duration;
                foreach (var body in bodies) {
                    body.SetBlendShapeWeight(blinkIndex, Mathf.LerpUnclamped(100f, 0f, Easing.Elastic.Out(t)));
                }

                yield return null;
            }
            foreach (var body in bodies) {
                body.SetBlendShapeWeight(blinkIndex, 0f);
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void PlayAudioPack(AudioPack pack) {
        AudioPack.PlayClipAtPoint(pack, transform.position);
    }

    public void PlayAudioPackByName(string name) {
        PlayAudioPack(AudioPackLibrary.GetAudioPackByName(name));
    }

    private List<Renderer> dickRenderers;
    private static readonly int DickOffset = Shader.PropertyToID("_DickOffset");
    private static readonly int DickForward = Shader.PropertyToID("_DickForward");
    private static readonly int BulgeBlend = Shader.PropertyToID("_BulgeBlend");

    private void SetDickTipOpenAmount(Penetrator penetrator, float amountInMeters) {
        dickRenderers ??= new List<Renderer>();
        penetrator.GetOutputRenderers(dickRenderers);
        foreach(var dickRenderer in dickRenderers) {
            if (dickRenderer is not SkinnedMeshRenderer dickSkinnedMeshRenderer) continue;
            float tipExpandAmountPerBlend = 0.2f;
            amountInMeters /= tipExpandAmountPerBlend;
            foreach (var dickTipIndex in dickTipIndicies) {
                float triggerAmount = Mathf.Max(Mathf.Min(amountInMeters, 1f), 0f);
                amountInMeters -= triggerAmount;
                dickSkinnedMeshRenderer.SetBlendShapeWeight(dickTipIndex, triggerAmount * 100f);
            }
        }
    }

    public void SetCumInflationAmount(float amount) {
        cumInflation.SetSize(Mathf.Sqrt(amount), this);
    }
    
}
