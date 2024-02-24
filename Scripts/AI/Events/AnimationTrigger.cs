using UnityEngine;

namespace AI.Events {
    public class AnimationTrigger : Event {
        private string triggerName;
        public AnimationTrigger(string name) {
            triggerName = name;
        }

        public void ApplyToAnimator(Animator animator) {
            animator.SetTrigger(triggerName);
        }

        public string GetTriggerName() => triggerName;
    }
}