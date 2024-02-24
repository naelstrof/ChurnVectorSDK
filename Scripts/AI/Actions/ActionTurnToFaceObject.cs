using AI;
using AI.Actions;
using UnityEngine;

[System.Serializable]
public class ActionTurnToFaceObject : Action {
    private GameObject target;
    private Vector3 startLookDir;
    private float startTime;
    private float duration;
    public ActionTurnToFaceObject(GameObject target, float duration, Vector3 startLookDir = default) {
        this.target = target;
        this.startLookDir = startLookDir;
        this.duration = duration;
    }

    public override ActionTransition OnStart(Actor actor) {
        if (startLookDir != Vector3.zero) {
            actor.SetLookDirection(startLookDir.normalized);
        }
        startTime = Time.time;
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time - startTime > duration) {
            return new ActionTransitionDone("Looked for the duration!");
        }

        var knowledge = actor.GetKnowledgeOf(target);
        if (!knowledge.TryGetLastKnownPosition(out Vector3 lastKnownPosition)) {
            return continueWork;
        }
        Vector3 dir = lastKnownPosition - actor.gameObject.transform.position;
        actor.SetLookDirection(dir.normalized);
        return continueWork;
    }
}
