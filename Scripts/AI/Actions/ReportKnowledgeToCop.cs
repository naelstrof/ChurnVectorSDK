using AI.Events;
using UnityEngine;

namespace  AI.Actions {
    
public class ReportKnowledgeToCop : Action {
    private Civilian target;
    private GameObject about;
    private float lastSeenTime;
    private const float interestTime = 10f;
    public ReportKnowledgeToCop(Civilian target, GameObject about) {
        this.about = about;
        this.target = target;
    }

    public override ActionTransition OnStart(Actor actor) {
        actor.RaiseEvent(new AddTrackingCharacter(target));
        lastSeenTime = Time.time;
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time - lastSeenTime > interestTime) {
            return new ActionTransitionDone("Lost track trying to report a crime...");
        }
        if (!actor.GetKnowledgeOf(target.gameObject).TryGetLastKnownPosition(out Vector3 lastKnownPosition)) {
            return new ActionTransitionDone("Lost track trying to report a crime...");
        }
        lastSeenTime = Time.time;
        if (Vector3.Distance(lastKnownPosition, actor.transform.position) > 10f) {
            return new ActionTransitionSuspendFor(new FollowPathToPoint(lastKnownPosition, Vector3.down, 0.25f), "Gotta get closer to the cop!");
        }
        actor.RaiseEvent(new ShareKnowledge(target, about));
        return new ActionTransitionChangeTo(new ActionTurnToFaceObject(target.gameObject, 2f), "Reporting the situation!");
    }

    public override void OnEnd(Actor actor) {
        actor.RaiseEvent(new RemoveTrackingCharacter(target));
    }
}

}
