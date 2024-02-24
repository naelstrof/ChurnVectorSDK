#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class WorldImporter : AssetPostprocessor {
    private void OnPostprocessModel(GameObject g) {
        if (g.name == "World") {
            SetLayerRecursive(g.transform, LayerMask.NameToLayer("World"));
        }
    }

    private void SetLayerRecursive(Transform target, int layer) {
        foreach (Transform t in target) {
            if (t == target) {
                continue;
            }

            SetLayerRecursive(t, layer);
        }
        target.gameObject.layer = layer;
    }
}
#endif