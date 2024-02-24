using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes.Subscenes {
    [System.Serializable]
    public class ShowDialogueWithTarget : CameraCut {
        [SerializeField] private Dialogue dialogue;
        [SerializeField, SerializeReference, SubclassSelector] private List<DialogueCharacter> characters;
        private const float tweenDuration = 0.6f;
        private Coroutine routine;

        protected override void OnStart() {
            routine = GameManager.StaticStartCoroutine(dialogue.Begin(characters));
            base.OnStart();
        }

        public override void OnEnd() {
            base.OnEnd();
            if (routine != null) {
                GameManager.StaticStopCoroutine(routine);
                dialogue.ForceEnd();
            }
        }

        protected override IEnumerator Update() {
            yield return base.Update();
            yield return new WaitUntil(()=>!dialogue.GetPlaying());
        }
    }
}
