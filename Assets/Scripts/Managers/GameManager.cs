using System;
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These are set in the Unity editor beforehand
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private GameObject lilDogPrefab;
    [SerializeField] private GameObject dynamicSpawnLoc;
    
    // Track the camera so that we can move it around during gameplay
    // Attach it to the dog during play, show the house in menus, etc
    [SerializeField] private Camera mainCamera;

    // This is serializable only for debug purposes, generated
    // at runtime.
    [SerializeField] private GameObject thePlayer;
    [SerializeField] private List<GameObject> foodPrefabList;

    [SerializeField] private int gameScore;
    [SerializeField] private Timer gameTimer;
    [SerializeField] private KeyCode menuKey = KeyCode.Escape;

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
        SetUpTimer();

        _inputManager.Init(thePlayer.GetComponent<PlayerScript>());
        SpawnFood(new Vector3(0, 0, 3), 0);
        SpawnFood(new Vector3(3, 0, 0), 0);

        _uiManager.Init(gameTimer);
    }

    private void SetUpTimer()
    {
        gameTimer = new Timer(60000);
        gameTimer.Elapsed += EndGame;
        gameTimer.AutoReset = false;
        gameTimer.Enabled = true;
    }

    private void SpawnDog()
    {
        print("Creatin that dawg");
        // First spawn an instance of the player's model and PlayerScript
        thePlayer = Instantiate(lilDogPrefab, new Vector3(0, 0, 0), Quaternion.identity, dynamicSpawnLoc.transform);
        // Next, attach the camera behind the dog
        // and position it
        mainCamera.gameObject.transform.SetParent(thePlayer.transform);
        mainCamera.gameObject.transform.localPosition = new Vector3(0, 1, -2);
    }

    private void SpawnFood(Vector3 spawnLocation, int prefabIndex) {
        print("Making a snack");
        Instantiate(foodPrefabList[prefabIndex], spawnLocation, Quaternion.identity, dynamicSpawnLoc.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(menuKey))
        {
            _inputManager.ToggleEnabled();
            _uiManager.ToggleEnabled();
        }
    }

    private static void EndGame(System.Object source, ElapsedEventArgs e) {
        print("Head to the endgame screen");
        //THis need to show the endgame scoring and then display the menu.
    }


}
