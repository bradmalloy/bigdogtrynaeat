using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UiManager _uiManager;
    public InputManager _inputManager;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
    }

    private void StartUp()
    {
        _uiManager = UiManager.Instance;
        SpawnDog();
        _inputManager = InputManager.Instance;
    }

    private void SpawnDog()
    {
        print("Creatin that dawg");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
