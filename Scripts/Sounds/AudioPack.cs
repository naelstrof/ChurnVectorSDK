using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New AudioPack", menuName = "Data/AudioPack", order = 16)]
public class AudioPack : ScriptableObject {
    [SerializeField]
    private List<AudioClip> clips;
    [SerializeField, Range(0f,1f)]
    private float volume = 1f;
    [SerializeField, Range(0f,1f)]
    private float pitchVariance = 0.1f;
    [SerializeField]
    private AudioMixerGroup targetGroup;
    [SerializeField, Tooltip("How interesting a sound is, a value of 1 is perfectly interesting and will always cause NPCs to turn if they hear it. Values above 0.5 will cause a visual to show the player they made an absurdly interesting noise.")]
    private float soundInterestLevel = 0.1f;
    [SerializeField, Tooltip("If an NPC that is aware of the player hears this noise, they'll know it's made by the player and have their known position updated.")]
    private bool isObviousPlayerNoise = false;
    
    [SerializeField, Tooltip("If this sound should play over larger distances at elevated volumes. Things that target the player should be set.")]
    private bool important = false;

    private static AnimationCurve audioFalloff = new() {keys=new Keyframe[] { new (0f, 1f, 0, -3.1f), new (1f, 0f, 0f, 0f) } };

    private AudioClip lastClip;

    public AudioClip GetClip() {
        var newClip = clips[Random.Range(0, clips.Count)];
        int tries = 0;
        while (clips.Count > 1 && newClip == lastClip && tries++ < 10) {
            newClip = clips[Random.Range(0, clips.Count)];
        }
        lastClip = newClip;
        return newClip;
    }

    public float GetInterestLevel() {
        return soundInterestLevel;
    }

    public bool IsObviousPlayerNoise() {
        return isObviousPlayerNoise;
    }

    public float GetVolume() {
        return volume;
    }

    public float GetPitch() {
        return 1f + Random.Range(-pitchVariance, pitchVariance);
    }
    public float GetPitchVariance() {
        return pitchVariance;
    }

    public AudioMixerGroup GetAudioMixerGroup() {
        return targetGroup;
    }

    public AudioClip Play(AudioSource source, float vol = 1f) {
        source.volume = volume * vol;
        source.pitch = GetPitch();
        source.outputAudioMixerGroup = targetGroup;
        AudioClip clip = GetClip();
        source.clip = clip;
        source.Play();
        return clip;
    }

    public AudioClip PlayOneShot(AudioSource source, float oneShotVolume = 1f) {
        source.volume = volume;
        source.pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
        source.outputAudioMixerGroup = targetGroup;
        AudioClip clip = GetClip();
        source.PlayOneShot(clip, oneShotVolume);
        return clip;
    }
    
    public static AudioSource PlayClipAtPoint(AudioPack pack, Vector3 position, float volume = 1f) {
        GameObject obj = new GameObject("OneOffSoundEffect", typeof(AudioSource));
        obj.transform.position = position;
        AudioSource source = obj.GetComponent<AudioSource>();
        source.spatialBlend = 1f;
        source.minDistance = 1f;
        
        if (pack.important) {
            source.maxDistance = 80f;
        } else {
            source.maxDistance = 25f;
        }

        source.rolloffMode = AudioRolloffMode.Custom;
        source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioFalloff);
        pack.Play(source, volume);
        Destroy(obj, source.clip.length + 0.1f);
        return source;
    }
}
