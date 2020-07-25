using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLGameManager : MonoBehaviour
{

    #region Singleton
    public static FLGameManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
    #endregion

    public GameObject ScorePanel;
    public float GameCurrentTimeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

    }

    public void IncreaseSpeedLevels()           // after clicking into the game from the instruction screens. Placed in instruction button
    {
        Invoke("increaseDifficultyToMed", 20f);    // after 22 seconds, things will speed up x1.3
        Invoke("increaseDifficultyToHigh", 40f);    // after 32 seconds, things will speed up x1.5
        Invoke("increaseDifficultyToVeryHigh", 60f);    // after 50 seconds, things will speed up x2
        Invoke("increaseDifficultyToExtremelyHigh", 80f);    // after 80 seconds, things will speed up x4
    }


    // SHOW A COUNTDOWN TO WHEN THE SPEED WILL INCREASE


    // speed up x1.3
    private void increaseDifficultyToMed()
    {
        Debug.Log("Level up");
        if (!Player_FL.instance.Health_Script.IsDead())
            Time.timeScale = 1.3f;

        GameCurrentTimeScale = 1.3f;
    }

    // speed up x1.8
    private void increaseDifficultyToHigh()
    {
        Debug.Log("Level up");
        if (!Player_FL.instance.Health_Script.IsDead())
            Time.timeScale = 1.5f;

        GameCurrentTimeScale = 1.5f;

    }

    // speed up x1.8
    private void increaseDifficultyToVeryHigh()
    {
        Debug.Log("Level up");

        if (!Player_FL.instance.Health_Script.IsDead())
            Time.timeScale = 2.0f;

        GameCurrentTimeScale = 2.0f;

    }

    // speed up x1.8
    private void increaseDifficultyToExtremelyHigh()
    {
        Debug.Log("Level up");

        if (!Player_FL.instance.Health_Script.IsDead())
            Time.timeScale = 4f;

        GameCurrentTimeScale = 4f;

    }

    public void DisplayScorePanel()
    {
        ScorePanel.SetActive(true);
    }

    // Not used now 
    /*
    public void ResetToNormalSpeed()
    {
        Time.timeScale = 1f;
    }
    */




    // end game button in scorePanel script

}
