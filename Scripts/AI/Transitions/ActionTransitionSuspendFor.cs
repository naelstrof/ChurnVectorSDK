using AI.Actions;

public class ActionTransitionSuspendFor : ActionTransitionWithTarget {
    public ActionTransitionSuspendFor(Action nextAction, string reason) : base(nextAction, reason) { }
}
