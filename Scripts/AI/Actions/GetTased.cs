using AI.Events;
using Event = AI.Events.Event;

namespace AI.Actions {
    public class GetTased : Action {
        private CharacterBase by;

        public GetTased(CharacterBase by) {
            this.by = by;
        }

        private class EventTased : Event {
        }
        public override ActionTransition OnStart(Actor actor) {
            actor.RaiseEvent(new EventTased());
            return continueWork;
        }

        public override ActionTransition Update(Actor actor) {
            if (actor.GetTaseCount() == 0) {
                return new ActionTransitionChangeTo(new GetSurprised(by.gameObject, null), "Why'd you tase me???");
            }
            return continueWork;
        }

        public override void OnEnd(Actor actor) {
            actor.RaiseEvent(new EventTased());
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                // Capture alerts, we're too busy to change states!
                case KnowledgeChanged:
                case Shoved:
                case DroppedByCharacter:
                    return new ActionEventResponseTransition(continueWork);
                default: return base.OnReceivedEvent(actor,e);
            }
        }
    }
}
