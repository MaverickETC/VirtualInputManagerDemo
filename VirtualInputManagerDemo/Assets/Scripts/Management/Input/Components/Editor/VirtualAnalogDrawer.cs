using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualAnalog))]
public class VirtualAnalogDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Property References ========== \\
        var ctrl = property.FindPropertyRelative("isUsingController").boolValue;

        var xAxisPositive =
            property.FindPropertyRelative("xAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
        var xAxisNegative =
            property.FindPropertyRelative("xAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");
        var yAxisPositive =
            property.FindPropertyRelative("yAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
        var yAxisNegative =
            property.FindPropertyRelative("yAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");
        
        var analogButton =
            property.FindPropertyRelative("button").FindPropertyRelative("key");
        
        var controllerAnalog =
            property.FindPropertyRelative("controllerAnalog");
        var controllerAnalogButton =
            property.FindPropertyRelative("controllerAnalogButton");

        var dir =
            property.FindPropertyRelative("direction").vector2Value;

        // Dimension Variables ========== \\
        float x = position.x; float y = position.y;
        float w = position.width; float h = position.height;
        float labelWidth = EditorGUIUtility.labelWidth / 2 + 10;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float lineSpace = EditorGUIUtility.standardVerticalSpacing;
        
        // Rects ========== \\
        Rect c1Rect = new Rect(x, y, labelWidth, h) ;
        Rect c2Rect = new Rect(x + labelWidth, y, h, h);
        Rect c3Rect = new Rect(c2Rect.xMax, y, w - (c1Rect.width + c2Rect.width), h);

        Rect c3r1Rect = new Rect(c3Rect) { height = lineHeight };
        Rect c3r2Rect = new Rect(c3r1Rect) { y = c3r1Rect.yMax };
        Rect c3r3Rect = new Rect(c3r2Rect) { y = c3r2Rect.yMax };

        Rect ax1Rect = new Rect(c3r1Rect) { width = c3r1Rect.width / 2 };
        Rect ax2Rect = new Rect(ax1Rect) { x = ax1Rect.xMax };

        Rect ay1Rect = new Rect(ax1Rect) { y = ax1Rect.yMax };
        Rect ay2Rect = new Rect(ax2Rect) { y = ax2Rect.yMax };

        Rect posRect = new Rect(c2Rect);
        posRect.size /= 5;
        posRect.center = c2Rect.center + new Vector2(dir.x, -dir.y) * (c2Rect.width / 2 - posRect.width / 2);

        // Styles ========== \\
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.UpperLeft,
        };

        // Draw ========== \\
        GUI.Label(c1Rect, label, labelStyle);
        GUI.Box(c2Rect, "");
        Color _temp = GUI.backgroundColor;
        GUI.backgroundColor = Color.blue;
        GUI.Box(posRect, "");
        GUI.backgroundColor = _temp;

        if (ctrl)
        {
            EditorGUI.PropertyField(c3r2Rect, controllerAnalog, GUIContent.none);
            EditorGUI.PropertyField(c3r3Rect, controllerAnalogButton, GUIContent.none);
        }
        else
        {
            EditorGUI.PropertyField(ax1Rect, xAxisNegative, GUIContent.none);
            EditorGUI.PropertyField(ax2Rect, xAxisPositive, GUIContent.none);
            EditorGUI.PropertyField(ay1Rect, yAxisNegative, GUIContent.none);
            EditorGUI.PropertyField(ay2Rect, yAxisPositive, GUIContent.none);
            EditorGUI.PropertyField(c3r3Rect, analogButton, GUIContent.none);
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 3 + EditorGUIUtility.standardVerticalSpacing;
    }
}
