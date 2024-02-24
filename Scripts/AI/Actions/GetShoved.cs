using AI.Events;
using UnityEngine;

namespace AI.Actions {
    public class GetShoved : Action {
        private CharacterBase by;
        private float startTime;
        public GetShoved(CharacterBase by) {
            this.by = by;
        }
        private class Shoved : AnimationTrigger {
            public Shoved(string name) : base(name) {
            }
        }

        public override ActionTransition OnResume(Actor actor) {
            if (Time.time - startTime >= 2.5f) {
                return new ActionTransitionDone("Whatever, back to what I was doing.");
            }

            var knowledge = actor.GetKnowledgeOf(CharacterBase.GetPlayer().gameObject);
            switch (knowledge.GetKnowledgeLevel()) {
                case KnowledgeDatabase.KnowledgeLevel.Ignorant:
                case KnowledgeDatabase.KnowledgeLevel.Investigative:
                    return new ActionTransitionSuspendFor(new ActionTurnToFaceObject(by.gameObject, 2f), "Who the heck shoved me?");
                case KnowledgeDatabase.KnowledgeLevel.Alert: break;
            }

            return new ActionTransitionDone("Done being shoved!");
        }

        public override ActionTransition OnStart(Actor actor) {
            startTime = Time.time;
            actor.RaiseEvent(new Shoved("Shoved"));
            return new ActionTransitionSuspendFor(new DoNothing(1.5f), "OOF");
        }
    }
}
