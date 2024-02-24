using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CumStorage {
    public delegate void EmitCumAction(float volume, CumSource churnable);
    public delegate void ChurnedAction(CumSource churnable);
    public event ChurnedAction startChurn;
    public event ChurnedAction drained;
    public class CumSource {
        private readonly IChurnable churnable;
        private readonly float startTime;
        private float drainedPercentage = 0f;

        public IChurnable GetChurnable() => churnable;
        public CumSource(IChurnable churnable) {
            this.churnable = churnable;
            startTime = Time.time;
        }
        public bool GetDoneChurning() {
            return (Time.time - startTime) > churnable.GetChurnDuration();
        }

        public float GetChurnProgress() {
            float t = (Time.time - startTime) / churnable.GetChurnDuration();
            return t;
        }

        public float GetVolume() {
            return Mathf.Lerp(churnable.GetVolumeSolid(), churnable.GetVolumeChurned(), GetChurnProgress())*(1f-drainedPercentage);
        }

        public void SetDrainedPercentage(float drainedPercentage, out float volumeChange) {
            Assert.IsTrue(GetDoneChurning());
            float percentDifference = drainedPercentage - this.drainedPercentage;
            volumeChange = churnable.GetVolumeChurned() * percentDifference;
            this.drainedPercentage = drainedPercentage;
        }
    }

    public bool GetDoneChurning() {
        foreach (var source in sources) {
            if (!source.GetDoneChurning()) {
                return false;
            }
        }
        return true;
    }

    public float GetChurnProgress() {
        if (sources.Count <= 0) {
            return 1f;
        }
        float churnProgress = 0f;
        foreach (var source in sources) {
            churnProgress += source.GetChurnProgress();
        }
        return churnProgress / sources.Count;
    }

    public float GetChurnProgress(IChurnable churnable) {
        foreach (var source in sources) {
            if (source.GetChurnable() == churnable) {
                return source.GetChurnProgress();
            }
        }

        return 0f;
    }

    public CumStorage() {
        sources = new List<CumSource>();
    }

    public List<CumSource> GetSources() => sources;

    private List<CumSource> sources;
    public void AddChurnable(IChurnable churnable) {
        var source = new CumSource(churnable);
        sources.Add(source);
        startChurn?.Invoke(source);
    }

    public float GetVolume() {
        float volume = 0f;
        foreach (var source in sources) {
            volume += source.GetVolume();
        }
        return volume;
    }

    private IEnumerator DrainRoutine(float emitInterval, ChurnedAction startEvent, EmitCumAction emitEvent, ChurnedAction endEvent) {
        List<CumSource> churnedSources = new List<CumSource>();
        foreach (var source in sources) {
            if (source.GetDoneChurning()) {
                churnedSources.Add(source);
                // Only do one character at a time!
                break;
            }
        }
        WaitForSeconds interval = new WaitForSeconds(emitInterval);
        float fullDuration = Mathf.Sqrt(Mathf.Max(churnedSources.Count,1f)) * 8f;
        foreach (var churnedSource in churnedSources) {
            startEvent?.Invoke(churnedSource);
            float startDrainTime = Time.time;
            float drainDuration = fullDuration/churnedSources.Count;
            yield return interval;
            while (Time.time - startDrainTime < drainDuration) {
                float t = (Time.time - startDrainTime) / drainDuration;
                churnedSource.SetDrainedPercentage(t, out float volumeChange);
                emitEvent?.Invoke(volumeChange, churnedSource);
                yield return interval;
            }
            churnedSource.SetDrainedPercentage(1f, out float finalVolumeChange);
            emitEvent?.Invoke(finalVolumeChange, churnedSource);
            yield return new WaitForSeconds(0.1f);
            endEvent?.Invoke(churnedSource);
            drained?.Invoke(churnedSource);
            sources.Remove(churnedSource);
        }
    }

    public void BeginEmission(float emitInterval, MonoBehaviour owner, ChurnedAction startEvent = null, EmitCumAction emitEvent = null, ChurnedAction endEvent = null) {
        owner.StartCoroutine(DrainRoutine(emitInterval, startEvent, emitEvent, endEvent));
    }

}
