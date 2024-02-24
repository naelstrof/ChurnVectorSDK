using System.Collections.Generic;
using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using Event = AI.Events.Event;
using Grabbed = AI.Actions.Grabbed;

namespace ActorActions {
    [System.Serializable]
    public class ActionBeAPatrollingCop : BaseAction {

        [System.Serializable]
        private class PatrolNode
        {
            public Transform transform;
            public float waitDuration = 5;
        }
        [SerializeField] private CycleMode cycleMode;
        [SerializeField] private List<PatrolNode> patrolNodes;
        private int currentNode;
        private Vector3 patrolPosition;
        private int dir = 1;

        private enum CycleMode {
            Loop,
            PingPong,
        }

        public override ActionTransition OnResume(Actor actor) {
            var knowledge = actor.GetKnowledgeOf(CharacterBase.GetPlayer().gameObject);
            switch(knowledge.GetKnowledgeLevel()) {
                case KnowledgeDatabase.KnowledgeLevel.Investigative:
                    return new ActionTransitionSuspendFor(new Investigate(CharacterBase.GetPlayer().gameObject), "Oh yeah I was looking for something...");
                case KnowledgeDatabase.KnowledgeLevel.Alert:
                    return new ActionTransitionSuspendFor(new ActionFight(CharacterBase.GetPlayer()), "Done being surprised, WEEWOO!");
            }
            actor.SetLookDirection(patrolNodes[currentNode].transform.forward);
            return TryPatrol(actor);
        }

        private ActionTransition TryPatrol(Actor actor) {
            if (Vector3.Distance(patrolPosition, actor.transform.TransformPoint(Vector3.down)) > 1f) {
                return new ActionTransitionSuspendFor(new FollowPathToPoint(patrolPosition, Vector3.down), "Going to patrol point");
            }
            float lastWaitDuration = patrolNodes[currentNode].waitDuration;
            if (cycleMode == CycleMode.Loop) {
                currentNode = ++currentNode % patrolNodes.Count;
            } else {
                currentNode += dir;
                if (currentNode >= patrolNodes.Count) {
                    dir *= -1;
                    currentNode = Mathf.Max(patrolNodes.Count - 2,0);
                } else if (currentNode < 0) {
                    dir *= -1;
                    currentNode = Mathf.Min(patrolNodes.Count-1,1);
                }
            }
            
            if (NavMesh.SamplePosition(patrolNodes[currentNode].transform.position, out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                patrolPosition = hit.position;

            } else {
                Debug.LogError($"Patrol node {patrolNodes[currentNode]} is off the navmesh grid.", actor.gameObject);
            }
            actor.SetLookDirection(patrolNodes[currentNode].transform.forward);
            return new ActionTransitionSuspendFor(new DoNothing(lastWaitDuration), "Going to chill for a sec.");
        }

        public override ActionTransition OnStart(Actor actor) {
            if (NavMesh.SamplePosition(patrolNodes[currentNode].transform.position, out NavMeshHit hit, FollowPathToPoint.maxDistanceFromNavmesh, NavMesh.AllAreas)) {
                patrolPosition = hit.position;
            }
            return TryPatrol(actor);
        }

        public override ActionTransition Update(Actor actor) {
            return TryPatrol(actor);
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case GrabbedByCharacter grabbedByCharacter:
                    return new ActionEventResponseTransition(
                        new ActionTransitionSuspendFor(new Grabbed(grabbedByCharacter.GetCharacter()), "Ack! Grabbed!"));
                case KnowledgeChanged alert:
                    if (alert.GetKnowledge().target.TryGetComponent(out CharacterBase character)) {
                        switch (alert.GetKnowledge().GetKnowledgeLevel()) {
                            case KnowledgeDatabase.KnowledgeLevel.Investigative:
                                return new ActionEventResponseTransition(new ActionTransitionSuspendFor(
                                    new Investigate(alert.GetKnowledge().target), "What's going on over there?"));
                            case KnowledgeDatabase.KnowledgeLevel.Alert:
                                return new ActionEventResponseTransition(new ActionTransitionSuspendFor(
                                    new GetSurprised(alert.GetKnowledge().target, new ActionFight(character)), "WEEWOO I'm the CV police!"));
                        }
                    }
                    if (alert.GetKnowledge().target.TryGetComponent(out Condom condom)) {
                        return new ActionEventResponseTransition(new ActionTransitionSuspendFor( new GetSurprised(alert.GetKnowledge().target, new ReturnCondomToRecombobulator(condom)), "Is that... is that a condom??"));
                    }
                    return ignoreResponse;
                case HeardInterestingNoise noise: {
                    if (actor.GetKnowledgeOf(noise.GetOwner().gameObject).GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
                        if (Random.Range(0f, 1f) < noise.GetHeardSound().GetInterestLevel()) {
                            return new ActionEventResponseTransition(new ActionTransitionSuspendFor(
                                new ActionTurnToFaceObject(noise.GetOwner().gameObject, 2f, noise.GetNoiseHeardDirection()),
                                "What was that?"));
                        }
                    }
                    return ignoreResponse;
                }
                default: return base.OnReceivedEvent(actor,e);
            }
        }
    }
}
