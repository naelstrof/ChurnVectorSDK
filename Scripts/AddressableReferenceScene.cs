#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceScene : AssetReference {
    [SerializeField]
    private string name;
    public string GetName() => name;
    public override bool ValidateAsset(Object obj) {
#if UNITY_EDITOR
        var type = obj.GetType();
        return typeof(SceneAsset).IsAssignableFrom(type);
#else
            return false;
#endif
    }
    public override bool ValidateAsset(string path) {
#if UNITY_EDITOR
        var type = AssetDatabase.GetMainAssetTypeAtPath(path);
        return typeof(SceneAsset).IsAssignableFrom(type);
#else
            return false;
#endif
    }
    public void OnValidate() {
#if UNITY_EDITOR
        if (editorAsset == null) return;
        name = editorAsset.name;
#endif
    }
}