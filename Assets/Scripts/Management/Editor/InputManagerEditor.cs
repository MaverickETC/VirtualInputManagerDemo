using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Update serialized object
        serializedObject.Update();

        // Get a reference to the inspected object
        // InputManager manager = target as InputManager;

        // Get child properties
        var joystickList = serializedObject.FindProperty("joystickList");

        // Make sure that there is at least one controller at all times
        if (InputManager.NumberOfJoysticks == 0)
        {
            InputManager.AddJoystick();
            serializedObject.Update();
        }

        GUILayout.BeginHorizontal(); /*----------------------------------------*/
        // Button : "Add Joystick"
        using (new EditorGUI.DisabledGroupScope(InputManager.NumberOfJoysticks >= 4))
            if (GUILayout.Button("Add Joystick (Max 4)"))
            {
                InputManager.AddJoystick();
                serializedObject.Update();
            }
        // Button : "Remove All Joysticks"
        if (GUILayout.Button("Remove All Joysticks"))
        {
            InputManager.ClearJoysticks();
            serializedObject.Update();
        }
        GUILayout.EndHorizontal(); /*----------------------------------------*/

        GUIStyle toggleStyle = new GUIStyle(EditorStyles.toggleGroup)
        {
            imagePosition = ImagePosition.ImageOnly
        };

        // Create booleans that are used to remember the display of a joystick
        bool[] ShowJoystick = new bool[joystickList.arraySize];

        // Draw all of the joystick components
        for (int i = 0; i < joystickList.arraySize; i++)
        {
            ShowJoystick[i] = EditorPrefs.GetBool("Show Joystick " + (i + 1), false);

            GUILayout.BeginHorizontal(); /*----------------------------------------*/

            ShowJoystick[i] = EditorGUILayout.Foldout(ShowJoystick[i], "Joystick " + (i + 1), true);
            if (GUILayout.Button("?"))
            {
                InputManager.GetJoystick(i).ShowInfo();
            }
            if (GUILayout.Button("Save Defaults"))
            {
                string JoystickOrController = InputManager.GetJoystick(i).isUsingController ? "Controller " + (i + 1) : "Joystick " + (i + 1);

                for (int a = 0; a < InputManager.GetJoystick(i).axisList.Count; a++)
                {
                    EditorPrefs.SetInt(JoystickOrController + " Analog " + a + " X (+)", (int)InputManager.GetJoystick(i).analogList[a].xAxis.positiveButton.key);
                    EditorPrefs.SetInt(JoystickOrController + " Analog " + a + " X (-)", (int)InputManager.GetJoystick(i).analogList[a].xAxis.negativeButton.key);
                    EditorPrefs.SetInt(JoystickOrController + " Analog " + a + " Y (+)", (int)InputManager.GetJoystick(i).analogList[a].yAxis.positiveButton.key);
                    EditorPrefs.SetInt(JoystickOrController + " Analog " + a + " Y (-)", (int)InputManager.GetJoystick(i).analogList[a].yAxis.negativeButton.key);

                    var analogID = joystickList.GetArrayElementAtIndex(i).FindPropertyRelative("analogList").GetArrayElementAtIndex(a).FindPropertyRelative("controllerAnalogID");
                    EditorPrefs.SetInt(JoystickOrController + " Analog " + a + " Controller", analogID.intValue);
                }

                for (int x = 0; x < InputManager.GetJoystick(i).axisList.Count; x++)
                {
                    EditorPrefs.SetInt(JoystickOrController + " Axis " + x + " (+)", (int)InputManager.GetJoystick(i).axisList[x].positiveButton.key);
                    EditorPrefs.SetInt(JoystickOrController + " Axis " + x + " (-)", (int)InputManager.GetJoystick(i).axisList[x].negativeButton.key);

                    var axisID = joystickList.GetArrayElementAtIndex(i).FindPropertyRelative("axisList").GetArrayElementAtIndex(x).FindPropertyRelative("controllerAxisID");
                    EditorPrefs.SetInt(JoystickOrController + " Axis " + x + " Controller", axisID.intValue);
                }

                for (int b = 0; b < InputManager.GetJoystick(i).buttonList.Count; b++)
                    EditorPrefs.SetInt(JoystickOrController + " Button " + b, (int)InputManager.GetJoystick(i).buttonList[b].key);
            }
            if (GUILayout.Button("Restore Defaults"))
            {
                GetDefaultInputMap(i);
            }

            GUILayout.EndHorizontal(); /*----------------------------------------*/

            // Save whether the joystick is currently displayed
            EditorPrefs.SetBool("Show Joystick " + (i + 1), ShowJoystick[i]);

            // Draw the joystick property if it is shown
            if (ShowJoystick[i])
            {
                EditorGUILayout.Separator();

                // Track whether the controller toggle has changed
                bool controllerToggleChanged = InputManager.GetJoystick(i).isUsingController;

                InputManager.GetJoystick(i).isUsingController =
                    EditorGUILayout.Toggle("Enable Controller", InputManager.GetJoystick(i).isUsingController, toggleStyle);

                // When the controller toggle changes, get the corresponding default controls
                if (controllerToggleChanged != InputManager.GetJoystick(i).isUsingController)
                {
                    // Debug.Log("Changed!");
                    GetDefaultInputMap(i);

                    // Update serialized object
                    serializedObject.Update();
                }


                EditorGUILayout.Space();

                // Draw the joystick property
                EditorGUILayout.PropertyField(joystickList.GetArrayElementAtIndex(i), true);

                EditorGUILayout.Separator();
            }
        }


        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    void GetDefaultInputMap(int i)
    {
        // Get child properties
        var joystickList = serializedObject.FindProperty("joystickList");

        // Get a reference to the inspected object
        // InputManager manager = target as InputManager;

        string JoystickOrController = InputManager.GetJoystick(i).isUsingController ? "Controller " + (i + 1) : "Joystick " + (i + 1);

        for (int a = 0; a < InputManager.GetJoystick(i).analogList.Count; a++)
        {
            InputManager.GetJoystick(i).analogList[a].xAxis.positiveButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Analog " + a + " X (+)", (int)KeyCode.None);
            InputManager.GetJoystick(i).analogList[a].xAxis.negativeButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Analog " + a + " X (-)", (int)KeyCode.None);
            InputManager.GetJoystick(i).analogList[a].yAxis.positiveButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Analog " + a + " Y (+)", (int)KeyCode.None);
            InputManager.GetJoystick(i).analogList[a].yAxis.negativeButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Analog " + a + " Y (-)", (int)KeyCode.None);

            var analogID = joystickList.GetArrayElementAtIndex(i).FindPropertyRelative("analogList").GetArrayElementAtIndex(a).FindPropertyRelative("controllerAnalogID");
            analogID.intValue = EditorPrefs.GetInt(JoystickOrController + " Analog " + a + " Controller", 0);
        }

        for (int x = 0; x < InputManager.GetJoystick(i).axisList.Count; x++)
        {
            InputManager.GetJoystick(i).axisList[x].positiveButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Axis " + x + " (+)", (int)KeyCode.None);
            InputManager.GetJoystick(i).axisList[x].negativeButton.key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Axis " + x + " (-)", (int)KeyCode.None);

            var axisID = joystickList.GetArrayElementAtIndex(i).FindPropertyRelative("axisList").GetArrayElementAtIndex(x).FindPropertyRelative("controllerAxisID");
            axisID.intValue = EditorPrefs.GetInt(JoystickOrController + " Axis " + x + " Controller", 0);
        }

        for (int b = 0; b < InputManager.GetJoystick(i).buttonList.Count; b++)
            InputManager.GetJoystick(i).buttonList[b].key = (KeyCode)EditorPrefs.GetInt(JoystickOrController + " Button " + b, (int)KeyCode.None);
    }
}
