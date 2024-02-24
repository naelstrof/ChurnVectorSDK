using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour {
    [SerializeField] private GameObject levelDisplayPrefab;
    [SerializeField] private MapInspectPanel mapInspector;
    IEnumerator Start() {
        yield return new WaitUntil(() => !LevelManager.IsLoading());
        foreach (var level in LevelManager.GetLevels()) {
            var obj = Instantiate(levelDisplayPrefab, transform);
            MapButton button = obj.GetComponent<MapButton>();
            
            button.SetLevel(level);
            button.SetMapInspectPanel(mapInspector);
        }
    }
}
