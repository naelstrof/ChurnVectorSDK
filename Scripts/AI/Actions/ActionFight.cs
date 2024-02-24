using AI;
using AI.Actions;
using AI.Events;
using UnityEngine;

public class ActionFight : Action {
    private CharacterBase target;
    private bool searched = false;

    public ActionFight(CharacterBase target) {
        this.target = target;
    }

    public override ActionTransition OnStart(Actor actor) {
        actor.SetSprint(true);
        if (actor.GetKnowledgeOf(target.gameObject).GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Alert) {
            foreach (var cop in actor.GetAllCops()) {
                if (cop.knowledgeDatabase.GetKnowledge(target.gameObject).GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
                    return new ActionTransitionSuspendFor(new RadioForReinforcements(target),
                        "Calling in reinforcements!");
                }
            }
        }

        return continueWork;
    }

    public override void OnEnd(Actor actor) {
        actor.SetSprint(false);
        base.OnEnd(actor);
    }

    public override ActionTransition OnResume(Actor actor) {
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        var knowledge = actor.GetKnowledgeOf(target.gameObject);
        if (knowledge.GetKnowledgeLevel() == KnowledgeDatabase.KnowledgeLevel.Ignorant) {
            return new ActionTransitionChangeTo(new DoNothing(5f),"I think I lost them...");
        }
        knowledge.TryGetLastKnownDirection(out Vector3 lastKnownDirection);
        if (!knowledge.TryGetLastKnownPosition(out Vector3 lastKnownPosition)) {
            if (!searched) {
                searched = true;
                return new ActionTransitionSuspendFor(
                    new ActionSearch(target.gameObject, lastKnownPosition, lastKnownDirection, true, 0), "Where'd they go?");
            } else {
                return new ActionTransitionSuspendFor(new DoNothing(1f), "I lost them...");
            }
        }
        

        float wantedDistance = 7f;
        float distance = Vector3.Distance(lastKnownPosition, actor.transform.position);
        bool visible = actor.IsVisible(target.gameObject);
        bool dirVisible = actor.IsDirectlyVisible(target.gameObject);
        if (!visible && distance < 2f) {
            searched = true;
            return new ActionTransitionSuspendFor(new ActionSearch(target.gameObject, lastKnownPosition, lastKnownDirection, !searched, 0), "Where'd they go?");
        }
        if (distance > wantedDistance+5f || !dirVisible) {
            return new ActionTransitionSuspendFor(new FollowPathToPoint(lastKnownPosition, Vector3.down, 0.1f), "Gotta get closer!");
        }
        return new ActionTransitionSuspendFor(new FireTaser(target), "Firing!");
    }
        public override ActionEventResponse OnReceivedEvent(Actor actor, AI.Events.Event e) {
            switch (e) {
                case SuccessfulTase taseTarget:
                    return new ActionEventResponseTransition( new ActionTransitionSuspendFor(new PullOnTaseTarget(target, taseTarget.GetTarget()), "GOT EM, SIR"));
                // Catch and ignore knowledge events about stuff we're not fighting.
                case KnowledgeChanged knowledgeChanged:
                    if (knowledgeChanged.GetKnowledge().target != target.gameObject) {
                        return ignoreResponse;
                    }
                    break;
            }
            return base.OnReceivedEvent(actor, e);
        }
}
