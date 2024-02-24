using UnityEngine;

public class AnimatorHelper : MonoBehaviour {
    void PlayAudioPack(AudioPack pack) {
        AudioPack.PlayClipAtPoint(pack, transform.position);
    }
}
