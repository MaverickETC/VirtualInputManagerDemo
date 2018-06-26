using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualButton))]
public class VirtualButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Property References ========== \\
        var ctrl = property.FindPropertyRelative("isUsingController").boolValue;

        var key = !ctrl ? property.FindPropertyRelative("key") : property.FindPropertyRelative("controllerButton");
        var time = property.FindPropertyRelative("holdTime").intValue;
        var timeS = property.FindPropertyRelative("holdTimeSeconds").floatValue;

        // Dimension Variables ========== \\
        float x = position.x; float y = position.y;
        float w = position.width; float h = position.height;
        float labelWidth = EditorGUIUtility.labelWidth / 2 + 10;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float lineSpace = EditorGUIUtility.standardVerticalSpacing;

        // Rects ========== \\
        Rect c1Rect = new Rect(x, y, labelWidth, h);
        Rect c2Rect = new Rect(x + labelWidth, y, w-labelWidth, h);

        Rect c2r1Rect = new Rect(c2Rect) { height = lineHeight };
        Rect c2r2Rect = new Rect(c2r1Rect) { y = c2r1Rect.y + lineHeight };

        // Styles ========== \\
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.UpperLeft,
        };
        GUIStyle timerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
        };

        // Draw ========== \\
        GUI.Label(c1Rect, label, labelStyle);
        EditorGUI.PropertyField(c2r1Rect, key,GUIContent.none);
        GUI.Label(c2r2Rect, time + " frames | " + timeS + " seconds", timerStyle);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}
