using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableOnPause : MonoBehaviour {
    void Awake() {
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
}
