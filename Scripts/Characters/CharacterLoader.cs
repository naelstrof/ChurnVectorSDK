using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[SelectionBase]
public class CharacterLoader : InitializationManagerInitialized {
    [SerializeField] private CharacterSpawnInfo characterSpawnInfo;

    public AsyncOperationHandle<Civilian> GetCharacterAsync() {
        Quaternion desiredRotation = QuaternionExtensions.LookRotationUpPriority(transform.forward, Vector3.up);
        return characterSpawnInfo.GetCharacter(transform.position, desiredRotation);
    }

    private void OnCompletedSpawnCharacter(AsyncOperationHandle<Civilian> obj) {
        var characterBase = obj.Result;

        characterBase.GetBody().position = transform.position;
        characterBase.transform.position = transform.position;
        Quaternion desiredRotation = QuaternionExtensions.LookRotationUpPriority(transform.forward, Vector3.up);
        characterBase.SetFacingDirection(desiredRotation);
        characterBase.transform.rotation = desiredRotation;
        characterBase.GetBody().rotation = desiredRotation;
        characterBase.GetBody().velocity = Vector3.zero;
        
        gameObject.SetActive(false);
    }

    public override InitializationManager.InitializationStage GetInitializationStage() {
        return InitializationManager.InitializationStage.AfterMods;
    }

    public override void OnInitialized(DoneInitializingAction doneInitializingAction) {
        Quaternion desiredRotation = QuaternionExtensions.LookRotationUpPriority(transform.forward, Vector3.up);
        var handle = characterSpawnInfo.GetCharacter(transform.position, desiredRotation);
        handle.Completed += OnCompletedSpawnCharacter;
        handle.Completed += (obj) => doneInitializingAction?.Invoke(this);
    }
}
