using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class BriefingState : MusicalState {
    [SerializeField] private AudioClip endStinger;
    private Coroutine waitThenPlayStinger;
    private bool playedStinger = false;
    public override void OnStart(MusicManager manager, float overallVolume, AudioMixerGroup group) {
        base.OnStart(manager, overallVolume, group);
        source.clip = music;
        source.loop = false;
        source.volume = overallVolume;
        source.Play();
        waitThenPlayStinger = manager.StartCoroutine(WaitThenPlayStinger());
        playedStinger = false;
    }

    public override void OnEnd(MusicManager manager, bool instantly) {
        manager.StopCoroutine(waitThenPlayStinger);
        if (!playedStinger && !instantly) {
            source.loop = false;
            source.clip = endStinger;
            source.Play();
            playedStinger = true;
            Object.Destroy(source, endStinger.length+0.1f);
        } else {
            Object.Destroy(source);
        }
    }

    private IEnumerator WaitThenPlayStinger() {
        // Gotta wait one frame for somereason.
        yield return null;
        yield return new WaitUntil(() => (!source.isPlaying && !paused) || playedStinger);
        if (playedStinger) yield break;
        source.loop = false;
        source.clip = endStinger;
        source.Play();
    }
}
