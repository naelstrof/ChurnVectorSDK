using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapLevelSelect : MonoBehaviour {
    [SerializeField]
    private MapButton mapButtonPrefab;
    [SerializeField]
    private UnityEvent onSelectMap;
    private List<MapButton> mapButtons;
    private void OnEnable() {
        mapButtons = new List<MapButton>();
        foreach (var level in LevelManager.GetLevels()) {
            var mapButtonObj = Instantiate(mapButtonPrefab.gameObject, transform);
            var mapButton = mapButtonObj.GetComponent<MapButton>();
            mapButton.SetLevel(level);
            mapButton.GetComponent<Button>().onClick.AddListener(() => onSelectMap.Invoke());
            mapButtons.Add(mapButton);
        }
    }

    private void OnDisable() {
        foreach (var button in mapButtons) {
            Destroy(button.gameObject);
        }

        mapButtons.Clear();
    }
}
