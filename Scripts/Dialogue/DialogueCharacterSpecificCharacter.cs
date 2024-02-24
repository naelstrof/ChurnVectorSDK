using UnityEngine;

[System.Serializable]
public class DialogueCharacterSpecificCharacter : DialogueCharacter {
    [SerializeField] private CharacterBase character;
    [SerializeField] private DialogueTheme overrideTheme;
    
    // Cannot have a constructor with SerializedReference, whoops.
    public static DialogueCharacterSpecificCharacter Get(CharacterBase target) {
        var dialogueCharacterSpecificCharacter = new DialogueCharacterSpecificCharacter();
        dialogueCharacterSpecificCharacter.character = target;
        return dialogueCharacterSpecificCharacter;
    }

    public override Transform GetTransform() {
        return character.GetLimb(HumanBodyBones.Head).transform;
    }

    public override DialogueTheme GetDialogueTheme() {
        if (overrideTheme == null) {
            return character.GetDialogueTheme();
        }
        return overrideTheme;
    }
}
