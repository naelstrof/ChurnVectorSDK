using UnityEngine;
using UnityEngine.InputSystem;

public class CutscenePlayer : MonoBehaviour {
    [SerializeField] private GameEvent triggerEvent;
    [SerializeField] private Cutscene cutscene;

    private void OnEnable() {
        triggerEvent.triggered += OnTriggered;
    }

    private void OnDisable() {
        triggerEvent.triggered -= OnTriggered;
    }

    void OnTriggered() {
        StartCoroutine(cutscene.Begin(gameObject));
    }
}
