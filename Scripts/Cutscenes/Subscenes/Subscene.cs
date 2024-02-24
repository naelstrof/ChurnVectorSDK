using System.Collections;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class Subscene {

        private bool isDone;
        protected virtual void OnStart() {
        }

        public virtual void OnEnd() {
            isDone = true;
        }

        protected virtual IEnumerator Update() {
            yield break;
        }
    
        public IEnumerator Begin() {
            isDone = false;
            OnStart();
            yield return Update();
            OnEnd();
        }

        public bool IsDone() => isDone;
    }
}
