using AI;
using AI.Actions;
using UnityEngine;

public class ActionTurnToFaceDirection : Action {
    private Vector3 direction;
    private float startTime;
    private float duration;
    public ActionTurnToFaceDirection(Vector3 direction, float duration) {
        this.direction = direction;
        this.duration = duration;
    }

    public override ActionTransition OnStart(Actor actor) {
        actor.SetLookDirection(direction);
        startTime = Time.time;
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        actor.SetLookDirection(direction);
        if (Time.time - startTime < duration) {
            return continueWork;
        }
        return new ActionTransitionDone("Looked at it.");
    }
}
