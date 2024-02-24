using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueLibrary : MonoBehaviour {
    private static DialogueLibrary instance;
    
    [SerializeField] private DialogueTheme condomTheme;
    [SerializeField] private DialogueGroup[] groups;

    public enum DialogueGroupType {
        FoundCondom,
        ChurnFinished,
        CondomTalk,
        Vore,
        Recombobulate,
        VoreAfterRecombob,
        CivExclaim,
        Investigate,
        PartnerSex,
        PartnerSexFinished,
    }
    

    [Serializable]
    private class DialogueGroup {
        [SerializeField] public DialogueGroupType type;
        [SerializeField] public Dialogue[] dialogues;
    }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static DialogueTheme GetCondomTheme() => instance.condomTheme;
    public static Dialogue GetDialogue(DialogueGroupType type) {
        foreach (var group in instance.groups) {
            if (group.type != type) continue;
            return group.dialogues[Random.Range(0, group.dialogues.Length)];
        }
        return null;
    }
}
