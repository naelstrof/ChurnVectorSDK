using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public abstract class CharacterDetector : CharacterBase {
    private static List<CharacterDetector> characters = new();
    public static void AddTrackingGameObjectToAll(GameObject obj) {
        foreach (var character in characters) {
            character.AddTrackingGameObject(obj);
        }
    }

    public static void RemoveTrackingGameObjectFromAll(GameObject obj) {
        foreach (var character in characters) {
            character.RemoveTrackingGameObject(obj);
        }

        KnowledgeDatabase.ForcePoll();
    }

    public abstract Actor GetActor();
    private static RaycastHit[] raycastHits = new RaycastHit[16];
    private static Collider[] colliders = new Collider[64];
    private static NavMeshPath audioPath;
    private Transform headTransform;
    private Vector3 localEyeCenter;

    private const float spottedInSeconds = 1.4f;
    private const float maxSpottedInSeconds = 4f;
    private const float visionConeDegrees = 55f;
    private const float maxSightDistance = 18f;
    public const float awarenessBuffer = 0.3f;

    private bool ignorePlayer;

    private List<CharacterDetector> nearbyDetectors;
    private List<GameObject> trackedGameObjects;
    public KnowledgeDatabase knowledgeDatabase;

    private Coroutine cryRoutine;

    protected override void OnEnable() {
        base.OnEnable();
        knowledgeDatabase.knowledgeLevelChanged += OnKnowledgeDatabaseLevelChanged;
        knowledgeDatabase.OnEnable();
        if (trackedGameObjects.Count == 0 || GetPlayer() == null || !trackedGameObjects.Contains(GetPlayer().gameObject)) {
            StartCoroutine(StartRoutine());
        }
    }

    protected override void OnDisable() {
        base.OnDisable();
        knowledgeDatabase.knowledgeLevelChanged -= OnKnowledgeDatabaseLevelChanged;
        knowledgeDatabase.OnDisable();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        characters.Remove(this);
    }


    protected override void Awake() {
        base.Awake();
        knowledgeDatabase = new KnowledgeDatabase(this);
        characters.Add(this);
        headTransform = GetLimb(HumanBodyBones.Head);
        localEyeCenter = headTransform.InverseTransformPoint((GetDisplayAnimator().GetBoneTransform(HumanBodyBones.LeftEye).position + GetDisplayAnimator().GetBoneTransform(HumanBodyBones.RightEye).position) * 0.5f);
        audioPath = new NavMeshPath();
        trackedGameObjects = new();
        var detectorDisplayDisks = Instantiate(GameManager.GetLibrary().detectorDisplayDisks, transform, true);
        detectorDisplayDisks.transform.position = GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head).position + (Vector3.up * 0.5f);
    }

    private IEnumerator StartRoutine() {
        yield return new WaitUntil(() => GetPlayer() != null);
        if (trackedGameObjects.Contains(GetPlayer().gameObject)) {
            yield break;
        }
        trackedGameObjects.Add(GetPlayer().gameObject);
    }

    public bool CanSee(GameObject target) {
        Vector3 eyePosition = headTransform.TransformPoint(localEyeCenter);
        Transform targetTransform;
        if (target.TryGetComponent(out CharacterBase character)) {
            targetTransform = character.GetRandomLimb();
        } else {
            targetTransform = target.transform;
        }

        Vector3 lookPosition = targetTransform.transform.position;

        Vector3 diff = (lookPosition - eyePosition);
        Vector3 dir = diff.normalized;

        Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
        float angleFromFacingDirectionToPlayer = Vector3.Angle(GetFacingDirection() * Vector3.forward, dirToPlayer);
        if (angleFromFacingDirectionToPlayer > visionConeDegrees) {
            return false;
        }

        if (diff.magnitude > maxSightDistance) {
            Debug.DrawLine(eyePosition, eyePosition + dir * maxSightDistance, Color.red);
            return false;
        }
        int hits = Physics.RaycastNonAlloc(eyePosition, dir, raycastHits, diff.magnitude, visibilityMask);
        if (hits > 0) {
            Debug.DrawLine(eyePosition, raycastHits[0].point, Color.red);
            return false;
        }
        Debug.DrawLine(eyePosition, lookPosition, Color.green);
        return true;
    }

    public bool UnobscuredLineOfSight(GameObject target) {
        Vector3 eyePosition = headTransform.TransformPoint(localEyeCenter);
        Transform targetTransform;
        if (target.TryGetComponent(out CharacterBase character)) {
            targetTransform = character.GetRandomLimb();
        } else {
            targetTransform = target.transform;
        }

        Vector3 lookPosition = targetTransform.transform.position;

        Vector3 diff = (lookPosition - eyePosition);
        Vector3 dir = diff.normalized;

        float facingAmount = Vector3.Dot(GetFacingDirection() * Vector3.forward, dir);
        if (facingAmount < 0.1f) {
            return false;
        }

        if (diff.magnitude > maxSightDistance) {
            Debug.DrawLine(eyePosition, eyePosition + dir * maxSightDistance, Color.red);
            return false;
        }
        int hits = Physics.RaycastNonAlloc(eyePosition, dir, raycastHits, diff.magnitude, solidWorldMask);
        if (hits > 0) {
            Debug.DrawLine(eyePosition, raycastHits[0].point, Color.red);
            return false;
        }
        Debug.DrawLine(eyePosition, lookPosition, Color.green);
        return true;
    }


    public int GetLevelOfObscurity(GameObject target) {
        Vector3 eyePosition = headTransform.TransformPoint(localEyeCenter);
        Transform targetTransform;
        if (target.TryGetComponent(out CharacterBase character)) {
            targetTransform = character.GetRandomLimb();
        } else {
            targetTransform = target.transform;
        }

        Vector3 lookPosition = targetTransform.transform.position;

        Vector3 diff = (lookPosition - eyePosition);
        Vector3 dir = diff.normalized;
        int hits = Physics.RaycastNonAlloc(eyePosition, dir, raycastHits, diff.magnitude, obscurityLevelMask);
        return hits;
    }

    public float GetNoticability() {
        float visibility = 1.25f;
        if (IsVoring()) {
            visibility += 3f;
        }
        if ((IsSprinting() && GetBody().velocity.With(y:0f).magnitude > 0.05f) || GetBody().velocity.With(y:0f).magnitude > 4f) {
            visibility += 1f;
        }
        
        if (!grounded) {
            visibility += 1f;
        }

        visibility += Mathf.Sqrt(1f + GetBallVolume())-1f;
        visibility += (GetGrabbed() != null ? 2f : 0f);

        visibility *= Mathf.Lerp(1f, 0.75f, GetCrouchAmount());
        visibility *= (ticketLock.GetLocked(TicketLock.LockFlags.Kinematic) ? 0.5f : 1f);
        
        return Mathf.Min(visibility,GetMaxNoticability());
    }

    public float GetMaxNoticability() {
        return 4f;
    }

    private void AttemptDetect(GameObject target) {
        if (IsPlayer()) {
            return;
        }

        if (ignorePlayer && target.TryGetComponent(out CharacterBase p) && p.IsPlayer()) {
            return;
        }

        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);

        Vector3 dirToPlayer = (target.transform.position - transform.position).normalized;
        float angleFromFacingDirectionToPlayer = Vector3.Angle(GetFacingDirection() * Vector3.forward, dirToPlayer);

        if (distanceToPlayer > maxSightDistance || angleFromFacingDirectionToPlayer > visionConeDegrees) return; // early termination for base state

        if (!CanSee(target)) return;
        float distanceMultiplier = distanceToPlayer / maxSightDistance;
        float multiplier = Mathf.Lerp((1f-distanceMultiplier) * (1f-distanceMultiplier), 1f-distanceMultiplier, 0.25f);

        if (target.TryGetComponent(out CharacterDetector detector)) {
            multiplier *= detector.GetNoticability();
        } else { // Must be a condom, we detect those much faster.
            multiplier += 1f;
        }

        float facingAmount = (visionConeDegrees * 2f - angleFromFacingDirectionToPlayer) / visionConeDegrees;
        multiplier *= (Mathf.Clamp01(facingAmount) * Mathf.Clamp01(facingAmount));
        knowledgeDatabase.AddAwareness(target, Time.deltaTime * Mathf.Max(1f / maxSpottedInSeconds, 1f / spottedInSeconds * multiplier), KnowledgeDatabase.KnowledgeLevel.Alert, target.transform.position);
    }

    protected override void Update() {
        base.Update();
        if (grabbedBy != null) {
            knowledgeDatabase.AddAwareness(grabbedBy.gameObject, Time.deltaTime, KnowledgeDatabase.KnowledgeLevel.Alert, grabbedBy.transform.position);
        }
        knowledgeDatabase.Update();
        foreach (var character in trackedGameObjects) {
            AttemptDetect(character);
        }
        if (ticketLock.GetLocked()) {
            return;
        }
    }

    protected IEnumerator Cry() {
        yield return null;
        while (true) {
            var knowledge = knowledgeDatabase.GetKnowledge(GetPlayer().gameObject);
            if (knowledge.GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Alert || !knowledge.TryGetLastKnownPosition(out Vector3 position)) {
                break;
            }
            if (!ticketLock.GetLocked() && !IsGrabbed()) {
                PlayInvestigativeAudioPackAtPoint(GetPlayer(), GameManager.GetLibrary().yowl, transform.position, 15f);
            }
            yield return new WaitForSeconds(4f);
        }

        cryRoutine = null;
    }

    private static IEnumerator PlayAudibleNoiseOnSource(CharacterBase owner, AudioPack pack, AudioSource source, float unobstructedRadius, float volume = 1f) {
        while (source != null && source.isPlaying) {
            int hits = Physics.OverlapSphereNonAlloc(source.transform.position, unobstructedRadius, colliders, characterMask);
            if (!NavMesh.SamplePosition(source.transform.position, out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, 1)) {
                yield return new WaitForSeconds(1f);
                continue;
            }

            for (int i = 0; i < hits; i++) {
                Collider collider = colliders[i];
                if (!collider.TryGetComponent(out CharacterDetector detector)) continue;
                int walkableMask = 1;
                if (!NavMesh.CalculatePath(hit.position, detector.transform.position, walkableMask, audioPath)) {
                    continue;
                }
                if (audioPath.status != NavMeshPathStatus.PathComplete) {
                    continue;
                }

                float audioDistance = 0f;
                for (int j = 0; j < audioPath.corners.Length - 1; j++) {
                    audioDistance += Vector3.Distance(audioPath.corners[j], audioPath.corners[j + 1]);
                }

                if (audioDistance < unobstructedRadius) {
                    for (int j = 0; j < audioPath.corners.Length - 1; j++) {
                        Debug.DrawLine(audioPath.corners[j], audioPath.corners[j + 1], Color.cyan, 5f);
                    }
                    Vector3 dir = (source.transform.position - collider.transform.position).normalized;
                    if (audioPath.corners.Length >= 2) {
                        dir = (audioPath.corners[^2] - collider.transform.position).normalized;
                    }
                    detector.OnHearInvestigativeAudioPack(owner, pack, source.transform.position, dir, audioDistance / unobstructedRadius);
                }
            }

            float veryInterestingNoiseLevel = 0.25f;
            if (pack.GetInterestLevel() > veryInterestingNoiseLevel) {
                var visual = new GameObject("AudioVisualization", typeof(VisualEffect));
                visual.transform.position = source.transform.position;
                var vfx = visual.GetComponent<VisualEffect>();
                vfx.visualEffectAsset = GameManager.GetLibrary().audioVisualizer;
                vfx.SetFloat("InterestLevel", pack.GetInterestLevel().Remap(veryInterestingNoiseLevel, 1f, 0f, 1f) * volume);
                vfx.SetFloat("Radius", unobstructedRadius);
                Destroy(visual, 1.1f);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public static void PlayInvestigativeAudioPackOnSource(CharacterBase owner, AudioPack pack, AudioSource target, float unobstructedRadius, float volume) {
        pack.Play(target, volume);
        owner.StartCoroutine(PlayAudibleNoiseOnSource(owner, pack, target, unobstructedRadius, volume));
    }

    public static AudioSource PlayLoopingInvestigativeAudioPackOnTransform(CharacterBase owner, AudioPack pack, Transform target, float unobstructedRadius) {
        AudioSource source = target.gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1f;
        source.minDistance = 1f;
        source.maxDistance = 25f;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.loop = true;
        pack.Play(source);
        owner.StartCoroutine(PlayAudibleNoiseOnSource(owner, pack, source, unobstructedRadius));
        return source;
    }

    public static AudioSource PlayInvestigativeAudioPackAtPoint(CharacterBase owner, AudioPack pack, Vector3 position, float unobstructedRadius, float volume = 1f) {
        var source = AudioPack.PlayClipAtPoint(pack, position, volume);
        GameManager.StaticStartCoroutine(PlayAudibleNoiseOnSource(owner, pack, source, unobstructedRadius, volume));
        return source;
    }

    protected virtual void OnHearInvestigativeAudioPack(CharacterBase owner, AudioPack source, Vector3 position, Vector3 heardDirection, float normalizedAudioDistance) {
        if (IsPlayer() || owner == this) {
            return;
        }
        if (!ignorePlayer || !owner.IsPlayer()) {
            if (source.IsObviousPlayerNoise()) {
                knowledgeDatabase.AddAwareness(owner.gameObject, normalizedAudioDistance * source.GetInterestLevel(), KnowledgeDatabase.KnowledgeLevel.Investigative, position);
            } else {
                knowledgeDatabase.AddAwareness(owner.gameObject, normalizedAudioDistance * source.GetInterestLevel(), KnowledgeDatabase.KnowledgeLevel.Investigative);
            }
        }

        GetActor()?.RaiseEvent(new HeardInterestingNoise(owner, position, heardDirection, source, normalizedAudioDistance));
    }

    public override void OnEndInteract(CharacterBase from) {
        base.OnEndInteract(from);
        GetActor()?.RaiseEvent(new DroppedByCharacter(grabbedBy));
    }

    public override void OnBeginInteract(CharacterBase from) {
        base.OnBeginInteract(from);
        GetActor()?.RaiseEvent(new GrabbedByCharacter(from));
        knowledgeDatabase.AddAwareness(from.gameObject, 100f, KnowledgeDatabase.KnowledgeLevel.Alert, from.transform.position);
    }

    protected override void OnCharacterImpact(Collider by, ImpactAnalysis impactAnalysis) {
        base.OnCharacterImpact(by, impactAnalysis);
        CharacterBase other = by.GetComponentInParent<CharacterBase>();
        if (other != null && impactAnalysis.GetImpactMagnitude() > 0.15f) {
            if (other.IsPlayer()) {
                knowledgeDatabase.AddAwareness(other.gameObject, 0.5f, KnowledgeDatabase.KnowledgeLevel.Alert, other.transform.position);
            }
            Vector3 dir = Vector3.Normalize(transform.position - other.transform.position);
            float angle = Vector3.SignedAngle(other.GetBody().velocity.normalized, dir, Vector3.up);
            const float arbitraryPushForceMultiplier = 4f;
            body.AddForce(Vector3.Cross(other.GetBody().velocity, Mathf.Sign(angle) * Vector3.down) * arbitraryPushForceMultiplier, ForceMode.Impulse);
            GetActor()?.RaiseEvent(new Shoved(other, impactAnalysis));
        }
    }

    protected virtual void OnKnowledgeDatabaseLevelChanged(KnowledgeDatabase.KnowledgeLevel lastLevel, KnowledgeDatabase.Knowledge knowledge) {
        if (IsPlayer()) {
            return;
        }
        if (cryRoutine == null && knowledge.target == GetPlayer().gameObject && knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Alert) {
            StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.CivExclaim).Begin(new List<DialogueCharacter> { DialogueCharacterSpecificCharacter.Get(this), new DialogueCharacterPlayer() }));
            cryRoutine = StartCoroutine(Cry());
        }

        if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Investigative && lastLevel == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.Investigate).Begin(new List<DialogueCharacter> { DialogueCharacterSpecificCharacter.Get(this), new DialogueCharacterPlayer() }));
        }

        GetActor()?.RaiseEvent(new KnowledgeChanged(knowledge));
    }

    public void AddTrackingGameObject(GameObject character) {
        if (trackedGameObjects.Contains(character)) {
            return;
        }
        trackedGameObjects.Add(character);
        if (CanSee(character)) {
            knowledgeDatabase.AddAwareness(character, 2f, KnowledgeDatabase.KnowledgeLevel.Investigative, character.transform.position);
        }
    }

    public void RemoveTrackingGameObject(GameObject character) {
        knowledgeDatabase.ForgetImmediately(character);
        trackedGameObjects.Remove(character);
    }

    public void ReceiveKnowledge(CharacterDetector from, GameObject about) {
        knowledgeDatabase.ReceiveKnowledge(about.gameObject, from.knowledgeDatabase.GetKnowledge(about.gameObject));
    }

    public int GetNearbyCharacters<T>(Vector3 point, float radius, List<T> characters) where T : CharacterBase {
        int hits = Physics.OverlapSphereNonAlloc(point, radius, colliders, characterMask);
        characters.Clear();
        for (int i = 0; i < hits; i++) {
            colliders[i].TryGetComponent(out T character);
            if (character != null && CanSee(character.gameObject)) {
                characters.Add(character);
            }
        }
        return hits;
    }

    public override void OnTaseStart(CharacterBase by) {
        base.OnTaseStart(by);
        GetActor()?.RaiseEvent(new GotTased(by));
    }

    public void SetIgnorePlayer(bool ignore) {
        ignorePlayer = ignore;
    }
}
