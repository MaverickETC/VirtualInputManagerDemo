using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualAxis))]
public class VirtualAxisDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property); // ==============================

        // Get child properties
        // var ID = property.FindPropertyRelative("joystickID");
        var isUsingController = property.FindPropertyRelative("isUsingController");

        var axisID = property.FindPropertyRelative("controllerAxisID");

        var positiveButton = property.FindPropertyRelative("positiveButton");
        var negativeButton = property.FindPropertyRelative("negativeButton");
        var axisPosition = property.FindPropertyRelative("position");

        // Set up property rects
        Rect negativeButtonRect = new Rect(position.x, position.y, position.width / 3, EditorGUIUtility.singleLineHeight);
        Rect axisPositionRect = new Rect(position.x + (position.width * 1 / 3), position.y, position.width / 3, EditorGUIUtility.singleLineHeight);
        Rect positiveButtonRect = new Rect(position.x + (position.width * 2 / 3), position.y, position.width / 3, EditorGUIUtility.singleLineHeight);

        Rect axisControllerIDRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
        Rect axisControllerPositionRect = new Rect(position.x + (position.width * 1 / 2), position.y, position.width / 2, EditorGUIUtility.singleLineHeight);

        if (!isUsingController.boolValue)
        {
            // Draw negative key property
            var key = negativeButton.FindPropertyRelative("key");
            key.intValue = (int)(KeyCode)EditorGUI.EnumPopup(negativeButtonRect, GUIContent.none, (KeyCode)key.intValue);

            // Draw axis position
            EditorGUI.ProgressBar(EditorGUI.IndentedRect(axisPositionRect), 0.5f * axisPosition.floatValue + 0.5f, "(-) " + label.text + " (+)");

            // Draw positive key property
            key = positiveButton.FindPropertyRelative("key");
            key.intValue = (int)(KeyCode)EditorGUI.EnumPopup(positiveButtonRect, GUIContent.none, (KeyCode)key.intValue);
        }
        else
        {
            // Draw controller axis dropdown
            axisID.intValue = EditorGUI.IntPopup(axisControllerIDRect, axisID.intValue, VirtualJoystick.ControllerAxisID.Values.ToArray(), VirtualJoystick.ControllerAxisID.Keys.ToArray());
            // Draw axis position
            EditorGUI.ProgressBar(EditorGUI.IndentedRect(axisControllerPositionRect), 0.5f * axisPosition.floatValue + 0.5f, "(-) " + label.text + " (+)");
        }
    }
}