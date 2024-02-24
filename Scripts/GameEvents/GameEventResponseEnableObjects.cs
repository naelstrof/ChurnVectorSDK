using UnityEngine;

[System.Serializable]
public class GameEventResponseEnableObjects : GameEventResponse {
    [SerializeField] private GameObject[] targets;
    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        foreach (var target in targets) {
            target.SetActive(true);
        }
    }
}
