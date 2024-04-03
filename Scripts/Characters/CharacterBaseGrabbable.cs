using System.Collections.Generic;
using DPG;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CharacterBase))]
public class CharacterBaseEditor : Editor {
    public void OnSceneGUI() {
        var character = (CharacterBase)target;
        character.OnSceneGUI(serializedObject);
    }
}
#endif

public partial class CharacterBase : IInteractable, IVorable {
    [SerializeField,HideInInspector]
    private Vector3 noseOffset;
    
    private List<Transform> cockVoreTransformPath;
    
    private List<Vector3> positionPath;
    private CatmullSpline path;

    private TicketLock.Ticket ticket;
    private TicketLock.Ticket grabLock;
    private TicketLock.Ticket grabbedLock;
    
    private Transform cockVoreTransform;
    private RaycastHit[] hits = new RaycastHit[32];
    protected CharacterBase grabbedBy;
    
    private List<Material> bodyMaterials;
    public bool IsGrabbed() => grabbedBy != null;

    private DragDirection grabbedDirection;
    public DragDirection GetGrabDirection() => grabbedDirection;
    public enum DragDirection
    {
        Front = 0,
        Back = 1,
    }

    private class GrabbingIK : MonoBehaviour {
        private CharacterBase target;
        private CharacterBase self;
        private Animator animator;

        private void OnEnable() {
            animator = GetComponent<Animator>();
        }

        public void SetCharacter(CharacterBase self, CharacterBase target) {
            this.target = target;
            this.self = self;
        }
        public void OnAnimatorIK(int layerIndex) {
            if (self.IsVoring()) {
                animator.SetLookAtPosition(target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head).position);
                animator.SetLookAtWeight(0.1f, 0f, 0.1f, 0.5f, 0.1f);
                return;
            }

            Transform handTarget = target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.LeftHand);
            Transform footTarget = target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.LeftFoot);
            Transform grabTarget;
            if (Vector3.Distance(handTarget.position, animator.transform.position) <
                Vector3.Distance(footTarget.position, animator.transform.position)) {
                grabTarget = handTarget;
            } else {
                grabTarget = footTarget;
            }
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0.5f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, grabTarget.position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, grabTarget.position);
            animator.SetLookAtPosition(target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head).position);
            animator.SetLookAtWeight(1f, 0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    private bool beingVored = false;
    private static readonly int WorldDickPosition = Shader.PropertyToID("_WorldDickPosition");
    private static readonly int WorldDickNormal = Shader.PropertyToID("_WorldDickNormal");
    private static readonly int WorldDickBinormal = Shader.PropertyToID("_WorldDickBinormal");
    public bool IsBeingVored() => beingVored;

    public delegate void GrabbedChangedAction(CharacterBase grabbedBy, DragDirection direction);

    public event GrabbedChangedAction grabChanged;
    public delegate void CockVoreAction(CharacterBase other);

    public event CockVoreAction startCockVoreAsPrey;
    public event CockVoreMachine.VoreEventAction updateCockVoreAsPrey;
    public event CockVoreAction endCockVoreAsPrey;
    public event CockVoreAction cancelCockVoreAsPrey;

    private void OnEnableGrabbable() {
        bodyMaterials = new List<Material>();
        foreach (var skinnedMeshRenderers in GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
            foreach (var mat in skinnedMeshRenderers.materials) {
                bodyMaterials.Add(mat);
            }
        }
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>(true)) {
            foreach (var mat in meshRenderer.materials) {
                bodyMaterials.Add(mat);
            }
        }

        cockVoreTransformPath = new List<Transform>();
        cockVoreTransformPath.Add(GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Hips));
        cockVoreTransformPath.Add(GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Chest));
        cockVoreTransformPath.Add(GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Neck));
        cockVoreTransformPath.Add(GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head));

        path = new CatmullSpline(new []{Vector3.zero,Vector3.one});
        positionPath = new List<Vector3>();
    }

    private CatmullSpline GetPath() {
        positionPath.Clear();
        Vector3 hipPos = cockVoreTransformPath[0].position;
        Vector3 feetPos = (GetDisplayAnimator().GetBoneTransform(HumanBodyBones.LeftFoot).position + GetDisplayAnimator().GetBoneTransform(HumanBodyBones.RightFoot).position)*0.5f;
        Vector3 proj = (feetPos - hipPos).normalized;
        float arbitraryExtraFeetLength = 0.2f;
        positionPath.Add(feetPos + proj * arbitraryExtraFeetLength);
        foreach (var t in cockVoreTransformPath) {
            positionPath.Add(t.position);
        }

        if (noseOffset != Vector3.zero) {
            positionPath.Add(cockVoreTransformPath[^1].TransformPoint(noseOffset));
        }

        path.SetWeightsFromPoints(positionPath);
        return path;
    }

    private void UpdateGrabbable() {
        if (grabbedBy != null && !grabbedBy.IsVoring()) {
            Vector3 dir = transform.position - grabbedBy.transform.position;
            grabbedBy.SetFacingDirection(Quaternion.RotateTowards(grabbedBy.GetFacingDirection(),QuaternionExtensions.LookRotationUpPriority(dir.normalized, Vector3.up), Time.deltaTime*270f));
        }
        positionPath.Clear();
        foreach (var t in cockVoreTransformPath) {
            positionPath.Add(t.position);
        }
        path.SetWeightsFromPoints(positionPath);
    }
    
    private bool CanBeGrabbedBy(CharacterBase grabbingCharacter) {
        if (grabbingCharacter == this) {
            return false;
        }

        var pos = grabbingCharacter.transform.position;
        var diff = transform.position - pos;
        if (Physics.RaycastNonAlloc(new Ray(pos, diff.normalized), hits, diff.magnitude, solidWorldMask) > 0) {
            return false;
        }

        return true;
    }

    public bool CanInteract(CharacterBase from) {
        return grabbedBy == null && CanBeGrabbedBy(from);
    }

    public bool ShouldInteract(CharacterBase from) {
        if (from.IsPlayer()) return true;
        if (from.cockVoreDick == null) {
            return false;
        }
        foreach (var group in from.GetCharacterGroups()) {
            if (usedByGroups.Contains(group)) {
                return true;
            }
        }
        return false;
    }

    public virtual void OnBeginInteract(CharacterBase from) {
        from.Grab(this);
        grabbedDirection = UnityEngine.Random.Range(0f, 1f) < 0.5f ? DragDirection.Back : DragDirection.Front;
        if (activeInteractable != null) {
            StopInteractionWith(activeInteractable);
        }
        grabbedBy = from;
        grabbedBy.GetDisplayAnimator().gameObject.AddComponent<GrabbingIK>().SetCharacter(grabbedBy, this);
        grabChanged?.Invoke(from, grabbedDirection);
        grabLock ??= from.ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock);
        grabbedLock ??= ticketLock.AddLock(from, TicketLock.LockFlags.IgnoreUsables);
        collider.direction = 2;
        float height = collider.height;
        collider.center = collider.center.With(y:(height - 1f) / 2f - 1f + collider.radius);
        KnowledgeDatabase.ForcePoll();
    }

    public virtual void OnEndInteract(CharacterBase from) {
        collider.direction = 1;
        float height = collider.height;
        collider.center = collider.center.With(y:(height - 1f) / 2f);
        
        from.ticketLock.RemoveLock(ref grabLock);
        ticketLock.RemoveLock(ref grabbedLock);
        from.Grab(null);
        if (grabbedBy != null) {
            var grabbingIK = grabbedBy.GetDisplayAnimator().gameObject.GetComponent<GrabbingIK>();
            Destroy(grabbingIK);
            grabbedBy = null;
        }
        grabChanged?.Invoke(null, 0);
    }

    public Bounds GetInteractBounds() {
        return GetComponent<Collider>().bounds;
    }

    public bool CanCockVore(CharacterBase from) {
        return true;
    }

    public void OnStartVoreAsPrey(CharacterBase from) {
        from.ticketLock.RemoveLock(ref grabLock);
        ticket ??= ticketLock.AddLock(from);

        beingVored = true;
        foreach (var mat in bodyMaterials) {
            mat.EnableKeyword("_COCKVORESQUISHENABLED_ON");
        }
        startCockVoreAsPrey?.Invoke(this);
    }

    public void OnVoreVisualsUpdateAsPrey(CockVoreMachine.VoreStatus status) {
        var otherCVRotation = GetCockVoreRotation(1f-status.progressAdjusted);
        Quaternion neededRotation = status.dickTipRotation * Quaternion.Inverse(otherCVRotation);
        transform.rotation = neededRotation * transform.rotation;
        
        var otherCVPosition = GetCockVorePosition(1f-status.progressAdjusted);
        Vector3 diff = status.dickTipPosition - otherCVPosition;
        transform.position += diff;
        
        foreach (var mat in bodyMaterials) {
            mat.SetVector(WorldDickPosition, new Vector4(status.dickTipPosition.x, status.dickTipPosition.y, status.dickTipPosition.z, 1f));
            mat.SetVector(WorldDickNormal, new Vector4(status.dickTipNormal.x, status.dickTipNormal.y, status.dickTipNormal.z, 0f));
            mat.SetVector(WorldDickBinormal, new Vector4(status.dickTipBinormal.x, status.dickTipBinormal.y, status.dickTipBinormal.z, 0f));
            //mat.SetVector("_WorldSlurpPosition", new Vector4(blobPosition.x, blobPosition.y, blobPosition.z, 1f));
        }

        updateCockVoreAsPrey?.Invoke(status);
    }

    public void OnFinishedVoreAsPrey(CharacterBase from) {
        body.rotation = QuaternionExtensions.LookRotationUpPriority(transform.forward, Vector3.up);
        if (grabbedBy != null) {
            grabLock ??= grabbedBy.ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock);
        }
        ticketLock.RemoveLock(ref ticket);
        foreach (var mat in bodyMaterials) {
            mat.DisableKeyword("_COCKVORESQUISHENABLED_ON");
        }
        beingVored = false;
        endCockVoreAsPrey?.Invoke(from);
        gameObject.SetActive(false);
    }

    public void OnCancelledVoreAsPrey(CharacterBase from) {
        if (grabbedBy != null) {
            grabLock ??= grabbedBy.ticketLock.AddLock(this, TicketLock.LockFlags.FacingDirectionLock);
        }
        beingVored = false;
        ticketLock.RemoveLock(ref ticket);
        body.rotation = QuaternionExtensions.LookRotationUpPriority(GetFacingDirection() * Vector3.forward, Vector3.up);
        var lastpos = body.position;
        var pos = from.GetBody().position;
        var diff = lastpos - pos;
        float colliderRadiusButSlightlySmaller = 0.2f;
        float realColliderRadiusDiff = 0.05f;
        if (Physics.SphereCast(pos, colliderRadiusButSlightlySmaller, diff.normalized, out RaycastHit info, diff.magnitude, solidWorldMask)) {
            body.position = info.point - diff*realColliderRadiusDiff;
        }

        foreach (var mat in bodyMaterials) {
            mat.DisableKeyword("_COCKVORESQUISHENABLED_ON");
        }
        cancelCockVoreAsPrey?.Invoke(this);
    }

    public Quaternion GetCockVoreRotation(float progress) {
        var p = GetPath();
        Vector3 forward;
        switch(grabbedDirection) {
            case DragDirection.Front: forward = p.GetVelocityFromT(progress).normalized; break;
            case DragDirection.Back: forward = -p.GetVelocityFromT(1f-progress).normalized; break;
            default: throw new UnityException("Unsupported grab direction");
        }
        var rotation = Quaternion.LookRotation(forward, transform.forward);
        return rotation;
    }
    public Vector3 GetCockVorePosition(float progress) {
        var p = GetPath();
        switch(grabbedDirection) {
            case DragDirection.Front: return p.GetPositionFromT(progress);
            case DragDirection.Back: return p.GetPositionFromT(1f - progress);
            default: throw new UnityException("Unsupported grab direction");
        }
    }

    private void OnDrawGizmos() {
        voreMachine?.OnDrawGizmos();
        if (cockVoreTransformPath == null || cockVoreTransformPath.Count <= 1) {
            return;
        }

        Gizmos.color = Color.red;
        if (path == null || path.GetWeights().Count != (cockVoreTransformPath.Count-1)*4) {
            path = new CatmullSpline(new []{Vector3.zero,Vector3.one});
            positionPath = new List<Vector3>();
        }
        positionPath.Clear();
        foreach (var t in cockVoreTransformPath) {
            positionPath.Add(t.position);
        }
        if (positionPath.Count < 2) {
            return;
        }
        path.SetWeightsFromPoints(positionPath);
        Vector3 lastPoint = path.GetPositionFromT(0f);
        for (int i=0;i<64;i++) {
            Vector3 newPoint = path.GetPositionFromT(i/64f);
            Gizmos.DrawLine(lastPoint, newPoint);
            lastPoint = newPoint;
        }

        foreach(var point in positionPath) {
            Gizmos.DrawSphere(point, 0.02f);
        }
    }

    #if UNITY_EDITOR
    public void OnSceneGUI(SerializedObject obj) {
        if (GetDisplayAnimator() == null || GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head) == null) {
            return;
        }
        var noseOffsetProp = obj.FindProperty("noseOffset");
        var head = GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head);
        Vector3 pos = head.TransformPoint(noseOffsetProp.vector3Value);
        Quaternion rot = GetDisplayAnimator().transform.rotation;
        EditorGUI.BeginChangeCheck();
        var newPos = Handles.PositionHandle(pos, rot);
        Handles.Label(pos, "Nose");
        if (EditorGUI.EndChangeCheck()) {
            noseOffsetProp.vector3Value = head.InverseTransformPoint(newPos);
            EditorUtility.SetDirty(this);
            obj.ApplyModifiedProperties();
        }
    }
    #endif
}
