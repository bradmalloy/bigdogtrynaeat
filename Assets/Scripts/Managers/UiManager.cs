using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public sealed class UiManager : MonoBehaviour
{
    [SerializeField] private Timer gameTimer;
    [SerializeField] private GameObject uiTree;

    private bool isEnabled;

    public void Init(Timer timer)
    {
        Debug.Log("UiManager starting up, assigning timer.");
        gameTimer = timer;

        if (gameTimer != null)
        {
            Debug.Log("Looks good.");
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
}