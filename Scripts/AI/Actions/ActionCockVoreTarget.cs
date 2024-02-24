using AI;
using AI.Actions;
using UnityEngine;

public class ActionCockVoreTarget : Action {
    private CharacterBase target;
    public ActionCockVoreTarget(CharacterBase target) {
        this.target = target;
    }

    public override ActionTransition OnStart(Actor actor) {
        actor.SetSprint(true);
        return DoWork(actor);
    }
    
    private ActionTransition DoWork(Actor actor) {
        var knowledge = actor.GetKnowledgeOf(target.gameObject);
        if (knowledge.GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Alert) {
            return new ActionTransitionDone("I lost them...");
        }

        knowledge.TryGetLastKnownPosition(out Vector3 targetPosition);
        float distance = Vector3.Distance(actor.transform.position, targetPosition);
        if (distance > FollowPathToPoint.maxDistanceFromNavmesh) {
            return new ActionTransitionSuspendFor(new FollowPathToPoint(targetPosition, Vector3.down, 0.2f), "Gotta get closer to cock-vore them!");
        }
        if (distance < FollowPathToPoint.maxDistanceFromNavmesh && actor.IsVisible(target.gameObject)) {
            return new ActionTransitionChangeTo(new UseInteractable(target), "Got them!");
        }
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        return DoWork(actor);
    }

    public override void OnEnd(Actor actor) {
        actor.SetSprint(false);
    }
}
