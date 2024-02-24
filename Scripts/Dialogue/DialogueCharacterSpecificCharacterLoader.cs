using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class DialogueCharacterSpecificCharacterLoader : DialogueCharacter {
    [SerializeField] private CharacterLoader characterLoader;
    [SerializeField] private DialogueTheme overrideTheme;
    
    public override Transform GetTransform() {
        var handle = characterLoader.GetCharacterAsync();
        if (handle is { IsDone: true, Status: AsyncOperationStatus.Succeeded }) {
            return handle.Result.GetLimb(HumanBodyBones.Head).transform;
        }
        return null;
    }

    public override DialogueTheme GetDialogueTheme() {
        if (overrideTheme == null) {
            var handle = characterLoader.GetCharacterAsync();
            if (handle is { IsDone: true, Status: AsyncOperationStatus.Succeeded }) {
                return handle.Result.GetDialogueTheme();
            }
            return null;
        }
        return overrideTheme;
    }
}
