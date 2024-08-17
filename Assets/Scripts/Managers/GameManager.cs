using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These are set in the Unity editor beforehand
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;
    
    // This is serializable only for debug purposes, generated
    // at runtime.
    [SerializeField] private PlayerScript thePlayer;

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
        // Now that the Player exists, give it to InputManager
        // and have it start up
        _inputManager.Init(thePlayer);
    }

    private void SpawnDog()
    {
        print("Creatin that dawg");
        // replace this with an Instantiate of a prefab
        thePlayer = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
