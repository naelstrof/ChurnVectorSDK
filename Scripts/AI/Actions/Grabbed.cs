using AI.Events;
using UnityEngine;
using Event = AI.Events.Event;

namespace AI.Actions {
    public class Grabbed : Action {
        private CharacterBase by;

        public Grabbed(CharacterBase by) {
            this.by = by;
        }

        private class EventGrabbed : Event {
        }
        public override ActionTransition OnStart(Actor actor) {
            actor.RaiseEvent(new EventGrabbed());
            actor.SetWishDirection(Vector3.zero);
            return continueWork;
        }

        public override ActionTransition OnResume(Actor actor) {
            if (!actor.GetGrabbed()) {
                return new ActionTransitionChangeTo(new GetSurprised(by.gameObject, null), "They released me!");
            }
            return continueWork;
        }

        public override ActionTransition Update(Actor actor) {
            if (!actor.GetGrabbed()) {
                return new ActionTransitionChangeTo(new GetSurprised(by.gameObject, null), "They released me!");
            }
            return continueWork;
        }

        public override void OnEnd(Actor actor) {
            base.OnEnd(actor);
            actor.SetWishDirection(Vector3.zero);
            actor.RaiseEvent(new EventGrabbed());
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                // Capture alerts, we're too busy to change states!
                case KnowledgeChanged:
                case Shoved:
                    return new ActionEventResponseTransition(continueWork);
                case DroppedByCharacter:
                    return new ActionEventResponseTransition(new ActionTransitionChangeTo(new GetSurprised(by.gameObject, null), "I've been released. Who grabbed me?"));
            }
            return base.OnReceivedEvent(actor,e);
        }
    }
}
