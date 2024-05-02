using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using Event = AI.Events.Event;

namespace AI.Actions {
    public class Investigate : Action {
        private GameObject target;
        private bool searched;

        public Investigate(GameObject target) {
            this.target = target;
        }

        public class InvestigateEvent : AnimationBool {
            public InvestigateEvent(string name, bool value) : base(name, value) { }
        }

        private ActionTransition InvestigateInfo(Actor actor) {
            var knowledge = actor.GetKnowledgeOf(target);
            if (knowledge.GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Investigative) {
                return new ActionTransitionDone("Okay, done investigating.");
            }
            bool visible = actor.IsVisible(target);
            if (visible) {
                knowledge.TryGetLastKnownPosition(out Vector3 position);
                if (Vector3.Distance(target.transform.position, actor.transform.position) > 10f) {
                    return new ActionTransitionSuspendFor(new FollowPathToPoint(position, Vector3.down, 0.5f), "Can't really see them well enough. Getting closer...");
                }
                return new ActionTransitionSuspendFor(new ActionTurnToFaceObject(target, 1f), "I am investigating this person!");
            } else {
                // If we don't know where to go, or the target location is unreachable!
                // TODO: Probably make a state that makes the character go GET DOWN FROM THERE and wave their arms or whatever.
                Vector3 knownPosition;
                if (knowledge.TryGetLastKnownPosition(out knownPosition) && knowledge.TryGetLastKnownDirection(out Vector3 knownDirection) && !searched) {
                    searched = true;
                    return new ActionTransitionSuspendFor(new ActionSearch(target, knownPosition, knownDirection, true, 0), "I think they went this way.");
                }
                if (!knowledge.TryGetLastKnownPosition(out knownPosition) || !NavMesh.SamplePosition(knownPosition, out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                    return new ActionTransitionSuspendFor(new LookLeftAndRight(), "Swear I saw something...");
                }
                if (Vector3.Distance(actor.gameObject.transform.position, hit.position) > 1.1f) {
                    return new ActionTransitionSuspendFor(new FollowPathToPoint(hit.position, Vector3.down),
                        "Going to go check out where I last saw them...");
                }
            }
            return new ActionTransitionSuspendFor(new LookLeftAndRight(), "Swear I saw something...");
        }

        public override ActionTransition OnStart(Actor actor) {
            actor.RaiseEvent(new InvestigateEvent("Investigate", true));
            Vector3 lookDir = Vector3.zero;
            var knowledge = actor.GetKnowledgeOf(target);
            if (knowledge.TryGetLastKnownPosition(out Vector3 lastKnownPosition)) {
                lookDir = Vector3.Normalize(lastKnownPosition - actor.transform.position);
            }
            return new ActionTransitionSuspendFor(new ActionTurnToFaceObject(target, 2f, lookDir), "Thinking about investigating...");
        }

        public override void OnEnd(Actor actor) {
            actor.RaiseEvent(new InvestigateEvent("Investigate", false));
        }

        public override ActionTransition OnResume(Actor actor) {
            return InvestigateInfo(actor);
        }

        public override ActionTransition Update(Actor actor) {
            return InvestigateInfo(actor);
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case Shoved shoved:
                    return new ActionEventResponseTransition( new ActionTransitionSuspendFor(new GetShoved(shoved.GetOther()), "OOF, what the?"));
                case HeardInterestingNoise noise:
                    bool visible = actor.IsVisible(noise.GetOwner().gameObject);
                    if (noise.GetOwner() == target || visible) {
                        return ignoreResponse;
                    }
                    return new ActionEventResponseTransition( new ActionTransitionSuspendFor(new ActionTurnToFaceDirection(noise.GetNoiseHeardDirection(), 0.25f), "Was that them?"));
            }
            return base.OnReceivedEvent(actor, e);
        }
    }
}
