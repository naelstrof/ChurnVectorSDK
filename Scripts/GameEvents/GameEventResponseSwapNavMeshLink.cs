using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class GameEventResponseSwapNavMeshLink : GameEventResponse {
    [SerializeField] private NavMeshLink[] targets;
    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        foreach (var target in targets) {
            (target.startPoint, target.endPoint) = (target.endPoint, target.startPoint);
            target.UpdateLink();
        }
    }
}
