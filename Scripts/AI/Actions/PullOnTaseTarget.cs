using AI.Events;
using UnityEngine;

namespace AI.Actions {
    
public class PullOnTaseTarget : Action {
    private CharacterBase target;
    private CharacterBase actualTase;
    public PullOnTaseTarget(CharacterBase target, CharacterBase actualTase) {
        this.target = target;
        this.actualTase = actualTase;
    }

    public override ActionTransition OnStart(Actor actor) {
        if (actualTase != target) {
            return new ActionTransitionSuspendFor(new GetSurprised(actualTase.gameObject, null), "Oh no, asshole got in the way!");
        }

        return continueWork;
    }

    public override ActionTransition OnResume(Actor actor) {
        if (actualTase != target) {
            if (Vector3.Distance(actor.transform.position, actualTase.transform.position) > 5f) {
                return new ActionTransitionSuspendFor(new FollowPathToPoint(actualTase.transform.position, Vector3.down, 1f),"Lemme unhook those tasers...");
            }

            if (actor.GetCharacter() is Civilian cop) {
                cop.OnReloadTaser();
            }
            return new ActionTransitionChangeTo(new DoNothing(2.8f), "Reloading my taser...");
        }

        if (actor.GetTaseTarget() != target) {
            if (actor.GetCharacter() is Civilian cop) {
                cop.OnReloadTaser();
            }
            return new ActionTransitionChangeTo(new DoNothing(2.8f),"They broke the taser!");
        }

        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (actor.GetTaseTarget() != target) {
            if (actor.GetCharacter() is Civilian cop) {
                cop.OnReloadTaser();
            }
            return new ActionTransitionChangeTo(new DoNothing(2.8f),"They broke the taser!");
        }

        var knowledge = actor.GetKnowledgeOf(target.gameObject);
        if (!knowledge.TryGetLastKnownPosition(out Vector3 position)) {
            return continueWork;
        }

        actor.SetLookDirection(Vector3.Normalize(position - actor.transform.position));

        if (Vector3.Distance(position, actor.transform.position) > 8f) {
            return new ActionTransitionSuspendFor(new FollowPathToPoint(position, Vector3.down,0.25f), "They're tugging!");
        }

        return continueWork;
    }
}

}
