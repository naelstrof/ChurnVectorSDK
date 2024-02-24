using UnityEngine.Localization;

[System.Serializable]
public class DialogueLine {
    public enum Actor {
        First,
        Second,
        Third
    }
    public Actor actor;
    public LocalizedString line;
}
