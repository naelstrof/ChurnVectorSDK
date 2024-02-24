using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SneakVolumeHandler : MonoBehaviour {
    private Volume volume;

    private void Awake() {
        volume = GetComponent<Volume>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Pauser.pauseChanged += OnPause;
        OnPause(Pauser.GetPaused());
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameObject.SetActive(LevelManager.IsLevel(scene));
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Pauser.pauseChanged -= OnPause;
    }

    void OnPause(bool paused) {
        gameObject.SetActive(!paused);
    }

    void Update() {
        if (CharacterBase.GetPlayer() == null) {
            volume.weight = 0f;
            return;
        }
        volume.weight = CharacterBase.GetPlayer().GetCrouchAmount();
    }
}
