using System.Collections.Generic;
using AI.Events;
using UnityEngine;
using UnityEngine.VFX;

public partial class Civilian : CharacterDetector {
    [SerializeField] private Transform taserOutput;
    
    private TaserConnection taserAttachment;
    private Modifier speedModifer;
    private static List<Civilian> allCops = new List<Civilian>();
    public static List<Civilian> GetAllCops() => allCops;
    public bool GetAimingWeapon() => inputGenerator.GetAimingWeapon();
    public bool IsCop() => taserOutput != null;
    public event InputGenerator.InputTriggeredAction reloaded;
    private class TaserConnection {
        private AudioSource source;
        public TaserConnection(CharacterBase target, Transform taserOutput, RaycastHit hit, VisualEffectAsset lightning, AudioPack taserZapping) {
            this.target = target;
            this.taserOutput = taserOutput;
            targetTransform = hit.transform;
            localTargetPosition = targetTransform.InverseTransformPoint(hit.point);
            
            var taserGameObject = new GameObject("TaserLine", typeof(VisualEffect));
            taserLine = taserGameObject.GetComponent<VisualEffect>();
            taserLine.visualEffectAsset = lightning;
            taserLine.SetVector3("StartPos", taserOutput.position);
            taserLine.SetVector3("EndPos", hit.point);
            
            source = target.gameObject.AddComponent<AudioSource>();
            source.spatialBlend = 1f;
            source.minDistance = 1f;
            source.maxDistance = 25f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.loop = true;
            taserZapping.Play(source);
        }

        public void LateUpdate() {
            taserLine.SetVector3("StartPos", taserOutput.position);
            taserLine.SetVector3("EndPos", targetTransform.TransformPoint(localTargetPosition));
        }

        public void Destroy() {
            Object.Destroy(source);
            Object.Destroy(taserLine.gameObject);
        }

        public static bool Valid(Transform taserOutput, Vector3 hitPos) {
            return Vector3.Distance(taserOutput.position, hitPos) < 25f;
        }

        public float GetDistance() => Vector3.Distance(taserOutput.position, targetTransform.TransformPoint(localTargetPosition));

        public CharacterBase target;
        private Transform targetTransform;
        private Vector3 localTargetPosition;
        private VisualEffect taserLine;
        private Transform taserOutput;
    }

    protected override void Awake() {
        base.Awake();
        speedModifer = new Modifier(0.5f);
    }

    private void CopOnEnable() {
        if (GetActor()?.IsCop() == true) {
            allCops.Add(this);
        }

        if (inputGenerator == null) {
            return;
        }
        inputGenerator.fireWeapon += OnFireTaser;
        inputGenerator.reloadWeapon += OnReloadTaser;
    }

    private void CopOnDisable() {
        if (GetActor()?.IsCop() == true) {
            allCops.Remove(this);
        }
        if (inputGenerator == null) {
            return;
        }
        inputGenerator.fireWeapon -= OnFireTaser;
        inputGenerator.reloadWeapon -= OnReloadTaser;
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        if (taserAttachment != null) {
            taserAttachment.LateUpdate();
            if (taserAttachment.GetDistance() > 15f) {
                OnReloadTaser();
            }
        }
    }

    private void OnProjectileHit(RaycastHit hit) {
        CharacterBase target = hit.collider.GetComponentInParent<CharacterBase>();
        if (target == null || target == this) {
            return;
        }
        if (!TaserConnection.Valid(taserOutput, hit.point)) {
            return;
        }

        if (taserAttachment != null) {
            taserAttachment.target.OnTaseEnd(this);
            taserAttachment.target.RemoveSpeedModifier(speedModifer);
            taserAttachment.Destroy();
            taserAttachment = null;
        }

        target.OnTaseStart(this);
        target.AddSpeedModifier(speedModifer);
        taserAttachment = new TaserConnection(target, taserOutput, hit, GameManager.GetLibrary().lightning, GameManager.GetLibrary().taserZapping);
        
        GetActor()?.RaiseEvent(new SuccessfulTase(target));
    }

    public CharacterBase GetTaseTarget() {
        if (taserAttachment != null) {
            return taserAttachment.target;
        }
        return null;
    }

    public void OnReloadTaser() {
        if (taserAttachment == null || !IsCop()) {
            return;
        }

        taserAttachment.target.OnTaseEnd(this);
        taserAttachment.target.RemoveSpeedModifier(speedModifer);
        taserAttachment.Destroy();
        taserAttachment = null;
        reloaded?.Invoke();
    }

    public Vector3 GetBarrelPosition() => taserOutput.position;

    public void OnFireTaser() {
        if (taserAttachment != null || !IsCop()) {
            return;
        }

        var pos = GetLimb(HumanBodyBones.Chest).transform.position;
        var diff = GetBarrelPosition() - pos;
        
        // Got stuck in wall
        if (Physics.Raycast(new Ray(pos, diff.normalized), diff.magnitude, solidWorldMask)) {
            return;
        }

        AudioPack.PlayClipAtPoint(GameManager.GetLibrary().taserShot, GetBarrelPosition());
        var projectile = Instantiate(GameManager.GetLibrary().taserProjectile.gameObject).GetComponent<Projectile>();
        projectile.Fire(taserOutput.position,  inputGenerator.GetLookDirection());
        projectile.hitSomething += OnProjectileHit;
    }
}
