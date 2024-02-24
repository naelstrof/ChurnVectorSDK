using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components; using UnityEngine.UI;

public class DialoguePrefab : KeepOnScreen {
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private Image background;
    [SerializeField] private LocalizeStringEvent localizeEvent;
    private float alphaMultiplier;

    protected void Awake() {
        background.color = background.color.With(a:0f);
        targetText.color = targetText.color.With(a:0f);
        targetText.maxVisibleCharacters = 0;
        alphaMultiplier = 1f;
    }

    private void OnEnable() {
        StartCoroutine(CheckObscured());
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        if (targetText.maxVisibleCharacters <= 0) return;
        background.color = background.color.With(a:alpha*alphaMultiplier);
        targetText.color = targetText.color.With(a:alpha*alphaMultiplier);
    }
    private IEnumerator CheckObscured() {
        while (isActiveAndEnabled) {
            yield return null;
            Vector3 diff = OrbitCamera.GetPlayerIntendedPosition() - GetAttachPosition();
            if (diff.magnitude > 30f) {
                alphaMultiplier = 0f;
                yield return new WaitForSeconds(1f);
                continue;
            }

            if (Physics.Raycast(new Ray(GetAttachPosition(), diff.normalized), diff.magnitude,
                    CharacterBase.visibilityMask)) {
                alphaMultiplier = 0.25f;
            } else {
                alphaMultiplier = 1f;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public LocalizeStringEvent GetLocalizeStringEvent() => localizeEvent;
    public void SetText(string text) {
        targetText.text = text;
    }
    public void SetMaxVisibleCharacters(int count) {
        if (background == null) {
            return;
        }

        background.color = background.color.With(a: count == 0 ? 0f : alpha*alphaMultiplier);
        targetText.color = targetText.color.With(a: count == 0 ? 0f : alpha*alphaMultiplier);
        targetText.maxVisibleCharacters = count;
    }
}
