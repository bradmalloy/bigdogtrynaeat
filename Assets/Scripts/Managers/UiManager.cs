using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class UiManager : MonoBehaviour
{
    // Where we get data
    private GameTimer gameTimer;
    private GameManager gameManager;

    // Turn this on or off
    [FormerlySerializedAs("uiTree")] [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject startMenu;

    // Text fields
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI targetScoreText;
    
    private bool IsInit;
    private bool isEnabled;

    public void Init(GameTimer timer, GameManager theBoss)
    {
        Debug.Log("UiManager starting up, assigning timer and game manager");
        gameTimer = timer;
        gameManager = theBoss;

        if (gameTimer != null && gameManager != null)
        {
            Debug.Log("Timer and manager provided, UIManager starting.");
            IsInit = true;
        }
        else
        {
            Debug.LogError("Timer or manager wasn't provided, can't start UIManager");
        }
    }

    public void ToggleEnabled()
    {
        isEnabled = !isEnabled;
        pauseMenu.SetActive(isEnabled);
        if (isEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    public void HideStartMenu()
    {
        startMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInit)
        {
            timerText.text = gameTimer.GetRemainingTime();
            scoreText.text = "Score: " + gameManager.GetPlayer().GetScore();
            targetScoreText.text = "Goal: " + gameManager.GetPlayer().GetNextScoreTarget();
        }
    }

    private void Start()
    {
        // Ensure we can't see the pause menu at the start
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
        
        // and that we _can_ see the start menu
        startMenu.SetActive(true);
    }
}