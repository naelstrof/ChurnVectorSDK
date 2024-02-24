using AI;
using AI.Actions;
using UnityEngine;
using UnityEngine.AI;

public class ActionGetOnNavmesh : Action {
    private Vector3 localFootOffset;
    private int cornerIndex;
    private float startTime;
    private float duration;
    
    public ActionGetOnNavmesh(Vector3 localFootOffset, float duration = 4f) {
        this.localFootOffset = localFootOffset;
        this.duration = duration;
    }

    public override ActionTransition OnStart(Actor actor) {
        startTime = Time.time;
        return DoWork(actor);
    }

    private ActionTransition DoWork(Actor actor) {
        if (duration != 0f && Time.time - startTime > duration) {
            return new ActionTransitionDone("Couldn't quite make it to the navmesh in time, giving up...");
        }
        if (!NavMesh.SamplePosition(actor.gameObject.transform.TransformPoint(localFootOffset), out NavMeshHit targetHit, 1f, NavMesh.AllAreas)) {
            return new ActionTransitionSuspendFor(new ActionMoveRandomly(),"I have fallen off the navmesh! Moving randomly to try to get back...");
        }
        return new ActionTransitionDone("Found the navmesh!");
    }


    public override ActionTransition OnResume(Actor actor) {
        return DoWork(actor);
    }

    public override ActionTransition Update(Actor actor) {
        return new ActionTransitionDone("I should never say this!");
    }
}
