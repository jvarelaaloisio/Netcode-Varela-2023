using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditableObject))]
public class EditableObjectCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        //---->
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.IntField("Current HP",
                                 ((EditableObject)target).CurrentHp);
        EditorGUI.EndDisabledGroup();
        //<----
        if (GUILayout.Button("click me"))
        {
            Debug.Log($"you clicked me");
        }
        GUILayout.EndHorizontal();
    }
}
