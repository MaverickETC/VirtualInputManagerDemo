using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualJoystick))]
public class VirtualJoystickDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var analogList = property.FindPropertyRelative("analogList");
        var axisList = property.FindPropertyRelative("axisList");
        var buttonList = property.FindPropertyRelative("buttonList");

        float analogListHeight = 0;
        float axisListHeight = 0;
        float buttonListHeight = 0;

        for (int i = 0; i < analogList.arraySize; i++)
        {
            var analogProp = analogList.GetArrayElementAtIndex(i);
            analogListHeight += EditorGUI.GetPropertyHeight(analogProp, GUIContent.none);
        }
        for (int i = 0; i < axisList.arraySize; i++)
        {
            var axisProp = axisList.GetArrayElementAtIndex(i);
            axisListHeight += EditorGUI.GetPropertyHeight(axisProp, GUIContent.none);
        }
        for (int i = 0; i < buttonList.arraySize; i++)
        {
            var buttonProp = buttonList.GetArrayElementAtIndex(i);
            buttonListHeight += EditorGUI.GetPropertyHeight(buttonProp, GUIContent.none);
        }

        float totalListHeight = analogListHeight + axisListHeight + buttonListHeight;

        return (base.GetPropertyHeight(property, label) * 6) + totalListHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property); // ==============================
        
        // Get child properties
        var ID = property.FindPropertyRelative("ID");
        var isUsingController = property.FindPropertyRelative("isUsingController");

        var analogList = property.FindPropertyRelative("analogList");
        var axisList = property.FindPropertyRelative("axisList");
        var buttonList = property.FindPropertyRelative("buttonList");

        // Set up property rects
        Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect childPropertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Set up header style
        GUIStyle headerStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        var target = property.serializedObject.targetObject;

        // ANALOGS | ==================================================== \\

        // Draw header
        EditorGUI.LabelField(headerRect, "Analogs", headerStyle);

        // Move buttons underneath header
        buttonRect.y = headerRect.y + EditorGUIUtility.singleLineHeight;

        // Draw buttons
        using (new EditorGUI.DisabledGroupScope(analogList.arraySize >= 3))
        {
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y, buttonRect.width / 2, buttonRect.height), "Add (Max 3)"))
            {
                analogList.InsertArrayElementAtIndex(analogList.arraySize);
            }
        }
        if (GUI.Button(new Rect(buttonRect.x + (position.width * 1 / 2), buttonRect.y, buttonRect.width /2, buttonRect.height), "Clear"))
        {
            analogList.ClearArray();
        }

        // Draw properties
        for (int i = 0; i < analogList.arraySize; i++)
        {
            // Get child property
            var childProperty = analogList.GetArrayElementAtIndex(i);

            // Change the child property rect height to the height of the property
            childPropertyRect.height = EditorGUI.GetPropertyHeight(childProperty);

            // If this is the first child, move down a line.
            // Otherwise, move down by the amount of the child property's height.
            if (i == 0) childPropertyRect.y = buttonRect.y + buttonRect.height;
            else childPropertyRect.y += childPropertyRect.height;

            // Update property
            childProperty.FindPropertyRelative("joystickID").intValue = ID.intValue;
            childProperty.FindPropertyRelative("isUsingController").boolValue = isUsingController.boolValue;

            // Draw property
            EditorGUI.PropertyField(childPropertyRect, childProperty, new GUIContent("Analog " + i));
        }

        // Move the next header below the last child property
        headerRect.y = buttonRect.y + buttonRect.height + (childPropertyRect.height * analogList.arraySize);

        // ============================================================ \\
        // AXES | ==================================================== \\
        
        // Draw header
        EditorGUI.LabelField(headerRect, "Axes", headerStyle);

        // Move buttons underneath header
        buttonRect.y = headerRect.y + headerRect.height;

        // Draw buttons
        using (new EditorGUI.DisabledGroupScope(axisList.arraySize >= 4))
        {
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y, buttonRect.width / 2, buttonRect.height), "Add (Max 4)"))
            {
                axisList.InsertArrayElementAtIndex(axisList.arraySize);
            }
        }
            
        if (GUI.Button(new Rect(buttonRect.x + (position.width * 1 / 2), buttonRect.y, buttonRect.width / 2, buttonRect.height), "Clear"))
        {
            axisList.ClearArray();
        }

        // Move the child properties underneath the buttons
        childPropertyRect.y = buttonRect.y + buttonRect.height;

        // Draw properties
        for (int i = 0; i < axisList.arraySize; i++)
        {
            // Get child property
            var childProperty = axisList.GetArrayElementAtIndex(i);

            // Change the child property rect height to the height of the property
            childPropertyRect.height = EditorGUI.GetPropertyHeight(childProperty);

            // If this is the first child, move down a line.
            // Otherwise, move down by the amount of the child property's height.
            if (i == 0) childPropertyRect.y = buttonRect.y + buttonRect.height;
            else childPropertyRect.y += childPropertyRect.height;

            // Update property
            childProperty.FindPropertyRelative("joystickID").intValue = ID.intValue;
            childProperty.FindPropertyRelative("isUsingController").boolValue = isUsingController.boolValue;

            // Draw property
            EditorGUI.PropertyField(childPropertyRect, childProperty, new GUIContent("Axis " + i));
        }

        // Move the next header below the last child property
        headerRect.y = buttonRect.y + buttonRect.height + (childPropertyRect.height * axisList.arraySize);

        // ============================================================ \\
        // BUTTONS | ==================================================== \\

        // Draw header
        EditorGUI.LabelField(headerRect, "Buttons", headerStyle);

        // Move buttons underneath header
        buttonRect.y = headerRect.y + headerRect.height;

        // Draw buttons
        using (new EditorGUI.DisabledGroupScope(buttonList.arraySize >= 10))
        {
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y, buttonRect.width / 2, buttonRect.height), "Add (Max 10)"))
            {
                buttonList.InsertArrayElementAtIndex(buttonList.arraySize);
            }
        }
        if (GUI.Button(new Rect(buttonRect.x + (position.width * 1 / 2), buttonRect.y, buttonRect.width / 2, buttonRect.height), "Clear"))
        {
            buttonList.ClearArray();
        }

        // Move the child properties underneath the buttons
        childPropertyRect.y = buttonRect.y + buttonRect.height;

        // Draw properties
        for (int i = 0; i < buttonList.arraySize; i++)
        {
            // Get child property
            var childProperty = buttonList.GetArrayElementAtIndex(i);

            // Change the child property rect height to the height of the property
            childPropertyRect.height = EditorGUI.GetPropertyHeight(childProperty);

            // If this is the first child, move down a line.
            // Otherwise, move down by the amount of the child property's height.
            if (i == 0) childPropertyRect.y = buttonRect.y + buttonRect.height;
            else childPropertyRect.y += childPropertyRect.height;

            // Update property
            childProperty.FindPropertyRelative("joystickID").intValue = ID.intValue;
            childProperty.FindPropertyRelative("isUsingController").boolValue = isUsingController.boolValue;

            // Draw property
            EditorGUI.PropertyField(childPropertyRect, childProperty, new GUIContent("Button " + i));
        }

        // Move the next header below the last child property
        headerRect.y = buttonRect.y + buttonRect.height + (childPropertyRect.height * buttonList.arraySize);
        // ============================================================ \\
    }
}
