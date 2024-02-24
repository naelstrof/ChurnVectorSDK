using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private static RaycastHit[] raycastHits = new RaycastHit[32];
    private static RaycastHitComparer raycastHitComparer = new();
    private float radius = 0.25f;
    private Vector3 velocity;
    private Vector3 lastPosition;
    private Vector3 position;
    private double lastUpdateTime;
    public const float speed = 20f;
    public delegate void ProjectileHitAction(RaycastHit hit);
    public event ProjectileHitAction hitSomething;
    
    public void Fire(Vector3 startPosition, Vector3 startDir) {
        position = startPosition;
        velocity = startDir.normalized*speed;
        lastPosition = startPosition-velocity*Time.fixedDeltaTime;
    }

    private class RaycastHitComparer : IComparer<RaycastHit> {
        public int Compare(RaycastHit x, RaycastHit y) {
            return x.distance.CompareTo(y.distance);
        }
    }

    private void Update() {
        float smoothing = 0.25f;
        double t = ((Time.timeAsDouble - smoothing*Time.fixedDeltaTime) - lastUpdateTime) / Time.fixedDeltaTime;
        transform.position = Vector3.LerpUnclamped(lastPosition, position, (float)t);
        transform.forward = velocity.normalized;
        transform.localScale = Vector3.one * radius + Vector3.forward * (velocity.magnitude*0.05f);
    }

    private void FixedUpdate() {
        Vector3 moveDelta = velocity * Time.deltaTime;
        int hits = Physics.SphereCastNonAlloc(new Ray(position, moveDelta.normalized), radius, raycastHits, moveDelta.magnitude);
        lastPosition = position;
        position += moveDelta;
        velocity += Physics.gravity * Time.deltaTime;
        lastUpdateTime = Time.timeAsDouble;
        if (hits == 0) {
            return;
        }
        Array.Sort(raycastHits, 0, hits, raycastHitComparer);
        if (raycastHits[0].rigidbody != null) {
            raycastHits[0].rigidbody.AddForce(velocity*4f, ForceMode.Impulse);
        }

        for(int i=0;i<hits;i++) {
            if (raycastHits[i].point != Vector3.zero) {
                hitSomething?.Invoke(raycastHits[i]);
            }
        }
        Destroy(gameObject);
    }
}
