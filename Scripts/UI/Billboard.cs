using UnityEngine;

public class Billboard : MonoBehaviour {
    private Vector3 originalPosition;
    [SerializeField]
    private Vector2 paddingMeters;
    private void Awake() {
        originalPosition = transform.localPosition;
    }
    void LateUpdate() {
        var orbitCam = OrbitCamera.GetCamera();
        var parent = transform.parent;
        var screenPoint = orbitCam.WorldToScreenPoint(parent.TransformPoint(originalPosition));
        //if (screenPoint.z < 0) {
            //transform.position = transform.parent.TransformPoint(originalPosition);
            //return;
        //}
        var screenPointY = orbitCam.WorldToScreenPoint(parent.TransformPoint(originalPosition)+orbitCam.transform.up*paddingMeters.y);
        var screenPointX = orbitCam.WorldToScreenPoint(parent.TransformPoint(originalPosition)+orbitCam.transform.right*paddingMeters.x);
        Vector2 realPadding = new Vector2(Vector3.Distance(screenPoint, screenPointX), Vector3.Distance(screenPoint, screenPointY));
        screenPoint = new Vector3(Mathf.Clamp(screenPoint.x, realPadding.x, Screen.width-realPadding.x), Mathf.Clamp(screenPoint.y, realPadding.y, Screen.height-realPadding.y), screenPoint.z);
        var newWorldPosition = orbitCam.ScreenToWorldPoint(screenPoint);
        transform.position = newWorldPosition;
        transform.rotation = Quaternion.LookRotation((OrbitCamera.GetPlayerIntendedPosition() - transform.position).normalized, Vector3.up);
    }
}
