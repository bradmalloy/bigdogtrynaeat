using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager
{

    private static InputManager instance = null;

    private InputManager()
    {

    }

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }
}