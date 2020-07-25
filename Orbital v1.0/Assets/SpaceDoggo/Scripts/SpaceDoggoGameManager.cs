using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDoggoGameManager : MonoBehaviour
{

    #region Singleton
    public static SpaceDoggoGameManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
    #endregion

    public GameObject ScorePanel;

    public float timescalenow = 1;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("increaseDifficultyToMed", 16.0f);    // after 16 seconds, things will speed up x1.3
        Invoke("increaseDifficultyToHigh", 32.0f);    // after 32 seconds, things will speed up x1.8
        Invoke("increaseDifficultyToVeryHigh", 50.0f);    // after 50 seconds, things will speed up x2.5
        Invoke("increaseDifficultyToExtremelyHigh", 90.0f);    // after 80 seconds, things will speed up x2.5
    }



    // speed up x1.3
    private void increaseDifficultyToMed()
    {
        if (!Player.instance.m_Health.IsDead())
            Time.timeScale = 1.3f;

        timescalenow = 1.3f;
    }

    // speed up x1.8
    private void increaseDifficultyToHigh()
    {
        if (!Player.instance.m_Health.IsDead())
            Time.timeScale = 1.8f;

        timescalenow = 1.8f;

    }

    // speed up x1.8
    private void increaseDifficultyToVeryHigh()
    {
        if (!Player.instance.m_Health.IsDead())
            Time.timeScale = 2.5f;

        timescalenow = 2.5f;

    }

    // speed up x1.8
    private void increaseDifficultyToExtremelyHigh()
    {
        if (!Player.instance.m_Health.IsDead())
            Time.timeScale = 4f;

        timescalenow = 4f;

    }

    public void DisplayScorePanel()
    {
        ScorePanel.SetActive(true);
    }

    public void ResetToNormalSpeed()
    {
        Time.timeScale = 1f;
    }



    // end game button in scorePanel script

}

