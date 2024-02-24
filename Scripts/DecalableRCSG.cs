using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DecalableRCSG : MonoBehaviour {
    [SerializeField]
    private GameObject meshParent;
    [SerializeField]
    private PhysicsMaterialExtensionDatabase database;
    void Start() {
        var thing = transform.Find("[generated-meshes]");
        if (meshParent == null && thing != null) {
            meshParent = thing.gameObject;
        }
        
        List<Renderer> mapRenderers = new List<Renderer>(meshParent.GetComponentsInChildren<Renderer>());
        foreach (var r in mapRenderers) {
            if (r is MeshRenderer meshRenderer) {
                var meshFilter = r.gameObject.GetComponent<MeshFilter>();
                var meshCollider = r.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.sharedMesh;
                meshCollider.sharedMaterial = database.GetPhysicMaterial(r.material);
                var decalableMap = r.gameObject.AddComponent<DecalableCollider>();
                decalableMap.SetDecalableRenderers(new []{r});
            }
        }
    }

    private void OnValidate() {
#if UNITY_EDITOR
        // Can't be static, or footsteps fail!
        if (!Application.isPlaying) {
            var staticFlags = GameObjectUtility.GetStaticEditorFlags(gameObject);
            GameObjectUtility.SetStaticEditorFlags(gameObject, staticFlags & ~StaticEditorFlags.BatchingStatic);
        }
#endif
        var thing = transform.Find("[generated-meshes]");
        if (thing == null) {
            return;
        }
        if (meshParent == null) {
            meshParent = thing.gameObject;
        }
    }
}
