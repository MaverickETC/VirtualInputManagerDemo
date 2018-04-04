using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VirtualJoystick
{
    public static Dictionary<int, string> ControllerAnalogID = new Dictionary<int, string>
    {
        { 0 , "Left Analog" },
        { 1 , "Right Analog" },
        { 2 , "D-Pad" }
    };
    public static Dictionary<int, string> ControllerAxisID = new Dictionary<int, string>
    {
        { 0 , "Left Analog X" },
        { 1 , "Left Analog Y" },
        { 2 , "Right Analog X" },
        { 3 , "Right Analog Y" },
        { 4 , "D-Pad X" },
        { 5 , "D-Pad Y" },
        { 6 , "Left Trigger" },
        { 7 , "Right Trigger" },
        { 8 , "Both Triggers" }
    };

    /// <summary>
    /// The joystick's ID
    /// </summary>
    public int ID;

    /// <summary>
    /// Whether the virtual joystick is currently using a controller
    /// </summary>
    public bool isUsingController;

    // Virtual Joystick components
    public List<VirtualAnalog> analogList = new List<VirtualAnalog>(1);
    public List<VirtualAxis> axisList = new List<VirtualAxis>(1);
    public List<VirtualButton> buttonList = new List<VirtualButton>(4);


    /// <summary>
    /// Virtual joystick constructor
    /// </summary>
    public VirtualJoystick()
    {
        // Create all analogs
        for (int i = 0; i < analogList.Capacity; i++)
            analogList.Add(new VirtualAnalog(this));
        // Create all axes
        for (int i = 0; i < axisList.Capacity; i++)
            axisList.Add(new VirtualAxis(this));
        // Create all buttons
        for (int i = 0; i < buttonList.Capacity; i++)
            buttonList.Add(new VirtualButton(this));
    }


    /// <summary>
    /// Update the joystick
    /// </summary>
    public void Update()
    {
        // Update all analogs
        foreach(VirtualAnalog analog in analogList)
            analog.Update(this);
        // Update all axes
        foreach (VirtualAxis axis in axisList)
            axis.Update(this);
        // Update all buttons
        foreach (VirtualButton button in buttonList)
            button.Update(this);
    }

    /// <summary>
    /// Show info
    /// </summary>
    public void ShowInfo()
    {
        // Update all analogs
        foreach (VirtualAnalog analog in analogList)
            analog.ShowInfo();
        // Update all axes
        foreach (VirtualAxis axis in axisList)
            axis.ShowInfo();
        // Update all buttons
        foreach (VirtualButton button in buttonList)
            button.ShowInfo();
    }
}
