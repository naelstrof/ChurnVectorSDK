using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using Naelstrof.Inflatable;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Condom : MonoBehaviour, IInteractable {
    private static List<Condom> condoms = new ();
    public static List<Condom> GetCondoms() => condoms;


    [SerializeField] private Transform JiggleHead;
    [SerializeField] private Transform JiggleTail;
    [SerializeField] private Transform condomCoupleLocation;
    [SerializeField] private Inflatable cumAmount;
    [SerializeField] private List<Dialogue> randomDialogue;
    [SerializeField] private MeshRenderer photosRenderer;
    [SerializeField] private DialogueTheme dialogueTheme;

    private List<Material> condomMaterials;
    private TicketLock.Ticket grabLock;
    private Transform targetTipFillLocation;
    private Rigidbody body;
    private float filledAmount = 0f;
    private CharacterBase grabbedBy;
    private static readonly int FlattenAmount = Shader.PropertyToID("_FlattenAmount");
    private IChurnable contents;
    private static readonly int BaseColorMap = Shader.PropertyToID("_BaseColorMap");

    private class GrabbingIK : MonoBehaviour {
        private Transform target;
        private Animator animator;

        private void OnEnable() {
            animator = GetComponent<Animator>();
        }

        public void SetCharacter(Transform target) {
            this.target = target;
        }
        public void OnAnimatorIK(int layerIndex) {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0.5f);
            var position = target.position;
            animator.SetIKPosition(AvatarIKGoal.LeftHand, position);
            animator.SetIKPosition(AvatarIKGoal.RightHand, position);
            animator.SetLookAtPosition(position);
            animator.SetLookAtWeight(1f, 0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    private void OnEnable() {
        condoms.Add(this);
    }

    private void Awake() {
        body = GetComponent<Rigidbody>();
        cumAmount.OnEnable();
        photosRenderer.enabled = false;
        condomMaterials = new List<Material>();
        foreach (var r in GetComponentsInChildren<Renderer>()) {
            condomMaterials.AddRange(r.materials);
        }
        SetFlattenAmount(0f);
    }

    private void SetFlattenAmount(float amount) {
        foreach (var mat in condomMaterials) {
            mat.SetFloat(FlattenAmount, amount);
        }
    }

    private IEnumerator BecomeFlatOverTime() {
        float startTime = Time.time;
        float duration = 1f;
        while(Time.time-startTime<duration) {
            float t = (Time.time - startTime) / duration;
            SetFlattenAmount(Easing.Bounce.Out(t));
            yield return null;
        }

        SetFlattenAmount(1f);
    }

    private void Update() {
        Vector3 jiggleVector = JiggleHead.position - JiggleTail.position;
        Vector3 biVector = Vector3.up;
        float tolerance = 0.01f;
        if (Mathf.Abs(Mathf.Abs(Vector3.Dot(jiggleVector.normalized,biVector)) - 1f) < tolerance) {
            biVector = Vector3.right;
        }

        foreach (var mat in condomMaterials) {
            mat.SetVector("_SquishVector", jiggleVector);
            mat.SetVector("_SquishBivector", biVector);
        }

        if (grabbedBy != null && !grabbedBy.IsVoring()) {
            Vector3 dir = transform.position - grabbedBy.transform.position;
            grabbedBy.SetFacingDirection(Quaternion.RotateTowards(grabbedBy.GetFacingDirection(),QuaternionExtensions.LookRotationUpPriority(dir.normalized, Vector3.up), Time.deltaTime*270f));
        }
        if (targetTipFillLocation != null && body.isKinematic) {
            Quaternion neededRotation =
                targetTipFillLocation.rotation * Quaternion.Inverse(condomCoupleLocation.rotation);
            var condomTransform = transform;
            condomTransform.rotation = neededRotation * condomTransform.rotation;
            Vector3 neededDiff = targetTipFillLocation.position - condomCoupleLocation.position;
            condomTransform.position += neededDiff;
        }

    }
    public IChurnable GetChurnable() => contents;

    private IEnumerator TalkOccasionally() {
        while (isActiveAndEnabled) {
            yield return new WaitForSeconds(Random.Range(10f, 45f));
            yield return randomDialogue[Random.Range(0, randomDialogue.Count)].Begin(new List<DialogueCharacter> { DialogueCharacterInanimateObject.Get(transform, dialogueTheme), new DialogueCharacterPlayer() });
        }
    }

    private void FixedUpdate() {
        if (body.isKinematic) {
            return;
        }
        body.velocity = (body.velocity*0.95f).With(y:body.velocity.y);
    }

    public void OnCondomStartFill(Transform tipFillLocation) {
        targetTipFillLocation = tipFillLocation;
        body.isKinematic = true;
    }
    public void OnCondomSetFluid(float amount) {
        filledAmount = amount;
        cumAmount.SetSize(Mathf.Sqrt(filledAmount*0.5f), this);
    }

    public void OnCondomFinishedFill(IChurnable churnable) {
        contents = churnable;
        photosRenderer.enabled = true;
        photosRenderer.material.SetTexture(BaseColorMap, churnable.GetHeadSprite().texture);
        CharacterDetector.AddTrackingGameObjectToAll(gameObject);
        body.isKinematic = false;
        if (filledAmount > 0.9f) {
            StartCoroutine(TalkOccasionally());
        }

        StartCoroutine(BecomeFlatOverTime());
    }

    private void OnDisable() {
        if (grabbedBy != null) {
            grabbedBy.StopInteractionWith(this);
        }
        CharacterDetector.RemoveTrackingGameObjectFromAll(gameObject);
        condoms.Remove(this);
    }

    public float GetFluidAmount() => filledAmount;
    
    private bool CanBeGrabbedBy(CharacterBase grabbingCharacter) {
        float distance = Vector3.Distance(grabbingCharacter.transform.position, transform.position);
        if (distance > FollowPathToPoint.maxDistanceFromNavmesh) {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("SaleZoneTrigger")) {
            return;
        }
        
        Destroy(gameObject);
    }
    public bool CanInteract(CharacterBase from) {
        return grabbedBy == null && CanBeGrabbedBy(from);
    }

    public bool ShouldInteract(CharacterBase from) {
        if (grabbedBy != from && grabbedBy != null) {
            return false;
        }
        if (from is CharacterDetector detector && !from.IsPlayer()) {
            return detector.knowledgeDatabase.GetKnowledge(gameObject).GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Ignorant;
        }
        return from.IsPlayer();
    }

    public void OnBeginInteract(CharacterBase from) {
        body.interpolation = RigidbodyInterpolation.Interpolate;
        from.Grab(this);
        grabLock ??= from.ticketLock.AddLock(this, TicketLock.LockFlags.IgnoreUsables);
        from.GetDisplayAnimator().gameObject.AddComponent<GrabbingIK>().SetCharacter(transform);
        grabbedBy = from;
    }

    public void OnEndInteract(CharacterBase from) {
        body.interpolation = RigidbodyInterpolation.None;
        from.Grab(null);
        from.ticketLock.RemoveLock(ref grabLock);
        Destroy(from.GetDisplayAnimator().gameObject.GetComponent<GrabbingIK>());
        grabbedBy = null;
    }

    public Bounds GetInteractBounds() {
        return GetComponent<Collider>().bounds;
    }
}
