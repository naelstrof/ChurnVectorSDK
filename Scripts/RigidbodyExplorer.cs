using System.Globalization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class RigidbodyExplorer : ScriptableWizard {
    [MenuItem("Tools/Naelstrof/Rigidbody Explorer")]
    static void CreateWizard() {
        DisplayWizard<RigidbodyExplorer>("Rigidbody Explorer", "Close");
    }

    private float maxDiff = 1f;

    [SerializeField]
    private PhysicsMaterialExtensionDatabase physicsDatabase;
    protected override bool DrawWizardGUI() {
        bool changed = base.DrawWizardGUI();
        
        if (physicsDatabase == null) {
            return true;
        }
        
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        GUILayout.Button("", GUILayout.Width(24));
        GUI.enabled = true;
        EditorGUILayout.LabelField("Name");
        EditorGUILayout.LabelField("Real Mass");
        EditorGUILayout.LabelField("Mass Estimation");
        GUI.enabled = false;
        GUILayout.Button("fix", GUILayout.Width(24));
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        foreach (var rigidbody in FindObjectsOfType<Rigidbody>(false)) {
            if (rigidbody.GetComponentInParent<CharacterBase>() != null) {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("", GUILayout.Width(24))) {
                Selection.activeGameObject = rigidbody.gameObject;
            }

            EditorGUILayout.LabelField(rigidbody.name);
            float newMass = EditorGUILayout.FloatField(rigidbody.mass);
            if (newMass != rigidbody.mass) {
                rigidbody.mass = newMass;
                EditorUtility.SetDirty(rigidbody);
            }

            float probableMass = 0f;
            foreach (var collider in rigidbody.GetComponentsInChildren<Collider>(true)) {
                probableMass += (collider.bounds.size.x * collider.bounds.size.y * collider.bounds.size.z)*physicsDatabase.GetMassPerCubicMeter(collider.sharedMaterial);
            }

            probableMass *= 0.1f;

            float diff = Mathf.Abs(probableMass - newMass);
            if (maxDiff != Mathf.Max(diff, maxDiff)) {
                maxDiff = Mathf.Max(diff,maxDiff);
                changed = true;
            }

            float lerpT = maxDiff < 0.01 ? 0f : diff / maxDiff;
            GUIStyle s = new GUIStyle(EditorStyles.textField) {
                normal = {
                    textColor = Color.Lerp(Color.white, Color.red, lerpT)
                }
            };

            EditorGUILayout.LabelField(probableMass.ToString(CultureInfo.CurrentCulture), s);
            if (diff < 0.001f) {
                GUI.enabled = false;
            }
            if (GUILayout.Button("fix", GUILayout.Width(24))) {
                rigidbody.mass = probableMass;
                EditorUtility.SetDirty(rigidbody);
                maxDiff = 0f;
                changed = true;
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        return changed;
    }
}

#endif