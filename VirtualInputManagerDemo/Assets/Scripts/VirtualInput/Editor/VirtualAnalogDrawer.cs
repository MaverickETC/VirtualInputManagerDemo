using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualAnalog))]
public class VirtualAnalogDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 4;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property); // ==============================

        // Get child properties
        // var ID = property.FindPropertyRelative("joystickID");
        var isUsingController = property.FindPropertyRelative("isUsingController");

        var analogID = property.FindPropertyRelative("controllerAnalogID");

        var xAxis = property.FindPropertyRelative("xAxis");
        var yAxis = property.FindPropertyRelative("yAxis");
        var analogDirection = property.FindPropertyRelative("direction");

        // Set up analog rects
        Rect analogAreaRect = new Rect(position);
        analogAreaRect.width = analogAreaRect.height;
        Vector2 c = analogAreaRect.center;
        analogAreaRect.size *= 0.8f;
        analogAreaRect.center = c;

        Rect analogPositionRect = new Rect(analogAreaRect);
        analogPositionRect.size *= 0.2f;
        Vector2 direction = new Vector2(analogDirection.vector2Value.x, -analogDirection.vector2Value.y);
        float scaleFactor = 15;
        analogPositionRect.center = analogAreaRect.center + (direction * scaleFactor);

        // Set up position rects
        Rect line1Rect = new Rect(analogAreaRect.x + analogAreaRect.width + 10, position.y, position.width - analogAreaRect.width - 20, EditorGUIUtility.singleLineHeight);
        Rect line2Rect = new Rect(line1Rect); line2Rect.y += line1Rect.height;
        Rect line3Rect = new Rect(line2Rect); line3Rect.y += line2Rect.height;
        Rect line4Rect = new Rect(line3Rect); line4Rect.y += line3Rect.height;

        Rect xAxis_nKeyRect = new Rect(line2Rect.x, line2Rect.y, line2Rect.width / 2, line2Rect.height);
        Rect xAxis_pKeyRect = new Rect(line2Rect.x + (line2Rect.width / 2), line2Rect.y, line2Rect.width / 2, line2Rect.height);
        Rect yAxis_nKeyRect = new Rect(line4Rect.x, line4Rect.y, line4Rect.width / 2, line4Rect.height);
        Rect yAxis_pKeyRect = new Rect(line4Rect.x + (line4Rect.width / 2), line4Rect.y, line4Rect.width / 2, line4Rect.height);

        Rect analogControllerIDRect = new Rect(line3Rect);
        Rect analogControllerLabelRect = new Rect(line2Rect);

        // Draw analog visualization
        EditorGUI.DrawRect(analogAreaRect, Color.black);
        EditorGUI.DrawRect(analogPositionRect, Color.white);

        if (!isUsingController.boolValue)
        {
            // Draw x-axis label
            EditorGUI.LabelField(line1Rect, "(-) X-Axis (+)");

            // Draw x-axis property
            var pKey = xAxis.FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var nKey = xAxis.FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            nKey.intValue = (int)(KeyCode)EditorGUI.EnumPopup(xAxis_nKeyRect, GUIContent.none, (KeyCode)nKey.intValue);
            pKey.intValue = (int)(KeyCode)EditorGUI.EnumPopup(xAxis_pKeyRect, GUIContent.none, (KeyCode)pKey.intValue);

            // Draw y-axis label
            EditorGUI.LabelField(line3Rect, "(-) Y-Axis (+)");

            // Draw y-axis property
            pKey = yAxis.FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            nKey = yAxis.FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            nKey.intValue = (int)(KeyCode)EditorGUI.EnumPopup(yAxis_nKeyRect, GUIContent.none, (KeyCode)nKey.intValue);
            pKey.intValue = (int)(KeyCode)EditorGUI.EnumPopup(yAxis_pKeyRect, GUIContent.none, (KeyCode)pKey.intValue);
        }
        else
        {
            // Draw controller analog label
            EditorGUI.LabelField(analogControllerLabelRect, "Using Analog:");
            // Draw controller analog dropdown
            analogID.intValue = EditorGUI.IntPopup(analogControllerIDRect, analogID.intValue, VirtualJoystick.ControllerAnalogID.Values.ToArray(), VirtualJoystick.ControllerAnalogID.Keys.ToArray());
        }

        EditorGUI.EndProperty(); // ==============================

    }
}