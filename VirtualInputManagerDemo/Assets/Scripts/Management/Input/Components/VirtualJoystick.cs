using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Virtual input joystick component
/// </summary>
[Serializable]
public class VirtualJoystick
{
    /// <summary>
    /// The current ID of the joystick
    /// </summary>
    public int ID;
    /// <summary>
    /// Whether the component is using a joystick
    /// </summary>
    public bool isUsingController;

    [SerializeField] VirtualAnalog[] analogCollection;
    [SerializeField] VirtualAxis[] axisCollection;
    [SerializeField] VirtualButton[] buttonCollection;

    /// <summary>
    /// Constructor
    /// </summary>
    public VirtualJoystick()
    {
        analogCollection = new VirtualAnalog[1];
        axisCollection = new VirtualAxis[1];
        buttonCollection = new VirtualButton[1];
    }
    /// <summary>
    /// Update the component
    /// </summary>
    public void Update(int id = 0)
    {
        ID = id;

        for (int c = 0; c < analogCollection.Length; ++c)
            analogCollection[c].Update(this, ID);
        for (int c = 0; c < axisCollection.Length; c++)
            axisCollection[c].Update(this, ID);
        for (int c = 0; c < buttonCollection.Length; c++)
            buttonCollection[c].Update(this, ID);
    }
}