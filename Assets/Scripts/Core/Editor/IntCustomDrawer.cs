using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// [CustomPropertyDrawer(typeof(int))]
public class IntCustomDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUILayout.Label($"My int: {property.displayName}");
        property.intValue
            = EditorGUILayout.IntField("value", property.intValue);
        EditorGUI.HelpBox(GUILayoutUtility.GetRect(100, 100, 100, 100),
                          "This is a message",
                          MessageType.Error);
    }
}
