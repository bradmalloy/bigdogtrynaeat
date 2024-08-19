using System;
using System.Timers;
using UnityEngine;

public class GameTimer
{
    private Timer timer;
    private int remainingSeconds;
    private readonly Action callback;
    private bool paused = false;

    public GameTimer(int seconds, Action EndGame)
    {
        remainingSeconds = seconds;
        callback = EndGame;
        // The Timer will trigger every 1000 milliseconds, 
        // not a _duration_ of 1000 milliseconds
        timer = new Timer(1000);
        timer.Elapsed += resetTimer;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    public void TogglePause()
    {
        Debug.Log("Pausing/unpausing the timer.");
        paused = !paused;
        if (paused) { timer.Stop(); }
        else { timer.Start(); }
    }

    void resetTimer(System.Object source, ElapsedEventArgs e)
    {
        if (remainingSeconds > 0)
        {
            remainingSeconds--;
        }
        else
        {
            timer.Enabled = false;
            callback();
        }

    }

    public string GetRemainingTime()
    {
        int mins = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        string sec = "" + seconds;
        if (seconds < 10)
        {
            sec = "0" + seconds;
            return "00:" + sec;
        }

        return "" + mins + ":" + seconds;
    }

}
