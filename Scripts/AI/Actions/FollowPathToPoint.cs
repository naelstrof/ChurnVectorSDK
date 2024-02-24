using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using Event = AI.Events.Event;

public class FollowPathToPoint : Action {
    private NavMeshPath path;
    private Vector3 localFootOffset;
    private Vector3 targetLocation;
    private int cornerIndex;
    private float lastProgressionTime;
    private const float stuckTimeout = 10f;
    private float startTime;
    private float duration;
    private bool useOffmeshLinks;
    private bool usingOffmeshLink;
    public const float maxDistanceFromNavmesh = 2f;
    public class UseInteractableInterrupt : Event {
        private IInteractable interactable;
        public UseInteractableInterrupt(IInteractable interactable) {
            this.interactable = interactable;
        }
        public IInteractable GetInteractable() => interactable;
    }
    
    public FollowPathToPoint(Vector3 targetLocation, Vector3 localFootOffset, float duration = 0f, bool useOffmeshLinks = true) {
        path = new NavMeshPath();
        this.targetLocation = targetLocation;
        this.localFootOffset = localFootOffset;
        this.duration = duration;
        this.useOffmeshLinks = useOffmeshLinks;
    }

    public override ActionTransition OnStart(Actor actor) {
        startTime = Time.time;
        lastProgressionTime = Time.time;
        
        if (!NavMesh.SamplePosition(targetLocation, out NavMeshHit targetHit, maxDistanceFromNavmesh, NavMesh.AllAreas)) {
            return RaiseEventAndTransition(actor, new FailedToFindPath(), new ActionTransitionChangeTo(new DoNothing(1f),"Target location is off the navmesh, can't path to it..."));
        }
        targetLocation = targetHit.position;

        if (!NavMesh.SamplePosition(actor.gameObject.transform.TransformPoint(localFootOffset), out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
            return new ActionTransitionSuspendFor(new ActionGetOnNavmesh(localFootOffset), "Need to get on the navmesh before I can get to the point");
        }
        
        Vector3 currentPosition = hit.position;
        bool success = NavMesh.CalculatePath(currentPosition, targetLocation, NavMesh.AllAreas, path);

        if (path.status != NavMeshPathStatus.PathComplete) {
            success &= Vector3.Distance(path.corners[^1], targetLocation) < maxDistanceFromNavmesh;
        }

        if (success && path.corners.Length > 1) {
            cornerIndex = 1;
            if (useOffmeshLinks) {
                var link = OfflinkDetector.GetLinkFromPosition(path.corners[cornerIndex]);
                usingOffmeshLink = link != null && !usingOffmeshLink;
                if (link != null && link.TryGetComponent(out NavMeshLinkToInteractable linkToInteractable)) {
                    return new ActionTransitionSuspendFor(new UseInteractable(linkToInteractable.GetInteractable()), "I gotta use this interactable to get where I need!");
                }
            }
            actor.SetWishDirection((path.corners[cornerIndex] - currentPosition).normalized);
            actor.SetLookDirection((path.corners[cornerIndex] - currentPosition).normalized);
            return continueWork;
        }

        return RaiseEventAndTransition(actor, new FailedToFindPath(), new ActionTransitionChangeTo(new DoNothing(1f), "Couldn't find path to location..." + targetLocation));
    }

    public override ActionTransition OnResume(Actor actor) {
        return OnStart(actor);
    }

    public override void OnEnd(Actor actor) {
        actor.SetWishDirection(Vector3.zero);
        base.OnEnd(actor);
    }

    public override void OnSuspend(Actor actor) {
        actor.SetWishDirection(Vector3.zero);
        base.OnSuspend(actor);
    }

    public override ActionTransition Update(Actor actor) {
        if (!usingOffmeshLink && duration != 0f && Time.time - startTime > duration) {
            return new ActionTransitionDone("Got closer to destination.");
        }

        if (path.corners == null || path.corners.Length == 0) {
            return RaiseEventAndTransition(actor, new FailedToFindPath(), new ActionTransitionDone("Couldn't find path to location..."));
        }

        if (Time.time > lastProgressionTime + stuckTimeout) {
            return new ActionTransitionDone("I got stuck on the way there!");
        }

        Vector3 currentPosition = actor.gameObject.transform.TransformPoint(localFootOffset);
        Vector3 wishDir = (path.corners[cornerIndex] - currentPosition).normalized;
        if (Mathf.Abs(Vector3.Dot(wishDir, Vector3.up)) > 0.9f) {
            return new ActionTransitionDone("Oh no, somehow we're targetting a corner on another floor! I give up!");
        }

        actor.SetWishDirection(wishDir);
        actor.SetLookDirection(wishDir);
        Debug.DrawLine(currentPosition, path.corners[cornerIndex]);
        if (!(Vector3.Distance(currentPosition, path.corners[cornerIndex]) < 0.25f)) return continueWork;
        cornerIndex++;
        lastProgressionTime = Time.time;
        if (cornerIndex < path.corners.Length) {
            if (useOffmeshLinks) {
                var link = OfflinkDetector.GetLinkFromPosition(path.corners[cornerIndex]);
                usingOffmeshLink = link != null && !usingOffmeshLink;
                if (usingOffmeshLink && link.TryGetComponent(out NavMeshLinkToInteractable linkToInteractable)) {
                    return RaiseEventAndTransition(actor, new UseInteractableInterrupt(linkToInteractable.GetInteractable()), new ActionTransitionSuspendFor(new UseInteractable(linkToInteractable.GetInteractable()), "I gotta use this interactable to get where I need!"));
                }
            }
            return continueWork;
        }

        return new ActionTransitionDone("Got to the location!");
    }
}
