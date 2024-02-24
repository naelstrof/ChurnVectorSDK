using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;
using UnityEngine.AI;
using Event = AI.Events.Event;
using Grabbed = AI.Actions.Grabbed;

namespace ActorActions {
    [System.Serializable]
    public class ActionBeACop : BaseAction {
        public override ActionTransition OnResume(Actor actor) {
            var knowledge = actor.GetKnowledgeOf(CharacterBase.GetPlayer().gameObject);
            switch(knowledge.GetKnowledgeLevel()) {
                case KnowledgeDatabase.KnowledgeLevel.Investigative:
                    return new ActionTransitionSuspendFor(new Investigate(CharacterBase.GetPlayer().gameObject), "Oh yeah I was looking for something...");
                case KnowledgeDatabase.KnowledgeLevel.Alert:
                    return new ActionTransitionSuspendFor(new ActionFight(CharacterBase.GetPlayer()), "WEEWOO!");
            }
            return continueWork;
        }

        public override ActionTransition Update(Actor actor) {
            var need = actor.GetRandomInteractable();
            if (need != null) {
                return new ActionTransitionSuspendFor(new WanderToInteractable(need), "I want to wander!");
            } else {
                // Try to wander randomly if we got nothing to do.
                if (!NavMesh.SamplePosition(actor.transform.position, out NavMeshHit targetHit, FollowPathToPoint.maxDistanceFromNavmesh*2f, NavMesh.AllAreas)) {
                    return continueWork;
                }
                return new ActionTransitionSuspendFor(new FollowPathToPoint(targetHit.position, Vector3.down, 5f), "I want to wander!");
            }
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case GrabbedByCharacter grabbedByCharacter:
                    return new ActionEventResponseTransition(
                        new ActionTransitionSuspendFor(new Grabbed(grabbedByCharacter.GetCharacter()), "Ack! Grabbed!"));
                case KnowledgeChanged alert:
                    if (alert.GetKnowledge().GetKnowledgeLevel() != KnowledgeDatabase.KnowledgeLevel.Ignorant) {
                        actor.StopUsingAnything();
                    }

                    if (alert.GetKnowledge().target.TryGetComponent(out CharacterBase character)) {
                        switch (alert.GetKnowledge().GetKnowledgeLevel()) {
                            case KnowledgeDatabase.KnowledgeLevel.Investigative:
                                return new ActionEventResponseTransition(new ActionTransitionSuspendFor(
                                    new Investigate(alert.GetKnowledge().target), "What's going on over there?"));
                            case KnowledgeDatabase.KnowledgeLevel.Alert:
                                return new ActionEventResponseTransition(new ActionTransitionSuspendFor( new GetSurprised(alert.GetKnowledge().target, new ActionFight(CharacterBase.GetPlayer())), "WEEWOO I'm the CV police!"));
                        }
                    }
                    if (alert.GetKnowledge().target.TryGetComponent(out Condom condom)) {
                        return new ActionEventResponseTransition(new ActionTransitionSuspendFor( new GetSurprised(alert.GetKnowledge().target, new ReturnCondomToRecombobulator(condom)), "Is that... is that a condom??"));
                    }
                    return ignoreResponse;
                case HeardInterestingNoise noise: {
                    if (actor.GetKnowledgeOf(noise.GetOwner().gameObject).GetKnowledgeLevel() ==
                        KnowledgeDatabase.KnowledgeLevel.Ignorant) {
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
