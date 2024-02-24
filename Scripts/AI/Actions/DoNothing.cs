using AI;
using AI.Actions;
using UnityEngine;

public class DoNothing : Action {

    private float startTime;
    private float duration;
    
    public DoNothing(float duration) {
        this.duration = duration;
    }
    public override ActionTransition OnStart(Actor actor) {
        startTime = Time.time;
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time > startTime + duration) {
            return new ActionTransitionDone( "Finished thinking.");
        }

        return continueWork;
    }
}
