
public class ActionTransitionWithReason : ActionTransition {
    protected string reason;
    public string GetTransitionReason() => reason;
    public ActionTransitionWithReason(string reason) : base() {
        this.reason = reason;
    }
}
