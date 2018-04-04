using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VirtualAxis
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
    /// The universal axis ID that this component uses (only relevent when using a controller)
    /// </summary>
    [SerializeField] private int controllerAxisID = 8;

    /// <summary>
    /// The button used for the positive side of the axis
    /// </summary>
    public VirtualButton positiveButton;
    /// <summary>
    /// The button used for the negative side of the axis
    /// </summary>
    public VirtualButton negativeButton;
    /// <summary>
    /// The current position of the axis
    /// </summary>
    [SerializeField] private float position;


    /// <summary>
    /// Virtual axis constructor
    /// </summary>
    /// <param name="joystick">The joystick to assign the component to</param>
    public VirtualAxis(VirtualJoystick joystick)
    {
        this.joystick = joystick;

        if (joystick == null) return;

        joystickID = this.joystick.ID;
        isUsingController = this.joystick.isUsingController;

        positiveButton = new VirtualButton(this.joystick);
        negativeButton = new VirtualButton(this.joystick);

        position = 0;
    }


    /// <summary>
    /// Update the axis
    /// </summary>
    public void Update(VirtualJoystick joystick)
    {
        joystickID = joystick.ID;
        isUsingController = joystick.isUsingController;

        positiveButton.Update(joystick);
        negativeButton.Update(joystick);

        if (!isUsingController)
        {
            position = 0;

            if (positiveButton != negativeButton)
            {
                if (positiveButton)
                    position++;

                if (negativeButton)
                    position--;
            }
            
            position = Mathf.Clamp(position, -1, 1);
        }
        else
        {
            position = Input.GetAxis("Controller " + (joystickID + 1) + " " + VirtualJoystick.ControllerAxisID[controllerAxisID]);
        }
    }

    /// <summary>
    /// Show information
    /// </summary>
    public void ShowInfo()
    {
        string info = string.Format("Joystick: {1}, Axis: {0}\nUsing Controller {2} ", joystick.axisList.IndexOf(this), joystickID, isUsingController);
        Debug.Log(info);
    }

    /// <summary>
    /// Returns the current position of the axis
    /// </summary>
    /// <param name="button">Virtual axis</param>
    public static implicit operator float(VirtualAxis axis)
    {
        return axis == null ? 0.0f : axis.position;
    }
}
