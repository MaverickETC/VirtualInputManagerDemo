using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VirtualButton
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
    /// The keycode that the component responds to
    /// </summary>
    public KeyCode key;
    /// <summary>
    /// Return true on the step that the button was pressed
    /// </summary>
    public bool Pressed { get { return Input.GetKeyDown(key); } }
    /// <summary>
    /// Return true on the step that the button was released
    /// </summary>
    public bool Released { get { return Input.GetKeyUp(key); } }
    /// <summary>
    /// The amount of time (in steps) that the button was held for
    /// </summary>
    public int holdTime;
    /// <summary>
    /// The amount of time (in seconds) that the button was held for
    /// </summary>
    public float holdTimeSeconds;


    /// <summary>
    /// Virtual button constructor
    /// </summary>
    /// <param name="joystick">The joystick to assign the component to</param>
    public VirtualButton(VirtualJoystick joystick)
    {
        this.joystick = joystick;

        if (this.joystick == null) return;

        holdTime = 0;
        holdTimeSeconds = 0.00f;

        joystickID = this.joystick.ID;
        isUsingController = this.joystick.isUsingController;
    }


    /// <summary>
    /// Update the button
    /// </summary>
    public void Update(VirtualJoystick joystick)
    {
        joystickID = joystick.ID;
        isUsingController = joystick.isUsingController;

        if (Input.GetKey(key))
        {
            holdTime++;
            holdTimeSeconds += Time.deltaTime;
        }
        else
        {
            holdTime = 0;
            holdTimeSeconds = 0.00f;
        }
    }

    /// <summary>
    /// Show information
    /// </summary>
    public void ShowInfo()
    {
        string info = string.Format("Joystick: {1}, Button: {0}\nUsing Controller {2} ", joystick.buttonList.IndexOf(this), joystickID, isUsingController);
        Debug.Log(info);
    }


    /// <summary>
    /// Returns true if the button's corresponding key is held down
    /// </summary>
    /// <param name="button">Virtual button</param>
    public static implicit operator bool(VirtualButton button)
    {
        return button == null ? false : Input.GetKey(button.key);
    }
}