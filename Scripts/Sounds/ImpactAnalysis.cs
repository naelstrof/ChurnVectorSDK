using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public struct ImpactAnalysis {
    private static ContactPoint[] contacts = new ContactPoint[32];
    private ContactPoint point;
    private float impactMagnitude;
    public ContactPoint GetContact() => point;
    public float GetImpactMagnitude() => impactMagnitude;
    public ImpactAnalysis(Rigidbody body, Collision collision) {
        float impactSpeed = 0f;
        int count = collision.GetContacts(contacts);
        for(int i=0;i<count;i++) {
            impactSpeed += Mathf.Abs(Vector3.Dot(contacts[i].normal, collision.relativeVelocity));
        }
        impactSpeed /= count;
        
        point = contacts[0];
        float impactImpulse = collision.impulse.magnitude/body.mass;
        impactMagnitude = Mathf.Min(impactSpeed, impactImpulse * 0.1f);
    }

    public static void DrawHandles(ImpactAnalysis analysis, Color color) {
        #if UNITY_EDITOR
        Handles.color = color;
        Handles.Label(analysis.point.point, $"{analysis.impactMagnitude:0.##}");
        Handles.DrawLine(analysis.point.point, analysis.point.point + analysis.point.normal * 0.1f);
        #endif
    }

    public override string ToString() {
        return $"Impact Magnitude: {impactMagnitude}";
    }
}
