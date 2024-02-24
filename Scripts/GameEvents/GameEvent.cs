using UnityEngine;


[CreateAssetMenu(fileName = "New GameEvent", menuName = "Data/GameEvent", order = 17)]
public class GameEvent : ScriptableObject {
    public delegate void TriggeredAction();
    public event TriggeredAction triggered;
    public void Raise() => triggered?.Invoke();
}
