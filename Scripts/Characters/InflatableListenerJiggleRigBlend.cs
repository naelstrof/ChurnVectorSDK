using JigglePhysics;
using Naelstrof.Inflatable;
using UnityEngine;

[System.Serializable]
public class InflatableListenerJiggleRigBlend : InflatableListener {
    [SerializeField]
    private Transform targetRootBone;
    [SerializeField]
    private JiggleRigBuilder targetBuilder;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0f,0f,1f,1f);

    private JiggleSettingsBlend targetBlend;
    public override void OnEnable() {
        base.OnEnable();
        foreach(var rig in targetBuilder.jiggleRigs) {
            if (rig.GetRootTransform() != targetRootBone ||
                rig.jiggleSettings is not JiggleSettingsBlend blend) continue;
            targetBlend = Object.Instantiate(blend);
            rig.jiggleSettings = targetBlend;
        }
        if (targetBlend == null) {
            Debug.LogError("Failed to find jiggle rig that has a root bone " + targetRootBone + " on " + targetBuilder + " with a JiggleSettingsBlend as its settings.", targetBuilder.gameObject);
        }
    }

    public override void OnSizeChanged(float newSize) {
        base.OnSizeChanged(newSize);
        targetBlend.normalizedBlend = curve.Evaluate(newSize);
    }
}
