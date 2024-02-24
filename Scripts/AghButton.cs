using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager;

public static class AghButton {
    [MenuItem("Tools/Naelstrof/FindInvalidTransforms")]
    public static void FindInvalidTransform() {
        foreach (GameObject g in Object.FindObjectsOfType<GameObject>(true)) {
            Quaternion checkQuat = g.transform.rotation;
            Vector3 checkPos = g.transform.position;
            if (float.IsNaN(checkQuat.x) ||
                float.IsNaN(checkQuat.y) ||
                float.IsNaN(checkQuat.z) ||
                float.IsNaN(checkQuat.w) ||
                float.IsNaN(checkPos.x) ||
                float.IsNaN(checkPos.y) ||
                float.IsNaN(checkPos.z)) {
                Selection.activeObject = g;
                Debug.Log("Found invalid transform!", g);
                return;
            }
        }

        Debug.Log("None found!");
    }

    [MenuItem("Tools/ChurnVector/Setup Selected Map Model")]
    public static void SetupMapModel() {
        PhysicsMaterialExtensionDatabase database = AssetDatabase.LoadAssetAtPath<PhysicsMaterialExtensionDatabase>( AssetDatabase.GUIDToAssetPath("aee73d22208ecde4d8ac1e8e7e4a29dd"));
        List<Renderer> mapRenderers = new List<Renderer>(Selection.activeGameObject.GetComponentsInChildren<Renderer>());
        foreach (var r in mapRenderers) {
            if (r is not MeshRenderer meshRenderer) continue;
            var meshFilter = r.gameObject.GetComponent<MeshFilter>();
            var meshCollider = r.gameObject.GetComponent<MeshCollider>();
            if (meshCollider == null) {
                meshCollider = r.gameObject.AddComponent<MeshCollider>();
            }
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshCollider.sharedMaterial = database.GetPhysicMaterial(r.sharedMaterial);
            var decalableMap = r.gameObject.GetComponent<DecalableCollider>();
            if (decalableMap == null) {
                decalableMap = r.gameObject.AddComponent<DecalableCollider>();
            }
            decalableMap.SetDecalableRenderers(new []{r});
        }
    }

    private static int RTLFix(TMP_Text t) {
        if (t.GetComponent<TextRTLFixer>() != null) {
            return 0;
        }
        Undo.RecordObject(t.gameObject, "added component");
        GameObject gameObject = t.gameObject;
        gameObject.AddComponent<TextRTLFixer>();
        EditorUtility.SetDirty(gameObject);
        return 1;
    }

    [MenuItem("Tools/Naelstrof/Text RTL Fixer")]
    public static void RTLFixer() {
        int fixes = 0;
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Added components");
        var undoIndex = Undo.GetCurrentGroup();
        foreach(GameObject g in Selection.gameObjects) {
            foreach(TMP_Text t in g.GetComponentsInChildren<TMP_Text>(true)) {
                fixes += RTLFix(t);
            }
        }
        foreach (var g in Object.FindObjectsOfType<GameObject>()) {
            foreach (TMP_Text t in g.GetComponentsInChildren<TMP_Text>(true)) {
                fixes += RTLFix(t);
            }
        }
        string[] pathsToAssets = AssetDatabase.FindAssets("t:GameObject");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
            foreach(TMP_Text t in go.GetComponentsInChildren<TMP_Text>(true)) {
                if (t.GetComponent<TextRTLFixer>() == null) {
                    Selection.activeGameObject = go;
                    Debug.Log($"Fixed {fixes} text mesh pros, need assistance, open this prefab and run again.");
                    return;
                }
                fixes += RTLFix(t);
            }
        }
        Undo.CollapseUndoOperations(undoIndex);
        Debug.Log($"Fixed {fixes} text mesh pros, none found left!");
    }

    [MenuItem("Tools/Naelstrof/HomogenizeButtons")]
    public static void HomogenizeButtons() {
        ColorBlock block = new ColorBlock {
            normalColor = new Color(0.9117684f,0.7877358f,1f),
            colorMultiplier = 1f,
            highlightedColor = new Color(0.8737137f, 0.5687505f,0.9056604f),
            pressedColor = new Color(0.485111f,0.4309808f, 0.6132076f),
            selectedColor = new Color(0.7538508f, 0.6062656f, 0.9811321f),
            disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f)
        };
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Changed button colors");
        var undoIndex = Undo.GetCurrentGroup();
        foreach(GameObject g in Selection.gameObjects) {
            foreach(Button b in g.GetComponentsInChildren<Button>(true)) {
                Undo.RecordObject(b, "Changed button color");
                b.colors = block;
                EditorUtility.SetDirty(b);
            }
            foreach(TMP_Dropdown d in g.GetComponentsInChildren<TMP_Dropdown>(true)) {
                Undo.RecordObject(d, "Changed button color");
                d.colors = block;
                EditorUtility.SetDirty(d);
            }
            foreach(Slider s in g.GetComponentsInChildren<Slider>(true)) {
                Undo.RecordObject(s, "Changed button color");
                s.colors = block;
                EditorUtility.SetDirty(s);
            }

            foreach (Toggle t in g.GetComponentsInChildren<Toggle>(true)) {
                Undo.RecordObject(t, "Changed button color");
                t.colors = block;
                EditorUtility.SetDirty(t);
            }
        }
        Undo.CollapseUndoOperations(undoIndex);
    }
    //[MenuItem("Tools/KoboldKare/Disable all GPU Instanced Materials")]
    public static void FindGPUInstancedMaterial() {
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Changed project gpu instancing");
        var undoIndex = Undo.GetCurrentGroup();
        string[] pathsToAssets = AssetDatabase.FindAssets("t:Material");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<Material>(path1);
            if (go.enableInstancing) {
                Undo.RecordObject(go, "Changed project gpu instancing");
                go.enableInstancing = false;
                EditorUtility.SetDirty(go);
            }
        }
        Undo.CollapseUndoOperations(undoIndex);
    }
    //[MenuItem("Tools/KoboldKare/Find Specular Workflow Materials")]
    public static void FindSpecularWorkflowMaterials() {
        string[] pathsToAssets = AssetDatabase.FindAssets("t:Material");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<Material>(path1);
            if (go.IsKeywordEnabled("_SPECULAR_SETUP")) {
                Selection.activeObject = go;
                return;
            }
        }
    }
    //[MenuItem("Tools/KoboldKare/Enable all environment reflections, specular highlights (to reduce shader variants.)")]
    public static void FindSpecularOffMaterials() {
        string[] pathsToAssets = AssetDatabase.FindAssets("t:Material");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var check = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(path1);
            if (check != null && check.source != PackageSource.Local && check.source != PackageSource.Embedded) {
                continue;
            }

            string reason = "";
            var go = AssetDatabase.LoadAssetAtPath<Material>(path1);
            if (go.IsKeywordEnabled("_SPECULARHIGHTLIGHTS_OFF")) {
                go.DisableKeyword("_SPECULARHIGHTLIGHTS_OFF");
                reason = "_SPECULARHIGHTLIGHTS_OFF";
            }
            if (go.IsKeywordEnabled("_RECEIVE_SHADOWS_OFF")) {
                go.DisableKeyword("_RECEIVE_SHADOWS_OFF");
                reason = "_RECEIVE_SHADOWS_OFF";
            }
            if (go.IsKeywordEnabled("_ENVIRONMENTREFLECTIONS_OFF")) {
                go.DisableKeyword("_ENVIRONMENTREFLECTIONS_OFF");
                reason = "_ENVIRONMENTREFLECTIONS_OFF";
            }
            if (go.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON")) {
                go.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                reason = "_ALPHAPREMULTIPLY_ON";
            }
            if (go.IsKeywordEnabled("_OCCLUSIONMAP")) {
                go.DisableKeyword("_OCCLUSIONMAP");
                reason = "_OCCLUSIONMAP";
            }
            if (go.IsKeywordEnabled("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A")) {
                go.DisableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                reason = "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A";
            }

            if (!string.IsNullOrEmpty(reason)) {
                EditorUtility.SetDirty(go);
                Debug.Log($"Selecting {go} because {reason}");
                Selection.activeObject = go;
                return;
            }
        }

        Debug.Log("No found errornous materials.");
    }
    //[MenuItem("Tools/KoboldKare/Find Spacialized Audio")]
    public static void FindSpacializedAudio() {
        int count = 0;
        foreach(GameObject g in Selection.gameObjects) {
            foreach(var c in g.GetComponents<AudioSource>()) {
                if (c.spatialize) {
                    c.spatialize = false;
                    count++;
                    Selection.activeGameObject = g;
                    EditorUtility.SetDirty(g);
                    EditorUtility.SetDirty(c);
                }
            }
        }
        foreach(var g in Object.FindObjectsOfType<GameObject>()) {
            foreach(var c in g.GetComponents<AudioSource>()) {
                if (c.spatialize) {
                    c.spatialize = false;
                    count++;
                    Selection.activeGameObject = g;
                    EditorUtility.SetDirty(g);
                    EditorUtility.SetDirty(c);
                }
            }
        }
        string[] pathsToAssets = AssetDatabase.FindAssets("t:GameObject");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
            foreach(var c in go.GetComponentsInChildren<AudioSource>(true)) {
                if (c.spatialize) {
                    c.spatialize = false;
                    count++;
                    Selection.activeGameObject = go;
                }
            }
        }
        Debug.Log("Found and attempted to fix " + count + " audio sources.");
    }
    [MenuItem("Tools/Naelstrof/Find Missing Script")]
    public static void FindMissingScript() {
        foreach(GameObject g in Selection.gameObjects) {
            foreach(var c in g.GetComponents<Component>()) {
                if (c == null) {
                    if (g.hideFlags == HideFlags.HideAndDontSave || g.hideFlags == HideFlags.HideInHierarchy || g.hideFlags == HideFlags.HideInInspector) {
                        Debug.Log("Found hidden gameobject with a missing script, deleted " + g);
                        Object.DestroyImmediate(g);
                        continue;
                    }
                    Selection.activeGameObject = g;
                    return;
                }
            }
        }
        foreach(var g in Object.FindObjectsOfType<GameObject>()) {
            foreach(var c in g.GetComponents<Component>()) {
                if (c == null) {
                    if (g.hideFlags == HideFlags.HideAndDontSave || g.hideFlags == HideFlags.HideInHierarchy || g.hideFlags == HideFlags.HideInInspector) {
                        Debug.Log("Found hidden gameobject with a missing script, deleted " + g);
                        Object.DestroyImmediate(g);
                        continue;
                    }
                    Selection.activeGameObject = g;
                    return;
                }
            }
        }
        string[] pathsToAssets = AssetDatabase.FindAssets("t:GameObject");
        foreach (var path in pathsToAssets) {
            var path1 = AssetDatabase.GUIDToAssetPath(path);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
            foreach(var c in go.GetComponentsInChildren<Component>(true)) {
                if (c == null) {
                    Selection.activeGameObject = go;
                    return;
                }
            }
        }
        Debug.Log("No missing scripts found anywhere! Good job.");
    }
}

#endif