using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual input button component
/// </summary>
[Serializable]
public class VirtualButton
{
    /// <summary>
    /// Whether the component is using a joystick
    /// </summary>
    [SerializeField] bool isUsingController;

    /// <summary>
    /// The key that is mapped to this button
    /// </summary>
    [SerializeField] KeyCode key;
    [SerializeField] XboxControllerButton controllerButton;

    /// <summary>
    /// Whether the button was pressed
    /// </summary>
    public bool IsPressed { get { return Input.GetKeyDown(key); } }
    /// <summary>
    /// Whether the button was released
    /// </summary>
    public bool IsReleased { get { return Input.GetKeyUp(key); } }

    /// <summary>
    /// The amount of time (in frames) the button has been held for.
    /// </summary>
    [SerializeField] int holdTime;
    /// <summary>
    /// The amount of time (in seconds) the button has been held for.
    /// </summary>
    [SerializeField] float holdTimeSeconds;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="key">The key to map to the button</param>
    public VirtualButton(VirtualJoystick joystick)
    {
        ResetHoldTime();
    }
    
    /// <summary>
    /// Update component
    /// </summary>
    public void Update(VirtualJoystick joystick, int id = 0)
    {
        isUsingController = joystick.isUsingController;

        bool isHeld = Input.GetKey(key);
        if (isUsingController)
            isHeld = Input.GetKey("joystick " + (id+1) + " button " + (int)controllerButton);

        if (isHeld)
        {
            IncrementHoldTime();
            return;
        }

        ResetHoldTime();
    }
    
    /// <summary>
    /// Increase the hold timer
    /// </summary>
    void IncrementHoldTime()
    {
        holdTime++;
        holdTimeSeconds += Time.deltaTime;
    }
    /// <summary>
    /// Reset the hold timer
    /// </summary>
    void ResetHoldTime()
    {
        holdTime = 0;
        holdTimeSeconds = 0;
    }

    /// <summary>
    /// Get the hold time of the button
    /// </summary>
    /// <param name="inSeconds"></param>
    /// <returns></returns>
    public float GetHoldTime(bool inSeconds)
    {
        return inSeconds ? holdTimeSeconds : holdTime;
    }

    /// <summary>
    /// Returns a boolean on whether the button is currently being held
    /// </summary>
    /// <param name="button">The virtual button</param>
    public static implicit operator bool(VirtualButton button)
    {
        return Input.GetKey(button.key);
    }
}