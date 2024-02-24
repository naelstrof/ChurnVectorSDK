using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class CharacterSpawnInfo {
    [SerializeField] private CivilianReference civilianPrefab;
    [SerializeField,SerializeReference,SubclassSelector] private InputGenerator inputSource;
    [SerializeField] private List<CharacterGroup> overrideGroups;
    [SerializeField] private List<CharacterGroup> overrideUseByGroups;
    private AsyncOperationHandle<Civilian> handle;

    public AsyncOperationHandle<Civilian> GetCharacter() {
        if (handle.IsValid()) {
            return handle;
        }
        //handle = civilianPrefab.LoadAssetAsync<GameObject>();
        handle = civilianPrefab.InstantiateAsync();
        handle.Completed += OnLoadComplete;
        return handle;
    }
    public AsyncOperationHandle<Civilian> GetCharacter(Vector3 position, Quaternion rotation) {
        if (handle.IsValid()) {
            return handle;
        }
        //handle = civilianPrefab.LoadAssetAsync<GameObject>();
        handle = civilianPrefab.InstantiateAsync(position, rotation);
        handle.Completed += OnLoadComplete;
        return handle;
    }

    private void OnLoadComplete(AsyncOperationHandle<Civilian> obj) {
        if (obj.Result is not Civilian characterBase) {
            throw new UnityException("Loaded a non-civilian as a character! Characters must have it as a behavior!");
        }
        characterBase.SetInputGenerator(inputSource);
        characterBase.SetGroups(overrideGroups);
        characterBase.SetUseByGroups(overrideUseByGroups);
    }

}
