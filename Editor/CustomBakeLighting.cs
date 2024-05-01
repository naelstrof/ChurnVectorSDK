using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AI;

public class CustomBakeLighting : MonoBehaviour {
    [MenuItem("Tools/ChurnVector/Update Lighting")]
    public static void BakeLighting() {
        #if RealtimeCSG
        RealtimeCSG.CSGModelManager.BuildLightmapUvs();
        #endif
        GenerateLightProbes.Generate();
        Lightmapping.Clear();
        Lightmapping.BakeAsync();
    }

    #if RealtimeCSG
    [MenuItem("Tools/ChurnVector/Force CSG Rebuild")]
    public static void ForceRebuild() {
        RealtimeCSG.CSGModelManager.ForceRebuild();
    }
    #endif
}

#endif
