using System.Collections.Generic;
using UnityEngine;

public class ChurnHandler : MonoBehaviour {
    [SerializeField]
    private ChurnPanel churnPanelPrefab;

    private Dictionary<CumStorage.CumSource, ChurnPanel> spawnedPrefabs;
    private void Awake() {
        spawnedPrefabs = new Dictionary<CumStorage.CumSource, ChurnPanel>();
    }

    private void OnEnable() {
        CharacterBase.playerEnabled += OnPlayerEnable;
        CharacterBase.playerDisabled += OnPlayerDisable;
    }

    private void OnPlayerDisable(CharacterBase player) {
        player.GetStorage().startChurn -= OnStartChurn;
        player.GetStorage().drained -= OnDrained;
        foreach (var pair in spawnedPrefabs) {
            Destroy(pair.Value.gameObject);
        }
        spawnedPrefabs.Clear();
    }

    private void OnPlayerEnable(CharacterBase player) {
        player.GetStorage().startChurn += OnStartChurn;
        player.GetStorage().drained += OnDrained;
        foreach (var cumSource in player.GetStorage().GetSources()) {
            var churnPanelGameObject = Instantiate(churnPanelPrefab.gameObject, transform);
            var churnPanel = churnPanelGameObject.GetComponent<ChurnPanel>();
            churnPanel.Initialize(cumSource);
            spawnedPrefabs.Add(cumSource, churnPanel);
        }
    }

    private void OnDisable() {
        CharacterBase.playerEnabled -= OnPlayerEnable;
        CharacterBase.playerDisabled -= OnPlayerDisable;
    }

    private void OnStartChurn(CumStorage.CumSource churnable) {
        var churnPanelGameObject = Instantiate(churnPanelPrefab.gameObject, transform);
        var churnPanel = churnPanelGameObject.GetComponent<ChurnPanel>();
        churnPanel.Initialize(churnable);
        spawnedPrefabs.Add(churnable, churnPanel);
    }

    private void OnDrained(CumStorage.CumSource churnable) {
        Destroy(spawnedPrefabs[churnable].gameObject);
        spawnedPrefabs.Remove(churnable);
    }
    
}
