using System.Collections.Generic;
using UnityEngine;

public class AudioPackLibrary : MonoBehaviour {
    private static AudioPackLibrary instance;
    [SerializeField]
    private List<AudioPack> packs;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static AudioPack GetAudioPackByName(string name) {
        foreach (var pack in instance.packs) {
            if (pack.name == name) {
                return pack;
            }
        }
        return null;
    }
}
