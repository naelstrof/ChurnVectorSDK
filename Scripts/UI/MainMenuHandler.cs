using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    private void Start() {
        Pauser.pauseChanged += OnPauseChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameObject.SetActive(!LevelManager.InLevel());
    }

    private void OnDestroy() {
        Pauser.pauseChanged -= OnPauseChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnPauseChanged(bool paused) {
        if (LevelManager.InLevel()) {
            gameObject.SetActive(paused);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameObject.SetActive(!LevelManager.IsLevel(scene));
    }
}
