using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour {
    [SerializeField] private TMPro.TMP_Text textDisplay;
    [SerializeField] private LocalizedString newGameString;
    [SerializeField] private int saveSlot;
    void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            SaveManager.SelectSaveSlot(saveSlot);
            GameManager.ShowMenuStatic(GameManager.MainMenuMode.Pause);
        });
        var data = SaveManager.GetData(saveSlot);
        if (!data.HasKey("levels")) {
            textDisplay.text = newGameString.GetLocalizedString();
            return;
        }

        int completedLevels = 0;
        int levelCount = 0;
        foreach (var level in data["levels"]) {
            levelCount++;
            if (level.Value["completed"].AsBool) {
                completedLevels++;
            }
        }

        float completionPercent = (float)completedLevels / levelCount;
        textDisplay.text = $"{completionPercent:P02}";
    }
}
