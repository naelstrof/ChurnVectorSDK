using System.Collections;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class FadeInBackground : Subscene {
        [SerializeField]
        private float duration;
        [SerializeField]
        private CanvasGroup group;

        protected override IEnumerator Update() {
            float startTime = Time.time;
            while (Time.time - startTime < duration) {
                float t = (Time.time - startTime) / duration;
                group.alpha = Mathf.Max(group.alpha,t);
                yield return null;
            }
        }

        public override void OnEnd() {
            group.alpha = 1f;
            base.OnEnd();
        }
    }
}