using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    public float timeRemaining = 4f;
    public TextMeshProUGUI timeText_FL;
    public TextMeshProUGUI timeText_TapTap;
    private bool GameStarted = false;

    private void Start()
    {
        timeRemaining = 4f;
    }

    void Update()
    {
        if (GameStarted)
        {

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0f;
            }

        }

    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Debug.Log(seconds);
        timeText_FL.text = seconds.ToString();
        timeText_TapTap.text = seconds.ToString();
    }


    // PUT IN PLAYGAME BUTTON in room scene.
    public void SetGameStartedTrue()
    {
        GameStarted = true;
    }
}
