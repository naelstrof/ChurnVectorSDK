using UnityEngine;

public class VersionNumberText : MonoBehaviour {
    void Start() {
        GetComponent<TMPro.TMP_Text>().text = Application.version;
    }
}
