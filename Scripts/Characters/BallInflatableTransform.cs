using JigglePhysics;
using UnityEngine;

namespace Naelstrof.Inflatable {
    [System.Serializable]
    public class BallInflatableTransform : InflatableListener {
        [SerializeField] private Transform ballsRoot;
        [SerializeField] JiggleRigBuilder targetJiggleRig;
        private Vector3 startScale;
        private JiggleSettingsBlend ballBlend;

        public override void OnEnable() {
            startScale = ballsRoot.localScale;
            var rig = targetJiggleRig.GetJiggleRig(ballsRoot);
            if (rig is { jiggleSettings: JiggleSettingsBlend blend }) {
                ballBlend = Object.Instantiate(blend);
            } else {
                Debug.LogWarning("Failed to find blend for jiggle rig bone " + ballsRoot, targetJiggleRig.gameObject);
                ballBlend = null;
            }
            rig.jiggleSettings = ballBlend;
        }
        public override void OnSizeChanged(float newSize) {
            ballsRoot.localScale = startScale*Mathf.Max(newSize*0.5f+0.5f,1f);
            if (ballBlend != null) {
                ballBlend.normalizedBlend = Mathf.Clamp01((newSize - 1f) / 2f);
            }
        }
    }
}
