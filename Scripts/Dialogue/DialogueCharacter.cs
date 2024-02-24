using UnityEngine;
[System.Serializable]
public abstract class DialogueCharacter {
    public abstract Transform GetTransform();
    public abstract DialogueTheme GetDialogueTheme();
}
