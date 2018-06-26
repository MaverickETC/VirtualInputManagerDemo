using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    InputManager Manager { get { return (InputManager)target; } }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var joystickCollection = serializedObject.FindProperty("joystickCollection");

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Add Joystick"))
                AddJoystick();
            if (GUILayout.Button("Remove All Joysticks"))
                RemoveAllJoysticks();
        }

        for (int i = 0; i < joystickCollection.arraySize; i++)
        {
            using (new EditorGUILayout.VerticalScope(boxStyle))
            {
                var joystick = joystickCollection.GetArrayElementAtIndex(i);
                joystick.FindPropertyRelative("ID").intValue = i;
                EditorGUILayout.PropertyField(joystick, new GUIContent("Joystick " + i), true);
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Add a joystick to the collection
    /// </summary>
    void AddJoystick()
    {
        InputManager.AddJoystick();
    }
    /// <summary>
    /// Add a joystick to the collection
    /// </summary>
    void RemoveAllJoysticks()
    {
        InputManager.RemoveAllJoysticks();
    }
}