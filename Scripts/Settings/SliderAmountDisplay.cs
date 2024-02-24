using System.Collections;
using System.Globalization;
using UnityEngine;
using TMPro;

public class SliderAmountDisplay : MonoBehaviour
{
    public CanvasGroup group;
    private WaitForEndOfFrame wait = new();
    public TextMeshProUGUI targetText;
    public IEnumerator FadeOut() {
        float start = Time.realtimeSinceStartup;
        // Wait for 2 seconds in real time.
        while (Time.realtimeSinceStartup < start + 2f) {
            yield return null;
        }
        while (group.alpha != 0) {
            group.alpha = Mathf.MoveTowards(group.alpha, 0f, Time.unscaledDeltaTime);
            yield return wait;
        }
    }
    public void UpdateText(float single) {
        if (single.ToString(CultureInfo.CurrentCulture).Length > 4) {
            targetText.text = single.ToString("0.00");
        } else {
            targetText.text = single.ToString(CultureInfo.CurrentCulture);
        }
        group.alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
}
