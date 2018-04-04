using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : BaseManager<InputManager>
{
    [SerializeField] private List<VirtualJoystick> joystickList = new List<VirtualJoystick>(4);

    public static int NumberOfJoysticks { get { return Instance.joystickList.Count; } }

    private void Update()
    {
        // Update all joysticks if they are plugged in
        foreach (VirtualJoystick joystick in joystickList)
        {
            joystick.ID = joystickList.IndexOf(joystick);
            joystick.Update();
        }
    }

    public static void AddJoystick()
    {
        Instance.joystickList.Add(new VirtualJoystick());
    }
    public static void ClearJoysticks()
    {
        Instance.joystickList.Clear();
    }

    public static VirtualJoystick GetJoystick(int joystickID)
    {
        return Instance.joystickList[joystickID];
    }
    public static VirtualButton GetButton(int joystickID, int buttonID)
    {
        var joystick = GetJoystick(joystickID);

        if (joystick != null)
            return joystick.buttonList.Count > buttonID ? joystick.buttonList[buttonID]: null;

        return null;
    }
    public static VirtualAxis GetAxis(int joystickID, int axisID)
    {
        var joystick = GetJoystick(joystickID);

        if (joystick != null)
            return joystick.axisList.Count > axisID ? joystick.axisList[axisID] : null;

        return null;
    }
    public static VirtualAnalog GetAnalog(int joystickID, int analogID)
    {
        var joystick = GetJoystick(joystickID);

        if (joystick != null)
            return joystick.analogList.Count > analogID ? joystick.analogList[analogID] : null;

        return null;
    }
}