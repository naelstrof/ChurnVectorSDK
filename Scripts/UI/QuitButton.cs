using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScriptableSettings;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour {
    [SerializeField] private AssetReferenceScene mainMenu;
    [SerializeField] private LocalizedString quitString;
    [SerializeField] private LocalizedString returnToMainMenuString;
    [SerializeField] private TMPro.TMP_Text quitDisplay;
    private void OnEnable() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1) {
        if (LevelManager.IsLevel(scene)) {
            quitDisplay.text = returnToMainMenuString.GetLocalizedString();
            var lse = quitDisplay.gameObject.GetComponent<LocalizeStringEvent>();
            if (lse != null) {
                lse.StringReference = returnToMainMenuString;
            }
        } else {
            quitDisplay.text = quitString.GetLocalizedString();
            var lse = quitDisplay.gameObject.GetComponent<LocalizeStringEvent>();
            if (lse != null) {
                lse.StringReference = quitString;
            }
        }
    }

    private void OnDisable() {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }
    void OnClick() {
        if (SceneManager.GetActiveScene().name != mainMenu.GetName()) {
            SceneLoader.LoadScene(mainMenu);
        } else {
            SettingsManager.Save();
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void OnValidate() {
#if UNITY_EDITOR
        mainMenu.OnValidate();
#endif
    }
}
