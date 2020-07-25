using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// placed in main camera
// NAMED AS ENGLISHTOPICSELECTOR BUT IT IS FOR ALL SUBJECTS. Just dont want to change it now in case anything happen lol. 
public class EnglishTopicSelector : MonoBehaviour
{
    public GameObject[] TopicHeaders;       // game object cuz we just want to set active it
    public GameObject[] ScrollAreas;
    private DateTime QuizStartTime;

    int topicIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        topicIndex = PlayerPrefs.GetInt("topicIndex");

        TopicHeaders[topicIndex - 1].SetActive(true);
        ScrollAreas[topicIndex - 1].SetActive(true);

        QuizStartTime = TimeManager.instance.now;
    }


    public void TimeOfExit()
    {
        TimeSpan LengthOfRevision = TimeManager.instance.now - QuizStartTime;

        Debug.Log("MINUTES: " + LengthOfRevision.Minutes);

        // if revised for more than 20 minutes. go and earn the award
        if(LengthOfRevision.Minutes > 19)
        {
            PlayerPrefs.SetInt("Revised", 1);       // forever set, hence earned a perma reward
        }
    }
}
