using System.Collections;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class TriggerGameEvent : Subscene {
        [SerializeField]
        private GameEvent gameEvent;

        protected override IEnumerator Update() {
            yield return null;
        }

        public override void OnEnd() {
            base.OnEnd();
            gameEvent.Raise();
        }
    }
}