using System.Threading.Tasks;
using PenetrationTech;
using UnityEngine;
using UnityEngine.VFX;

public class BreedingStand : NeedStation, ICumContainer {
    [SerializeField] private Condom condomPrefab;
    [SerializeField, SerializeReference, SubclassSelector] private OrbitCameraConfiguration fuckConfiguration; 
    [SerializeField] private Transform condomAttachmentLocation;
    [SerializeField] protected int condomsAllowedUntilBreak = 3;
    [SerializeField] protected GameObject[] brokenGraphics;
    [SerializeField] protected GameObject[] workingGraphics;
    
    [SerializeField] protected VisualEffectAsset breakVFX;
    [SerializeField] protected AudioPack breakAudioPack;

    protected bool broken = false;
    protected Penetrator currentDick;
    protected FuckSimulation simulation;
    protected Penetrable penetrable;
    private TicketLock.Ticket lockTicket;
    private Condom currentCondom;
    protected float cumAccumulation = 0f;
    protected int condomsFinished;
    private static readonly int ThrustBackForward = Animator.StringToHash("ThrustBackForward");
    private static readonly int ThrustDownUp = Animator.StringToHash("ThrustDownUp");


    protected virtual void Update() {
        if (simulation != null) {
            simulation.SimulateStep(Time.deltaTime);
            Vector3 desiredHipPositionWorld = simulation.GetHipPosition();
            Vector3 hipToRoot = -Vector3.up * 0.75f;
            Vector3 desiredHipPositionAnimatorSpace = beingUsedBy.GetDisplayAnimator().transform.InverseTransformPoint(desiredHipPositionWorld+hipToRoot);
            float forwardHipThrustAmount = desiredHipPositionAnimatorSpace.z;
            float upHipThrustAmount = desiredHipPositionAnimatorSpace.y;
            beingUsedBy.GetDisplayAnimator().SetFloat(ThrustBackForward, forwardHipThrustAmount*2f);
            beingUsedBy.GetDisplayAnimator().SetFloat(ThrustDownUp, upHipThrustAmount*2f);
        }
    }

    protected void FixedUpdate() {
        if (simulation == null || beingUsedBy == null) return;
        if (beingUsedBy.voreContainer is Balls balls) {
            var ballsBody = balls.GetBallsRigidbody();
            if (ballsBody != null) {
                ballsBody.AddForce(OrbitCamera.GetCamera().transform.forward * 8f, ForceMode.Acceleration);
            }
        }
    }

    public override bool CanInteract(CharacterBase from) {
        //return from.GetBallVolume() > 0 && !broken;
        return !broken && from.CanCockVorePlayer();
    }

    public override bool ShouldInteract(CharacterBase from) {
        return from.IsPlayer();
    }

    public override void OnBeginInteract(CharacterBase from) {
        from.SetFacingDirection(QuaternionExtensions.LookRotationUpPriority(penetrable.GetPath().GetVelocityFromT(0).normalized, Vector3.up));
        base.OnBeginInteract(from);
        currentDick = from.GetComponentInChildren<Penetrator>();
        currentDick.Penetrate(penetrable);
        simulation = new FuckSimulation(OrbitCamera.GetCamera(), currentDick.GetRootBone(), penetrable, currentDick, from.GetDisplayAnimator());
        OrbitCamera.AddConfiguration(fuckConfiguration);
        if (from.voreContainer is Balls balls) {
            var ballsBody = balls.GetBallsRigidbody();
            if (ballsBody != null) {
                Physics.IgnoreCollision(ballsBody.gameObject.GetComponent<SphereCollider>(),
                    beingUsedBy.GetComponent<CapsuleCollider>(), true);
            }
        }
    }

    protected virtual void SetBroken(bool broken) {
        if (!this.broken && broken) {
            GameObject visualEffectGameObject = new GameObject("TemporaryVFX", typeof(VisualEffect));
            visualEffectGameObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            VisualEffect visualEffect = visualEffectGameObject.GetComponent<VisualEffect>();
            visualEffect.visualEffectAsset = breakVFX;
            visualEffect.Play();
            Destroy(visualEffectGameObject, 3f);
            AudioPack.PlayClipAtPoint(breakAudioPack, transform.position);
        }

        this.broken = broken;
        foreach (var obj in brokenGraphics) {
            obj.gameObject.SetActive(broken);
        }
        foreach (var obj in workingGraphics) {
            obj.gameObject.SetActive(!broken);
        }
    }

    public override void OnEndInteract(CharacterBase from) {
        if (from.voreContainer is Balls balls) {
            var ballsBody = balls.GetBallsRigidbody();
            if (ballsBody != null) {
                Physics.IgnoreCollision(ballsBody.gameObject.GetComponent<SphereCollider>(),
                    from.GetComponent<CapsuleCollider>(), false);
            }
        }

        base.OnEndInteract(from);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentDick.Penetrate(null);
        currentDick.SetTargetHole(null);
        currentDick.Penetrate(null);
        currentDick.SetTargetHole(null);
        simulation = null;
        OrbitCamera.RemoveConfiguration(fuckConfiguration);
    }
    
    public virtual void FinishCondom(IChurnable churnable) {
        cumAccumulation = 0f;
        
        if (currentCondom == null) {
            return;
        }

        currentCondom.OnCondomFinishedFill(churnable);
        currentCondom = null;

        if (++condomsFinished >= condomsAllowedUntilBreak) {
            if (beingUsedBy != null) {
                beingUsedBy.StopInteractionWith(this);
            }
            SetBroken(true);
        }
    }

    public virtual void AddCum(float amount) {
        if (currentCondom == null) {
            var newGameObject = Instantiate(condomPrefab.gameObject, condomAttachmentLocation.transform.position, condomAttachmentLocation.rotation);
            currentCondom = newGameObject.GetComponent<Condom>();
            currentCondom.OnCondomStartFill(condomAttachmentLocation);
        }

        cumAccumulation += amount;
        currentCondom.OnCondomSetFluid(cumAccumulation);
    }

    public override Task OnInitialized() {
        if (penetrable == null) {
            penetrable = GetComponent<Penetrable>();
        }

        var link = penetrable.gameObject.AddComponent<LinkPenetrableToCumContainer>();
        link.SetCumContainer(this);
        SetBroken(false);
        return base.OnInitialized();
    }
}
