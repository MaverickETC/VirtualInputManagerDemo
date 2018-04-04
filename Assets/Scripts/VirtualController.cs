using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualController : MonoBehaviour
{
    public Image AnalogStick;
    Vector3 AnalogStickIdlePosition;

    public Image ButtonA;
    public Image ButtonB;
    public Image ButtonX;
    public Image ButtonY;

    Color ButtonAColor;
    Color ButtonBColor;
    Color ButtonXColor;
    Color ButtonYColor;

    public void Awake()
    {
        AnalogStick = transform.Find("Analog").Find("AnalogStick").GetComponent<Image>();
        AnalogStickIdlePosition = AnalogStick.rectTransform.localPosition;

        ButtonA = transform.Find("Buttons").Find("A").GetComponent<Image>();
        ButtonAColor = ButtonA.color;
        ButtonB = transform.Find("Buttons").Find("B").GetComponent<Image>();
        ButtonBColor = ButtonB.color;
        ButtonX = transform.Find("Buttons").Find("X").GetComponent<Image>();
        ButtonXColor = ButtonX.color;
        ButtonY = transform.Find("Buttons").Find("Y").GetComponent<Image>();
        ButtonYColor = ButtonY.color;
    }

    public void Update()
    {
        VirtualAnalog analog = InputManager.GetAnalog(0, 0);

        VirtualButton a = InputManager.GetButton(0, 0);
        VirtualButton b = InputManager.GetButton(0, 1);
        VirtualButton x = InputManager.GetButton(0, 2);
        VirtualButton y = InputManager.GetButton(0, 3);

        AnalogStick.rectTransform.localPosition = AnalogStickIdlePosition + (Vector3)((Vector2)analog * 40);

        ButtonA.color = a ? Color.white : ButtonAColor;
        ButtonB.color = b ? Color.white : ButtonBColor;
        ButtonX.color = x ? Color.white : ButtonXColor;
        ButtonY.color = y ? Color.white : ButtonYColor;
    }
}