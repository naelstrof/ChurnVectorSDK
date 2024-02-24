using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityScriptableSettings;

public class VolumeSettingListener : MonoBehaviour {
    private Volume v;
    [SerializeField]
    private SettingInt bloomOption;
    [SerializeField]
    private SettingInt blurOption;
    [SerializeField]
    private SettingFloat postExposure;
    [SerializeField]
    private SettingInt dofOption;
    void Start() {
        v = GetComponent<Volume>();
        bloomOption.changed += OnBloomChanged;
        blurOption.changed += OnBlurChanged;
        postExposure.changed += OnPostExposureChange;
        dofOption.changed += OnDepthOfFieldChanged;
        OnBloomChanged(bloomOption.GetValue());
        OnBlurChanged(blurOption.GetValue());
        OnPostExposureChange(postExposure.GetValue());
        OnDepthOfFieldChanged(dofOption.GetValue());
    }

    void OnBlurChanged(int value) {
        if (!v.profile.TryGet(out MotionBlur blur)) { return; }
        blur.active = (value!=0);
        blur.quality.Override(value-1);
    }

    void OnBloomChanged(int value) {
        if (!v.profile.TryGet(out Bloom bloom)) { return; }
        bloom.active = (value!=0);
        bloom.highQualityFiltering = (value > 1);
    }
    
    void OnDepthOfFieldChanged(int value) {
        if (!v.profile.TryGet(out DepthOfField depth)) { return; }
        depth.active = (value!=0);
    }

    void OnPostExposureChange(float value) {
        if (!v.profile.TryGet(out ColorAdjustments adjustments)) { return; }
        adjustments.postExposure.Override(value);
    }
}