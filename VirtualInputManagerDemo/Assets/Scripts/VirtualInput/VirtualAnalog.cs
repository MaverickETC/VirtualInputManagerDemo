using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VirtualAnalog
{
    /// <summary>
    /// The virtual joystick that this component belongs to
    /// </summary>
    [NonSerialized] private VirtualJoystick joystick;

    /// <summary>
    /// The ID of virtual joystick that this component belongs to is using a controller
    /// </summary>
    [SerializeField] private int joystickID;
    /// <summary>
    /// Whether the virtual joystick that this component belongs to is using a controller
    /// </summary>
    [SerializeField] private bool isUsingController;

    /// <summary>
    /// The universal analog ID that this component uses (only relevent when using a controller)
    /// </summary>
    [SerializeField] private int controllerAnalogID = 0;

    /// <summary>
    /// The horizontal axis of the analog
    /// </summary>
    public VirtualAxis xAxis;
    /// <summary>
    /// The vertical axis of the analog
    /// </summary>
    public VirtualAxis yAxis;
    /// <summary>
    /// The direction in which the analog is currently moved towards
    /// </summary>
    [SerializeField] private Vector2 direction;


    /// <summary>
    /// Virtual analog constructor
    /// </summary>
    /// <param name="joystick">The joystick to assign the component to</param>
    public VirtualAnalog(VirtualJoystick joystick)
    {
        this.joystick = joystick;

        if (this.joystick == null) return;
        
        joystickID = this.joystick.ID;
        isUsingController = this.joystick.isUsingController;

        xAxis = new VirtualAxis(this.joystick);
        yAxis = new VirtualAxis(this.joystick);

        direction = Vector2.zero;
    }


    /// <summary>
    /// Update the analog
    /// </summary>
    public void Update(VirtualJoystick joystick)
    {
        joystickID = joystick.ID;
        isUsingController = joystick.isUsingController;

        xAxis.Update(joystick);
        yAxis.Update(joystick);

        if (!isUsingController)
        {
            direction = new Vector2(xAxis, yAxis);
            direction.Normalize();
        }
        else
        {
            float h = Input.GetAxis("Controller " + (joystickID + 1) + " " + VirtualJoystick.ControllerAnalogID[controllerAnalogID] + " X");
            float v = Input.GetAxis("Controller " + (joystickID + 1) + " " + VirtualJoystick.ControllerAnalogID[controllerAnalogID] + " Y");

            direction = new Vector2(h, v);
        }
    }

    /// <summary>
    /// Show information
    /// </summary>
    public void ShowInfo()
    {
        string info = string.Format("Joystick: {1}, Analog: {0}\nUsing Controller {2} ", joystick.analogList.IndexOf(this), joystickID, isUsingController);
        Debug.Log(info);
    }


    /// <summary>
    /// Returns the current direction of the analog
    /// </summary>
    /// <param name="button">Virtual analog</param>
    public static implicit operator Vector2(VirtualAnalog analog)
    {
        return analog == null ? Vector2.zero : analog.direction;
    }
}