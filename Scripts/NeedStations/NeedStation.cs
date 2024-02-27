using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class NeedStation : InitializationManagerInitialized, IInteractable {
    [SerializeField, SerializeReference, SubclassSelector]
    private List<GameEventResponse> onUsedResponses;
    private static RaycastHit[] raycastHits = new RaycastHit[16];
    private CharacterGroup animationGroup;

    private bool activated;
    private Collider attachedCollider;
    public delegate void EscapeAction(CharacterBase target);
    public static event EscapeAction escaped;

    [Serializable]
    public class IKTarget {
        [SerializeField] private NeedsAvatarIKGoal goal;
        public Transform target;
        [Range(0f,1f)]
        public float rotationWeight = 1f;
        [Range(0f,1f)]
        public float positionWeight = 1f;

        public void ApplyIK(Animator animator) {
            if (goal == NeedsAvatarIKGoal.Hips) return;
            animator.SetIKPosition(ConvertIK(goal), target.position);
            animator.SetIKRotation(ConvertIK(goal), target.rotation);
            animator.SetIKPositionWeight(ConvertIK(goal), positionWeight);
            animator.SetIKRotationWeight(ConvertIK(goal), rotationWeight);
        }

        public void ApplyUpdate(Animator animator) {
            if (goal != NeedsAvatarIKGoal.Hips) return;
            var diff = target.position - animator.GetBoneTransform(HumanBodyBones.Hips).position;
            animator.transform.position += diff;
        }

        public void OnDestroy(Animator animator) {
            if (goal == NeedsAvatarIKGoal.Hips) {
                animator.transform.localPosition = Vector3.down;
            }
        }
    }

    [SerializeField] private IKTarget[] ikTargets;
    [SerializeField] private bool hidesUser = false;
    [SerializeField] private bool escape = false;
    [SerializeField] private List<CharacterGroup> validGroups;
    
    public class NeedStationInfo : MonoBehaviour {
        private NeedStation owner;
        private IKTarget[] ikTargets;
        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void SetOwner(NeedStation owner) {
            this.owner = owner;
        }

        public void Finish() {
            owner.OnFinishedAction();
        }

        public void SetTargets(IKTarget[] targets) {
            ikTargets = targets;
        }

        private void OnAnimatorIK(int layerIndex) {
            foreach (var ikTarget in ikTargets) {
                ikTarget.ApplyIK(animator);
            }        
        }

        private void Activate() {
            owner.Activate();
        }

        private void Update() {
            foreach (var ikTarget in ikTargets) {
                ikTarget.ApplyUpdate(animator);
            }
        }

        private void OnDestroy() {
            foreach (var ikTarget in ikTargets) {
                ikTarget.OnDestroy(animator);
            }
        }
    }
    [SerializeField]
    private RuntimeAnimatorController interactableController;
    [SerializeField]
    protected Transform animationTransform;
    
    
    private RuntimeAnimatorController controllerStorage;
    private TicketLock.Ticket lockTicket;
    protected CharacterBase beingUsedBy;
    
    public virtual bool CanInteract(CharacterBase from) {
        // Ugh, NPCs will walk around a corner with a condom and think they can't use it anymore...
        /*Vector3 target = GetInteractBounds().center;
        Vector3 origin = from.transform.position;
        Vector3 diff = target - origin;
        int hits = Physics.RaycastNonAlloc(origin, diff.normalized, raycastHits, diff.magnitude, CharacterBase.solidWorldMask);
        for (int i = 0; i < hits; i++) {
            if (raycastHits[i].collider == attachedCollider) {
                continue;
            }
            if (Vector3.Distance(raycastHits[i].point, target) < 0.1f) {
                continue;
            }

            var t = raycastHits[i].collider.transform;
            bool isSelf = false;
            while (t != null) {
                if (t == transform) {
                    isSelf = true;
                    break;
                }
                t = t.parent;
            }
            
            if (isSelf) {
                continue;
            }

            Debug.DrawLine(origin, raycastHits[i].point, Color.red);
            return false;
        }*/
        if (escape && !ObjectiveManager.HasCompletedObjectives()) {
            return false;
        }

        return beingUsedBy == null && isActiveAndEnabled;
    }

    public virtual bool ShouldInteract(CharacterBase from) {
        foreach (var group in from.GetCharacterGroups()) {
            if (validGroups.Contains(group)) {
                return true;
            }
        }
        return false;
    }

    public virtual void OnBeginInteract(CharacterBase from) {
        beingUsedBy = from;
        activated = false;
        if (hidesUser) {
            lockTicket ??= from.ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock | TicketLock.LockFlags.Kinematic | TicketLock.LockFlags.IgnoreCollisions);
        } else {
            lockTicket ??= from.ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock | TicketLock.LockFlags.Kinematic);
        }

        from.SetFacingDirection(QuaternionExtensions.LookRotationUpPriority(animationTransform.forward, Vector3.up));
        var characterAnimator = from.GetDisplayAnimator();
        var needStationInfo = characterAnimator.gameObject.AddComponent<NeedStationInfo>();
        needStationInfo.SetOwner(this);
        needStationInfo.SetTargets(ikTargets);
        controllerStorage = characterAnimator.runtimeAnimatorController;
        characterAnimator.runtimeAnimatorController = interactableController;

        foreach (var displayGroup in beingUsedBy.GetDisplayGroups())
        {
            characterAnimator.SetBool(displayGroup.name, true);
        }

        var parentConstraint = from.transform.gameObject.AddComponent<ParentConstraint>();
        parentConstraint.SetSources(new List<ConstraintSource>() { new(){sourceTransform = animationTransform, weight = 1f}});
        parentConstraint.constraintActive = true;
        parentConstraint.translationOffsets = new[] { animationTransform.InverseTransformPoint(from.transform.position) };
        parentConstraint.rotationOffsets = new[] { (Quaternion.Inverse(animationTransform.rotation)*from.transform.rotation).eulerAngles };
        StartCoroutine(LerpToTransform(from));
    }

    public virtual void OnEndInteract(CharacterBase from) {
        beingUsedBy = null;
        from.ticketLock.RemoveLock(ref lockTicket);
        var characterAnimator = from.GetDisplayAnimator();      
        Destroy(characterAnimator.GetComponent<NeedStationInfo>());
        var parentConstraint = from.GetComponent<ParentConstraint>();
        Destroy(parentConstraint);
        characterAnimator.runtimeAnimatorController = controllerStorage;
        StopAllCoroutines();
    }

    public Bounds GetInteractBounds() {
        if (attachedCollider != null) {
            return attachedCollider.bounds;
        } else {
            return new Bounds(transform.position, Vector3.one * 0.1f);
        }
    }

    protected virtual void Activate() {
        if (!activated) {
            foreach (var response in onUsedResponses) {
                response.Invoke(this);
            }
            activated = true;
            if (!escape) return;
            escaped?.Invoke(beingUsedBy);
        }
    }

    public void OnFinishedAction() {
        Activate();
        beingUsedBy.StopInteractionWith(this);
    }

    public override InitializationManager.InitializationStage GetInitializationStage() {
        return InitializationManager.InitializationStage.AfterMods;
    }

    public override PleaseRememberToCallDoneInitialization OnInitialized(DoneInitializingAction doneInitializingAction) {
        attachedCollider = GetComponent<Collider>();
        return doneInitializingAction?.Invoke(this);
    }

    protected override void OnEnable() {
        base.OnEnable();
        InteractableLibrary.AddInteractable(this);
    }

    protected void OnDisable() {
        if (beingUsedBy != null) {
            beingUsedBy.StopInteractionWith(this);
        }
        InteractableLibrary.RemoveInteractable(this);
    }

    private IEnumerator LerpToTransform(CharacterBase from) {
        float startTime = Time.time;
        float duration = 0.8f;
        var parentConstraint = from.GetComponent<ParentConstraint>();
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            parentConstraint.SetTranslationOffset(0, Vector3.Lerp(parentConstraint.GetTranslationOffset(0), Vector3.zero, t));
            parentConstraint.SetRotationOffset(0, Vector3.Lerp(parentConstraint.GetRotationOffset(0), Vector3.zero, t));
            yield return null;
        }

        parentConstraint.SetTranslationOffset(0, Vector3.zero);
        parentConstraint.SetRotationOffset(0, Vector3.zero);
    }

    protected virtual void OnDrawGizmosSelected() {
        if (animationTransform == null) {
            return;
        }

        if (interactableController == null) {
            return;
        }

        if (interactableController.animationClips.Length == 0) {
            return;
        }
        Animator animator = animationTransform.GetComponentInChildren<Animator>();
        if (animator == null) {
            return;
        }

        animator.applyRootMotion = false;
        var clip = interactableController.animationClips[0];
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

    private static AvatarIKGoal ConvertIK(NeedsAvatarIKGoal goal) {
        switch(goal) {
            case NeedsAvatarIKGoal.LeftFoot:
                return AvatarIKGoal.LeftFoot;
            case NeedsAvatarIKGoal.RightFoot:
                return AvatarIKGoal.RightFoot;
            case NeedsAvatarIKGoal.LeftHand:
                return AvatarIKGoal.LeftHand;
            case NeedsAvatarIKGoal.RightHand:
                return AvatarIKGoal.RightHand;
            default:
                throw new NotImplementedException("This doesn't work with non AvatarIKGoal values");
        }
    }

    private enum NeedsAvatarIKGoal {
        LeftFoot,
        RightFoot,
        LeftHand,
        RightHand,
        Hips
    }
}
