using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotHider : MonoBehaviour {
    void Start() {
        if (Application.isEditor && SceneManager.GetActiveScene().name != "MainMenu") {
            GameManager.ShowMenuStatic(GameManager.MainMenuMode.Pause);
        }
    }
    
}
