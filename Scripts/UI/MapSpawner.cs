using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour {
    [SerializeField] private GameObject levelDisplayPrefab;
    [SerializeField] private MapInspectPanel mapInspector;
    private List<MapButton> buttons;
    private LevelCategory currentCategory;

    public void SetCategory(LevelCategory category) {
        currentCategory = category;
    }

    private void OnEnable() {
        StartCoroutine(SpawnPrefabs());
    }

    private void OnDisable() {
        foreach (var button in buttons) {
            Destroy(button.gameObject);
        }
        buttons = null;
    }

    IEnumerator SpawnPrefabs() {
        buttons ??= new List<MapButton>();
        yield return new WaitUntil(() => !LevelManager.IsLoading());
        foreach (var level in LevelManager.GetLevels()) {
            if (!level.IsPartOfCategory(currentCategory)) {
                continue;
            }
            var obj = Instantiate(levelDisplayPrefab, transform);
            MapButton button = obj.GetComponent<MapButton>();
            
            button.SetLevel(level);
            button.SetMapInspectPanel(mapInspector);
            buttons.Add(button);
        }
    }
}
