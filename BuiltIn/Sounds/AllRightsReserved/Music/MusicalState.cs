using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MusicalState  {
    [SerializeField] protected float delay;
    [SerializeField] protected AudioClip music;
    [SerializeField] private bool loop = true;
    protected AudioSource source;
    private float volumeMemory;
    protected bool paused;
    public virtual void OnStart(MusicManager manager, float overallVolume, AudioMixerGroup group) {
        source = manager.gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 0f;
        source.volume = overallVolume;
        source.clip = music;
        source.loop = loop;
        source.outputAudioMixerGroup = group;
        source.reverbZoneMix = 0f;
        source.PlayDelayed(delay);
        volumeMemory = overallVolume;
    }

    public virtual void OnEnd(MusicManager manager, bool instantly) {
        if (instantly) {
            Object.Destroy(source);
        } else {
            float fadeoutDuration = 2f;
            manager.StartCoroutine(FadeOut(fadeoutDuration));
            Object.Destroy(source, fadeoutDuration+0.1f);
        }
    }

    protected IEnumerator FadeOut(float duration) {
        float startTime = Time.time;
        float startingVolume = source.volume;
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            source.volume = Mathf.Lerp(startingVolume, 0f, t);
            yield return null;
        }
        source.volume = 0f;
    }

    public virtual void SetPaused(bool paused) {
        if (!this.paused && paused) {
            volumeMemory = source.volume;
            source.volume = 0f;
            source.Pause();
        } else {
            source.volume = volumeMemory;
            source.UnPause();
        }
        this.paused = paused;
    }
}
