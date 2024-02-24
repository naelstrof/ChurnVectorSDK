using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRayHandler : MonoBehaviour {
    private static XRayHandler instance;
    [SerializeField]
    private InputActionReference xrayButton;

    private float xrayAmount;
    
    [System.Serializable]
    private class ShaderMaterialReference {
        public Material material;
        public Shader associatedShader;
    }

    [SerializeField]
    private ShaderMaterialReference[] materialReferences;
    
    public delegate void XRayChangedAction(float xrayAmount);
    
    public static XRayChangedAction xrayChanged;
    public static float GetXRayAmount() => instance.xrayAmount;
    
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    private void OnEnable() {
        xrayButton.action.started += OnButtonDown;
        xrayButton.action.canceled += OnButtonUp;
    }

    private void OnButtonDown(InputAction.CallbackContext obj) {
        SetXRayState(true);
    }

    private void OnButtonUp(InputAction.CallbackContext obj) {
        SetXRayState(false);
    }

    private static void SetXRayState(bool on) {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.XRayToggle(on));
    }

    public static Material GetXRayMaterial(Shader type) {
        foreach (var materialReference in instance.materialReferences) {
            if (materialReference.associatedShader == type) {
                return materialReference.material;
            }
        }
        return instance.materialReferences[0].material;
    }
    private IEnumerator XRayToggle(bool on) {
        float startAmount = xrayAmount;
        float targetAlpha = on ? 1f : 0f;
        float startTime = Time.time;
        float duration = 2f;
        while (Time.time - startTime < duration) {
            float t = (Time.time - startTime) / duration;
            xrayAmount = Mathf.Lerp(startAmount, targetAlpha, t);
            xrayChanged?.Invoke(xrayAmount);
            yield return null;
        }
        xrayAmount = targetAlpha;
        xrayChanged?.Invoke(targetAlpha);
    }
}
