using System.Collections;
using System.Collections.Generic;
using Naelstrof.Easing;
using PenetrationTech;
using UnityEngine;

public class DickCum : MonoBehaviour {
    [SerializeField] private List<ParticleSystem> cumParticleSystems;
    private List<SkinnedMeshRenderer> dickRenderers;
    [SerializeField] private List<Penetrator> targetDicks;
    [SerializeField] private AudioPack glorp;
    [SerializeField] private CharacterBase attachedCharacter;

    public void SetInfo(ParticleSystem cumSystem, Penetrator penetrator, AudioPack glorpPack, CharacterBase attachedCharacter) {
        cumParticleSystems = new List<ParticleSystem>();
        cumParticleSystems.Add(cumSystem);
        targetDicks = new List<Penetrator>();
        targetDicks.Add(penetrator);
        glorp = glorpPack;
        this.attachedCharacter = attachedCharacter;
        ScanRenderers();
    }

    public delegate void LoadChangeAction(int newLoadCount);

    private float lastStimTime;

    private float stimulationBuffer;
    private const float maxStimulation = 4f;
    public delegate void StimulationChangeAction(float newAmount);
    public delegate void CumAction();
    public event CumAction cummed;
    public event StimulationChangeAction stimulated;
    private struct CumBlendshape {
        public int blendshapeIndex;
        public float progress;
    }

    private bool cumming = false;

    private List<CumBlendshape> _cumBlendshapes;
    private ICumContainer stand;

    private void ScanRenderers() {
        if (targetDicks == null) {
            return;
        }

        dickRenderers.Clear();
        foreach (var dick in targetDicks) {
            if (dick == null) {
                continue;
            }
            foreach (var r in dick.GetTargetRenderers()) {
                dickRenderers.Add(r.renderer as SkinnedMeshRenderer);
            }
        }

        if (dickRenderers.Count == 0) {
            return;
        }
        _cumBlendshapes = new List<CumBlendshape>();
        int total = 0;
        for (int i = 0; i < dickRenderers[0].sharedMesh.blendShapeCount; i++) {
            if (IsCumBlendshape(dickRenderers[0].sharedMesh,i)) {
                total++;
            }
        }

        for (int i = 0; i < dickRenderers[0].sharedMesh.blendShapeCount; i++) {
            if (IsCumBlendshape(dickRenderers[0].sharedMesh,i)) {
                _cumBlendshapes.Add(new CumBlendshape { blendshapeIndex = i, progress = _cumBlendshapes.Count/(float)total});
            }
        }
    }

    private void Awake() {
        dickRenderers = new List<SkinnedMeshRenderer>();
        ScanRenderers();
        lastStimTime = Time.time;
    }

    private void Update() {
        if (cumming) {
            foreach (var cumParticleSystem in cumParticleSystems) {
                if (!cumParticleSystem.isPlaying) {
                    cumParticleSystem.Play();
                }

                int index = cumParticleSystems.IndexOf(cumParticleSystem);
                if (index >= 0 && index < targetDicks.Count) {
                    if (targetDicks[index].TryGetPenetrable(out Penetrable penetrable) == false) {
                        var emissionModule = cumParticleSystem.emission;
                        emissionModule.rateOverTime = 60f;
                    }
                } else {
                    var emissionModule = cumParticleSystem.emission;
                    emissionModule.rateOverTime = 60f;
                }
            }
        } else {
            foreach (var cumParticleSystem in cumParticleSystems) {
                var emissionModule = cumParticleSystem.emission;
                emissionModule.rateOverTime = 0f;
            }
        }

        stimulated?.Invoke(stimulationBuffer/maxStimulation);

        if (!cumming) {
            float t = Mathf.Clamp01((Time.time - lastStimTime)/5f);
            stimulationBuffer = Mathf.MoveTowards(stimulationBuffer, 0f, Time.deltaTime * maxStimulation * 0.5f *t*t);
            
            stimulationBuffer = Mathf.MoveTowards(stimulationBuffer, 0f, Time.deltaTime * 0.025f * stimulationBuffer);
        }
    }

    private void Cum() {
        ScanRenderers();
        stimulationBuffer = 0f;
        attachedCharacter.BeginEmission(OnStartChurnEmission, OnCumEmission, OnFinishChurnEmission);
        cummed?.Invoke();
    }
    private bool IsCumBlendshape(Mesh sharedMesh, int blendshapeID) {
        return sharedMesh.GetBlendShapeName(blendshapeID).StartsWith("Pulse") || (sharedMesh.GetBlendShapeName(blendshapeID).StartsWith("Cum") && sharedMesh.GetBlendShapeName(blendshapeID) != "Cumflate");
    }

    private void OnStartChurnEmission(CumStorage.CumSource churnable) {
        cumming = true;
    }

    private void OnCumEmission(float amount, CumStorage.CumSource churnable) {
        StartCoroutine(CumPulseRoutine(amount, churnable));
    }
    private void OnFinishChurnEmission(CumStorage.CumSource churnable) {
        cumming = false;
        StartCoroutine(CumFinishRoutine(churnable, stand));
    }

    private IEnumerator CumFinishRoutine(CumStorage.CumSource churnable, ICumContainer stand) {
        yield return new WaitForSeconds(1.1f);
        stand.FinishCondom(churnable.GetChurnable());
    }

    private IEnumerator CumPulseRoutine(float cumAmount, CumStorage.CumSource churnable) {
        float pulseStartTime = Time.time;
        float pulseDuration = 0.6f;
        float bumpSize = 0.5f;
        AudioPack.PlayClipAtPoint(glorp, targetDicks.Count > 0 ? targetDicks[0].GetRootBone().position : transform.position);
        while (Time.time < pulseStartTime + pulseDuration) {
            float t = (Time.time - pulseStartTime) / pulseDuration;
            float totalProgress = t.Remap(0f, 1f, -bumpSize, 1f + bumpSize);
            foreach (var cumBlendshape in _cumBlendshapes) {
                float distance = Mathf.Abs(totalProgress - cumBlendshape.progress);
                float normalizedBumpDistance = (bumpSize - distance) / bumpSize;
                float triggerAmount = Easing.Cubic.InOut(Mathf.Clamp01(normalizedBumpDistance));
                foreach (var dick in dickRenderers) {
                    dick.SetBlendShapeWeight(cumBlendshape.blendshapeIndex, triggerAmount * 100f);
                }
            }
            yield return null;
        }

        foreach (var cumBlendshape in _cumBlendshapes) {
            foreach (var dick in dickRenderers) {
                dick.SetBlendShapeWeight(cumBlendshape.blendshapeIndex, 0f);
            }
        }

        if (targetDicks != null) {
            foreach (var targetDick in targetDicks) {
                if (!targetDick.TryGetPenetrable(out Penetrable targetPenetrable)) continue;
                if (!targetPenetrable.TryGetComponent(out LinkPenetrableToCumContainer link)) continue;
                var tryStand = link.GetCumContainer();
                if (tryStand == null) {
                    continue;
                }
                if (tryStand != stand) {
                    if (stand != null) {
                        stand.FinishCondom(churnable.GetChurnable());
                    }
                    stand = tryStand;
                    break;
                }
            }
            stand.AddCum(cumAmount);
        }
    }

    public void AddStimulation(float movement) {
        if (movement == 0f || cumming || !attachedCharacter.IsInteracting()) {
            return;
        }

        lastStimTime = Time.time;
        stimulationBuffer += Mathf.Abs(movement);
        stimulationBuffer = Mathf.Min(stimulationBuffer, maxStimulation);
        stimulated?.Invoke(stimulationBuffer/maxStimulation);
        if (!(stimulationBuffer >= maxStimulation-0.0001f)) return;
        Cum();
    }
}
