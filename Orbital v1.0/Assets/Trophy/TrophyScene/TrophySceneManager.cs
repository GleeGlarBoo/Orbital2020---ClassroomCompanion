using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophySceneManager : MonoBehaviour
{
    // All references to the objects and the placeholders
    // Have their own playerprefs to store if they have been earned before or not
    public GameObject[] trophyPlaceholders;
    public GameObject QuizMaster;
    public GameObject JackOfAllTrades;
    public GameObject Diligent;
    public GameObject beginnerGamer;
    public GameObject intermediateGamer;
    public GameObject StudyHardAndPlayHard;
    public GameObject GoodBoi;
    public GameObject GoodGoodBoi;
    public GameObject SuperGoodBoi;
    public GameObject JustMovedIn;
    public GameObject ReadyForCNYvisits;
    public GameObject aWellDecoratedRoom;


    // Start is called before the first frame update
    void Start()
    {
        // Should be landscape only now
        Screen.orientation = ScreenOrientation.Landscape;


        // Quiz Master Award
        if (PlayerPrefs.HasKey("Earned1"))
        {
            if (PlayerPrefs.GetInt("Earned1") == 1)
            {
                // Displaying trophy
                QuizMaster.SetActive(true);
                trophyPlaceholders[0].SetActive(false);
            }
        }


        // Jack of All Trades Award
        if (PlayerPrefs.HasKey("Earned2"))          // only has key once we earn it
        {
            if (PlayerPrefs.GetInt("Earned2") == 1)
            {
                JackOfAllTrades.SetActive(true);
                trophyPlaceholders[1].SetActive(false);
            }
        }

        // Diligent Award
        if (PlayerPrefs.HasKey("EarnedRevise"))
        {
            if (PlayerPrefs.GetInt("EarnedRevise") == 1)
            {
                Diligent.SetActive(true);
                trophyPlaceholders[2].SetActive(false);
            }
        }



        // Beginner Gamer Award
        if (PlayerPrefs.HasKey("Earned3"))          // only has key once we earn it
        {
            if (PlayerPrefs.GetInt("Earned3") == 1)
            {
                beginnerGamer.SetActive(true);
                trophyPlaceholders[3].SetActive(false);
            }
        }

        // intermediate Gamer Award. Only after earning the beginner gamer award, to prevent double adding to numGamesPlayed
        if (PlayerPrefs.HasKey("Earned3"))
        {
            if (PlayerPrefs.HasKey("Earned4"))          // only has key once we earn it
            {
                if (PlayerPrefs.GetInt("Earned4") == 1)
                {
                    intermediateGamer.SetActive(true);
                    trophyPlaceholders[4].SetActive(false);
                }
            }
        }


        // StudyHardAndPlayHard Award. Only after earning the intermediate gamer award, to prevent double adding to numGamesPlayed
        if (PlayerPrefs.HasKey("Earned4"))
        {
            if (PlayerPrefs.HasKey("Earned5"))          // only has key once we earn it
            {
                if (PlayerPrefs.GetInt("Earned5") == 1)
                {
                    StudyHardAndPlayHard.SetActive(true);
                    trophyPlaceholders[5].SetActive(false);
                }
            }
        }


        // Good Boi Award
        if (PlayerPrefs.HasKey("Earned6"))
        {
            GoodBoi.SetActive(true);
            trophyPlaceholders[6].SetActive(false);

        }



        // Good Good Boi Award
        if (PlayerPrefs.HasKey("Earned7"))
        {
            GoodGoodBoi.SetActive(true);
            trophyPlaceholders[7].SetActive(false);

        }


        // Super Good Boi Award
        if (PlayerPrefs.HasKey("Earned8"))
        {
            SuperGoodBoi.SetActive(true);
            trophyPlaceholders[8].SetActive(false);

        }


        // Just Moved In Award. If already earned it
        if (PlayerPrefs.HasKey("Earned9"))
        {
            JustMovedIn.SetActive(true);
            trophyPlaceholders[9].SetActive(false);
        }

        // Ready For CNY Visits Award. If already earned it
        if (PlayerPrefs.HasKey("Earned10"))
        {
            ReadyForCNYvisits.SetActive(true);
            trophyPlaceholders[10].SetActive(false);

        }

        // Well Decorated Room Award. If already earned it
        if (PlayerPrefs.HasKey("Earned11"))
        {
            aWellDecoratedRoom.SetActive(true);
            trophyPlaceholders[11].SetActive(false);

        }
    }


}
