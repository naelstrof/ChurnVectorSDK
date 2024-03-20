using System;
using System.Collections;
using System.Collections.Generic;
using DPG;
using PenetrationTech;
using UnityEngine;

public class PenetrableBlendshape : MonoBehaviour {
    [SerializeField] private Penetrable targetPenetrable;
    [SerializeField] private SkinnedMeshBlendshapePair blendshape;
    [SerializeField, Range(0f,1f)] private float normalizedDistance;

    private CatmullSpline cachedSpline;

    private void OnEnable() {
        targetPenetrable.penetrated += OnPenetrated;
        targetPenetrable.unpenetrated += OnUnpenetrated;
    }

    private void OnUnpenetrated(Penetrable penetrable, Penetrator penetrator) {
        blendshape.skinnedMeshRenderer.SetBlendShapeWeight(blendshape.blendshapeID, 0f);
    }

    private void OnDisable() {
        targetPenetrable.penetrated -= OnPenetrated;
        targetPenetrable.unpenetrated -= OnUnpenetrated;
    }

    private void OnPenetrated(Penetrable penetrable, Penetrator penetrator, Penetrator.PenetrationArgs penetrationargs) {
        float arcLength = penetrationargs.alongSpline.GetLengthFromSubsection(penetrable.GetPoints().Count-1, penetrationargs.penetrableStartIndex);
        float triggerDepth = normalizedDistance * arcLength;
        float maxDistance = 0.08f;
        float triggerAmount = Mathf.Clamp01(1f-(triggerDepth - penetrationargs.penetrationDepth) / maxDistance);
        blendshape.skinnedMeshRenderer.SetBlendShapeWeight(blendshape.blendshapeID, triggerAmount * 100f);
        //float samplePosition = triggerDepth - penetrationargs.penetratorData.GetWorldLength();
        //float girth = penetrationargs.penetratorData.GetWorldGirthRadius(triggerDepth);
    }

    private void OnDrawGizmosSelected() {
        if (targetPenetrable == null) {
            return;
        }
        cachedSpline ??= new CatmullSpline(new[] { Vector3.zero, Vector3.one });
        cachedSpline.SetWeightsFromPoints(targetPenetrable.GetPoints());
        Vector3 pos = cachedSpline.GetPositionFromDistance(normalizedDistance * cachedSpline.arcLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, 0.025f);
    }
}
