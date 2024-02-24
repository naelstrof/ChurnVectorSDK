using AI.Events;

namespace AI.Actions {
    [System.Serializable]
    public abstract class Action {
        protected static ActionEventResponseIgnore ignoreResponse = new ActionEventResponseIgnore();
        protected static ActionTransitionContinue continueWork = new ActionTransitionContinue();

        public virtual ActionTransition OnStart(Actor actor) {
            return continueWork;
        }

        public virtual void OnEnd(Actor actor) {
        }

        public virtual void OnSuspend(Actor actor) {
        }

        public virtual ActionTransition OnResume(Actor actor) {
            return continueWork;
        }

        public virtual ActionTransition Update(Actor actor) {
            return continueWork;
        }
        public virtual ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case Shoved shoved:
                    return new ActionEventResponseTransition(new ActionTransitionSuspendFor(new GetShoved(shoved.GetOther()), "oof, got shoved."));
                case GotTased tased:
                    return new ActionEventResponseTransition( new ActionTransitionSuspendFor(new GetTased(tased.GetTasedBy()), "YOW, taser!"));
            }
            return ignoreResponse;
        }

        public ActionTransition RaiseEventAndTransition(Actor actor, Event e, ActionTransition backupTransition) {
            var response = actor.RaiseEvent(e);
            if (response is ActionEventResponseTransition transition) {
                return transition.GetActionTransition();
            } else {
                return backupTransition;
            }
        }
    }
}
