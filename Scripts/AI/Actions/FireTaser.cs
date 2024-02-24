using AI.Events;

namespace AI.Actions {
    public class FireTaser : Action {
        private CharacterBase target;

        public FireTaser(CharacterBase target) {
            this.target = target;
        }

        public override ActionTransition OnStart(Actor actor) {
            actor.SetAimingWeapon(true);
            return new ActionTransitionSuspendFor(new AimWeapon(target), "Aiming weapon...");
        }

        public override ActionTransition OnResume(Actor actor) {
            if (actor.GetCharacter() is Civilian cop) {
                actor.fireWeapon?.Invoke();
            }
            return new ActionTransitionDone("Finished my attack!");
        }

        public override void OnEnd(Actor actor) {
            base.OnEnd(actor);
            actor.SetAimingWeapon(false);
        }
    }
}
