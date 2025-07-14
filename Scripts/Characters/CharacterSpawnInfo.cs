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

        bool assignedGroups = false;

        if (characterBase.gameObject.TryGetComponent(out ActorOverride overrides))
        {
            bool actionLocked = (overrideLocks.Locks & (1 << 0)) == (1 << 0);
            bool groupsLocked = (overrideLocks.Locks & (1 << 1)) == (1 << 1);

            if (!actionLocked)
                overrides.ApplyActionOverride(inputSource);

            if (!groupsLocked)
            {;
                overrides.ApplyGroupOverrides(characterBase, overrideGroups, overrideUseByGroups);
                assignedGroups = true;
            }
        }

        characterBase.SetInputGenerator(inputSource);
        if(!assignedGroups)
        {
            characterBase.SetGroups(overrideGroups);
            characterBase.SetUseByGroups(overrideUseByGroups);
        }
    }

}
