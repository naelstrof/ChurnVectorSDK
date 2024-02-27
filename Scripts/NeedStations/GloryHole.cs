using System.Collections.Generic;
using ActorActions;
using AI.Events;
using PenetrationTech;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GloryHole : BreedingStand {
    [SerializeField] private CharacterSpawnInfo submissivePrefabReference;
    [SerializeField] private RuntimeAnimatorController submissiveAnimatorController;
    [SerializeField] private Transform submissiveTargetPosition;
    [SerializeField] private IKTarget[] submissiveIKTargets;
    private RuntimeAnimatorController civilianController;

    private bool gotCum;
    private CharacterDetector submissive;
    private TicketLock.Ticket submissiveLock;
    private CharacterAnimatorController submissiveController;
    private float lastUseTime;
    
    private Vector3 knotForcePosition;
    private float fuckIntensity;
    private float lastFuckDepth;
    private static readonly int BeingFucked = Animator.StringToHash("BeingFucked");
    private static readonly int BackForward = Animator.StringToHash("ThrustBackForward");
    private static readonly int DownUp = Animator.StringToHash("ThrustDownUp");
    private static readonly int PushPullAmount = Animator.StringToHash("PushPullAmount");
    private static readonly int UpDownAmount = Animator.StringToHash("UpDownAmount");
    private static readonly int Depth = Animator.StringToHash("Depth");
    private AsyncOperationHandle<Civilian> handle;
    private DoneInitializingAction done;
    private bool initialized = false;


    private void OnCompletedLoadSubmissive(AsyncOperationHandle<Civilian> obj) {
        Transform transform1 = transform;
        submissive = obj.Result;
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
        penetrable = submissive.GetComponentInChildren<Penetrable>();
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
        base.OnInitialized(done);
        initialized = true;
    }

    public override bool CanInteract(CharacterBase from) {
        return base.CanInteract(from) && submissive.isActiveAndEnabled;
    }

    public override void OnBeginInteract(CharacterBase from) {
        base.OnBeginInteract(from);
        if (Time.time - lastUseTime > 8f) {
            GameManager.StaticStartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.PartnerSex).Begin(new DialogueCharacter[] {
                DialogueCharacterSpecificCharacter.Get(from),
                DialogueCharacterSpecificCharacter.Get(submissive),
            }));
            lastUseTime = Time.time;
            submissive.GetDisplayAnimator().SetBool(BeingFucked, true);
        }
    }

    protected override void Update() {
        if (!initialized) {
            return;
        }
        if (simulation != null) {
            simulation.SimulateStep(Time.deltaTime);
            Vector3 desiredHipPositionWorld = simulation.GetHipPosition();
            Debug.DrawRay(desiredHipPositionWorld, Vector3.up, Color.magenta);
            Vector3 hipToRoot = -Vector3.up * 0.75f;
            Vector3 desiredHipPositionAnimatorSpace = beingUsedBy.GetDisplayAnimator().transform.InverseTransformPoint(desiredHipPositionWorld+hipToRoot);
            float forwardHipThrustAmount = desiredHipPositionAnimatorSpace.z;
            float upHipThrustAmount = desiredHipPositionAnimatorSpace.y;
            Vector3 thrust = new Vector3(0f, upHipThrustAmount*2f, forwardHipThrustAmount*2f);
            beingUsedBy.GetDisplayAnimator().SetFloat(BackForward, forwardHipThrustAmount*2f);
            beingUsedBy.GetDisplayAnimator().SetFloat(DownUp, upHipThrustAmount*2f);
            
            float arbitraryAdjustment = 1.5f;
            Vector3 correctionForce = simulation.GetCorrrectiveForce() * arbitraryAdjustment;
            if (thrust.magnitude >= 1f) {
                correctionForce = Vector3.ProjectOnPlane(correctionForce, beingUsedBy.transform.TransformDirection(thrust.normalized));
            }
            knotForcePosition += correctionForce;
            knotForcePosition -= knotForcePosition * (Time.deltaTime*4f);
            knotForcePosition = Vector3.ProjectOnPlane(knotForcePosition, Vector3.Cross((submissive.GetDisplayAnimator().transform.position-beingUsedBy.GetDisplayAnimator().transform.position).normalized, Vector3.up));
            knotForcePosition = Vector3.ClampMagnitude(knotForcePosition, 1f);
        
            submissive.GetDisplayAnimator().SetFloat(PushPullAmount, Vector3.Dot(knotForcePosition, penetrable.GetPath().GetVelocityFromDistance(penetrable.GetActualHoleDistanceFromStartOfSpline()).normalized));
            submissive.GetDisplayAnimator().SetFloat(UpDownAmount, Vector3.Dot(knotForcePosition,Vector3.up));
            var length = currentDick.GetWorldLength();
            float currentDepth = (length - Vector3.Distance(penetrable.GetPath().GetPositionFromT(0f), currentDick.GetRootBone().position)) / length;
            float diff = Mathf.Abs(currentDepth-lastFuckDepth);
            lastFuckDepth = currentDepth;
            fuckIntensity += diff;
            fuckIntensity = Mathf.MoveTowards(fuckIntensity, 0f, Time.deltaTime);
            submissive.GetDisplayAnimator().SetFloat(Depth, diff);
        } else {
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
        submissive.AddChurnVolumeAndSolidVolume(churnable.GetVolumeChurned(), churnable.GetVolumeSolid());
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

    public override PleaseRememberToCallDoneInitialization OnInitialized(DoneInitializingAction doneInitializingAction) {
        handle = submissivePrefabReference.GetCharacter();
        lastUseTime = Time.time;
        if (handle.IsDone) {
            OnCompletedLoadSubmissive(handle);
            initialized = true;
            lastUseTime = Time.time;
            return base.OnInitialized(doneInitializingAction);
        } else {
            done = doneInitializingAction;
            handle.Completed += OnCompletedLoadSubmissive;
            return new IWillRememberToCallDoneInitialization();
        }
    }
}
