using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;

public sealed class UiManager : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject uiTree;
    [SerializeField] private GameObject uiTimer;
    private bool IsInit;

    private bool isEnabled;

    public void Init(GameTimer timer)
    {
        Debug.Log("UiManager starting up, assigning timer.");
        gameTimer = timer;

        if (gameTimer != null)
        {
            Debug.Log("Looks good.");
            IsInit = true;
        }
        else
        {
            Debug.LogError("Uh oh, no GARBAGIO was provided?");
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
            Debug.Log("Timer object" + uiTimer.name);
            TextMeshProUGUI textmesh = uiTimer.GetComponent<TextMeshProUGUI>();
            Debug.Log("Mesh object" + textmesh.name);
            textmesh.text = gameTimer.GetRemainingTime();
        }
    }
}