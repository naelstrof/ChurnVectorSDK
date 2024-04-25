using AI.Events;

namespace AI.Actions {
    
public class RadioForReinforcements : Action {
    private CharacterBase target;

    public delegate void RadioForReinforcementsAction();
    public static event RadioForReinforcementsAction callRadio;

    public RadioForReinforcements(CharacterBase target) {
        this.target = target;
    }

    public class RadioEvent : AnimationTrigger {
        public RadioEvent(string name) : base(name) { }
    }

    public override ActionTransition OnStart(Actor actor) {
        foreach(var cop in actor.GetAllCops()) {
            if (cop.knowledgeDatabase.GetKnowledge(target.gameObject).awareness < 1f) {
                actor.RaiseEvent(new RadioEvent("TalkRadio"));
                return new ActionTransitionSuspendFor(new DoNothing(1.5f), "Radioing in!");
            }
        }
        return new ActionTransitionDone("No ignorant cops in range!");
    }

    public override ActionTransition OnResume(Actor actor) {
        foreach (var cop in actor.GetAllCops()) {
            actor.RaiseEvent(new ShareKnowledge(cop, target.gameObject));
        }
        callRadio?.Invoke();
        return new ActionTransitionDone("Done radioing in, over!");
    }
}

}
