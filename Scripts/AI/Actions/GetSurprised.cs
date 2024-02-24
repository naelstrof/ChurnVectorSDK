using AI.Events;
using UnityEngine;

namespace AI.Actions {
    public class GetSurprised : Action {
        private GameObject by;
        private Action thenWhat;

        public GetSurprised(GameObject by, Action thenWhat) {
            this.by = by;
            this.thenWhat = thenWhat;
        }

        public class Surprised : AnimationTrigger {
            public Surprised(string name) : base(name) {
            }
        }

        public override ActionTransition OnStart(Actor actor) {
            actor.RaiseEvent(new Surprised("Surprise"));
            return new ActionTransitionSuspendFor(new ActionTurnToFaceObject(by, 1.8f), "I am looking!");
        }

        public override ActionTransition OnResume(Actor actor) {
            if (thenWhat != null) {
                return new ActionTransitionChangeTo(thenWhat, "going to do " + thenWhat);
            }
            return new ActionTransitionDone("Done being surprised.");
        }

        public override ActionTransition Update(Actor actor) {
            if (thenWhat != null) {
                return new ActionTransitionChangeTo(thenWhat, "going to do " + thenWhat);
            }
            return new ActionTransitionDone("Done being surprised.");
        }
    }
}
