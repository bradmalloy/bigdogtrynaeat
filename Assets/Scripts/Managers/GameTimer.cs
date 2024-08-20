using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private int remainingSeconds;
    private int startingSeconds;
    private bool paused = false;
    private GameManager manager;
    private Coroutine timerCoroutine;

    public void Init(int secondsDuration, GameManager theBoss)
    {
        startingSeconds = secondsDuration;
        remainingSeconds = secondsDuration;
        manager = theBoss;
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void TogglePause()
    {
        Debug.Log("Pausing/unpausing the timer.");
        paused = !paused;

        if (paused && timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        else if (!paused && timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (remainingSeconds > 0)
        {
            yield return new WaitForSeconds(1f);

            if (!paused)
            {
                remainingSeconds--;

                if (remainingSeconds <= 0)
                {
                    Debug.Log("The timer is over! Time to call EndGame.");
                    manager.EndGame();
                }
            }
        }
    }

    public string GetRemainingTime()
    {
        int mins = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        string sec = seconds < 10 ? "0" + seconds : "" + seconds;
        return "" + mins + ":" + sec;
    }

    public int GetElapsedSeconds()
    {
        return startingSeconds - remainingSeconds;
    }

    public int GetRemainingSeconds()
    {
        return remainingSeconds;
    }
}
