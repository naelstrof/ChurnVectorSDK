using System.Collections.Generic;
using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using Event = AI.Events.Event;

public class ActionSearch : Action {
    private int currentPoint;
    private Vector3 direction;
    private Vector3 lastKnownPosition;
    private GameObject target;
    private List<Vector3> searchPositions;
    private float startTime;
    private bool searching;
    private float overrideDistanceSearch;
    private bool keepMemoryAlive;
    private int failures;
    
    public ActionSearch(GameObject target, Vector3 lastKnownPosition, Vector3 direction, bool keepMemoryAlive, int failures, float overrideDistanceSearch = 0f) {
        this.keepMemoryAlive = keepMemoryAlive;
        this.target = target;
        this.direction = direction;
        this.lastKnownPosition = lastKnownPosition;
        this.overrideDistanceSearch = overrideDistanceSearch;
        this.failures = failures;
        searchPositions = new List<Vector3>();
    }
    public override ActionTransition OnStart(Actor actor) {
        if (failures > 4) {
            return new ActionTransitionDone("Failed to search...");
        }
        if (keepMemoryAlive) {
            actor.KeepMemoryAlive(target, true);
        }
        // Five fork fan search, ping pong'd
        startTime = Time.time;
        if (NavMesh.SamplePosition(lastKnownPosition, out NavMeshHit firstHit, 2f, NavMesh.AllAreas)) {
            Debug.DrawRay(firstHit.position, Vector3.up, Color.white, 10f);
            searchPositions.Add(firstHit.position);
        }
        for (int i = 0; i < 3; i++) {
            float angle = (i/2) * 20f;
            angle = i % 2 == 0 ? angle : -angle;
            float coneLength = overrideDistanceSearch == 0f ? Mathf.Clamp(Vector3.Distance(actor.transform.position, lastKnownPosition),1f, 8f) : overrideDistanceSearch;
            if (NavMesh.SamplePosition(lastKnownPosition + Vector3.down + (Quaternion.AngleAxis(angle, Vector3.up) * direction) * coneLength,
                    out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                searchPositions.Add(hit.position);
            }
        }
        if (searchPositions.Count == 0) {
            searchPositions.Add(lastKnownPosition);
        }
        return DoSearch(actor);
    }

    private ActionTransition DoSearch(Actor actor) {
        if (actor.IsVisible(target) || currentPoint >= searchPositions.Count) {
            return new ActionTransitionDone("Found them!");
        }

        if (searching && Time.time-startTime < 2f) {
            // Looking around a little.
            return continueWork;
        } else {
            searching = false;
        }

        if (Vector3.Distance(searchPositions[currentPoint], actor.transform.TransformPoint(Vector3.down)) < 0.25f) {
            currentPoint++;
            searching = true;
            startTime = Time.time;
            return continueWork;
        }
        return new ActionTransitionSuspendFor(new FollowPathToPoint(searchPositions[currentPoint], Vector3.down, 0.1f), "Moving toward this spot, memory: " + keepMemoryAlive);
    }

    public override ActionTransition OnResume(Actor actor) {
        return DoSearch(actor);
    }

    public override ActionTransition Update(Actor actor) {
        return DoSearch(actor);
    }

    public override void OnEnd(Actor actor) {
        if (keepMemoryAlive) {
            actor.KeepMemoryAlive(target, false);
        }
        base.OnEnd(actor);
    }

    public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
        switch (e) {
            case FailedToFindPath failedToFindPath:
                failures++;
                Vector3 targetPosition = target.transform.position.With(y:actor.transform.position.y);
                if (NavMesh.SamplePosition(lastKnownPosition, out NavMeshHit firstHit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                    return new ActionEventResponseTransition(new ActionTransitionChangeTo(
                        new ActionSearch(target, targetPosition,
                            Random.insideUnitSphere.With(y: 0f).normalized, false, failures, 8f),
                        "Can't path to target, executing search!"));
                }
                if (failures > 4) {
                    return new ActionEventResponseTransition( new ActionTransitionChangeTo( new DoNothing(1f), "Couldn't path to target, couldn't path to search!"));
                } else {
                    return new ActionEventResponseTransition( new ActionTransitionSuspendFor( new DoNothing(1f), "Couldn't path to search, waiting a sec."));
                }
                break;
            case KnowledgeChanged alert:
                return new ActionEventResponseTransition(continueWork);
        }
        return base.OnReceivedEvent(actor, e);
    }
}
