using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuickRestartButton : MonoBehaviour {
    void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameObject.SetActive(LevelManager.InLevel());
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameObject.SetActive(LevelManager.IsLevel(scene));
    }

    void OnClick() {
        Assert.IsTrue(LevelManager.InLevel());
        LevelManager.QuickRestart();
    }
}
