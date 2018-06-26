using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualAxis))]
public class VirtualAxisDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Property References ========== \\
        var ctrl = property.FindPropertyRelative("isUsingController").boolValue;

        var pButton =
            property.FindPropertyRelative("positiveButton").FindPropertyRelative("key");
        var nButton =
            property.FindPropertyRelative("negativeButton").FindPropertyRelative("key");
        var controllerAxis =
            property.FindPropertyRelative("controllerAxis");
        var axisPosition =
            property.FindPropertyRelative("position");

        // Dimension Variables ========== \\
        float x = position.x; float y = position.y;
        float w = position.width; float h = position.height;
        float labelWidth = EditorGUIUtility.labelWidth / 2 + 10;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float lineSpace = EditorGUIUtility.standardVerticalSpacing;

        // Rects ========== \\
        Rect c1Rect = new Rect(x, y, labelWidth, h);
        Rect c2Rect = new Rect(x + labelWidth, y, w - labelWidth, h);

        Rect c2r1Rect = new Rect(c2Rect) { height = lineHeight };
        Rect c2r2Rect = new Rect(c2r1Rect) { y = c2r1Rect.y + lineHeight };
        
        Rect b1Rect = new Rect(c2r1Rect) { width = c2r1Rect.width / 2 };
        Rect b2Rect = new Rect(b1Rect) { x = b1Rect.xMax };

        // Styles ========== \\
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.UpperLeft,
        };

        // Draw ========== \\
        GUI.Label(c1Rect, label, labelStyle);
        GUI.HorizontalSlider(c2r2Rect, axisPosition.floatValue, -1, 1);

        if (ctrl)
            EditorGUI.PropertyField(c2r1Rect, controllerAxis, GUIContent.none);
        else
        {
            EditorGUI.PropertyField(b1Rect, nButton, GUIContent.none);
            EditorGUI.PropertyField(b2Rect, pButton, GUIContent.none);
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}
