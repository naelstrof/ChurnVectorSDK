using AI.Events;
using UnityEngine;

namespace AI.Actions {
    public class LookLeftAndRight : Action {
        private int timer;
        private Quaternion forwardRot;
        public override ActionTransition OnStart(Actor actor) {
            forwardRot = actor.GetCharacter().GetFacingDirection();
            timer = 0;
            return new ActionTransitionSuspendFor(new DoNothing(1f), "prepping to look.");
        }

        public override ActionTransition OnResume(Actor actor) {
            timer++;
            switch (timer) {
                case 1: {
                    var turnLeft = Quaternion.AngleAxis(-75f, Vector3.up) * forwardRot;
                    return new ActionTransitionSuspendFor(new ActionTurnToFaceDirection(turnLeft * Vector3.forward, 1.5f), "Looking left!");
                }
                case 2: {
                    var turnRight = Quaternion.AngleAxis(75f, Vector3.up) * forwardRot;
                    return new ActionTransitionSuspendFor(new ActionTurnToFaceDirection(turnRight * Vector3.forward, 1.5f), "Looking right!");
                }
            } 
            return new ActionTransitionDone("Done looking.");
        }

        public override ActionTransition Update(Actor actor) {
            return new ActionTransitionDone("Done looking.");
        }
    }
}
