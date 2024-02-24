using AI;
using AI.Actions;
using AI.Events;
using Event = AI.Events.Event;

namespace ActorActions {
    public class ActionWaitToBeFilledWithCum : Action {
        public override void OnEnd(Actor actor) {
        }

        public override void OnSuspend(Actor actor) {
        }

        public override ActionTransition OnResume(Actor actor) {
            return continueWork;
        }

        public override ActionTransition Update(Actor actor) {
            return continueWork;
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case FilledWithCum:
                    return new ActionEventResponseTransition( new ActionTransitionChangeTo(new ActionBeACivilian(), "Ahh, full of cum, yay!"));
            }
            return base.OnReceivedEvent(actor,e);
        }
    }
}
