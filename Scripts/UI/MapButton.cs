using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
    [SerializeField] private Level targetLevel;
    [SerializeField] private Image levelPreview;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text levelDescription;
    [SerializeField] private MapInspectPanel mapInspectPanel;
    [SerializeField] private GameObject lockGraphics;
    
    private void OnEnable() {
        if (targetLevel != null) {
            SetLevel(targetLevel);
        }
    }

    public void SetMapInspectPanel(MapInspectPanel panel) {
        mapInspectPanel = panel;
    }

    public void SetLevel(Level target) {
        levelPreview.sprite = target.GetLevelPreview();
        var index = LevelManager.GetIndexOf(target);
        levelName.text = $"{(index+1).ToString()}: {target.GetLevelName()}";

        var playable = LevelManager.CanPlayLevel(target);
        GetComponent<Button>().interactable = playable;
        lockGraphics.SetActive(!playable);
        var localizeStringEventName = levelName.gameObject.GetComponent<LocalizeStringEvent>();
        if (localizeStringEventName != null) {
            localizeStringEventName.StringReference = target.GetLocalizedLevelName();
        }

        levelDescription.text = target.GetLevelDescription();
        var localizeStringEventDescription = levelName.gameObject.GetComponent<LocalizeStringEvent>();
        if (localizeStringEventDescription != null) {
            localizeStringEventDescription.StringReference = target.GetLocalizedLevelDescription();
        }
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => {
            if (mapInspectPanel.GetLevel() == target) {
                LevelManager.StartLevel(target, true);
                GameManager.ShowMenuStatic(GameManager.MainMenuMode.Pause);
            } else {
                mapInspectPanel.SetLevel(target);
            }
        });
    }
}
