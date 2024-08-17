using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These are set in the Unity editor beforehand
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private GameObject lilDogPrefab;
    [SerializeField] private GameObject dynamicSpawnLoc;
    [SerializeField] private GameObject wallSpawnLoc;
    [SerializeField] private GameObject floorSpawnLoc;

    // This is serializable only for debug purposes, generated
    // at runtime.
    [SerializeField] private GameObject thePlayer;

    [SerializeField] private int gameScore;
    [SerializeField] private System.Timers.Timer gameTimer;

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
        _inputManager.Init(thePlayer.GetComponent<PlayerScript>());
    }

    private void SpawnDog()
    {
        print("Creatin that dawg");
        thePlayer = Instantiate(lilDogPrefab, new Vector3(0, 0, 0), Quaternion.identity, dynamicSpawnLoc.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
