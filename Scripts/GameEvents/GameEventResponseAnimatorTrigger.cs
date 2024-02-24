using System;
using UnityEngine;

[System.Serializable]
public class GameEventResponseAnimatorTrigger : GameEventResponse {
    
    [Serializable]
    private class AnimatorTriggerTarget {
        [SerializeField]
        public Animator animator;
        [SerializeField]
        public string triggerName;
    }

    [SerializeField]
    private AnimatorTriggerTarget[] targets;

    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        foreach (var target in targets) {
            target.animator.SetTrigger(target.triggerName);
        }
    }
}
