using System;
using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using Naelstrof.Inflatable;
using PenetrationTech;
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
    private Quaternion localBallsRotation;
    private Coroutine removeClothesRoutine;
    private Coroutine emotionRoutine;
    private bool grimaced = false;
    private static readonly string[] recognizedEmotions = {"Surprised", "Curious", "Grimace"};

    [SerializeField]
    private Inflatable boner;

    private float lastBonerTime;
    
    [SerializeField]
    private Inflatable cockVoreSizeChange;
    
    [SerializeField] private SkinnedMeshRenderer dick;
    [SerializeField] private Transform balls;
    [SerializeField] private float ballStorageScale = 1f;
    [SerializeField] private float dickBulgeStart = 1.5f;
    [SerializeField] private float dickBulgeEnd = -0.5f;
    
    [FormerlySerializedAs("xrayOriginalRenderers")] [SerializeField] private List<Renderer> xrayBodyRenderers;
    [SerializeField] private List<Renderer> xrayDickRenderers;
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
    [Header("Cock vore pred settings")]
    [SerializeField] private InflatableCurve bulgeCurve;
    [SerializeField] private InflatableCurve tipOpenCurve;
    [Header("Penetrable settings")]
    [SerializeField] private Inflatable cumInflation;
    [SerializeField] private List<string> dickTipOpenBlendshapes;
    private float totalGrabMovement;
    
    private List<int> dickTipIndicies = new List<int>();
    private static RaycastHit[] hits = new RaycastHit[32];
    private static LayerMask groundMask;
    private VisualEffect clothRip;
    private float cumInflateAmount;
    private bool grabbed;
    private Vector3 lastPosition;

    private List<Material> dickMaterials;
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

    private void Awake() {
        dickMaterials = new List<Material>();
        if (dick != null) {
            foreach (var mat in dick.materials) {
                dickMaterials.Add(mat);
            }
        }

        if (balls != null) {
            localBallsPosition = balls.localPosition;
            localBallsRotation = balls.localRotation;
        }
        groundMask = LayerMask.GetMask("World");
        if (dick != null) {
            foreach (var blendshapeName in dickTipOpenBlendshapes) {
                var index = dick.sharedMesh.GetBlendShapeIndex(blendshapeName);
                if (index == -1) {
                    Debug.LogWarning($"Couldn't find blendshape {blendshapeName} on mesh {dick}", dick.gameObject);
                    continue;
                }
                dickTipIndicies.Add(index);
            }
            dickTipIndicies.RemoveAll((a) => a == -1);
        }

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
        character.ballsChanged += OnBallSizeChanged;
        character.tased += OnTased;

        character.grabChanged += OnGrabChanged;
        character.startCockVoreAsPrey += OnCockVoreStartAsPrey;
        character.updateCockVoreAsPrey += OnCockVoreUpdateAsPrey;
        character.endCockVoreAsPrey += OnCockVoreEndAsPrey;
        character.cancelCockVoreAsPrey += OnCockVoreEndAsPrey;

        character.cockVoreMachine.cockVoreStart += OnCockVoreStart;
        character.cockVoreMachine.cockVoreUpdate += OnCockVoreProgressChanged;
        character.cockVoreMachine.cockVoreEnd += OnCockVoreEnd;
        
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
        while (!character.GetStorage().GetDoneChurning()) {
            float movementFreq = 0.25f;
            float bulgeFreq = 1f;
            float perlinX = Mathf.PerlinNoise(Time.time*movementFreq, 0f).Remap(0f,1f,-0.25f, 0.25f);
            float perlinY = Mathf.PerlinNoise(0f, Time.time*movementFreq).Remap(0f,1f,-0.25f,0.25f);
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
            foreach (var p in transform.parent.GetComponentsInChildren<ProceduralDeformation>()) {
                p.AddTargetRenderer(copyr);
            }

            xrayRenderers.Add(copyr);
        }
        
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
            foreach (var p in transform.parent.GetComponentsInChildren<Penetrator>(true)) {
                var renderers = p.GetTargetRenderers();
                renderers.Add(new RendererSubMeshMask() { mask = ~0, renderer=copyr });
            }
            foreach (var listener in boner.listeners) {
                if (listener is InflatableBlendShape blendshape) {
                    if (blendshape.ContainsTargetRenderer(r as SkinnedMeshRenderer)) {
                        blendshape.AddTargetRenderer(copyr as SkinnedMeshRenderer);
                    }
                }
            }
            xrayRenderers.Add(copyr);
        }

        decalableAndXRayableRenderers.AddRange(xrayRenderers);
        decalableAndXRayableRenderers.AddRange(xrayBodyRenderers);
        decalableAndXRayableRenderers.AddRange(xrayDickRenderers);
        
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
        character.GetStorage().startChurn += OnStartChurn;
        character.ticketLock.locksChanged += OnLocksChanged;
    }

    private void OnDisable() {
        character.grabbedOther -= OnGrabOther;
        character.usedInteractable -= OnUsedInteractable;
        character.movementChanged -= OnMovementChanged;
        character.velocityChanged -= OnVelocityChanged;
        character.ballsChanged -= OnBallSizeChanged;
        character.GetStorage().startChurn -= OnStartChurn;
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

        character.cockVoreMachine.cockVoreStart -= OnCockVoreStart;
        character.cockVoreMachine.cockVoreUpdate -= OnCockVoreProgressChanged;
        character.cockVoreMachine.cockVoreEnd -= OnCockVoreEnd;

        if (balls != null) {
            balls.localPosition = localBallsPosition;
            balls.localRotation = localBallsRotation;
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

    private void OnCockVoreUpdateAsPrey(VoreMachine.CockVoreStatus status) {
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
        Vector3 localAnimatorSpace = Quaternion.Inverse(transform.rotation) * Vector3.ProjectOnPlane(velocity, Vector3.up).normalized;
        animator.SetFloat(ZDirection, localAnimatorSpace.z);
        animator.SetFloat(XDirection, localAnimatorSpace.x);
        animator.SetBool(Sprinting, character.IsSprinting());
        animator.SetFloat(CrouchAmount, character.GetCrouchAmount());
        var civ = (Civilian)character;
        animator.SetBool(Aiming, civ.GetAimingWeapon() && civ.IsCop());
    }

    private void OnMovementChanged(Vector3 wishDirection, Quaternion facingDirection) {
        if (character.IsBeingVored()) {
            return;
        }
        transform.rotation = facingDirection;
    }

    private void OnCockVoreStart(VoreMachine.CockVoreStatus status) {
        boner.SetSize(1f, this);
        lastBonerTime = Time.time;
        animator.SetInteger(VoreAnimation, Random.Range(0, 3));
        animator.SetBool("Voring", true);
        cockVoreSizeChange.SetSize(1f, this);
    }
    private void OnCockVoreEnd(VoreMachine.CockVoreStatus status) {
        status.dickTipRadius = 0f;
        SetDickTipOpenAmount(0f);
        cockVoreSizeChange.SetSize(0f, this);
        animator.SetBool("Voring", false);
    }

    private void OnCockVoreProgressChanged(VoreMachine.CockVoreStatus status) {
        animator.SetFloat(VoringProgress, status.progress);
        float bulgeAdjust = bulgeCurve.EvaluateCurve(status.progress);
        foreach (var mat in dickMaterials) {
            mat.SetFloat(BulgeProgress, Mathf.Lerp(dickBulgeEnd, dickBulgeStart, bulgeAdjust));
        }
        status.dickTipRadius = tipOpenCurve.EvaluateCurve(status.progress);
        SetDickTipOpenAmount(status.dickTipRadius);
    }

    private void OnVelocityChanged(Vector3 velocity) {
        this.velocity = velocity;
    }

    private void OnBallSizeChanged(bool active, float colliderSize, Vector3 position) {
        colliderSize *= ballStorageScale;
        if (balls == null) {
            return;
        }

        if (!active) {
            balls.localPosition = localBallsPosition;
            balls.localRotation = localBallsRotation;
            foreach (var mat in dickMaterials) {
                mat.DisableKeyword("_SPHERIZE_ON");
            }
            return;
        }
        foreach (var mat in dickMaterials) {
            mat.EnableKeyword("_SPHERIZE_ON");
            mat.SetVector(SpherePosition, position);
            mat.SetFloat(SphereRadius, colliderSize+.1f);
            mat.SetFloat(SpherizeAmount, Mathf.Clamp01(colliderSize*2f));
        }
        Vector3 hipToBalls = position - balls.parent.position;
        Vector3 hipToBallsDir = hipToBalls.normalized;
        balls.position = position - hipToBallsDir * colliderSize;
        Vector3 regularForward = balls.parent.TransformDirection(localBallsRotation * Vector3.forward);
        Vector3 regularUp = balls.parent.TransformDirection(localBallsRotation * Vector3.up);
        //regularForward = Vector3.Lerp(regularForward, regularUp, Mathf.Clamp01(Vector3.Dot(regularForward, hipToBallsDir)));
        Quaternion fromTo = Quaternion.FromToRotation(regularUp, hipToBallsDir);
        balls.rotation = QuaternionExtensions.LookRotationUpPriority(fromTo*regularForward, hipToBallsDir);
        //balls.localRotation = localBallsRotation;
        //balls.up = position - balls.parent.position;
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
    private void SetDickTipOpenAmount(float amountInMeters) {
        float tipExpandAmountPerBlend = 0.2f;
        amountInMeters /= tipExpandAmountPerBlend;
        for (int i = 0; i < dickTipIndicies.Count; i++) {
            float triggerAmount = Mathf.Max(Mathf.Min(amountInMeters, 1f),0f);
            amountInMeters -= triggerAmount;
            dick.SetBlendShapeWeight(dickTipIndicies[i], triggerAmount * 100f);
        }
    }

    public void SetCumInflationAmount(float amount) {
        cumInflation.SetSize(Mathf.Sqrt(amount), this);
    }
}
