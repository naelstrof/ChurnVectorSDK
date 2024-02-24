using UnityEngine;

namespace AI.Actions {
    
public class AimWeapon : Action {
    private Vector3 currentAim;
    private float startingTime;
    private CharacterBase target;
    private const float aimDuration = 1.8f;
    private const float rotationSpeed = 4f;
    private const float minRotationSpeed = 0.5f;

    public AimWeapon(CharacterBase target) {
        this.target = target;
    }

    public override ActionTransition OnStart(Actor actor) {
        startingTime = Time.time;
        currentAim = actor.GetLookDirection();
        return continueWork;
    }

    public override ActionTransition Update(Actor actor) {
        if (Time.time - startingTime > aimDuration) {
            return new ActionTransitionDone("Finished aiming");
        }

        float timeTillShoot = Mathf.Max(aimDuration - (Time.time - startingTime), 0f);
        if (!actor.TryGetAimPrediction(target, timeTillShoot, out Vector3 aimPrediction)) {
            actor.SetLookDirection(currentAim);
            return continueWork;
        }

        float angle = Vector3.Angle(currentAim, aimPrediction)*Mathf.Deg2Rad;
        currentAim = Vector3.RotateTowards(currentAim, aimPrediction, Mathf.Max(rotationSpeed * angle * Time.deltaTime, minRotationSpeed*Time.deltaTime), 1f);
        actor.SetLookDirection(currentAim);
        return base.Update(actor);
    }
}

}
