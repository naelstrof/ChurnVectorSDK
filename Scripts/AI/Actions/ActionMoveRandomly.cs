using AI;
using AI.Actions;
using UnityEngine;

public class ActionMoveRandomly : Action {
    private Vector3 direction;
    private float startTime;
    private const float duration = 1f;
    public ActionMoveRandomly() {
        direction = Random.insideUnitSphere.With(y: 0f);
        if (direction == Vector3.zero) {
            direction = Vector3.forward;
        } else {
            direction = direction.normalized;
        }
    }
    public override ActionTransition OnStart(Actor actor) {
        startTime = Time.time;
        actor.SetLookDirection(direction);
        actor.SetWishDirection(direction);
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time - startTime > duration) {
            return new ActionTransitionDone("Done moving randomly");
        }
        actor.SetLookDirection(direction);
        actor.SetWishDirection(direction);
        return continueWork;
    }

    public override void OnEnd(Actor actor) {
        actor.SetWishDirection(Vector3.zero);
    }
}
