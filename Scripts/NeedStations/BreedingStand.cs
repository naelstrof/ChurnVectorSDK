using System;
using System.Threading.Tasks;
using DPG;
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
    [SerializeField] protected AnimationClip thrustInAnimation;

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

    protected Vector3 GetThrustValue() {
        Vector3 desiredHipPositionWorld = simulation.GetHipPosition();
        Vector3 hipToRoot = -Vector3.up * 0.75f;
        Vector3 desiredHipPositionAnimatorSpace = beingUsedBy.GetDisplayAnimator().transform.InverseTransformPoint(desiredHipPositionWorld+hipToRoot);
        float forwardHipThrustAmount = desiredHipPositionAnimatorSpace.z;
        float upHipThrustAmount = desiredHipPositionAnimatorSpace.y;
        return new Vector3(0, upHipThrustAmount, forwardHipThrustAmount);
    }
    protected virtual void FixedUpdate() {
        if (simulation == null || beingUsedBy == null) return;
        simulation.SimulateStep(Time.deltaTime);
        if (simulation == null) return; // Step can trigger this to become null
        var thrust = GetThrustValue();
        beingUsedBy.GetDisplayAnimator().SetFloat(ThrustBackForward, thrust.z*2f);
        beingUsedBy.GetDisplayAnimator().SetFloat(ThrustDownUp, thrust.y*2f);
        if (beingUsedBy.voreContainer is Balls balls) {
            var ballsBody = balls.GetBallsRigidbody();
            if (ballsBody != null) {
                if(beingUsedBy.IsPlayer())
                    ballsBody.AddForce(OrbitCamera.GetCamera().transform.forward * 8f, ForceMode.Acceleration);
                else
                    ballsBody.AddForce(beingUsedBy.transform.forward * -8f, ForceMode.Acceleration);
            }
        }
    }

    public override bool CanInteract(CharacterBase from) {
        return !broken && beingUsedBy == null && from.CanCockVorePlayer() && (from.GetBallVolume() > 0 || from.IsPlayer());
    }

    public override bool ShouldInteract(CharacterBase from) {
        if (from.voreContainer == null || from.GetBallVolume() <= 0) return false;

        return base.ShouldInteract(from);
    }

    public override void OnBeginInteract(CharacterBase from) {
        Vector3 dir = penetrable.GetPoints()[1] - penetrable.GetPoints()[0];
        from.SetFacingDirection(QuaternionExtensions.LookRotationUpPriority(dir.normalized, Vector3.up));
        base.OnBeginInteract(from);
        currentDick = from.GetComponentInChildren<Penetrator>();
        if (currentDick is PenetratorJiggleDeform jiggleDeformDick) {
            jiggleDeformDick.SetLinkedPenetrable(penetrable);
        } else {
            throw new UnityException("Don't currently support anything except jiggle deform dicks...");
        }

        var animator = from.GetDisplayAnimator();
        Vector3 lastPosition = from.transform.position;
        Quaternion lastRotation = animator.transform.rotation;
        from.transform.position = animationTransform.transform.position;
        animator.transform.rotation = animationTransform.transform.rotation;
        thrustInAnimation.SampleAnimation(animator.gameObject, 0f);
        
        Vector3 dickPosition = currentDick.GetRootTransform().TransformPoint(currentDick.GetRootPositionOffset());
        Vector3 holePosition = penetrable.GetPoints()[0];
        penetrable.GetHole(out Vector3 _unused, out Vector3 holeNormal);
        Vector3 worldOffset = holePosition - dickPosition;
        targetOffset = animationTransform.InverseTransformVector(worldOffset-holeNormal*0.01f);
        
        from.transform.position = lastPosition;
        animator.transform.rotation = lastRotation;

        //simulation = new FuckSimulation(OrbitCamera.GetCamera(), currentDick.GetRootTransform(), penetrable, currentDick, from.GetDisplayAnimator());
        //OrbitCamera.AddConfiguration(fuckConfiguration);
        if (beingUsedBy.IsPlayer())
        {
            simulation = new FuckSimulation(OrbitCamera.GetCamera(), currentDick.GetRootTransform(), penetrable, currentDick, from.GetDisplayAnimator());
            OrbitCamera.AddConfiguration(fuckConfiguration);
        }
        else
            simulation = new FuckSimulationAuto(from, currentDick.GetRootTransform(), penetrable, currentDick, from.GetDisplayAnimator());
        
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
        if(from.IsPlayer()) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        if (currentDick is PenetratorJiggleDeform jiggleDeformDick) {
            jiggleDeformDick.SetLinkedPenetrable(null);
        } else {
            throw new UnityException("Don't currently support anything except jiggle deform dicks...");
        }
        if (simulation is FuckSimulationAuto autoSim)
            autoSim.Disable();
        simulation = null;
        OrbitCamera.RemoveConfiguration(fuckConfiguration);
    }
    
    public virtual void FinishCondom(IChurnable churnable) {
        cumAccumulation = 0f;
        
        if (currentCondom == null) {
            return;
        }
        if (churnable is CharacterBase churnableCharacter)
        {
            churnableCharacter.voreContainer.SetActive(false);
            if(churnableCharacter.IsPlayer()) {
                AttachCameraToTarget(currentCondom.gameObject);
                churnableCharacter.RemovePredConfig();
            }            
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

    public void AttachCameraToTarget(GameObject target) {
        if (target == null)
            return;

        OrbitCameraBasicConfiguration basicConfig = new OrbitCameraBasicConfiguration();
        GameObject targetPivot = new GameObject("CondomPivot", typeof(OrbitCameraPivotBasic), typeof(OnDestroyEventHandler));
        targetPivot.transform.SetParent(target.transform, false);
        OrbitCameraPivotBasic basicPivot = targetPivot.GetComponent<OrbitCameraPivotBasic>();
        basicPivot.SetInfo(basicPivot.GetScreenOffset(), 2f, 65);
        basicConfig.SetPivot(basicPivot);
        targetPivot.GetComponent<OnDestroyEventHandler>().onDestroyOrDisable += (obj) => {
            OrbitCamera.RemoveConfiguration(basicConfig);
        };

        OrbitCamera.AddConfiguration(basicConfig);
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
