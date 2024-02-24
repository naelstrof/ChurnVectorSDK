using UnityEngine;

[System.Serializable]
public class DialogueCharacterInanimateObject : DialogueCharacter {
    [SerializeField] private Transform target;
    [SerializeField] private DialogueTheme theme;

    public static DialogueCharacterInanimateObject Get(Transform obj, DialogueTheme theme) {
        DialogueCharacterInanimateObject instance = new DialogueCharacterInanimateObject {
            target = obj,
            theme = theme,
        };
        return instance;
    }

    public override Transform GetTransform() {
        return target;
    }

    public override DialogueTheme GetDialogueTheme() {
        return theme;
    }
}
