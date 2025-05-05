using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class OverrideLocks
{
    [SerializeField] private int enabledLocks;

    public int Locks => enabledLocks;
}

[CustomPropertyDrawer(typeof(OverrideLocks))]
public class MultiSelectExampleDrawer : PropertyDrawer
{
    private readonly string[] options = { "Starting Action", "Override Groups" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty locksProperty = property.FindPropertyRelative("enabledLocks");

        if (locksProperty != null)
        {
            locksProperty.intValue = EditorGUI.MaskField(position, label, locksProperty.intValue, options);
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Expected 'enabledLocks' field not found.");
        }
    }
}