using UnityEngine;

public class LoadDisplay : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Image loadDisplay;
    public void SetDisplay(bool on) {
        loadDisplay.color = loadDisplay.color.With(a:on?1f:0f);
    }
}
