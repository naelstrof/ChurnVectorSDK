using AI.Actions;

public class ActionTransitionChangeTo : ActionTransitionWithTarget {
    public ActionTransitionChangeTo(Action nextAction, string reason) : base(nextAction, reason) { }
}
