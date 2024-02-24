using System.Collections.Generic;
using UnityEngine;

public class GameEventListener : MonoBehaviour {
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField, SerializeReference, SubclassSelector]
    private List<GameEventResponse> responses;

    private void OnEnable() {
        gameEvent.triggered += OnRaisedEvent;
    }
    private void OnDisable() {
        gameEvent.triggered -= OnRaisedEvent;
    }

    private void OnRaisedEvent() {
        foreach (var response in responses) {
            response.Invoke(this);
        }
    }
}
