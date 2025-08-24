using System.Collections;
using System.Collections.Generic;
using JigglePhysics;
using Naelstrof.Easing;
using Naelstrof.Inflatable;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Balls : VoreContainer {
    [SerializeField] private Inflatable inflater;
    // This group is passed on to needs stations for the purpose of group specific animations
    public delegate void BallsChangedAction(bool active, float colliderSize, Vector3 position);
    public BallsChangedAction ballsChanged;
    private GameObject gameObject;
    private AudioSource audioSource;
    private Animator targetAnimator;
    private Rigidbody body;
    private SphereCollider collider;
    private SpringJoint joint;
    private CharacterBase target;
    private Transform hip;
    private CumStorage storage;
    public override CumStorage GetStorage() => storage;
    private NavMeshObstacle navMeshObstacle;
    private float churnAccumulator;
    private const float churnTick = 3f;
    private bool churning = false;
    public override Transform GetStorageTransform() {
        return GetBallsTransform();
    }

    public Transform GetBallsTransform() => gameObject.transform;
    public Rigidbody GetBallsRigidbody() => body;
    public override void Initialize(CharacterBase target) {
        storage = new CumStorage();
        storage.startChurn += OnStartChurn;
        
        this.target = target;
        targetAnimator = target.GetDisplayAnimator();
        gameObject = new GameObject("Balls", typeof(Rigidbody), typeof(SphereCollider), typeof(SpringJoint), typeof(DecalableCollider)) {
            transform = { position = target.transform.position }
        };
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 25f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        gameObject.AddComponent<PhysicsAudio>();
        navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        navMeshObstacle.shape = NavMeshObstacleShape.Box;
        navMeshObstacle.carving = true;
        navMeshObstacle.enabled = this.target.IsPlayer();
        gameObject.layer = LayerMask.NameToLayer("Characters");
        hip = target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Hips);
        body = gameObject.GetComponent<Rigidbody>();
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.drag = 1f;
        body.angularDrag = 5f;
        joint = gameObject.GetComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = target.GetBody();
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
        collider = gameObject.GetComponent<SphereCollider>();
        collider.sharedMaterial = GameManager.GetLibrary().ballsMaterial;
        var decalableCollider = gameObject.GetComponent<DecalableCollider>();
        decalableCollider.SetDecalableRenderers(target.GetComponentsInChildren<SkinnedMeshRenderer>());
        joint.spring = 400f;
        joint.damper = 40f;
        joint.minDistance = 0.15f;
        joint.enableCollision = true;
        joint.connectedAnchor = target.GetBody().transform.InverseTransformPoint(hip.position);
        inflater.OnEnable();
        inflater.changed += OnSizeChanged;
        navMeshObstacle.enabled = target.IsPlayer();
        if (target.IsPlayer()) {
            foreach (var builder in target.GetComponentsInChildren<JiggleRigBuilder>()) {
                foreach (var rig in builder.jiggleRigs) {
                    List<Collider> existingColliders = new List<Collider>(rig.GetColliders());
                    existingColliders.Add(collider);
                    rig.SetColliders(existingColliders);
                }
            }
        }

        UpdateInflater();
        SetActive(false);
    }


    private void OnStartChurn(CumStorage.CumSource churnable) {
        if (churning == false) {
            if (!gameObject.activeSelf) {
                SetActive(true);
            }
        }
    }

    public override void OnJump(float velocity) {
        body.velocity = Vector3.up*(velocity*0.5f);
        //body.AddForce(Vector3.up * (velocity*10f), ForceMode.Impulse);
    }

    public override void LateUpdate() {
        if (!gameObject.activeSelf) {
            return;
        }
        churnAccumulator += Time.deltaTime;
        if (churnAccumulator > churnTick) {
            churnAccumulator -= churnTick;
            if (!gameObject.activeSelf) {
                SetActive(true);
            }

            var churnPack = GameManager.GetLibrary().churnPack;
            var gurglePack = GameManager.GetLibrary().tummyGurglesPack;
            if (!storage.GetDoneChurning()) {
                if (target.IsPlayer()) {
                    CharacterDetector.PlayInvestigativeAudioPackOnSource(target, churnPack, audioSource, 8f, Easing.Cubic.Out(1f - storage.GetChurnProgress()));
                } else {
                    churnPack.Play(audioSource);
                }

                float pitchShift = 1f - Mathf.Pow(storage.GetVolume()/10f + 1f, -2f);
                float pitchVariance = churnPack.GetPitchVariance();
                audioSource.pitch = Mathf.Lerp(1f+pitchVariance, 1f-pitchVariance, pitchShift);
            } else {
                if (!audioSource.isPlaying && storage.GetVolume() >= 0.5f) {
                    if (target.IsPlayer()) {
                        CharacterDetector.PlayInvestigativeAudioPackOnSource(target, gurglePack, audioSource, Mathf.Min(2f*storage.GetVolume(),8f), Easing.Cubic.Out(Mathf.Clamp01(storage.GetVolume()*0.5f)));
                    } else {
                        churnPack.Play(audioSource);
                    }

                    float pitchShift = 1f - Mathf.Pow(storage.GetVolume()/10f + 1f, -2f);
                    float pitchVariance = gurglePack.GetPitchVariance();
                    audioSource.pitch = Mathf.Lerp(1f+pitchVariance, 1f-pitchVariance, pitchShift);
                }
            }

            UpdateInflater();
        }
        joint.connectedAnchor = target.GetBody().transform.InverseTransformPoint(hip.position);
        ballsChanged?.Invoke(gameObject.activeSelf, collider.radius, gameObject.transform.position);
    }

    public override void AddChurnable(IChurnable churnable) {
        storage.AddChurnable(churnable);
        UpdateInflater();
    }

    public override void SetActive(bool active) {
        if (active && !gameObject.activeSelf) {
            gameObject.SetActive(true);
            target.StartCoroutine(DisableCollisionForAWhile());
            var hipPosition = hip.transform.position - targetAnimator.transform.forward*Mathf.Max(collider.radius, 0.1f);
            gameObject.transform.position = hipPosition;
            body.position = hipPosition;
            body.velocity = targetAnimator.transform.forward * -3f;
            inflater.OnEnable();
        } else if (!active && gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DisableCollisionForAWhile() {
        CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(targetCollider, collider, true);
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        do {
            yield return wait;
        } while (Physics.ComputePenetration(targetCollider, targetCollider.transform.position,
                 targetCollider.transform.rotation, collider, collider.transform.position, collider.transform.rotation,
                 out Vector3 direction, out float distance));
        Physics.IgnoreCollision(targetCollider, collider, false);
    }

    private void OnSizeChanged(float newSize) {
        float radius = Mathf.Max(0.1f, Mathf.Sqrt(Mathf.Abs(newSize - 1f)) * 0.3f);
        collider.radius = radius;
        joint.minDistance = radius+0.1f; 
        // mass of a sphere (assuming 1kg/m^3)
        body.mass = Mathf.Pow(radius*(4f/3f)*Mathf.PI,3f);
        bool active = newSize > 1f;
        SetActive(active);
        ballsChanged?.Invoke(active, radius, gameObject.transform.position);
        body.WakeUp();
        if (target.IsPlayer()) {
            navMeshObstacle.size = Vector3.one * collider.radius;
        }
    }

    private void UpdateInflater() {
        //inflater.SetSize(Mathf.Log(cumAmount*2f + 1f, 2f)+1f+leftToChurn*4f, target);
        inflater.SetSize(Mathf.Sqrt(storage.GetVolume()*4f)+1f, target);
    }

    public void BeginEmission(CumStorage.ChurnedAction startEvent = null, CumStorage.EmitCumAction emitEvent = null, CumStorage.ChurnedAction endEvent = null) {
        storage.BeginEmission(1.2f, target, startEvent, emitEvent, endEvent);
    }

    // Stop tween coroutines that might have become stuck when recombobulated
    public void OnReEnabled() {
        inflater.EarilyTerminateTween(target);
    }
}
