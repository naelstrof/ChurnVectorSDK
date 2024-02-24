using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityScriptableSettings;
public class CameraSettingListener : MonoBehaviour {
    public SettingInt antiAliasing;
    private HDAdditionalCameraData camData;
    void Start() {
        camData = GetComponent<HDAdditionalCameraData>();
        antiAliasing.changed += OnAntiAliasingChanged;
        OnAntiAliasingChanged(antiAliasing.GetValue());
    }

    // Temporal anti-aliasing clears depth, causing world UI elements to fail to draw properly :sob:
    void OnAntiAliasingChanged(int value) {
        switch (value) {
            case 0: camData.antialiasing = HDAdditionalCameraData.AntialiasingMode.None; break;
            case 1:
                camData.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                camData.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Low;
                break;
            default:
                camData.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                camData.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Medium;
                break;
            case 3:
                camData.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                camData.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.High;
                break;
        }
    }
}