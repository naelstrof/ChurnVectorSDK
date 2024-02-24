using System.Collections.Generic;
using PenetrationTech;
using SkinnedMeshDecals;
using UnityEngine;
using UnityEngine.VFX;

//[System.Serializable]
public class VoreMachine {
    private static WaitForEndOfFrame waitForEndOfFrame = new();
    private float bigSplashDelayNormalized = 0.75f;

    private List<CockVoreStatus> currentVores;
    public class CockVoreStatus {
        public ICockVorable other;
        public Penetrator dick;
        public Vector3 dickTipPosition;
        public Vector3 dickTipNormal;
        public Vector3 dickTipBinormal;
        public Quaternion dickTipRotation;
        public float progress;
        public float progressAdjusted;
        public float direction;
        public float dickTipRadius;
        public bool didBigSplash;
    }

    public delegate void CockVoreEventAction(CockVoreStatus status);
    
    public event CockVoreEventAction cockVoreStart;
    public event CockVoreEventAction cockVoreUpdate;
    public event CockVoreEventAction cockVoreEnd;
    
    private AudioSource loopingCockGlorpSound;
    private VisualEffect bigSplash;
    
    private CharacterBase pred;
    private Penetrator dick;
    private float lastDecalTime;
    
    public bool IsCockvoring() => currentVores.Count != 0;

    public void Initialize(CharacterBase pred, Penetrator dick) {
        this.pred = pred;
        this.dick = dick;
        currentVores = new List<CockVoreStatus>();
        var splashObj = new GameObject("BigSplashEffect", typeof(VisualEffect));
        splashObj.transform.SetParent(pred.transform);
        splashObj.transform.localScale = Vector3.one * 0.6f;
        bigSplash = splashObj.GetComponent<VisualEffect>();
        bigSplash.visualEffectAsset = GameManager.GetLibrary().bigSplash;
        if (bigSplash.visualEffectAsset != null) {
            bigSplash.SetVector4("Color", Vector4.one);
        }
        splashObj.gameObject.SetActive(false);
    }

    public void StopVore() {
        foreach(var status in currentVores) {
            if (status.progress < 0.50f) {
                status.direction = -2f;
            }
        }
    }

    public void StartVore(ICockVorable target) {
        foreach (var s in currentVores) {
            if (s.other != target) continue;
            s.direction = 1f;
            return;
        }

        CockVoreStatus status = new CockVoreStatus {
            other = target,
            progress = 0f,
            progressAdjusted = 0f,
            dick = dick,
            direction = 1f,
        };
        currentVores.Add(status);
        cockVoreStart?.Invoke(status);
        status.other.OnStartCockvoreAsPrey(pred);
        var slippySlidePack = GameManager.GetLibrary().slippySlidePack;
        loopingCockGlorpSound = CharacterDetector.PlayLoopingInvestigativeAudioPackOnTransform(pred, slippySlidePack, pred.transform, 10f);
    }

    public void LateUpdate() {
        for (int i = 0; i < currentVores.Count && i != -1; i++) {
            var status = currentVores[i];
            const float duration = 8f;
            if (status.progress is >= 0f and < 1f) {
                status.progress += status.direction * Time.deltaTime / duration;
                float t = status.progress;
                if (t > bigSplashDelayNormalized && !status.didBigSplash && status.direction > 0f) {
                    if (!bigSplash.gameObject.activeInHierarchy) {
                        bigSplash.gameObject.SetActive(true);
                    }
                    bigSplash.Play();
                    status.didBigSplash = true;
                }

                if (t < bigSplashDelayNormalized && status.didBigSplash && status.direction < 0f) {
                    status.didBigSplash = false;
                }

                var cockVoreCurve = GameManager.GetLibrary().cockVorePath;
                float tAdjust = cockVoreCurve.EvaluateCurve(t);
                var path = dick.GetPath();
                float cockLength = dick.GetWorldLength();
                float distanceAlongDick = cockLength * tAdjust;
                Vector3 normal = path.GetVelocityFromDistance(cockLength).normalized;
                Vector3 binormal = path.GetBinormalFromT(path.GetTimeFromDistance(cockLength)).normalized;
                Quaternion endRotation = Quaternion.LookRotation(normal, binormal);
                Vector3 position = path.GetPositionFromDistance(cockLength);
                bigSplash.transform.position = position;
                bigSplash.transform.rotation = endRotation;
                status.dickTipRotation = endRotation;
                status.dickTipPosition = position;
                status.dickTipNormal = normal;
                status.dickTipBinormal = binormal;
                status.progressAdjusted = tAdjust;
                status.other.OnCockVoreVisualsUpdateAsPrey(status);
                if (Time.time - lastDecalTime > 0.1f) {
                    var decalableCollider = status.other.transform.GetComponent<DecalableCollider>();
                    if (decalableCollider != null) {
                        foreach (var renderer in decalableCollider.GetDecalableRenderers()) {
                            float radius = 0.25f;
                            PaintDecal.RenderDecal(renderer, GameManager.GetLibrary().cumProjector,
                                position - Vector3.forward * radius,
                                Quaternion.identity, Vector2.one * (radius * 2f), radius * 2f, "_DecalColorMap",
                                RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                        }
                    }
                    lastDecalTime = Time.time;
                }

                cockVoreUpdate?.Invoke(status);
                return;
            }

            if (loopingCockGlorpSound != null) {
                Object.Destroy(loopingCockGlorpSound);
            }
            
            pred.StopInteractionWith(status.other as IInteractable);
            if (status.progress >= 1f) {
                pred.AddChurnable(status.other as IChurnable);
                status.other.OnFinishedCockvoreAsPrey(pred);
                CharacterDetector.PlayInvestigativeAudioPackAtPoint(pred, GameManager.GetLibrary().glorpPack, pred.transform.position, 10f);
            } else {
                status.other.OnCancelledCockvoreAsPrey(pred);
            }
            
            status.progress = 1f;
            status.progressAdjusted = 1f;
            cockVoreUpdate?.Invoke(status);
            cockVoreEnd?.Invoke(status);
            currentVores.RemoveAt(i--);
        }
    }

}
