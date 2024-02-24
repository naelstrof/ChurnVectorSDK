using Naelstrof.Easing;
using UnityEngine;

[System.Serializable]
public class OrbitCameraCharacterHitmanConfiguration : OrbitCameraConfiguration {
    [SerializeField]
    private OrbitCameraPivotBase standPivot; //Headpivot
    [SerializeField]
    private OrbitCameraPivotBase crouchPivot;
    [SerializeField]
    private OrbitCameraPivotBase buttPivotCenter;
    
    private OrbitCameraPivotBasic ballsPivot;
    [SerializeField]
    private CharacterBase character;

    private OrbitCameraData? lastData;

    public void SetPivots(CharacterBase character, OrbitCameraPivotBase standPivot, OrbitCameraPivotBase crouchPivot, OrbitCameraPivotBase buttPivotCenter) {
        this.standPivot = standPivot;
        this.crouchPivot = crouchPivot;
        this.buttPivotCenter = buttPivotCenter;
        this.character = character;
    }

    public override OrbitCameraData GetData(Camera cam) {
        if (ballsPivot == null) {
            if (!character.GetBallsTransform().gameObject.TryGetComponent(out ballsPivot)) {
                ballsPivot = character.GetBallsTransform().gameObject.AddComponent<OrbitCameraPivotBasic>();
                ballsPivot.SetInfo(new Vector2(0.5f, 0.4f), 2f, 70f);
            }
        }
        
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
        OrbitCameraData hitmanCameraWithButtAndBalls = OrbitCameraData.Lerp(hitmanCameraWithButt, ballsPivot.GetData(cam), Mathf.Lerp(0f,0.4f,character.GetBallVolume()));

        lastData = hitmanCameraWithButtAndBalls;
        return lastData.Value;
    }
}
