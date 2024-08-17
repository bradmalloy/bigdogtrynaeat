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
        // SpawnDog creates the Player instance, which
        // many other Managers need as a reference
        SpawnDog();
        _uiManager = UiManager.Instance;
        _inputManager = InputManager.Instance;
        // Now that the Player exists, give it to InputManager
        //_inputManager.SetPlayer(null);
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
