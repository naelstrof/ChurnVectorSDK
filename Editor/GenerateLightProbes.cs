using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
public static class GenerateLightProbes {
    public static void Generate() {
        GameObject lightProbes = GameObject.Find("Light Probe Group");
        if(lightProbes == null){
            lightProbes = new GameObject("Light Probe Group");
            lightProbes.AddComponent<LightProbeGroup>();
        }

        var triangulation = NavMesh.CalculateTriangulation();
        List<Vector3> probeLocations = new List<Vector3>();
        for (int i = 0; i < triangulation.indices.Length; i += 3) {
            var v1 = triangulation.vertices[triangulation.indices[i]];
            var v2 = triangulation.vertices[triangulation.indices[i+1]];
            var v3 = triangulation.vertices[triangulation.indices[i+2]];
            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
            if (normal.y < .7f) {
                continue;
            }
            probeLocations.Add(v1);
            probeLocations.Add(v2);
            probeLocations.Add(v3);
            var aDist = Vector3.Distance(v1,v2);
            for (float f = 0f; f < 1f; f += 5f / aDist) {
                probeLocations.Add(Vector3.Lerp(v1,v2,f));
                probeLocations.Add(Vector3.Lerp(v1,v2,f)+Vector3.up*3f);
            }
            var bDist = Vector3.Distance(v2,v3);
            for (float f = 0f; f < 1f; f += 5f / bDist) {
                probeLocations.Add(Vector3.Lerp(v2,v3,f));
                probeLocations.Add(Vector3.Lerp(v2,v3,f)+Vector3.up*3f);
            }
            var cDist = Vector3.Distance(v1,v3);
            for (float f = 0f; f < 1f; f += 5f / cDist) {
                probeLocations.Add(Vector3.Lerp(v1,v3,f));
                probeLocations.Add(Vector3.Lerp(v2,v3,f)+Vector3.up*3f);
            }
        }
        lightProbes.GetComponent<LightProbeGroup>().probePositions = probeLocations.ToArray();
    }
}

#endif