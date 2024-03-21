using Naelstrof.Easing;
using UnityEngine;

[System.Serializable]
public class OrbitCameraCharacterFPSConfiguration : OrbitCameraConfiguration {
    [SerializeField]
    private OrbitCameraPivotBase standPivot; //Headpivot
    [SerializeField]
    private OrbitCameraPivotBase crouchPivot;
    
    private OrbitCameraPivotBasic ballsPivot;
    [SerializeField]
    private CharacterBase character;

    private OrbitCameraData? lastData;

    public void SetPivots(CharacterBase character, OrbitCameraPivotBase standPivot, OrbitCameraPivotBase crouchPivot) {
        this.standPivot = standPivot;
        this.crouchPivot = crouchPivot;
        this.character = character;
    }

    public override OrbitCameraData GetData(Camera cam) {
        if (standPivot == null) {
            lastData ??= OrbitCamera.GetCurrentCameraData();
            return lastData.Value;
        }
        
        OrbitCameraData crouchCamera = crouchPivot.GetData(cam);
        OrbitCameraData shoulderCamera = standPivot.GetData(cam);
        OrbitCameraData hitmanCamera = OrbitCameraData.Lerp(shoulderCamera,crouchCamera, character.GetCrouchAmount());
        lastData = hitmanCamera;
        return lastData.Value;
    }
}
