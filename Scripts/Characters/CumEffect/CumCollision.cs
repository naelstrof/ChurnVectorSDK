using System.Collections.Generic;
using PenetrationTech;
using SkinnedMeshDecals;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CumCollision : MonoBehaviour {
    [SerializeField]
    private Material blitMaterial;
    [SerializeField]
    private Penetrator attachedDick;
    
    private ParticleSystem cumSystem;
    private List<ParticleCollisionEvent> particleCollisionEvents;
    private void Start() {
        cumSystem = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    public void SetAttachedDick(Penetrator dick) {
        attachedDick = dick;
    }

    private void ProcessEvent(GameObject targetObject, ParticleCollisionEvent particleCollisionEvent) {
        if (!targetObject.TryGetComponent(out DecalableCollider decalableCollider)) {
            return;
        }

        foreach (Renderer targetRenderer in decalableCollider.GetDecalableRenderers()) {
            float radius = 0.2f;
            Debug.DrawLine(particleCollisionEvent.intersection,
                particleCollisionEvent.intersection + particleCollisionEvent.normal, Color.cyan, 1f);
            PaintDecal.RenderDecal(targetRenderer, blitMaterial, particleCollisionEvent.intersection + particleCollisionEvent.normal * radius,
                Quaternion.FromToRotation(Vector3.forward, -particleCollisionEvent.normal), Vector2.one * radius * 2f,
                radius * 2f, "_DecalColorMap", RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        }
    }

    private void Update() {
        if (attachedDick == null) {
            return;
        }

        var spline = attachedDick.GetPath();
        transform.position = spline.GetPositionFromDistance(attachedDick.GetWorldLength()*0.9f);
        transform.rotation =
            Quaternion.LookRotation(spline.GetVelocityFromDistance(attachedDick.GetWorldLength() * 0.9f).normalized,
                Vector3.up);
    }

    private void OnParticleCollision(GameObject other) {
        cumSystem.GetCollisionEvents(other, particleCollisionEvents);
        foreach (var particleCollisionEvent in particleCollisionEvents) {
            ProcessEvent(other, particleCollisionEvent);
        }
    }
}
