using UnityEngine;

public class NeedStationFinisher : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<NeedStation.NeedStationInfo>().Finish();
    }
}
