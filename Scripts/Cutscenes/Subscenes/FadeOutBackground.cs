using System.Collections;
using UnityEngine;

namespace Cutscenes.Subscenes {
	[System.Serializable]
    public class FadeOutBackground : Subscene {
        [SerializeField]
        private float duration;
        [SerializeField]
        private CanvasGroup group;

        protected override IEnumerator Update() {
            float startTime = Time.time;
            while (Time.time - startTime < duration) {
                float t = (Time.time - startTime) / duration;
                group.alpha = Mathf.Min(group.alpha,1f-t);
                yield return null;
            }
        }

        public override void OnEnd() {
            group.alpha = 0f;
            base.OnEnd();
        }
    }
}