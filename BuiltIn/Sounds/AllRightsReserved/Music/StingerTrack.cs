using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class StingerTrack : MusicalState {
    [SerializeField] private AudioClip startStinger;
    [SerializeField] private AudioClip startStingerRepeated;
    [SerializeField] private AudioClip endStinger;

    private static Dictionary<AudioClip, double> lastPlayedStartStingers = new Dictionary<AudioClip, double>();

    private Coroutine waitThenPlayMusic;
    public override void OnStart(MusicManager manager, float overallVolume, AudioMixerGroup group) {
        base.OnStart(manager, overallVolume, group);
        if (lastPlayedStartStingers.ContainsKey(startStinger)) {
            if (Time.timeAsDouble - lastPlayedStartStingers[startStinger] < 5f) {
                source.clip = startStingerRepeated;
            }
            lastPlayedStartStingers[startStinger] = Time.timeAsDouble;
        } else {
            source.clip = startStinger;
            lastPlayedStartStingers.Add(startStinger, Time.timeAsDouble);
        }
        source.loop = false;
        source.volume = overallVolume;
        source.Play();
        waitThenPlayMusic = manager.StartCoroutine(WaitThenPlayMusic());
    }

    public override void OnEnd(MusicManager manager, bool instantly) {
        manager.StopCoroutine(waitThenPlayMusic);
        if (!instantly && source.isPlaying) {
            source.loop = false;
            source.clip = endStinger;
            source.Play();
            Object.Destroy(source, endStinger.length+0.1f);
        } else {
            Object.Destroy(source);
        }
    }

    private IEnumerator WaitThenPlayMusic() {
        yield return new WaitUntil(() => !source.isPlaying && !paused);
        yield return new WaitForSeconds(0.2f);
        source.loop = true;
        source.clip = music;
        source.Play();
    }
}
