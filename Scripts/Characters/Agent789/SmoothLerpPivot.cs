using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SmoothLerpPivot : OrbitCameraPivotBase {
    [SerializeField] private Vector2 screenOffset = Vector2.one * 0.5f;
    [SerializeField] float desiredDistanceFromPivot = 1f;
    [SerializeField] float baseFOV = 65f;

    [SerializeField] Transform target;

    [SerializeField] Transform frameOfReference;
    public void SetInfo(Vector2 screenOffset, float desiredDistanceFromPivot) {
        this.screenOffset = screenOffset;
        this.desiredDistanceFromPivot = desiredDistanceFromPivot;
    }

    private void LateUpdate() {
        Vector3 targetLocalPosition = frameOfReference.worldToLocalMatrix.MultiplyPoint(target.position);
        Vector3 pivotLocalPosition = frameOfReference.worldToLocalMatrix.MultiplyPoint(transform.position);

        pivotLocalPosition = Vector3.MoveTowards(pivotLocalPosition, targetLocalPosition, Time.deltaTime*0.5f);

        transform.position = frameOfReference.localToWorldMatrix.MultiplyPoint(pivotLocalPosition);
    }
    protected class HitResultSorter : IComparer<RaycastHit> {
        public int Compare(RaycastHit x, RaycastHit y) {
            return x.distance.CompareTo(y.distance);
        }
    }
    
    public override OrbitCameraData GetData(Camera cam) {
        var rotation = cam.transform.rotation;
        return new OrbitCameraData {
            screenPoint = screenOffset,
            position = transform.position,
            distance = GetDistanceFromPivot(cam, rotation, screenOffset),
            clampPitch = true,
            clampYaw = false,
            fov = GetFOV(rotation),
            rotation = rotation,
        };
    }

    private float GetDistanceFromPivot(Camera cam, Quaternion rotation, Vector2 screenOffset) {
        CastNearPlane(cam, rotation, screenOffset, transform.position, transform.position - cam.transform.forward * desiredDistanceFromPivot, out float newDistance);
        return newDistance;
    }

    // Worms-eye view gets more FOV, birds-eye view gets less
    private float GetFOV(Quaternion camRotation) {
        Vector3 forward = camRotation * Vector3.back;
        float fov = Mathf.Lerp(Mathf.Min(baseFOV * 1.5f,100f), baseFOV, Easing.OutCubic(Mathf.Clamp01(forward.y + 1f)));
        return fov;
    }
}
