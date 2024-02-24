using UnityEngine;

[System.Serializable]
public class DialogueCharacterPlayer : DialogueCharacter {
    [SerializeField] private DialogueTheme themeOverride;
    public override Transform GetTransform() {
        return CharacterBase.GetPlayer().GetLimb(HumanBodyBones.Head).transform;
    }

    public override DialogueTheme GetDialogueTheme() {
        if (themeOverride == null) {
            return CharacterBase.GetPlayer().GetDialogueTheme();
        }
        return themeOverride;
    }
}
