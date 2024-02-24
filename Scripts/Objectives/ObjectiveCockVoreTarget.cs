using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class ObjectiveCockVoreTarget : Objective {
    [SerializeField]
    private CharacterLoader targetLoader;
    private Civilian target;

    private ObjectiveStatus status;
    
    public override void OnRegister() {
        var handle = targetLoader.GetCharacterAsync();
        if (handle.IsDone) {
            OnCompletedLoadCharacter(handle);
        } else {
            handle.Completed += OnCompletedLoadCharacter;
        }
    }

    private void OnCompletedLoadCharacter(AsyncOperationHandle<Civilian> obj) {
        target = obj.Result;
        target.endCockVoreAsPrey += OnCockVore;
    }

    public override void OnUnregister() {
        if (target == null) {
            return;
        }
        target.endCockVoreAsPrey -= OnCockVore;
    }

    private void OnCockVore(CharacterBase other) {
        status = ObjectiveStatus.Completed;
        Complete();
    }

    public override ObjectiveStatus GetStatus() => status;
}
