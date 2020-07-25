using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_FL : MonoBehaviour
{
    bool isPaused;
    float GameCurTimeScale;

    private void Start()
    {
        isPaused = false;
    }


    public void OnClick()
    {
        if (!isPaused)
        {
            GameCurTimeScale = FLGameManager.instance.GameCurrentTimeScale;
            Time.timeScale = 0;
            isPaused = true;

            Debug.Log(GameCurTimeScale);
        }
        else
        {
            Time.timeScale = GameCurTimeScale;
            isPaused = false;
        }
    }
}
