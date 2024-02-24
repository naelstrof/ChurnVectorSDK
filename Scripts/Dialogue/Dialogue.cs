using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Data/Dialogue", order = 8)]
public class Dialogue : ScriptableObject {
    [SerializeField] private List<DialogueLine> lines;
    private List<DialoguePrefab> dialoguePrefabs;
    private bool playing = false;
    public bool GetPlaying() => playing;

    public void ForceEnd() {
        playing = false;
        foreach (var dialoguePrefab in dialoguePrefabs) {
            if (dialoguePrefab != null) {
                Destroy(dialoguePrefab.gameObject);
            }
        }
        dialoguePrefabs.Clear();
    }

    public IEnumerator Begin(IList<DialogueCharacter> characters) {
        try {
            playing = true;
            dialoguePrefabs = new();
            WaitForSeconds interval = new WaitForSeconds(0.1f);
            foreach (var line in lines) {
                if ((int)line.actor > characters.Count) {
                    continue;
                }
                var character = characters[(int)line.actor];
                var obj = Instantiate(character.GetDialogueTheme().GetDialoguePrefab().gameObject);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                var prefab = obj.GetComponent<DialoguePrefab>();
                prefab.AttachTo(character.GetTransform());
                dialoguePrefabs.Add(prefab);
                
                var handle = line.line.GetLocalizedStringAsync();
                yield return handle;
                string lineString = handle.Result;
                prefab.GetLocalizeStringEvent().StringReference = line.line;
                prefab.SetText(lineString);
                prefab.SetMaxVisibleCharacters(0);

                float startTime = Time.time;
                float duration = lineString.Length * 0.04f;
                while (Time.time < startTime + duration) {
                    float t = (Time.time - startTime) / duration;
                    int visibleCharacters = Mathf.CeilToInt(t * lineString.Length);
                    prefab.SetMaxVisibleCharacters(visibleCharacters);
                    AudioPack.PlayClipAtPoint(character.GetDialogueTheme().GetTalkPack(), prefab.GetAttachPosition());
                    yield return interval;
                }
                prefab.SetMaxVisibleCharacters(lineString.Length+1);
                yield return new WaitForSeconds(1.4f);
                prefab.SetMaxVisibleCharacters(0);
                Destroy(prefab);
                dialoguePrefabs.Remove(prefab);
            }
        } finally {
            playing = false;
            foreach (var dialoguePrefab in dialoguePrefabs) {
                Destroy(dialoguePrefab.gameObject);
            }

            dialoguePrefabs.Clear();
        }
    }
}
