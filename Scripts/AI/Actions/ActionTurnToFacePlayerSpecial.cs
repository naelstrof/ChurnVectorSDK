using AI;
using AI.Actions;
using UnityEngine;

[System.Serializable]
public class ActionTurnToFacePlayerSpecial : Action {
    private float startTime;
    private const float duration = 1f;
    
    public override ActionTransition OnStart(Actor actor) {
        startTime = Time.time;
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time - startTime > duration) {
            return new ActionTransitionChangeTo(new DoNothing(0.25f),"Looked for the duration!");
        }
        Vector3 dir = CharacterBase.GetPlayer().transform.position - actor.gameObject.transform.position;
        actor.SetLookDirection(dir.normalized);
        return continueWork;
    }
}
