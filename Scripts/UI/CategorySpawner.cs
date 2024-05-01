using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class CategorySpawner : MonoBehaviour {
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private MapSpawner mapLevelSelect;
    
    [SerializeField] private GameObject selfPanel;
    [SerializeField] private GameObject activatePanel;
    [SerializeField] private TMP_Text errorText;
    private AsyncOperationHandle<IList<LevelCategory>>? handle;
    private List<MapCategoryPanel> spawnedPrefabs;
    private void OnEnable() {
        StartCoroutine(SpawnRoutine());
    }

    private void OnDisable() {
        if (spawnedPrefabs == null) {
            return;
        }

        foreach (var obj in spawnedPrefabs) {
            Destroy(obj.gameObject);
        }

        spawnedPrefabs = null;
    }

    private IEnumerator SpawnRoutine() {
        yield return new WaitUntil(() => !Modding.IsLoading());
        List<string> keys = new List<string> {"ChurnVectorCategory"};
        handle ??= Addressables.LoadAssetsAsync<LevelCategory>(keys, (level) => { }, Addressables.MergeMode.Union, false);
        var loadHandle = handle.Value;
        yield return new WaitUntil(() => loadHandle.IsDone);
        foreach (var category in loadHandle.Result) {
            errorText.gameObject.SetActive(false);
            OnFoundCategory(category);
        }
    }

    private bool HasLevels(LevelCategory category) {
        foreach (var level in LevelManager.GetLevels()) {
            if (level.IsPartOfCategory(category)) {
                return true;
            }
        }
        return false;
    }

    private void OnFoundCategory(LevelCategory category) {
        if (!HasLevels(category)) {
            return;
        }
        spawnedPrefabs ??= new List<MapCategoryPanel>();
        var categoryObj = Instantiate(categoryPrefab, transform);
        MapCategoryPanel panel = categoryObj.GetComponent<MapCategoryPanel>();
        panel.SetCategory(category, mapLevelSelect);
        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy) {
            panel.GetComponent<Button>().Select();
        }

        panel.GetComponent<Button>().onClick.AddListener(() => {
            selfPanel.SetActive(false);
            activatePanel.SetActive(true);
        });
        spawnedPrefabs.Add(panel);
    }
}
