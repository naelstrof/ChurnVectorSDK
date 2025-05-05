using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class CharacterSpawnInfo {
    [SerializeField] private CivilianReference civilianPrefab;
    [SerializeField,SerializeReference,SubclassSelector] private InputGenerator inputSource;
    [SerializeField] private List<CharacterGroup> overrideGroups;
    [SerializeField] private List<CharacterGroup> overrideUseByGroups;
    [SerializeField] private OverrideLocks overrideLocks;
    private AsyncOperationHandle<Civilian> handle;
    
    public AsyncOperationHandle<Civilian> GetCharacter() {
        return GetCharacter(Vector3.zero, Quaternion.identity);
    }
    public AsyncOperationHandle<Civilian> GetCharacter(Vector3 position, Quaternion rotation) {
        if (handle.IsValid()) {
            return handle;
        }

        CivilianReference realReference = civilianPrefab;
        foreach (var mod in Modding.GetMods()) {
            foreach (var replacement in mod.GetDescription().GetReplacementCharacters()) {
                if (replacement.existingGUID == realReference.AssetGUID) {
                    realReference = new CivilianReference(replacement.replacementGUID);
                }
            } 
        }
        handle = realReference.InstantiateAsync(position, rotation);
        handle.Completed += OnLoadComplete;
        return handle;
    }

    private void OnLoadComplete(AsyncOperationHandle<Civilian> obj) {
        if (obj.Result is not Civilian characterBase) {
            throw new UnityException("Loaded a non-civilian as a character! Characters must have it as a behavior!");
        }

        if (characterBase.gameObject.TryGetComponent(out ActorOverride overrides))
        {
            if ((overrideLocks.Locks & (1 << 0)) != 1)
                overrides.ApplyActionOverride(inputSource);

            if ((overrideLocks.Locks & (1 << 1)) != 1)
                overrides.ApplyGroupOverrides(characterBase, overrideGroups, overrideUseByGroups);
        }

        characterBase.SetInputGenerator(inputSource);
        characterBase.SetGroups(overrideGroups);
        characterBase.SetUseByGroups(overrideUseByGroups);
    }

}
