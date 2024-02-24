using Naelstrof.Easing;
using UnityEngine;

[System.Serializable]
public class OrbitCameraCharacterOogleConfiguration : OrbitCameraConfiguration {
    [SerializeField]
    private OrbitCameraPivotBase standPivot; //Headpivot
    [SerializeField]
    private OrbitCameraPivotBase crouchPivot;
    [SerializeField]
    private OrbitCameraPivotBase buttPivotCenter;
    [SerializeField]
    private OrbitCameraPivotBase buttPivotRight;
    [SerializeField]
    private OrbitCameraPivotBase buttPivotLeft;
    [SerializeField]
    private OrbitCameraPivotBase dickPivot;
    [SerializeField]
    private CharacterBase character;

    private OrbitCameraData? lastData;

    public void SetPivots(CharacterBase character, OrbitCameraPivotBase standPivot, OrbitCameraPivotBase crouchPivot,
        OrbitCameraPivotBase buttPivotCenter, OrbitCameraPivotBase buttPivotRight, OrbitCameraPivotBase buttPivotLeft,
        OrbitCameraPivotBase dickPivot) {
        this.character = character;
        this.standPivot = standPivot;
        this.crouchPivot = crouchPivot;
        this.buttPivotCenter = buttPivotCenter;
        this.buttPivotLeft = buttPivotLeft;
        this.buttPivotRight = buttPivotRight;
        this.dickPivot = dickPivot;
    }

    public override OrbitCameraData GetData(Camera cam) {
        if (standPivot == null) {
            lastData ??= OrbitCamera.GetCurrentCameraData();
            return lastData.Value;
        }
        
        // The forward unit vector of the camera, goes from (0,-1,0) to (0,1,0) based on how down/up we're looking.
        Vector3 forward = cam.transform.forward;

        float downUpSoftReveresed = 1f - Easing.Sinusoidal.In(Mathf.Clamp01(forward.y));

        OrbitCameraData crouchCamera = crouchPivot.GetData(cam);
        OrbitCameraData shoulderCamera = standPivot.GetData(cam);
        OrbitCameraData hitmanCamera = OrbitCameraData.Lerp(shoulderCamera,crouchCamera, character.GetCrouchAmount());
        OrbitCameraData hitmanCameraWithButt = OrbitCameraData.Lerp(buttPivotCenter.GetData(cam),hitmanCamera, downUpSoftReveresed);
        
        Quaternion rot = Quaternion.AngleAxis(OrbitCamera.GetPlayerIntendedScreenAim().x, Vector3.up);
        float leftRight = (Vector3.Dot(character.GetFacingDirection()*Vector3.right, rot * Vector3.forward)+1f)/2f;
        OrbitCameraData cockVoreCamera = OrbitCameraData.Lerp(buttPivotRight.GetData(cam),buttPivotLeft.GetData(cam), leftRight);
        
        float forwardBack = Mathf.Clamp01(Vector3.Dot(character.GetFacingDirection()*Vector3.forward, rot * Vector3.forward));
        float forwardBackReversed = Mathf.Clamp01(-Vector3.Dot(character.GetFacingDirection()*Vector3.forward, rot * Vector3.forward));
        OrbitCameraData cockVoreCameraWithDick = OrbitCameraData.Lerp(cockVoreCamera, dickPivot.GetData(cam), Easing.Cubic.InOut(forwardBackReversed));
        
        lastData = OrbitCameraData.Lerp(cockVoreCameraWithDick, hitmanCameraWithButt, Easing.Cubic.InOut(forwardBack));
        return lastData.Value;
    }
}
