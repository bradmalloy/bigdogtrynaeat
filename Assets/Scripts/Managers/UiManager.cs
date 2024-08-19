using TMPro;
using UnityEngine;

public sealed class UiManager : MonoBehaviour
{
    // Where we get data
    private GameTimer gameTimer;
    private GameManager gameManager;

    // Turn this on or off
    [SerializeField] private GameObject uiTree;
    
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
        uiTree.SetActive(isEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInit)
        {
            timerText.text = gameTimer.GetRemainingTime();
            scoreText.text = "Score: " + gameManager.GetPlayer().GetScore();
            targetScoreText.text = "Next: " + "??";
        }
    }
}