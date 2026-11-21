
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HiddenObjective))]
public class HiddenObjectiveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            Rect rect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            SerializedProperty child = property.Copy();
            SerializedProperty endChild = child.GetEndProperty();

            child.NextVisible(true);

            while (!SerializedProperty.EqualContents(child, endChild))
            {
                if (IsDisplayedName(child.name))
                {
                    rect.height = EditorGUI.GetPropertyHeight(child, true);

                    EditorGUI.PropertyField(rect, child, true);

                    rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
                }

                if (!child.NextVisible(false))
                    break;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        float totalHeight = 0f;

        SerializedProperty child = property.Copy();
        SerializedProperty endChild = child.GetEndProperty();

        child.NextVisible(true);

        while (!SerializedProperty.EqualContents(child, endChild))
        {
            if (IsDisplayedName(child.name))
            {
                totalHeight += EditorGUI.GetPropertyHeight(child, true)
                    + EditorGUIUtility.standardVerticalSpacing;
            }

            if (!child.NextVisible(false))
                break;
        }

        return totalHeight;
    }

    private bool IsDisplayedName(string name)
    {
        return name != "objectiveLabelLocalizedString" &&
               name != "objectiveDescriptionLocalizedString" &&
               name != "primaryObjective";
    }
}
#endif
