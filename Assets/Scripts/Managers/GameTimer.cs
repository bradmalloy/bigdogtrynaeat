using System;
using System.Timers;

public class GameTimer
{
    private Timer timer;
    private int countdownInSeconds;
    private int remainingSeconds;
    private readonly Action callback;

    public GameTimer(int seconds, Action EndGame)
    {
        countdownInSeconds = seconds;
        remainingSeconds = seconds;
        callback = EndGame;
        timer = new Timer(1000);
        timer.Elapsed += resetTimer;
        timer.AutoReset = true;
        timer.Enabled = true;
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
        }

        return "" + mins + ":" + seconds;
    }

}
