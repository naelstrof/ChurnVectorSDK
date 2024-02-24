using UnityEngine;

namespace AI.Events {
    public class AnimationBool : Event {
        private string boolName;
        private bool value;
        public AnimationBool(string name, bool value) {
            boolName = name;
            this.value = value;
        }

        public void ApplyToAnimator(Animator animator) {
            animator.SetBool(boolName, value);
        }
        public string GetBoolName() => boolName;
        public bool GetBoolValue() => value;
    }
}