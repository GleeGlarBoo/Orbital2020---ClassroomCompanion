using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_FL : MonoBehaviour
{

    public float timeRemaining = 20f;
    private int timerMaxRounds = 4;
    private int currentTimerRounds = 0;
    public TextMeshProUGUI timeText;
    private bool GameStarted = false;

    private void Start()
    {
        // Starts the timer automatically
        currentTimerRounds = 0;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (GameStarted)
        {
            if (currentTimerRounds < timerMaxRounds)             // since we have 4 difficulty levels. 
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 20f;
                    currentTimerRounds++;
                }
            }
            else
            {
                DisplayMaxSpeed();
            }

        }

        if (Player_FL.instance.Health_Script.IsDead())           // if dead, display game over
        {
            GameStarted = false;
            DisplayGameover();
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Debug.Log(seconds);
        timeText.text = "Increasing speed in " + seconds.ToString() + " secs";
    }

    void DisplayGameover()
    {
        timeText.text = "Game Over!";
    }

    void DisplayMaxSpeed()
    {
        timeText.text = "Reached Max Speed!";
    }

    public void SetGameStartedTrue()
    {
        GameStarted = true;
    }
}
