using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual input axis component
/// </summary>
[Serializable]
public class VirtualAxis
{
    /// <summary>
    /// Whether the component is using a joystick
    /// </summary>
    [SerializeField] bool isUsingController;

    /// <summary>
    /// The positive button in the axis
    /// </summary>
    public VirtualButton positiveButton;
    /// <summary>
    /// The negative button in the axis
    /// </summary>
    [SerializeField] VirtualButton negativeButton;
    [SerializeField] XboxControllerAxis controllerAxis;

    /// <summary>
    /// The current position of the axis
    /// </summary>
    [SerializeField] private float position;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="positiveKey"></param>
    /// <param name="negativeKey"></param>
    public VirtualAxis(VirtualJoystick joystick)
    {
        positiveButton = new VirtualButton(joystick);
        negativeButton = new VirtualButton(joystick);
    }

    /// <summary>
    /// Update component
    /// </summary>
    public void Update(VirtualJoystick joystick, int id = 0)
    {
        // Determine whether the joystick is using a controller
        isUsingController = joystick.isUsingController;

        // Reset the axis position
        position = 0;

        // Update the buttons
        positiveButton.Update(joystick, id);
        negativeButton.Update(joystick, id);

        // Set the position of the axis
        if ((bool)positiveButton != (bool)negativeButton)
            position = positiveButton ? 1 : -1;

        if (isUsingController)
            position = Input.GetAxis("Controller " + (id + 1) + " " + controllerAxis.ToString());
    }

    /// <summary>
    /// Returns a float for the current position of the axis
    /// </summary>
    /// <param name="axis">The virtual axis</param>
    public static implicit operator float(VirtualAxis axis)
    {
        return axis.position;
    }
}