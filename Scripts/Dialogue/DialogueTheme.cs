using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Theme", menuName = "Data/Dialogue Theme", order = 8)]
public class DialogueTheme : ScriptableObject {
    [SerializeField] private DialoguePrefab dialoguePrefab;
    [SerializeField] private AudioPack talkPack;
    public DialoguePrefab GetDialoguePrefab() => dialoguePrefab;
    public AudioPack GetTalkPack() => talkPack;
}
