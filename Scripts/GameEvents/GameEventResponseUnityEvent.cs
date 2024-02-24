using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventResponseUnityEvent : GameEventResponse {
    [SerializeField]
    private UnityEvent onInvoke;

    public override void Invoke(MonoBehaviour owner) {
        base.Invoke(owner);
        onInvoke?.Invoke();
    }
}
