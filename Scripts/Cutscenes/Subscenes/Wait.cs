using System.Collections;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class Wait : Subscene {
        [SerializeField]
        private float duration;
        protected override IEnumerator Update() {
            float startTime = Time.unscaledTime;
            while (Time.unscaledTime - startTime < duration) {
                yield return null;
            }
        }
    }
}