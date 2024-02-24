using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class OrbitCameraPivotPlayer : OrbitCameraPivotBase {
    [SerializeField] private Vector2 screenOffset = Vector2.one * 0.5f;
    [SerializeField] float desiredDistanceFromPivot = 1f;
    [SerializeField] float baseFOV = 65f;
    private CharacterBase character;
    private float distanceMemory = 0f;
    private float distanceVel = 0f;
    
    protected override void Awake() {
        base.Awake();
        character = GetComponentInParent<CharacterBase>();
    }
    public override OrbitCameraData GetData(Camera cam) {
        var rotation = cam.transform.rotation;
        float desiredDistance = Math.Max(GetDistanceFromPivot(cam, rotation, screenOffset), 0.25f);
        if (Time.deltaTime != 0f) {
            distanceMemory = Mathf.SmoothDamp(distanceMemory, desiredDistance, ref distanceVel, 0.1f);
        }

        return new OrbitCameraData {
            screenPoint = screenOffset,
            position = GetPivotPosition(),
            distance = distanceMemory,
            clampPitch = true,
            clampYaw = false,
            fov = GetFOV(rotation),
            rotation = rotation,
        };
    }

    public float GetBaseFOV() => baseFOV;

    public void SetBaseFOV(float value) {
        baseFOV = value;
    }

    private Vector3 GetPivotPosition() {
        Vector3 offset = Vector3.Lerp(Vector3.zero, Vector3.down * 0.5f, character.GetCrouchAmount());
        return transform.position+offset;
    }
    private float GetDistanceFromPivot(Camera cam, Quaternion rotation, Vector2 screenOffset) {
        CastNearPlane(cam, rotation, screenOffset, GetPivotPosition(), GetPivotPosition() - cam.transform.forward * desiredDistanceFromPivot, out float newDistance);
        return newDistance;
    }

    // Worms-eye view gets more FOV, birds-eye view gets less
    private float GetFOV(Quaternion camRotation) {
        Vector3 forward = camRotation * Vector3.back;
        float fov = Mathf.Lerp(Mathf.Min(baseFOV * 1.5f,100f), baseFOV, Easing.OutCubic(Mathf.Clamp01(forward.y + 1f)));
        return fov;
    }
}
