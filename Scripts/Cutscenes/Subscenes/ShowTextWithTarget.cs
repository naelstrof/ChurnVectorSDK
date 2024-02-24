using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class ShowTextWithTarget : CameraCut {
        [SerializeField]
        private LocalizedString targetTextLocalized;
        [SerializeField]
        private TMPro.TMP_Text targetDisplay;

        protected override void OnStart() {
            base.OnStart();
            targetDisplay.text = targetTextLocalized.GetLocalizedString();
            var localizedStringEvent = targetDisplay.gameObject.GetComponent<LocalizeStringEvent>();
            if (localizedStringEvent != null) {
                localizedStringEvent.StringReference = targetTextLocalized;
            }
            targetDisplay.maxVisibleCharacters = 0;
        }

        public override void OnEnd() {
            base.OnEnd();
            targetDisplay.maxVisibleCharacters = 0;
            targetDisplay.text = "";
        }

        protected override IEnumerator Update() {
            float startTime = Time.time;
            float textDuration = targetDisplay.text.Length * 0.05f;
            while (Time.time < startTime + textDuration) {
                float t = (Time.time - startTime) / textDuration;
                targetDisplay.maxVisibleCharacters = Mathf.CeilToInt(t * targetDisplay.text.Length);
                yield return null;
            }
            targetDisplay.maxVisibleCharacters = targetDisplay.text.Length+1;
            yield return base.Update();
        }
    }
}
