using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<TManager> : MonoBehaviour where TManager : MonoBehaviour
{
    private static InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InputManager>();

            return instance;
        }
    }
}