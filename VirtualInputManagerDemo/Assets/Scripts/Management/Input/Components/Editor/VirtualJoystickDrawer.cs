using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(VirtualJoystick))]
public class VirtualJoystickDrawer : PropertyDrawer
{
    // Editor Layout Shortcuts
    float lineHeight = EditorGUIUtility.singleLineHeight;
    float lineSpacing = EditorGUIUtility.standardVerticalSpacing;

    // Editor Properties
    float headerHeight = EditorGUIUtility.singleLineHeight * 1.5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Property References ========== \\
        var ID = property.FindPropertyRelative("ID");
        var ctrl = property.FindPropertyRelative("isUsingController");

        var analogCollection = property.FindPropertyRelative("analogCollection");
        var axisCollection = property.FindPropertyRelative("axisCollection");
        var buttonCollection = property.FindPropertyRelative("buttonCollection");

        // Dimension Variables ========== \\
        float x = position.x; float y = position.y;
        float w = position.width; float h = position.height;
        float labelWidth = EditorGUIUtility.labelWidth / 2 + 20;

        // Rects ========== \\
        Rect foldoutRect = new Rect(x, y, labelWidth, lineHeight);
        Rect ctrlRect = new Rect(foldoutRect.xMax, y, labelWidth, lineHeight);
        Rect headerRect = new Rect(x, foldoutRect.y + lineHeight, w, headerHeight);

        Rect buttonRect = new Rect(ctrlRect.xMax+ 10, y, w - (foldoutRect.width + ctrlRect.width+10), lineHeight);
        Rect b1 = new Rect(buttonRect) { width = buttonRect.width / 3 };
        Rect b2 = new Rect(buttonRect) { width = buttonRect.width / 3 };
        Rect b3 = new Rect(buttonRect) { width = buttonRect.width / 3 };
        b2.x = b1.xMax; b3.x = b2.xMax;

        // Styles ========== \\
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.toggleGroup)
        {
        };
        GUIStyle ctrlStyle = new GUIStyle()
        {

        };
        GUIStyle headerStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16,
            fontStyle = FontStyle.Bold
        };
        headerStyle.normal.textColor = ctrl.boolValue ? Color.red : Color.black;

        // Draw ========== \\
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true, foldoutStyle);
        ctrl.boolValue = EditorGUI.ToggleLeft(ctrlRect, "Controller", ctrl.boolValue, ctrlStyle);
        if (GUI.Button(b1, "L")) { LoadJoystickConfiguration(property, ID.intValue, ctrl.boolValue); }
        GUI.Button(b2, "S");
        GUI.Button(b3, "R");
        if (property.isExpanded)
        {
            // Analogs
            GUI.Label(headerRect, "Analogs", headerStyle);
            DrawHeaderButtons(ref headerRect, analogCollection);
            DrawArrayProperty(ref headerRect, analogCollection, "Analog", ctrl.boolValue);
            
            // Axes
            GUI.Label(headerRect, "Axes", headerStyle);
            DrawHeaderButtons(ref headerRect, axisCollection);
            DrawArrayProperty(ref headerRect, axisCollection, "Axis", ctrl.boolValue);

            // Buttons
            GUI.Label(headerRect, "Buttons", headerStyle);
            DrawHeaderButtons(ref headerRect, buttonCollection);
            DrawArrayProperty(ref headerRect, buttonCollection, "Button", ctrl.boolValue);
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Property References ========== \\
        var analogCollection = property.FindPropertyRelative("analogCollection");
        var axisCollection = property.FindPropertyRelative("axisCollection");
        var buttonCollection = property.FindPropertyRelative("buttonCollection");
        
        // Establish total property height
        float totalPropertyHeight = 
            property.isExpanded ? lineHeight + (headerHeight * 3) : lineHeight;

        // Add components to the total height
        if (property.isExpanded)
        {
            AddArrayPropertyHeight(analogCollection, ref totalPropertyHeight);
            AddArrayPropertyHeight(axisCollection, ref totalPropertyHeight);
            AddArrayPropertyHeight(buttonCollection, ref totalPropertyHeight);
        }

        // Return the total height
        return totalPropertyHeight + lineSpacing;
    }

    void DrawArrayProperty(ref Rect r, SerializedProperty arrayProperty, string label = "", bool ctrl = false)
    {
        // Ignore the rest of the code if the serializedProperty is not an array
        if (!arrayProperty.isArray) return;

        // Make sure that this property is underneath the given rect
        Rect propertyRect = new Rect(r) { y = r.y + r.height + lineSpacing };

        // Draw each property within the array
        for (int i = 0; i < arrayProperty.arraySize; i++)
        {
            var property = arrayProperty.GetArrayElementAtIndex(i);
            property.FindPropertyRelative("isUsingController").boolValue = ctrl;
            propertyRect.height = EditorGUI.GetPropertyHeight(property);
            EditorGUI.PropertyField(propertyRect, property, new GUIContent(label + " " + i));


            Rect indexChangeRect = new Rect(propertyRect)
            {
                width = EditorGUIUtility.labelWidth / 2,
                height = EditorGUIUtility.singleLineHeight,
                y = propertyRect.y + EditorGUIUtility.singleLineHeight
            };

            Rect indexMoveUpRect = new Rect(indexChangeRect)
            {
                width = indexChangeRect.width / 2
            };
            Rect indexMoveDownRect = new Rect(indexMoveUpRect)
            {
                x = indexMoveUpRect.xMax
            };

            using (new EditorGUI.DisabledGroupScope(i <= 0))
                if (GUI.Button(indexMoveUpRect, "^"))
                    arrayProperty.MoveArrayElement(i, Mathf.Max(i - 1, 0));
            using (new EditorGUI.DisabledGroupScope(i >= arrayProperty.arraySize - 1))
                if (GUI.Button(indexMoveDownRect, "v"))
                    arrayProperty.MoveArrayElement(i, Mathf.Min(i + 1, arrayProperty.arraySize - 1));

            propertyRect.y += propertyRect.height;
        }

        // Move the given rect below all of the drawn properties
        r.y = propertyRect.y;
    }
    void DrawHeaderButtons(ref Rect r, SerializedProperty arrayProperty)
    {
        // Ignore the rest of the code if the serializedProperty is not an array
        if (!arrayProperty.isArray) return;

        // Make sure that this property is underneath the given rect
        Rect buttonAreaRect = new Rect(r);
        buttonAreaRect.width /= 4;
        buttonAreaRect.x += r.width - buttonAreaRect.width;

        Rect b1 = new Rect(buttonAreaRect);
        b1.width /= 2;

        Rect b2 = new Rect(b1);
        b2.x += b2.width;

        using (new EditorGUI.DisabledGroupScope(arrayProperty.arraySize >= 10))
            if (GUI.Button(b1, "+"))
            {
                if (arrayProperty.arraySize < 10)
                    arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
            }
        using (new EditorGUI.DisabledGroupScope(arrayProperty.arraySize <= 0))
            if (GUI.Button(b2, "-"))
            {
                if (arrayProperty.arraySize > 0)
                    arrayProperty.DeleteArrayElementAtIndex(arrayProperty.arraySize - 1);
            }
    }
    void AddArrayPropertyHeight(SerializedProperty arrayProperty, ref float height)
    {
        // Ignore the rest of the code if the serializedProperty is not an array
        if (!arrayProperty.isArray) return;

        // Add the height of each property within the array
        for (int i = 0; i < arrayProperty.arraySize; i++)
            height += EditorGUI.GetPropertyHeight(arrayProperty.GetArrayElementAtIndex(i));
    }

    #region Load Configuration Methods
    void LoadJoystickConfiguration(SerializedProperty property, int ID, bool usingController)
    {
        LoadAnalogConfigurations(property, ID, usingController);
        LoadAxisConfigurations(property, ID, usingController);
        LoadButtonConfigurations(property, ID, usingController);
        Debug.Log(string.Format("Default configuration loaded for {0} {1}.", usingController ? "Controller" : "Joystick", ID));
    }
    void LoadButtonConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var buttonCollection = property.FindPropertyRelative("buttonCollection");

        buttonCollection.arraySize =
            !usingController ?
            EditorPrefs.GetInt("Joystick " + ID + " Button Count", 1) :
            EditorPrefs.GetInt("Controller " + ID + " Button Count", 1);

        // Cycle through each button
        for (int i = 0; i < buttonCollection.arraySize; i++)
        {
            // Get the current button
            var button = buttonCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var key = button.FindPropertyRelative("key");
            var controllerButton = button.FindPropertyRelative("controllerButton");

            // Get saved or default values
            if (!usingController)
            key.intValue = EditorPrefs.GetInt("Joystick " + ID + " Button " + i, (int)KeyCode.None);
            else
            controllerButton.intValue = EditorPrefs.GetInt("Controller " + ID + " Button " + i, (int)XboxControllerButton.A);
        }
    }
    void LoadAxisConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var axisCollection = property.FindPropertyRelative("axisCollection");

        axisCollection.arraySize =
            !usingController ?
            EditorPrefs.GetInt("Joystick " + ID + " Axis Count", 1) :
            EditorPrefs.GetInt("Controller " + ID + " Axis Count", 1);

        // Cycle through each button
        for (int i = 0; i < axisCollection.arraySize; i++)
        {
            // Get the current button
            var axis = axisCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var pButton =
            axis.FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var nButton =
                axis.FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            var controllerAxis =
                axis.FindPropertyRelative("controllerAxis");

            // Get saved or default values
            if (!usingController)
            {
                pButton.intValue = EditorPrefs.GetInt("Joystick " + ID + " Axis " + i + " +", (int)KeyCode.None);
                nButton.intValue = EditorPrefs.GetInt("Joystick " + ID + " Axis " + i + " -", (int)KeyCode.None);
            }
            else
                controllerAxis.intValue = EditorPrefs.GetInt("Controller " + ID + " Axis " + i, (int)XboxControllerAxis.LeftStickX);
        }
    }
    void LoadAnalogConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var analogCollection = property.FindPropertyRelative("analogCollection");

        analogCollection.arraySize =
            !usingController ?
            EditorPrefs.GetInt("Joystick " + ID + " Analog Count", 1) :
            EditorPrefs.GetInt("Controller " + ID + " Analog Count", 1);

        // Cycle through each button
        for (int i = 0; i < analogCollection.arraySize; i++)
        {
            // Get the current button
            var analog = analogCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var xAxisPositive =
                analog.FindPropertyRelative("xAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var xAxisNegative =
                analog.FindPropertyRelative("xAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            var yAxisPositive =
                analog.FindPropertyRelative("yAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var yAxisNegative =
                analog.FindPropertyRelative("yAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");

            var analogButton =
                analog.FindPropertyRelative("button").FindPropertyRelative("key");

            var controllerAnalog =
                analog.FindPropertyRelative("controllerAnalog");
            var controllerAnalogButton =
                analog.FindPropertyRelative("controllerAnalogButton");

            // Get saved or default values
            if (!usingController)
            {
            xAxisPositive.intValue = EditorPrefs.GetInt("Joystick " + ID + " Analog " + i + " x+", (int)KeyCode.None);
            xAxisNegative.intValue = EditorPrefs.GetInt("Joystick " + ID + " Analog " + i + " x-", (int)KeyCode.None);
            yAxisPositive.intValue = EditorPrefs.GetInt("Joystick " + ID + " Analog " + i + " y+", (int)KeyCode.None);
            yAxisNegative.intValue = EditorPrefs.GetInt("Joystick " + ID + " Analog " + i + " y-", (int)KeyCode.None);
            analogButton.intValue = EditorPrefs.GetInt("Joystick " + ID + " Analog " + i + " Button", (int)KeyCode.None);
            }
            else
            {
            controllerAnalog.intValue = EditorPrefs.GetInt("Controller " + ID + " Analog " + i, (int)XboxControllerAnalog.LeftStick);
            controllerAnalogButton.intValue = EditorPrefs.GetInt("Controller " + ID + " Analog " + i + " Button", (int)XboxControllerButton.LeftStickClick);
            }
        }
    }
    #endregion

    #region Save Configuration Methods
    void SaveJoystickConfiguration(SerializedProperty property, int ID, bool usingController)
    {
        SaveAnalogConfigurations(property, ID, usingController);
        SaveAxisConfigurations(property, ID, usingController);
        SaveButtonConfigurations(property, ID, usingController);
        Debug.Log(string.Format("Default configuration have been saved for {0} {1}.", usingController ? "Controller" : "Joystick", ID));
    }
    void SaveButtonConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var buttonCollection = property.FindPropertyRelative("buttonCollection");

        if (!usingController)
            EditorPrefs.SetInt("Joystick " + ID + " Button Count", buttonCollection.arraySize);
        else
            EditorPrefs.SetInt("Controller " + ID + " Button Count", buttonCollection.arraySize);

        // Cycle through each button
        for (int i = 0; i < buttonCollection.arraySize; i++)
        {
            // Get the current button
            var button = buttonCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var key = button.FindPropertyRelative("key");
            var controllerButton = button.FindPropertyRelative("controllerButton");

            // Get saved or default values
            if (!usingController)
            EditorPrefs.SetInt("Joystick " + ID + " Button " + i, key.intValue);
            else
            EditorPrefs.SetInt("Controller " + ID + " Button " + i, controllerButton.intValue);
        }
    }
    void SaveAxisConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var axisCollection = property.FindPropertyRelative("axisCollection");

        if (!usingController)
            EditorPrefs.GetInt("Joystick " + ID + " Axis Count", axisCollection.arraySize);
        else
            EditorPrefs.GetInt("Controller " + ID + " Axis Count", axisCollection.arraySize);

        // Cycle through each button
        for (int i = 0; i < axisCollection.arraySize; i++)
        {
            // Get the current button
            var axis = axisCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var pButton =
            axis.FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var nButton =
                axis.FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            var controllerAxis =
                axis.FindPropertyRelative("controllerAxis");

            // Get saved or default values
            if (!usingController)
            {
                EditorPrefs.SetInt("Joystick " + ID + " Axis " + i + " +", pButton.intValue);
                EditorPrefs.SetInt("Joystick " + ID + " Axis " + i + " -", nButton.intValue);
            }
            else
                EditorPrefs.SetInt("Controller " + ID + " Axis " + i, controllerAxis.intValue);
        }
    }
    void SaveAnalogConfigurations(SerializedProperty property, int ID, bool usingController)
    {
        // Get this property's collection of buttons
        var analogCollection = property.FindPropertyRelative("analogCollection");

        if (!usingController)
            EditorPrefs.SetInt("Joystick " + ID + " Analog Count", analogCollection.arraySize);
        else
            EditorPrefs.SetInt("Controller " + ID + " Analog Count", analogCollection.arraySize);

        // Cycle through each button
        for (int i = 0; i < analogCollection.arraySize; i++)
        {
            // Get the current button
            var analog = analogCollection.GetArrayElementAtIndex(i);

            // Get properties from the button that need to be retrieved
            var xAxisPositive =
                analog.FindPropertyRelative("xAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var xAxisNegative =
                analog.FindPropertyRelative("xAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");
            var yAxisPositive =
                analog.FindPropertyRelative("yAxis").FindPropertyRelative("positiveButton").FindPropertyRelative("key");
            var yAxisNegative =
                analog.FindPropertyRelative("yAxis").FindPropertyRelative("negativeButton").FindPropertyRelative("key");

            var analogButton =
                analog.FindPropertyRelative("button").FindPropertyRelative("key");

            var controllerAnalog =
                analog.FindPropertyRelative("controllerAnalog");
            var controllerAnalogButton =
                analog.FindPropertyRelative("controllerAnalogButton");

            // Get saved or default values
            if (!usingController)
            {
                EditorPrefs.SetInt("Joystick " + ID + " Analog " + i + " x+", xAxisPositive.intValue);
                EditorPrefs.SetInt("oystick " + ID + " Analog " + i + " x-", xAxisNegative.intValue);
                EditorPrefs.SetInt("Joystick " + ID + " Analog " + i + " y+", yAxisPositive.intValue);
                EditorPrefs.SetInt("Joystick " + ID + " Analog " + i + " y-", yAxisNegative.intValue);
                EditorPrefs.SetInt("Joystick " + ID + " Analog " + i + " Button", analogButton.intValue);
            }
            else
            {
                EditorPrefs.SetInt("Controller " + ID + " Analog " + i, controllerAnalog.intValue);
                EditorPrefs.SetInt("Controller " + ID + " Analog " + i + " Button", controllerAnalogButton.intValue);
            }
        }
    }
    #endregion
}
