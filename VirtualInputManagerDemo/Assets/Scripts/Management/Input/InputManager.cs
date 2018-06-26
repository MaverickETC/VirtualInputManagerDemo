using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InputManager>();

            if (instance == null)
            {
                GameObject singleton = new GameObject(typeof(InputManager).ToString());
                instance = singleton.AddComponent<InputManager>();

                DontDestroyOnLoad(singleton);
            }

            return instance;
        }
    }

    [SerializeField] List<VirtualJoystick> joystickCollection;

    private void Reset()
    {
        joystickCollection = new List<VirtualJoystick>
            {
                new VirtualJoystick(),
                // new VirtualJoystick(),
                // new VirtualJoystick(),
                // new VirtualJoystick()
            };
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        for (int j = 0; j < joystickCollection.Count; j++)
            joystickCollection[j].Update(j);
    }

    public static void AddJoystick()
    {
        Instance.joystickCollection.Add(new VirtualJoystick());
    }
    public static void RemoveAllJoysticks()
    {
        Instance.joystickCollection.Clear();
        AddJoystick();
    }
    public static VirtualJoystick GetJoystick(int ID)
    {
        if (Instance.joystickCollection.Count - 1 < ID)
            return null;

        return Instance.joystickCollection[ID];
    }
}