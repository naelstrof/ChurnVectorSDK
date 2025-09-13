using System.Collections.Generic;
using DPG;
using SkinnedMeshDecals;
using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
public class CockVoreMachine : VoreMachine {
    [SerializeField]
    private Penetrator dick;
    
    private static WaitForEndOfFrame waitForEndOfFrame = new();
    private float bigSplashDelayNormalized = 0.75f;

    private List<VoreStatus> currentVores;
    private List<VoreStatus> removeVores; 
    
    private AudioSource loopingCockGlorpSound;
    private VisualEffect bigSplash;
    
    private CharacterBase pred;
    private float lastDecalTime;
    
    private static CatmullSpline cachedSpline;
    
    public override bool IsVoring() => currentVores.Count != 0;

    public Penetrator GetPenetrator() => dick;
    public override void Initialize(CharacterBase pred) {
        this.pred = pred;
        currentVores = new List<VoreStatus>();
        removeVores = new List<VoreStatus>();
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

    public override void StopVore() {
        foreach(var status in currentVores) {
            if (status.progress < 0.50f) {
                status.direction = -2f;
            }
        }
    }

    public override void StartVore(IVorable target) {
        foreach (var s in currentVores) {
            if (s.other != target) continue;
            s.direction = 1f;
            return;
        }

        VoreStatus status = new VoreStatus {
            other = target,
            progress = 0f,
            progressAdjusted = 0f,
            dick = dick,
            direction = 1f,
        };
        currentVores.Add(status);
        voreStart?.Invoke(status);
        status.other.OnStartVoreAsPrey(pred);
        var slippySlidePack = GameManager.GetLibrary().slippySlidePack;
        loopingCockGlorpSound = CharacterDetector.PlayLoopingInvestigativeAudioPackOnTransform(pred, slippySlidePack, pred.transform, 10f);
    }

    public override void LateUpdate() {
        cachedSpline ??= new CatmullSpline(new[] { Vector3.zero, Vector3.one });
        dick.GetFinalizedSpline(ref cachedSpline, out var distanceAlongSpline, out var insertionLerp, out var penetrationArgs);
        removeVores.Clear();
        foreach(var status in currentVores) {
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
                float cockLength = dick.GetSquashStretchedWorldLength()+distanceAlongSpline;
                float distanceAlongDick = cockLength * tAdjust;
                Vector3 normal = cachedSpline.GetVelocityFromDistance(cockLength).normalized;
                Vector3 binormal = cachedSpline.GetBinormalFromT(cachedSpline.GetTimeFromDistance(cockLength)).normalized;
                Quaternion endRotation = Quaternion.LookRotation(normal, binormal);
                Vector3 position = cachedSpline.GetPositionFromDistance(cockLength);
                bigSplash.transform.position = position;
                bigSplash.transform.rotation = endRotation;
                status.dickTipRotation = endRotation;
                status.dickTipPosition = position;
                status.dickTipNormal = normal;
                status.dickTipBinormal = binormal;
                status.progressAdjusted = tAdjust;
                status.other.OnVoreVisualsUpdateAsPrey(status);
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

                voreUpdate?.Invoke(status);
                continue;
            }

            if (loopingCockGlorpSound != null) {
                Object.Destroy(loopingCockGlorpSound);
            }
            
            pred.StopInteractionWith(status.other as IInteractable);
            if (status.progress >= 1f) {
                var prey = status.other as IChurnable;
                if(prey.GetContents() != null) {
                    pred.AddChurnable(prey.GetContents());
                }
                pred.AddChurnable(prey);
                status.other.OnFinishedVoreAsPrey(pred);
                CharacterDetector.PlayInvestigativeAudioPackAtPoint(pred, GameManager.GetLibrary().glorpPack, pred.transform.position, 10f);
            } else {
                status.other.OnCancelledVoreAsPrey(pred);
            }
            
            status.progress = 1f;
            status.progressAdjusted = 1f;
            voreUpdate?.Invoke(status);
            voreEnd?.Invoke(status);
            removeVores.Add(status);
        }

        foreach (var status in removeVores) {
            currentVores.Remove(status);
        }
    }

}
