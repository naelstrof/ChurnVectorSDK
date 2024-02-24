using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ParallaxUI : MonoBehaviour {
    private Vector2 position;
    private Vector2 velocity;
    private Camera cam;
    [Serializable]
    private class LayerObject {
        [SerializeField]
        private RectTransform obj;
        private Vector3 originalPosition;

        public void Initialize() {
            originalPosition = obj.anchoredPosition;
        }
        public void SetOffset(Vector3 offset) {
            obj.anchoredPosition = originalPosition + offset;
        }
    }
    
    [Serializable]
    private class Layer {
        [SerializeField]
        private LayerObject[] objects;

        public void Initialize() {
            foreach(var obj in objects) {
                obj.Initialize();
            }
        }

        public void SetOffset(Vector3 offset) {
            foreach(var obj in objects) {
                obj.SetOffset(offset);
            }
        }
    }
    
    [SerializeField]
    private float maxMovement = 1f;
    
    [SerializeField]
    private Layer[] layers;

    private void Awake() {
        foreach(var layer in layers) {
            layer.Initialize();
        }
    }
    
    private Vector2 GetScreenOffsetOfGameObject(GameObject target) {
        if (cam == null) {
            cam = Camera.main;
        }

        if (cam == null) {
            return Vector2.one / 2f;
        }
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        return (target.transform.position/resolution)-Vector2.one*0.5f;
    }

    private void Update() {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        Vector2 center = new Vector2(0.5f, 0.5f);
        Vector2 mousePosition = Mouse.current.position.ReadValue()/resolution-center;
        if (EventSystem.current.currentSelectedGameObject != null) {
            var offset = GetScreenOffsetOfGameObject(EventSystem.current.currentSelectedGameObject);
            mousePosition -= offset*3f;
        }
        position = Vector2.SmoothDamp(position, mousePosition, ref velocity, 0.25f, 2f, Time.unscaledDeltaTime);
        Vector2 diff = position - center;
        for(int i=0;i<layers.Length;i++) {
            float movementAmount = maxMovement * (((float)(i+1) / (float)layers.Length) * 0.75f + 0.25f);
            layers[i].SetOffset(movementAmount*diff);
        }
    }
}
