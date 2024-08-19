using System;
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // These are set in the Unity editor beforehand
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private GameObject lilDogPrefab;
    [SerializeField] private GameObject dynamicSpawnLoc;

    // Food spawning variables
    [SerializeField] private float totalFoodSpawnFill = 0.5f;
    [SerializeField] private float percentBadFood = 0.3f;
    [SerializeField] private List<GameObject> foodPrefabList;
    [SerializeField] private GameObject foodSpawnLocationsParent;
    
    // Track the camera so that we can move it around during gameplay
    // Attach it to the dog during play, show the house in menus, etc
    [SerializeField] private CameraRig mainCameraRig;

    // This is serializable only for debug purposes, generated
    // at runtime.
    [SerializeField] private GameObject thePlayer;

    [SerializeField] private int gameScore;
    [SerializeField] private GameTimer gameTimer;
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
        _inputManager.Init(thePlayer.GetComponent<PlayerScript>());
        
        // Spawn good and bad food, filling up 50% of the spawn locations
        SpawnAllFood();

        // Other misc start-game tasks
        SetUpTimer(); // must happen before UI init
        _uiManager.Init(gameTimer);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetUpTimer()
    {
        gameTimer = new GameTimer(60, EndGame);
    }

    private void SpawnDog()
    {
        print("Creatin that dawg");
        // First spawn an instance of the player's model and PlayerScript
        thePlayer = Instantiate(lilDogPrefab, new Vector3(0, 0, 0), Quaternion.identity, dynamicSpawnLoc.transform);
        // Attach the camera rig to the player so it will follow
        mainCameraRig.transform.SetParent(thePlayer.transform);
        mainCameraRig.SetPlayer(thePlayer);
        thePlayer.GetComponent<PlayerScript>().Init(mainCameraRig.transform);
    }

    private void SpawnAllFood()
    {
        // Given a list of locations we can spawn food, we're going to fill up
        // totalFoodSpawnFill % of those spots, and percentBadFood of those will
        // be bad food.
        
        // Remove the parent (which is included in GetComponentsInChildren) since it's not a spawn location
        List<Transform> spawnLocations = foodSpawnLocationsParent.GetComponentsInChildren<Transform>().Skip(1).ToList();
        int possibleSpawnLocationCount = spawnLocations.Count;
        Debug.Log("Spawn location total count: " + possibleSpawnLocationCount);
        int maxAllFoodSpawns = (int) Math.Ceiling(possibleSpawnLocationCount * totalFoodSpawnFill);
        Debug.Log("Percent we're going to fill: " + totalFoodSpawnFill);
        Debug.Log("Number of locations to fill (after mult): " + maxAllFoodSpawns);
        int maxBadFoodSpawns = (int) Math.Ceiling(maxAllFoodSpawns * percentBadFood);
        Debug.Log("Percent of bad food: " + percentBadFood);
        Debug.Log("Number of locations to fill with bad food: " + maxBadFoodSpawns);
        int maxGoodFoodSpawns = maxAllFoodSpawns - maxBadFoodSpawns;
        Debug.Log("Number of locations to fill with good food: " + maxGoodFoodSpawns);
        
        // Before we try to spawn, shake up the list
        // TODO

        int goodFoodCounter = 0;
        int badFoodCounter = 0;
        foreach (Transform location in spawnLocations)
        {
            if (goodFoodCounter >= maxGoodFoodSpawns && badFoodCounter >= maxBadFoodSpawns)
            {
                // Stop spawning if both maximum counts are reached
                break;
            }

            // Randomly choose between good and bad food
            bool spawnGoodFood = Random.value > 0.5f;

            if (spawnGoodFood && goodFoodCounter < maxGoodFoodSpawns)
            {
                SpawnFood(location.position, 0);
                Debug.Log("Spawning good food at " + location.gameObject.name);
                goodFoodCounter++;
            }
            else if (!spawnGoodFood && badFoodCounter < maxBadFoodSpawns)
            {
                SpawnFood(location.position, 1);
                Debug.Log("Spawning bad food at " + location.gameObject.name);
                badFoodCounter++;
            }
        }
    }

    private void SpawnFood(Vector3 spawnLocation, int prefabIndex) {
        print("Making a snack");
        Instantiate(foodPrefabList[prefabIndex], spawnLocation, Quaternion.identity, dynamicSpawnLoc.transform);
    }

    // Update is called once per frame
    void Update()
    {
        string thing = gameTimer.ToString();
        print(thing);
        if (Input.GetKeyDown(menuKey))
        {
            _inputManager.ToggleEnabled();
            _uiManager.ToggleEnabled();
        }
    }

    private static void EndGame() {
        print("Head to the endgame screen");
        //THis need to show the endgame scoring and then display the menu.
    }
}
