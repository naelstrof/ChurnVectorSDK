using System;
using UnityEngine;

namespace PenetrationTech {
    [Serializable]
    public class DickMovementListener : PenetratorListener {
        [SerializeField]
        private DickCum targetCum;

        private float lastDepth = 0f;

        public void SetDickCumTarget(DickCum target) {
            targetCum = target;
        }

        protected override void OnPenetrationDepthChange(float depth) {
            base.OnPenetrationDepthChange(depth);
            if (depth < 0f) return;
            if (Math.Abs(depth - lastDepth) < 0.001f) return;
            targetCum.AddStimulation(depth - lastDepth);
            lastDepth = depth;
        }
    }
}
