using UnityEngine;

[System.Serializable]
public class GameEventResponseDisableObjects : GameEventResponse {
    [SerializeField] private GameObject[] targets;
    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        foreach (var target in targets) {
            target.SetActive(false);
        }
    }
}
