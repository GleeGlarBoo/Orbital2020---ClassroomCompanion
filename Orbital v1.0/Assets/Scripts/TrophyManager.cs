using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    #region Singleton

    public static TrophyManager instance;

    void Awake()
    {
        // hence we Should only have one shop instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    #endregion

    //  All references to the objects and the placeholders
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

    // include all trophy alerts too
    public GameObject AwardedAlert;
    public GameObject[] awardAlerts;


    // Spent amount
    public int SpentAmount;         // added in ShopSlot script when user buys stuff (onBuyItem()), deducted in inventoryUI script when user sells stuff (onSellConfirm())

    // Start is called before the first frame update
    void Start()
    {
        // For testing now
        //Reset();

        if (PlayerPrefs.HasKey("SpentAmount"))
            SpentAmount = PlayerPrefs.GetInt("SpentAmount");
        else
            SpentAmount = 0;

        // Quiz Master Award
        // checking it in start because we will come back to room scene from the quiz scene and this will be checked
        // count is refreshed every day in time manager
        // we will check if the trophy is already active or not, if yes then we have already earned the reward and wont get a new one, if not then we earn a new trophy.
        if (PlayerPrefs.HasKey("Earned1"))
        {
            if (PlayerPrefs.GetInt("Earned1") == 1)
            {
                // Displaying trophy
                QuizMaster.SetActive(true);
                trophyPlaceholders[0].SetActive(false);
            }
        }
        else if (PlayerPrefs.HasKey("QuizPassedCounter"))               // first time earning the reward
        {
            Debug.Log("SHOULD NOT BE SEEN IF FIRST AWARD IS EARNED ALREADY");

            if (PlayerPrefs.GetInt("QuizPassedCounter") == 5)
            {

                // Displaying trophy
                QuizMaster.SetActive(true);
                trophyPlaceholders[0].SetActive(false);
                PlayerPrefs.SetInt("Earned1", 1);           // to indicate we have earned this one-time trophy already

                // Award panel
                // First time earning would require an awarded panel to show to the user that he has a new award
                AwardedAlert.SetActive(true);
                awardAlerts[0].SetActive(true);

                // Add to the count for the JackOfAllTrades award, done BELOW.
            }
        }


        // Jack of All Trades Award
        // Cant join in with the above code because that uses an else if statement. If trophy is already earned then the code wont run le
        if (PlayerPrefs.HasKey("Earned2"))          // only has key once we earn it
        {
            if (PlayerPrefs.GetInt("Earned2") == 1)
            {
                JackOfAllTrades.SetActive(true);
                trophyPlaceholders[1].SetActive(false);
            }
        }
        else if (PlayerPrefs.HasKey("QuizPassedCounter"))            // wont execute these code if already earned trophy 
        {
            Debug.Log("SHOULD NOT BE SEEN IF SECOND AWARD IS EARNED ALREADY");

            if (PlayerPrefs.GetInt("QuizPassedCounter") == 5 && PlayerPrefs.GetInt("WaitTillNextDayQuiz") == 0)       // wont execute more than once on the same day
            {
                if (PlayerPrefs.HasKey("numDaysForAward2"))             // could be 9 here going to 10, hence the code below
                {
                    int numDays = PlayerPrefs.GetInt("numDaysForAward2");
                    numDays++;
                    Debug.Log("HERE numDays Quiz: " + numDays);
                    PlayerPrefs.SetInt("numDaysForAward2", numDays);
                    PlayerPrefs.SetInt("WaitTillNextDayQuiz", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager


                    // Checking to see if we have just reached 10 days, if yes, user earn the award
                    if (PlayerPrefs.GetInt("numDaysForAward2") == 10)           // first time earning the reward
                    {
                        JackOfAllTrades.SetActive(true);
                        trophyPlaceholders[1].SetActive(false);
                        PlayerPrefs.SetInt("Earned2", 1);               // hence we automatically show the trophy and wont check if we have to earn it again anymore

                        // Award panel
                        // First time earning would require an awarded panel to show to the user that he has a new award
                        AwardedAlert.SetActive(true);
                        awardAlerts[1].SetActive(true);

                    }
                }
                else                                            // first day completing 5 quizzes in a day. Could be put above but since in our testing, we already earned the above award, we shall put it here to set it for the first time.
                {
                    PlayerPrefs.SetInt("numDaysForAward2", 1);
                    PlayerPrefs.SetInt("WaitTillNextDayQuiz", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager
                }

            }
        }

        // Diligent Award
        // as long as user stayed in revision panel for more than 20 mins once in the game, will always display this award 
        if (PlayerPrefs.HasKey("EarnedRevise"))
        {
            Diligent.SetActive(true);
            trophyPlaceholders[2].SetActive(false);

        }
        else if (PlayerPrefs.HasKey("Revised"))             // first time earning it
        {
            if (PlayerPrefs.GetInt("Revised") == 1)
            {
                Diligent.SetActive(true);
                trophyPlaceholders[2].SetActive(false);
                PlayerPrefs.SetInt("EarnedRevise", 1);

                // Award panel
                // First time earning would require an awarded panel to show to the user that he has a new award
                AwardedAlert.SetActive(true);
                awardAlerts[2].SetActive(true);

            }
        }



        // Beginner Gamer Award
        // Cant join in with the above code because that uses an else if statement. If trophy is already earned then the code wont run le
        if (PlayerPrefs.HasKey("Earned3"))          // only has key once we earn it
        {
            if (PlayerPrefs.GetInt("Earned3") == 1)
            {
                beginnerGamer.SetActive(true);
                trophyPlaceholders[3].SetActive(false);
            }
        }
        else if (PlayerPrefs.HasKey("HasPlayedGame"))            // wont execute these code if already earned trophy 
        {
            // Debug.Log("SHOULD NOT BE SEEN IF THIRD AWARD IS EARNED ALREADY");

            if (PlayerPrefs.GetInt("HasPlayedGame") == 1 && PlayerPrefs.GetInt("WaitTillNextDayGame") == 0)       // wont execute more than once on the same day
            {
                if (PlayerPrefs.HasKey("numGamesPlayed"))             // could be 9 here going to 10, hence the code below
                {
                    int numGames = PlayerPrefs.GetInt("numGamesPlayed");
                    numGames++;
                    Debug.Log("HERE numGames: " + numGames);
                    PlayerPrefs.SetInt("numGamesPlayed", numGames);
                    PlayerPrefs.SetInt("WaitTillNextDayGame", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager


                    // Checking to see if we have just reached 10 days, if yes, user earn the award
                    if (PlayerPrefs.GetInt("numGamesPlayed") == 20)           // first time earning the reward
                    {
                        beginnerGamer.SetActive(true);
                        trophyPlaceholders[3].SetActive(false);
                        PlayerPrefs.SetInt("Earned3", 1);               // hence we automatically show the trophy and wont check if we have to earn it again anymore

                        // Award panel
                        // First time earning would require an awarded panel to show to the user that he has a new award
                        AwardedAlert.SetActive(true);
                        awardAlerts[3].SetActive(true);

                    }
                }
                else                                            // first time completing a game. Could be put above but since in our testing, we already earned the above award, we shall put it here to set it for the first time.
                {
                    PlayerPrefs.SetInt("numGamesPlayed", 1);
                    PlayerPrefs.SetInt("WaitTillNextDayGame", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager
                }

            }
        }


        // intermediate Gamer Award. Only after earning the beginner gamer award, to prevent double adding to numGamesPlayed
        // Cant join in with the above code because that uses an else if statement. If trophy is already earned then the code wont run le
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
            else if (PlayerPrefs.HasKey("HasPlayedGame"))            // wont execute these code if already earned trophy 
            {
                // Debug.Log("SHOULD NOT BE SEEN IF THIRD AWARD IS EARNED ALREADY");

                if (PlayerPrefs.GetInt("HasPlayedGame") == 1 && PlayerPrefs.GetInt("WaitTillNextDayGame") == 0)       // wont execute more than once on the same day
                {
                    if (PlayerPrefs.HasKey("numGamesPlayed"))             // could be 9 here going to 10, hence the code below
                    {
                        int numGames = PlayerPrefs.GetInt("numGamesPlayed");
                        numGames++;
                        Debug.Log("HERE numGames: " + numGames);
                        PlayerPrefs.SetInt("numGamesPlayed", numGames);
                        PlayerPrefs.SetInt("WaitTillNextDayGame", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager


                        // Checking to see if we have just reached 10 days, if yes, user earn the award
                        if (PlayerPrefs.GetInt("numGamesPlayed") == 40)           // first time earning the reward
                        {
                            intermediateGamer.SetActive(true);
                            trophyPlaceholders[4].SetActive(false);
                            PlayerPrefs.SetInt("Earned4", 1);               // hence we automatically show the trophy and wont check if we have to earn it again anymore

                            // Award panel
                            // First time earning would require an awarded panel to show to the user that he has a new award
                            AwardedAlert.SetActive(true);
                            awardAlerts[4].SetActive(true);

                        }
                    }
                }
            }

        }


        // StudyHardAndPlayHard Award. Only after earning the intermediate gamer award, to prevent double adding to numGamesPlayed
        // Cant join in with the above code because that uses an else if statement. If trophy is already earned then the code wont run le
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
            else if (PlayerPrefs.HasKey("HasPlayedGame"))            // wont execute these code if already earned trophy 
            {
                // Debug.Log("SHOULD NOT BE SEEN IF THIRD AWARD IS EARNED ALREADY");

                if (PlayerPrefs.GetInt("HasPlayedGame") == 1 && PlayerPrefs.GetInt("WaitTillNextDayGame") == 0)       // wont execute more than once on the same day
                {
                    if (PlayerPrefs.HasKey("numGamesPlayed"))             // could be 9 here going to 10, hence the code below
                    {
                        int numGames = PlayerPrefs.GetInt("numGamesPlayed");
                        numGames++;
                        Debug.Log("HERE numGames: " + numGames);
                        PlayerPrefs.SetInt("numGamesPlayed", numGames);
                        PlayerPrefs.SetInt("WaitTillNextDayGame", 1);           // so we dont unneccesarrily add one more to numDays when we restart the game. Reset to 0 in TimeManager


                        // Checking to see if we have just reached 10 days, if yes, user earn the award
                        if (PlayerPrefs.GetInt("numGamesPlayed") == 60)           // first time earning the reward
                        {
                            StudyHardAndPlayHard.SetActive(true);
                            trophyPlaceholders[5].SetActive(false);
                            PlayerPrefs.SetInt("Earned5", 1);               // hence we automatically show the trophy and wont check if we have to earn it again anymore

                            // Award panel
                            // First time earning would require an awarded panel to show to the user that he has a new award
                            AwardedAlert.SetActive(true);
                            awardAlerts[5].SetActive(true);

                        }
                    }
                }
            }
        }


        // Good Boi Award
        if (PlayerPrefs.HasKey("Earned6"))
        {
            GoodBoi.SetActive(true);
            trophyPlaceholders[6].SetActive(false);

        }
        else if (GameManager.doggoLevel == 5)             // first time earning it
        {

            GoodBoi.SetActive(true);
            trophyPlaceholders[6].SetActive(false);
            PlayerPrefs.SetInt("Earned6", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[6].SetActive(true);
        }



        // Good Good Boi Award
        if (PlayerPrefs.HasKey("Earned7"))
        {
            GoodGoodBoi.SetActive(true);
            trophyPlaceholders[7].SetActive(false);

        }
        else if (GameManager.doggoLevel == 10)             // first time earning it
        {

            GoodGoodBoi.SetActive(true);
            trophyPlaceholders[7].SetActive(false);
            PlayerPrefs.SetInt("Earned7", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[7].SetActive(true);
        }


        // Super Good Boi Award
        if (PlayerPrefs.HasKey("Earned8"))
        {
            SuperGoodBoi.SetActive(true);
            trophyPlaceholders[8].SetActive(false);

        }
        else if (GameManager.doggoLevel == 15)             // first time earning it
        {

            SuperGoodBoi.SetActive(true);
            trophyPlaceholders[8].SetActive(false);
            PlayerPrefs.SetInt("Earned8", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[8].SetActive(true);
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



    private void Update()
    {

        // Just Moved In Award
        if (SpentAmount >= 500 && !PlayerPrefs.HasKey("Earned9"))             // first time earning it
        {

            JustMovedIn.SetActive(true);
            trophyPlaceholders[9].SetActive(false);
            PlayerPrefs.SetInt("Earned9", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[9].SetActive(true);
        }


        // Ready For CNY Visits Award
        if (SpentAmount >= 1500 && !PlayerPrefs.HasKey("Earned10"))             // first time earning it
        {

            ReadyForCNYvisits.SetActive(true);
            trophyPlaceholders[10].SetActive(false);
            PlayerPrefs.SetInt("Earned10", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[10].SetActive(true);
        }

        // Well Decorated Room Award
        if (SpentAmount >= 3000 && !PlayerPrefs.HasKey("Earned11"))             // first time earning it
        {

            aWellDecoratedRoom.SetActive(true);
            trophyPlaceholders[11].SetActive(false);
            PlayerPrefs.SetInt("Earned11", 1);

            // Award panel
            // First time earning would require an awarded panel to show to the user that he has a new award
            AwardedAlert.SetActive(true);
            awardAlerts[11].SetActive(true);
        }
    }

    // For buying something
    public void IncreaseSpentAmount(int amt)
    {
        SpentAmount += amt;
        PlayerPrefs.SetInt("SpentAmount", SpentAmount);
    }

    // For selling something
    public void DecreaseSpentAmount(int amt)
    {
        SpentAmount -= amt;
        PlayerPrefs.SetInt("SpentAmount", SpentAmount);
    }

    public void Reset()
    {
        if (PlayerPrefs.HasKey("Earned1"))
            PlayerPrefs.DeleteKey("Earned1");

        if (PlayerPrefs.HasKey("Earned2"))
            PlayerPrefs.DeleteKey("Earned2");

        if (PlayerPrefs.HasKey("numDaysForAward2"))
            PlayerPrefs.DeleteKey("numDaysForAward2");

        if (PlayerPrefs.HasKey("EarnedRevise"))
            PlayerPrefs.DeleteKey("EarnedRevise");

        if (PlayerPrefs.HasKey("Revised"))
            PlayerPrefs.DeleteKey("Revised");

        if (PlayerPrefs.HasKey("Earned3"))
            PlayerPrefs.DeleteKey("Earned3");

        if (PlayerPrefs.HasKey("Earned4"))
            PlayerPrefs.DeleteKey("Earned4");

        if (PlayerPrefs.HasKey("Earned5"))
            PlayerPrefs.DeleteKey("Earned5");

        if (PlayerPrefs.HasKey("numGamesPlayed"))
            PlayerPrefs.DeleteKey("numGamesPlayed");

        if (PlayerPrefs.HasKey("Earned6"))
            PlayerPrefs.DeleteKey("Earned6");

        if (PlayerPrefs.HasKey("Earned7"))
            PlayerPrefs.DeleteKey("Earned7");

        if (PlayerPrefs.HasKey("Earned8"))
            PlayerPrefs.DeleteKey("Earned8");

        if (PlayerPrefs.HasKey("Earned9"))
            PlayerPrefs.DeleteKey("Earned9");

        if (PlayerPrefs.HasKey("Earned10"))
            PlayerPrefs.DeleteKey("Earned10");

        if (PlayerPrefs.HasKey("Earned11"))
            PlayerPrefs.DeleteKey("Earned11");

        SpentAmount = 0;
    }
}
