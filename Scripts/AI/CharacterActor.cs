using System;
using System.Collections.Generic;
using AI.Actions;
using AI.Events;
using UnityEngine;
using Event = AI.Events.Event;

namespace AI {
    
[Serializable]
public partial class CharacterActor : Actor {
    protected Civilian character;
    public override void Initialize(GameObject character) {
        base.Initialize(character);
        this.character = gameObject.GetComponent<Civilian>();
    }

    public override ActionEventResponse RaiseEvent(Event e) {
        switch (e) {
            case Investigate.InvestigateEvent investigate:
                investigate.ApplyToAnimator(character.GetDisplayAnimator());
                return base.RaiseEvent(e);
            case AnimationTrigger animationTriggerEvent: animationTriggerEvent.ApplyToAnimator(character.GetDisplayAnimator()); return base.RaiseEvent(e);
            case AnimationBool animationBoolEvent: animationBoolEvent.ApplyToAnimator(character.GetDisplayAnimator()); return base.RaiseEvent(e);
            case AddTrackingCharacter trackingCharacter: character.AddTrackingGameObject(trackingCharacter.GetOther().gameObject); return base.RaiseEvent(e);
            case RemoveTrackingCharacter trackingCharacter: character.RemoveTrackingGameObject(trackingCharacter.GetOther().gameObject); return base.RaiseEvent(e);
            case ShareKnowledge shareKnowledge: shareKnowledge.GetTarget().ReceiveKnowledge(character, shareKnowledge.GetAbout()); return base.RaiseEvent(e);
        }
        return base.RaiseEvent(e);
    }

    public override KnowledgeDatabase.Knowledge GetKnowledgeOf(GameObject target) {
        return character.knowledgeDatabase.GetKnowledge(target);
    }

    public override bool GetGrabbed() {
        return character.IsGrabbed();
    }

    public override CharacterBase GetCharacter() {
        return character;
    }

    public override bool IsPointVisible(Vector3 point) {
        return Vector3.Dot(character.GetFacingDirection() * Vector3.forward, point - character.transform.position) > 0.1f;
    }

    public override bool IsVisible(GameObject target) {
        return character.CanSee(target);
    }


    public override bool IsDirectlyVisible(GameObject target) {
        return character.UnobscuredLineOfSight(target);
    }

    public override void GetNearbyCops(Vector3 point, List<Civilian> cops) {
        character.GetNearbyCharacters(point, 20f, cops);
        cops.RemoveAll((c) => (c.GetActor()?.IsCop() ?? false) == false);
    }

    public override bool UseInteractable(IInteractable interactable) {
        return character.InteractWith(interactable);
    }

    public override IInteractable GetRandomInteractable() {
        return InteractableLibrary.GetRandomInteractable(character);
    }

    public override bool IsStillUsing(IInteractable target) {
        return character.IsInteractingWith(target);
    }

    public override void StopUsingAnything() {
        character.StopInteraction();
    }

    public override bool ShouldUse(IInteractable interactable) {
        return interactable.ShouldInteract(character);
    }

    public override void KeepMemoryAlive(GameObject target, bool keepAlive) {
        character.knowledgeDatabase.KeepMemoryAlive(target, keepAlive);
    }
}

}