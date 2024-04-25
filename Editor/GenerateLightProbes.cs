using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
public static class GenerateLightProbes {
    private const float quantizeStep = 0.25f;
    [MenuItem("Tools/ChurnVector/Generate Light Probes")]
    public static void Generate() {
        UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        GameObject lightProbes = GameObject.Find("Light Probe Group");
        if(lightProbes == null){
            lightProbes = new GameObject("Light Probe Group");
            lightProbes.AddComponent<LightProbeGroup>();
        }

        var triangulation = NavMesh.CalculateTriangulation();
        Dictionary<Vector3Int, Vector3> probeLocations = new Dictionary<Vector3Int, Vector3>();
        for (int i = 0; i < triangulation.indices.Length; i += 3) {
            var v0 = triangulation.vertices[triangulation.indices[i]];
            var v1 = triangulation.vertices[triangulation.indices[i+1]];
            var v2 = triangulation.vertices[triangulation.indices[i+2]];
            Subdivide(probeLocations, v0, v1, v2);
        }
        lightProbes.GetComponent<LightProbeGroup>().probePositions = probeLocations.Values.ToArray();
    }

    private static void AddPoint(Dictionary<Vector3Int,Vector3> probeLocations, Vector3 point) {
        Vector3Int quantized = Vector3Int.FloorToInt(new Vector3(point.x / quantizeStep, point.y / quantizeStep, point.z / quantizeStep));
        probeLocations[quantized] = point;
    }

    private static void Subdivide(Dictionary<Vector3Int,Vector3> probeLocations, Vector3 v0, Vector3 v1, Vector3 v2) {
        Vector3 normal = Vector3.Cross((v1 - v0).normalized, (v2 - v0).normalized).normalized;
        if (normal.y < .7f) {
            return;
        }
        if (Vector3.Distance(v0,v1) < 5f && Vector3.Distance(v0, v2) < 5f && Vector3.Distance(v1,v2) < 5f) {
            AddPoint(probeLocations, v0+normal*0.05f);
            AddPoint(probeLocations, v1+normal*0.05f);
            AddPoint(probeLocations, v2+normal*0.05f);
            AddPoint(probeLocations, v0 + Vector3.up * 3f);
            AddPoint(probeLocations, v1 + Vector3.up * 3f);
            AddPoint(probeLocations, v2 + Vector3.up * 3f);
            return;
        }
        Vector3 v01 = Vector3.Lerp(v0, v1, 0.5f);
        Vector3 v02 = Vector3.Lerp(v0, v2, 0.5f);
        Vector3 v12 = Vector3.Lerp(v1, v2, 0.5f);
        Subdivide(probeLocations, v0, v01, v02);
        Subdivide(probeLocations, v01, v1, v12);
        Subdivide(probeLocations, v02, v01, v12);
        Subdivide(probeLocations, v02, v12, v2);
    }
}

#endif