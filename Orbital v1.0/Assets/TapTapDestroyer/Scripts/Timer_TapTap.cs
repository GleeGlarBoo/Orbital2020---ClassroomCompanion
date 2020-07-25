using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_TapTap : MonoBehaviour
{
    #region Singleton
    public static Timer_TapTap instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
    #endregion

    public float timeRemaining = 60f;
    public TextMeshProUGUI timeText;
    public bool GameStarted = false;

    public GameObject EndGamePanel;

    private void Start()
    {
        // Starts the timer automatically
        // Time.timeScale = 1;
    }

    void Update()
    {
        if (GameStarted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= (Time.deltaTime / MainLoop.instance.TimeScaleNow);          // since by default, timescale is 2, set in mainLoop

            /*    if (MainLoop.instance.TimeScaleNow == 2)
                    timeRemaining -= (Time.deltaTime / 2);          // since by default, timescale is 2, set in mainLoop
                else if (MainLoop.instance.TimeScaleNow == 3)
                    timeRemaining -= (Time.deltaTime / 3);
                else if (MainLoop.instance.TimeScaleNow == 4)
                    timeRemaining -= (Time.deltaTime / 4);
               // else
                 //   timeRemaining -= (Time.deltaTime / 5);

             */

                DisplayTime(timeRemaining);
            }
            else
            {
                //Debug.Log("Time has run out!");
                DisplayGameover();
                EndGamePanel.SetActive(true);
                MainLoop.instance.enableStones = false;

                timeRemaining = 0;
            }
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float seconds = Mathf.FloorToInt(timeToDisplay);

        // Debug.Log(seconds);
        timeText.text = "Time Left: " + seconds.ToString() + " secs";
    }

    void DisplayGameover()
    {
        timeText.text = "Game Over!";
    }


    public void SetGameStartedTrue()
    {
        GameStarted = true;
    }
}
