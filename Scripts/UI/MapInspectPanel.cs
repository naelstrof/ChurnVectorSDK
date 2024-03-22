using System;
using System.Text;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class MapInspectPanel : MonoBehaviour {
    [SerializeField] private TMPro.TMP_Text mapNameDisplay;
    [SerializeField] private TMPro.TMP_Text mapDescriptionDisplay;
    [SerializeField] private TMPro.TMP_Text completionDisplay;
    [SerializeField] private LocalizedString completionTime;
    [SerializeField] private LocalizedString completedObjectives;
    [SerializeField] private GameObject hideObject;
    [SerializeField] private Button playButton;
    [SerializeField] private Image mapImage;
    private Level currentLevel;

    public void OnEnable() {
        if (currentLevel != null) {
            SetLevel(currentLevel);
        }
    }

    public void UnsetLevel() {
        currentLevel = null;
    }

    public void SetLevel(Level level) {
        currentLevel = level;
        hideObject.SetActive(false);
        gameObject.SetActive(true);
        mapImage.sprite = level.GetLevelPreview();
        mapNameDisplay.text = level.GetLevelName();
        mapDescriptionDisplay.text = level.GetLevelDescription();
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => {
            LevelManager.StartLevel(currentLevel, true);
            GameManager.ShowMenuStatic(GameManager.MainMenuMode.Pause);
        });

        level.GetCompletionStatus(out var completed, out var objectives, out var completionTimeAsDouble);
        var objectiveListBuilder = new StringBuilder();
        if (!completed) {
            objectiveListBuilder.Append($"{completionTime.GetLocalizedString()}: -\n");
        } else {
            objectiveListBuilder.Append($"{completionTime.GetLocalizedString()}: {TimeSpan.FromSeconds(completionTimeAsDouble)}\n");
        }
        foreach(var obj in objectives) {
            objectiveListBuilder.Append("• ");
            objectiveListBuilder.Append(obj.name.GetLocalizedString());
            objectiveListBuilder.Append(": ");
            switch (obj.status) {
                case Objective.ObjectiveStatus.Failed: objectiveListBuilder.Append("<color=red>X</color>"); break;
                case Objective.ObjectiveStatus.Incomplete: objectiveListBuilder.Append("<color=grey>~</color>"); break;
                case Objective.ObjectiveStatus.Completed: objectiveListBuilder.Append("<color=green>✓</color>"); break;
            }
            objectiveListBuilder.Append("\n");
        }
        if (objectives.Count == 0) {
            objectiveListBuilder.Append($"{completedObjectives.GetLocalizedString()}: ???");
        }
        completionDisplay.text = objectiveListBuilder.ToString();
    }

    public Level GetLevel() {
        return currentLevel;
    }
}
