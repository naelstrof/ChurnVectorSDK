using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class CameraCut : Subscene {
        [SerializeField]
        private float duration;
        [SerializeField, SerializeReference, SubclassSelector]
        private OrbitCameraConfiguration configuration;
        private const float tweenDuration = 0.6f;
        protected override void OnStart() {
            base.OnStart();
            if (configuration == null) {
                return;
            }
            OrbitCamera.AddConfiguration(configuration, tweenDuration);
        }

        public override void OnEnd() {
            base.OnEnd();
            OrbitCamera.RemoveConfiguration(configuration, tweenDuration);
        }

        protected override IEnumerator Update() {
            if (configuration != null) {
                yield return new WaitForSeconds(tweenDuration);
            }

            yield return new WaitForSeconds(duration);
        }
    }
}
