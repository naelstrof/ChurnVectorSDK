using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class Shatterable : MonoBehaviour {
    
    [SerializeField, Range(0f,3f)] private float breakThreshold = 0.5f;
    [SerializeField, Range(0f,100f)] private float explosionForce = 30f;
    [SerializeField] private GameObject brokenVersion;
    [SerializeField] private AudioPack breakSound;

    private Rigidbody body;
    private Vector3 cachedVelocity;
    private Vector3 cachedAngularVelocity;
    private float radiusEstimation;
    
    private List<Tuple<float,ImpactAnalysis>> impacts;
    private void UpdateDecalableColliders() {
        var decalableCollider = GetComponentInChildren<DecalableCollider>();
        if (decalableCollider == null) {
            return;
        }
        var decalableColliderRenderers = new List<Renderer>(decalableCollider.GetDecalableRenderers());
        var brokenFragments = brokenVersion.GetComponentsInChildren<MeshRenderer>();
        foreach (var fragment in brokenFragments) {
            decalableColliderRenderers.Add(fragment);
        }
        decalableCollider.SetDecalableRenderers(decalableColliderRenderers.ToArray());
    }

    private void Awake() {
        body = GetComponent<Rigidbody>();
        impacts = new List<Tuple<float,ImpactAnalysis>>();
        var sharedMaterial = GetComponentInChildren<Collider>().sharedMaterial;
        var brokenFragments = brokenVersion.GetComponentsInChildren<MeshRenderer>();
        foreach (var fragment in brokenFragments) {
            var fragbody = fragment.gameObject.AddComponent<Rigidbody>();
            fragbody.mass = body.mass / brokenFragments.Length;
            if (!fragment.TryGetComponent(out Collider ignoredCollider)) {
                var meshCollider = fragment.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = fragment.GetComponent<MeshFilter>().sharedMesh;
                meshCollider.sharedMaterial = sharedMaterial;
                meshCollider.convex = true;
            }
            fragment.gameObject.AddComponent<PhysicsAudio>();
        }

        Bounds? calculatedBounds = null;
        UpdateDecalableColliders();

        foreach (var renderable in GetComponentsInChildren<Renderer>()) {
            if (renderable.transform.IsChildOf(brokenVersion.transform)) {
                continue;
            }
            if (calculatedBounds == null) {
                calculatedBounds = renderable.bounds;
            } else {
                calculatedBounds.Value.Encapsulate(renderable.bounds);
            }
        }
        radiusEstimation = calculatedBounds != null ? Mathf.Max(Mathf.Max(calculatedBounds.Value.extents.x, calculatedBounds.Value.extents.y), calculatedBounds.Value.extents.z) : 1f;
    }

    private void FixedUpdate() {
        cachedVelocity = body.velocity;
        cachedAngularVelocity = body.angularVelocity;
    }

    void OnCollisionEnter(Collision collision) {
        ImpactAnalysis impactAnalysis = new ImpactAnalysis(body, collision);
        impacts.Add(new Tuple<float, ImpactAnalysis>(Time.time,impactAnalysis));
        if (impactAnalysis.GetImpactMagnitude() > breakThreshold) {
            Break(impactAnalysis);
        }
    }

    private void Break(ImpactAnalysis impact) {
        // Allow time to settle.
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        //brokenVersion.transform.SetPositionAndRotation(transform.position, transform.rotation);
        brokenVersion.transform.SetParent(null);
        brokenVersion.SetActive(true);
        
        Vector3 center = body.transform.TransformPoint(body.centerOfMass);
        foreach (var fragment in brokenVersion.GetComponentsInChildren<Rigidbody>()) {
            Vector3 fromCenter = fragment.transform.TransformPoint(fragment.centerOfMass) - center;
            Vector3 cross = Vector3.Cross(cachedAngularVelocity, fromCenter)*fromCenter.magnitude;
            fragment.velocity = cachedVelocity + cross;
            fragment.AddExplosionForce(explosionForce*cachedVelocity.magnitude, center, radiusEstimation*2f, 0.1f);
        }

        //var debugDisplay = new GameObject("DebugDisplay", typeof(DebugDisplay));
        //debugDisplay.GetComponent<DebugDisplay>().SetImpact(impact);

        CharacterDetector.PlayInvestigativeAudioPackAtPoint(CharacterBase.GetPlayer(), breakSound, transform.position, breakSound.GetInterestLevel()*3f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        if (impacts == null || !Application.isPlaying) {
            return;
        }

        for (int i = 0; i < impacts.Count; i++) {
            float lifetime = 5f;
            if (impacts[i].Item1+lifetime < Time.time) {
                impacts.RemoveAt(i--);
            }
        }

        foreach (var impactMemory in impacts) {
            ImpactAnalysis.DrawHandles(impactMemory.Item2, Color.white);
        }
        #endif
    }

    public void Throw(Vector3 vector) {
        body.MovePosition(vector*Time.deltaTime);
        body.velocity = vector;
        body.gameObject.layer = LayerMask.NameToLayer("Default");
    }
    
}
