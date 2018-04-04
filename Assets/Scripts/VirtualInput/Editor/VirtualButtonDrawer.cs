using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualButton))]
public class VirtualButtonDrawer : PropertyDrawer
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
        var joystickID = property.FindPropertyRelative("joystickID");
        var isUsingController = property.FindPropertyRelative("isUsingController");

        var key = property.FindPropertyRelative("key");
        var holdTime = property.FindPropertyRelative("holdTime");
        var holdTimeSeconds = property.FindPropertyRelative("holdTimeSeconds");
        
        // Set up property rects
        Rect labelRect = new Rect(position.x, position.y, position.width/3, EditorGUIUtility.singleLineHeight);
        Rect keyRect = new Rect(position.x + (position.width * 1 / 3), position.y, position.width/3, EditorGUIUtility.singleLineHeight);
        Rect timeRect = new Rect(position.x + (position.width * 2 / 3), position.y, position.width/3, EditorGUIUtility.singleLineHeight);

        // Set up controller dropdown values for when controllers are plugged in
        int id = joystickID.intValue + 1;
        int controllerButtonOptionsCount = 10;

        string[] controllerButtonOptionLabels = new string[controllerButtonOptionsCount];
        int[] controllerButtonOptionValues = new int[controllerButtonOptionsCount];

        for (int i = 0; i < controllerButtonOptionsCount; i++)
        {
            string XboxControllerTranslation = "";
            switch (i)
            {
                case 0: XboxControllerTranslation = "A"; break;
                case 1: XboxControllerTranslation = "B"; break;
                case 2: XboxControllerTranslation = "X"; break;
                case 3: XboxControllerTranslation = "Y"; break;
                case 4: XboxControllerTranslation = "Left Bumper"; break;
                case 5: XboxControllerTranslation = "Right Bumper"; break;
                case 6: XboxControllerTranslation = "Back"; break;
                case 7: XboxControllerTranslation = "Start"; break;
                case 8: XboxControllerTranslation = "Left Stick Click"; break;
                case 9: XboxControllerTranslation = "Right Stick Click"; break;
                default: XboxControllerTranslation = "?"; break;
            }
            controllerButtonOptionLabels[i] = "Joystick " + id + " Button " + i + " (" + XboxControllerTranslation + ")";
            controllerButtonOptionValues[i] = (int)(KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button" + i);
        }

        // Draw label property
        EditorGUI.LabelField(labelRect, label);

        // Draw key property
        if (!isUsingController.boolValue)
            key.intValue = (int)(KeyCode)EditorGUI.EnumPopup(keyRect, GUIContent.none, (KeyCode)key.intValue);
        else
            key.intValue = EditorGUI.IntPopup(keyRect, key.intValue, controllerButtonOptionLabels, controllerButtonOptionValues);

        // Draw label property
        string timer = string.Format("{0} frames | {1} seconds", holdTime.intValue, Mathf.Round(holdTimeSeconds.floatValue * 100) / 100);
        EditorGUI.LabelField(timeRect, timer);

        EditorGUI.EndProperty(); // ==============================
    }
}