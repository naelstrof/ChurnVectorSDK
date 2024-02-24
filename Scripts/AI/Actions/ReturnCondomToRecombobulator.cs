using System.Collections.Generic;
using AI.Events;
using UnityEngine;
using Event = AI.Events.Event;

namespace AI.Actions {
    public class ReturnCondomToRecombobulator : Action {
        private Condom targetCondom;
        private List<IInteractable> interactableQueue;
        private CumRecombobulator targetCombobulator;
        private bool saidDialogue;
        public ReturnCondomToRecombobulator(Condom targetCondom) {
            this.targetCondom = targetCondom;
            interactableQueue = new List<IInteractable>();
            InteractableLibrary.TryGetInteractableOfType(out targetCombobulator, this.targetCondom.transform.position);
        }
        private ActionTransition DoWork(Actor actor) {
            if (targetCondom == null) {
                return new ActionTransitionDone("Oh, it's been removed!");
            }

            if (!actor.ShouldUse(targetCondom)) {
                return new ActionTransitionChangeTo(new ActionTurnToFaceObject(targetCondom.gameObject, 2f), "Whoa, someone's taking care of a big condom...");
            }

            actor.KeepMemoryAlive(targetCondom.gameObject, true);

            if (!actor.IsStillUsing(targetCondom)) {
                return new ActionTransitionSuspendFor(new UseInteractable(targetCondom, true), "Gotta pick up the condom.");
            }
            
            if (interactableQueue.Count != 0) {
                var interactable = interactableQueue[^1];
                float distanceToUsable = Vector3.Distance(actor.transform.position, interactable.transform.position);
                if (distanceToUsable > FollowPathToPoint.maxDistanceFromNavmesh) {
                    return new ActionTransitionSuspendFor(new FollowPathToPoint(interactable.transform.position, Vector3.down, 1f), "Gotta get closer to this door with the condom.");
                }
                interactableQueue.Remove(interactable);
                if (actor.IsStillUsing(targetCondom)) {
                    actor.StopUsingAnything();
                }
                return new ActionTransitionSuspendFor(new UseInteractable(interactable), "gotta open this door or somethin");
            }
            if (!saidDialogue) {
                saidDialogue = true;
                var character = actor.GetCharacter();
                character.StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.FoundCondom).Begin(new DialogueCharacter[] {
                        DialogueCharacterSpecificCharacter.Get(character),
                        DialogueCharacterInanimateObject.Get(targetCondom.transform, DialogueLibrary.GetCondomTheme()),
                }));
            }
            float distance = Vector3.Distance(actor.transform.position, targetCombobulator.transform.position);
            if (distance > FollowPathToPoint.maxDistanceFromNavmesh) {
                return new ActionTransitionSuspendFor(new FollowPathToPoint(targetCombobulator.transform.position, Vector3.down, 1f), "Gotta drag it to the cum recombobulator");
            }
            if (actor.IsStillUsing(targetCondom)) {
                actor.StopUsingAnything();
            }
            return new ActionTransitionChangeTo(new UseInteractable(targetCombobulator), "Okay, got it ready, using the recombobulator.");
        }

        public override ActionTransition OnStart(Actor actor) {
            return DoWork(actor);
        }

        public override void OnEnd(Actor actor) {
            if (targetCondom != null) {
                if (actor.IsStillUsing(targetCondom)) {
                    actor.StopUsingAnything();
                }

                actor.KeepMemoryAlive(targetCondom.gameObject, false);
            }
            base.OnEnd(actor);
        }

        public override ActionTransition OnResume(Actor actor) {
            return DoWork(actor);
        }

        public override ActionTransition Update(Actor actor) {
            return DoWork(actor);
        }

        public override ActionEventResponse OnReceivedEvent(Actor actor, Event e) {
            switch (e) {
                case KnowledgeChanged alert:
                    // Ignore other condoms!
                    if (alert.GetKnowledge().target.TryGetComponent(out Condom condom)) {
                        return new ActionEventResponseTransition(continueWork);
                    }
                    return base.OnReceivedEvent(actor,e);
                case FollowPathToPoint.UseInteractableInterrupt interrupt:
                    if (!interactableQueue.Contains(interrupt.GetInteractable())) {
                        interactableQueue.Add(interrupt.GetInteractable());
                        return new ActionEventResponseTransition(continueWork);
                    } else {
                        return base.OnReceivedEvent(actor,e);
                    }
                default: return base.OnReceivedEvent(actor,e);
            }
        }
    }
}
