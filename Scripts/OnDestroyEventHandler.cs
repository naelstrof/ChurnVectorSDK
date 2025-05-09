using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyEventHandler : MonoBehaviour {
    public event System.Action<GameObject> onDestroyOrDisable;
    void OnDestroy() {
        onDestroyOrDisable?.Invoke(gameObject);
    }

    void OnDisable() {
        onDestroyOrDisable?.Invoke(gameObject);
    }
}
