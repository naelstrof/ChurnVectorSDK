using UnityEngine;

namespace AI.Actions {
    public class UseInteractable : Action {
        private readonly IInteractable target;
        private bool returnControl;

        public UseInteractable(IInteractable target, bool returnControl = false) {
            this.target = target;
            this.returnControl = returnControl;
        }

        public ActionTransition Think(Actor actor) {
            if (Vector3.Distance(target.transform.position, actor.transform.position) >= FollowPathToPoint.maxDistanceFromNavmesh) {
                return new ActionTransitionSuspendFor(new FollowPathToPoint(target.transform.position, Vector3.down, 1f), "Too far from the interactable, going to walk closer.");
            }
            actor.UseInteractable(target);
            return continueWork;
        }

        public override ActionTransition OnStart(Actor actor) {
            return Think(actor);
        }

        public override ActionTransition OnResume(Actor actor) {
            return Think(actor);
        }

        public override void OnEnd(Actor actor) {
            if (!returnControl) {
                actor.StopUsingAnything();
            }
            base.OnEnd(actor);
        }

        public override ActionTransition Update(Actor actor) {
            if (actor.IsStillUsing(target) && !returnControl) {
                return continueWork;
            }
            return new ActionTransitionChangeTo(new DoNothing(1f),"Done using the item!");
        }
    }
}
