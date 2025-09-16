using System.Collections.Generic;
using System.Threading.Tasks;
using ActorActions;
using AI.Events;
using DPG;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GloryHole : BreedingStand {
    [SerializeField] private CharacterSpawnInfo submissivePrefabReference;
    [SerializeField] private RuntimeAnimatorController submissiveAnimatorController;
    [SerializeField] private Transform submissiveTargetPosition;
    [SerializeField] private IKTarget[] submissiveIKTargets;
    [SerializeField] private CharacterBase.PenetrableType submissivePenetrableTargetType;
    private RuntimeAnimatorController civilianController;

    private bool gotCum;
    private CharacterDetector submissive;
    private TicketLock.Ticket submissiveLock;
    private CharacterAnimatorController submissiveController;
    private float lastUseTime = 0f;
    
    private Vector3 knotForcePosition;
    private Vector3 knotForceVelocity;
    private float fuckIntensity;
    private float lastFuckDepth;
    private static readonly int BeingFucked = Animator.StringToHash("BeingFucked");
    private static readonly int BackForward = Animator.StringToHash("ThrustBackForward");
    private static readonly int DownUp = Animator.StringToHash("ThrustDownUp");
    private static readonly int PushPullAmount = Animator.StringToHash("PushPullAmount");
    private static readonly int UpDownAmount = Animator.StringToHash("UpDownAmount");
    private static readonly int Depth = Animator.StringToHash("Depth");
    private AsyncOperationHandle<Civilian> handle;
    private bool initialized = false;
    private CatmullSpline cachedSpline;

    private void OnCompletedLoadSubmissive(AsyncOperationHandle<Civilian> obj) {
    }

    public override bool CanInteract(CharacterBase from) {
        return base.CanInteract(from) && submissive.isActiveAndEnabled;
    }

    public override void OnBeginInteract(CharacterBase from) {
        base.OnBeginInteract(from);
        if (lastUseTime == 0 || Time.time - lastUseTime > 8f) {
            GameManager.StaticStartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.PartnerSex).Begin(new DialogueCharacter[] {
                DialogueCharacterSpecificCharacter.Get(from),
                DialogueCharacterSpecificCharacter.Get(submissive),
            }));
            lastUseTime = Time.time;
            submissive.GetDisplayAnimator().SetBool(BeingFucked, true);
        }
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (!initialized) {
            return;
        }
        if (simulation != null) {
            var thrust = GetThrustValue();
            
            float arbitraryAdjustment = 1.5f;
            Vector3 correctionForce = simulation.GetCorrrectiveForce() * arbitraryAdjustment;
            if (thrust.magnitude >= 1f) {
                correctionForce = Vector3.ProjectOnPlane(correctionForce, beingUsedBy.transform.TransformDirection(thrust.normalized));
            }

            float knotForceFriction = 0.5f;
            knotForceVelocity *= 1f-(knotForceFriction*knotForceFriction);
            knotForceVelocity += correctionForce * (Time.deltaTime * 100f);
            knotForceVelocity += (Vector3.zero-knotForcePosition) * (Time.deltaTime * 8f);

            knotForcePosition += knotForceVelocity * Time.deltaTime;
            knotForcePosition = Vector3.ProjectOnPlane(knotForcePosition, Vector3.Cross((submissive.GetDisplayAnimator().transform.position-beingUsedBy.GetDisplayAnimator().transform.position).normalized, Vector3.up));
            knotForcePosition = Vector3.ClampMagnitude(knotForcePosition, 1f);

            penetrable.GetHole(out var holePosition, out var holeNormal);
            submissive.GetDisplayAnimator().SetFloat(PushPullAmount, Vector3.Dot(knotForcePosition, holeNormal));
            submissive.GetDisplayAnimator().SetFloat(UpDownAmount, Vector3.Dot(knotForcePosition,Vector3.up));
            //var length = currentDick.GetSquashStretchedWorldLength();
            //float currentDepth = (length - Vector3.Distance(penetrable.GetPath().GetPositionFromT(0f), currentDick.GetRootBone().position)) / length;
            cachedSpline ??= new CatmullSpline(new[] { Vector3.zero, Vector3.one });
            currentDick.GetFinalizedSpline(ref cachedSpline, out var distanceAlongSpline, out var insertionLerp, out var penetrationArgs);
            float currentDepth = penetrationArgs?.penetrationDepth ?? 0f;
            float diff = Mathf.Abs(currentDepth-lastFuckDepth);
            lastFuckDepth = currentDepth;
            fuckIntensity += diff;
            fuckIntensity = Mathf.MoveTowards(fuckIntensity, 0f, Time.deltaTime);
            submissive.GetDisplayAnimator().SetFloat(Depth, diff);
        } else if (!broken) {
            submissive.GetDisplayAnimator().SetFloat(Depth, 0f);
        }
    }

    protected override void SetBroken(bool broken) {
        base.SetBroken(broken);
        if (broken) {
            foreach (var r in submissive.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                foreach(var mat in r.materials) {
                    mat.DisableKeyword("_PENETRABLEDEFORM_ON");
                }
            }
            submissive.ticketLock.RemoveLock(ref submissiveLock);
            Destroy(submissive.GetComponent<ParentConstraint>());
            submissive.GetDisplayAnimator().runtimeAnimatorController = civilianController;
            submissive.GetActor().RaiseEvent(new FilledWithCum());
            var needStationInfo = submissive.GetDisplayAnimator().GetComponent<NeedStationInfo>();
            Destroy(needStationInfo);
        }
    }

    public override void AddCum(float amount) {
        cumAccumulation += amount;
        submissiveController.SetCumInflationAmount(cumAccumulation);
        if (!gotCum && beingUsedBy != null) {
            StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.PartnerSexFinished).Begin(new DialogueCharacter[] {
                DialogueCharacterSpecificCharacter.Get(beingUsedBy),
                DialogueCharacterSpecificCharacter.Get(submissive),
            }));
            gotCum = true;
        }
    }

    public override void FinishCondom(IChurnable churnable) {
        if (churnable is CharacterBase churnableCharacter && churnableCharacter.IsPlayer())
        {
            submissive.SetContents(churnable);
            AttachCameraToTarget(submissive.gameObject);
            churnableCharacter.RemovePredConfig();
        } else {
            submissive.AddChurnVolumeAndSolidVolume(churnable.GetVolumeChurned(), churnable.GetVolumeSolid());
        }
        if (++condomsFinished >= condomsAllowedUntilBreak) {
            if (beingUsedBy != null) {
                beingUsedBy.StopInteractionWith(this);
            }
            SetBroken(true);
        }
    }
    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();
        if (submissiveTargetPosition == null) {
            return;
        }

        if (submissiveAnimatorController == null) {
            return;
        }

        if (submissiveAnimatorController.animationClips.Length == 0) {
            return;
        }
        Animator animator = submissiveTargetPosition.GetComponentInChildren<Animator>();
        if (animator == null) {
            return;
        }

        animator.applyRootMotion = false;
        var clip = submissiveAnimatorController.animationClips[0];
        clip.SampleAnimation(animator.gameObject, clip.length * 0.5f);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.Hips).position, animator.GetBoneTransform(HumanBodyBones.Head).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).position, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, animator.GetBoneTransform(HumanBodyBones.LeftHand).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.RightUpperArm).position, animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position, animator.GetBoneTransform(HumanBodyBones.RightHand).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, animator.GetBoneTransform(HumanBodyBones.LeftFoot).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position);
        Gizmos.DrawLine(animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, animator.GetBoneTransform(HumanBodyBones.RightFoot).position);
    }

    public override async Task OnInitialized() {
        handle = submissivePrefabReference.GetCharacter();
        await handle.Task;
        submissive = handle.Result;
        submissive.GetActor()?.OverrideActionNow(new ActionWaitToBeFilledWithCum());
        submissiveController = submissive.GetComponentInChildren<CharacterAnimatorController>();
        civilianController = submissive.GetDisplayAnimator().runtimeAnimatorController;
        submissive.GetDisplayAnimator().runtimeAnimatorController = submissiveAnimatorController;
        submissive.SetFacingDirection(submissiveTargetPosition.rotation);
        submissive.SetIgnorePlayer(true);
        submissiveLock ??= submissive.ticketLock.AddLock(this);
        var needStationInfo = submissive.GetDisplayAnimator().gameObject.AddComponent<NeedStationInfo>();
        needStationInfo.SetOwner(this);
        needStationInfo.SetTargets(submissiveIKTargets);
        penetrable = submissive.GetPenetrable(submissivePenetrableTargetType);
        var parentConstraint = submissive.transform.gameObject.AddComponent<ParentConstraint>();
        parentConstraint.SetSources(new List<ConstraintSource>() { new(){sourceTransform = submissiveTargetPosition, weight = 1f}});
        parentConstraint.constraintActive = true;
        parentConstraint.translationOffsets = new[] { Vector3.zero };
        parentConstraint.rotationOffsets = new[] { Vector3.zero };
        foreach (var r in submissive.GetComponentsInChildren<SkinnedMeshRenderer>()) {
            foreach(var mat in r.materials) {
                mat.EnableKeyword("_PENETRABLEDEFORM_ON");
            }
        }
        submissiveController.SetClothes(false);
        await base.OnInitialized();
        initialized = true;
    }
}
