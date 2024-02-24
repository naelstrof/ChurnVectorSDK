using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class GameEventResponseElevator : GameEventResponse {
    [SerializeField] private NavMeshLink floorLink;
    [SerializeField] private NeedStation upperFloorButton;
    [SerializeField] private NeedStation lowerFloorButton;
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private Transform elevator;
    [SerializeField] private AudioPack elevatorButtonSound;
    [SerializeField] private AudioPack elevatorMoveSound;
    private static readonly int Elevate = Animator.StringToHash("Elevate");

    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        bool raised = elevatorAnimator.GetBool(Elevate);
        raised = !raised;
        (floorLink.startPoint, floorLink.endPoint) = (floorLink.endPoint, floorLink.startPoint);
        floorLink.UpdateLink();
        upperFloorButton.gameObject.SetActive(!raised);
        lowerFloorButton.gameObject.SetActive(raised);
        elevatorAnimator.SetBool(Elevate, raised);
        AudioPack.PlayClipAtPoint(elevatorButtonSound, owner.transform.position);
        AudioPack.PlayClipAtPoint(elevatorMoveSound, elevator.transform.position);
    }
}
