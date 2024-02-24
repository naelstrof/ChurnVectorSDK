using System.Collections.Generic;
using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;

public class ActionRunAway : Action {
    private GameObject runAwayFrom;
    List<Civilian> getNearbyCops = new();
    public ActionRunAway(GameObject from) {
        runAwayFrom = from;
    }
    public class Relax : AnimationTrigger {
        public Relax(string name) : base(name) {
        }
    }
    
    private ActionTransition RunAway(Actor actor) {
        actor.GetNearbyCops(actor.transform.position, getNearbyCops);
        foreach (var cop in getNearbyCops) {
            return new ActionTransitionSuspendFor(new ReportKnowledgeToCop(cop, runAwayFrom), "Oh the CV police! gotta tell them!");
        }

        var knowledge = actor.GetKnowledgeOf(runAwayFrom.gameObject);
        Vector3 away;
        // Run from the last known else from the 'from' location if last known hasn't been init yet
        if (knowledge.TryGetLastKnownPosition(out Vector3 lastKnownPosition)) {
            away = (actor.transform.position - lastKnownPosition).normalized*10f;
            Debug.DrawLine(actor.gameObject.transform.position,  lastKnownPosition, Color.red, 5f);
        } else {
            away = (actor.transform.position - runAwayFrom.transform.position).normalized * 10f;
        }
        
        if (NavMesh.SamplePosition(away+actor.gameObject.transform.position, out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas) && hit.hit) {
            Debug.DrawLine(actor.gameObject.transform.position, hit.position, Color.cyan, 5f);
            return new ActionTransitionSuspendFor(new FollowPathToPoint(hit.position, Vector3.down), "Running away!!!");
        }

        Vector3 tryRight = Quaternion.AngleAxis(90f, Vector3.up) * away;
        Debug.DrawLine(tryRight + actor.gameObject.transform.position, actor.gameObject.transform.position, Color.red, 5f);
        if (NavMesh.SamplePosition(tryRight+actor.gameObject.transform.position, out NavMeshHit rightHit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas) && rightHit.hit) {
            Debug.DrawLine(actor.gameObject.transform.position, rightHit.position, Color.cyan, 5f);
            return new ActionTransitionSuspendFor(new FollowPathToPoint(rightHit.position, Vector3.down), "Running away to the right!!!");
        }
        Vector3 tryLeft = Quaternion.AngleAxis(90f, Vector3.down) * away;
        Debug.DrawLine(tryLeft + actor.gameObject.transform.position, actor.gameObject.transform.position, Color.red, 5f);
        if (NavMesh.SamplePosition(tryLeft+actor.gameObject.transform.position, out NavMeshHit leftHit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas) && leftHit.hit) {
            Debug.DrawLine(actor.gameObject.transform.position, leftHit.position, Color.cyan, 5f);
            return new ActionTransitionSuspendFor(new FollowPathToPoint(leftHit.position, Vector3.down), "Running away to the left!!!");
        }

        return new ActionTransitionSuspendFor(new DoNothing(1f), "Oooh I don't know where to go!");
    }

    public override ActionTransition OnStart(Actor actor) {
        actor.SetSprint(true);
        return RunAway(actor);
    }

    public override ActionTransition OnResume(Actor actor) {
        var knowledge = actor.GetKnowledgeOf(runAwayFrom.gameObject);
        if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            return new ActionTransitionDone("I think I lost them...");
        }
        return RunAway(actor);
    }

    public override void OnEnd(Actor actor) {
        actor.SetSprint(false);
        actor.RaiseEvent(new Relax("Relax"));
        base.OnEnd(actor);
    }

    public override ActionTransition Update(Actor actor) {
        var knowledge = actor.GetKnowledgeOf(runAwayFrom.gameObject);
        if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            return new ActionTransitionDone("I think I lost them...");
        }
        return RunAway(actor);
    }
    public override ActionEventResponse OnReceivedEvent(Actor actor, AI.Events.Event e) {
        switch (e) {
            case KnowledgeChanged knowledgeChanged:
                if (knowledgeChanged.GetKnowledge().target == runAwayFrom && knowledgeChanged.GetKnowledge().GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
                    return new ActionEventResponseTransition(new ActionTransitionDone("I think I lost them..."));
                }
                return ignoreResponse;
        }
        return base.OnReceivedEvent(actor,e);
    }
}
