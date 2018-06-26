using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual input analog component
/// </summary>
[Serializable]
public class VirtualAnalog
{
    /// <summary>
    /// Whether the component is using a joystick
    /// </summary>
    [SerializeField] bool isUsingController;
    /// <summary>
    /// The x-axis of the analog
    /// </summary>
    public VirtualAxis xAxis;
    /// <summary>
    /// The y-axis of the analog
    /// </summary>
    public VirtualAxis yAxis;
    /// <summary>
    /// The button on the axis
    /// </summary>
    public VirtualButton button;

    public XboxControllerAnalog controllerAnalog;
    public XboxControllerButton controllerAnalogButton;

    /// <summary>
    /// The current direction and magnitude of the analog
    /// </summary>
    [SerializeField] private Vector2 direction;

    /// <summary>
    /// Constructor
    /// </summary>
    public VirtualAnalog(VirtualJoystick joystick)
    {
        xAxis = new VirtualAxis(joystick);
        yAxis = new VirtualAxis(joystick);
        button = new VirtualButton(joystick);
    }


    /// <summary>
    /// Update component
    /// </summary>
    public void Update(VirtualJoystick joystick, int id = 0)
    {
        // Determine whether the joystick is using a controller
        isUsingController = joystick.isUsingController;

        // Reset the axis position
        direction = Vector2.zero;

        // Update each axis
        xAxis.Update(joystick, id);
        yAxis.Update(joystick, id);

        // Update the button
        button.Update(joystick, id);

        // Set the position of the axis
        direction = new Vector2(xAxis, yAxis).normalized;

        if (isUsingController)
        {
            float h = Input.GetAxis("Controller " + (id + 1) + " " + controllerAnalog.ToString() + "X");
            float v = Input.GetAxis("Controller " + (id + 1) + " " + controllerAnalog.ToString() + "Y");
            direction = Vector2.ClampMagnitude(new Vector2(h, v), 1);
        }

    }

    /// <summary>
    /// Returns a vector for the current position of the analog
    /// </summary>
    /// <param name="analog">The virtual analog</param>
    public static implicit operator Vector2(VirtualAnalog analog)
    {
        return analog.direction;
    }
}
