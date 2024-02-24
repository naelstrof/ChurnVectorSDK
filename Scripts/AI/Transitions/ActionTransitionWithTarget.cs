using AI.Actions;

public abstract class ActionTransitionWithTarget : ActionTransitionWithReason {
    protected Action nextAction;
    public Action GetTargetAction() => nextAction;
    public ActionTransitionWithTarget(Action nextAction, string reason) : base(reason) {
        this.nextAction = nextAction;
    }
}
