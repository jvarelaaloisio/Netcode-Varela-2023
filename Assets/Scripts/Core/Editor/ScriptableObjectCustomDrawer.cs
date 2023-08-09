using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectCustomDrawer : PropertyDrawer
{
    private Editor editor = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        
        if (property.objectReferenceValue == null)
            return;
        
        Rect foldoutPosition = new Rect(position.x + EditorGUIUtility.labelWidth - 15, position.y, 15, position.height);
        property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, GUIContent.none);

        if (property.isExpanded)
        {
            // Make child fields be indented
            EditorGUI.indentLevel++;

            if (!editor)
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
            editor.OnInspectorGUI();

            EditorGUI.indentLevel--;
        }
    }
}