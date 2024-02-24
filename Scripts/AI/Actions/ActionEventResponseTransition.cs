public class ActionEventResponseTransition : ActionEventResponse {
    private ActionTransition actionTransition;
    public ActionEventResponseTransition(ActionTransition transition) {
        actionTransition = transition;
    }
    public ActionTransition GetActionTransition() => actionTransition;
}
