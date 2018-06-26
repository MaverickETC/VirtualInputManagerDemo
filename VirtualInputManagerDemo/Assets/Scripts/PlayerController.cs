using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;

    private void Reset()
    {
        joystick = new VirtualJoystick();
    }
    private void Update()
    {
        //foreach (var j in joystick)
            joystick.Update();
    }
}
