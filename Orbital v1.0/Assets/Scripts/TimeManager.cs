using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// keeping track of days, weeks and terms! Can use this to unlock content
public class TimeManager : MonoBehaviour
{
    #region Singleton

    public static TimeManager instance;

    void Awake()
    {
        // hence we Should only have one shop instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        // Made a new game object for this script to latch on, otherwise when we load the new scene, the whole game manager is not destroyed and there will be errors
        DontDestroyOnLoad(this.GetComponent<TimeManager>()) ;            // when we load mini games, we need to access its values for the time
    }

    #endregion

    public DateTime now;
    public TextMeshProUGUI DayAndDate;
    public TextMeshProUGUI AcademicCalender;       

    // School Start Date - We want to get the actual date of when the school starts because maybe some school may differ
    int SchoolStartYear;
    int SchoolStartMonth;
    int SchoolStartDay;
    DateTime newYear = new DateTime(2020, 1, 1);                         // For error handling of user input
    DateTime ThreshHoldForUserInput = new DateTime(2020, 1, 20);         // but in case user input some ridiculous date, we have no choice but to set it to a common date - 2nd jan

    // School Terms - hardcoded and require game to update every year.
    // Could reduce hardcoding by getting current year in start() and then get the correct dates for each term of that year, 
    // but since we are following moe's timetable, we will need the information of the school year beforehand anyway, so just 
    // hardcode it would be the same
    // DateTime(Int32, Int32, Int32)  Initializes a new instance of the DateTime structure to the specified year, month, and day.
    // Consider taking away the hardcoding
    DateTime Term1Start;                                        // (set by user) to Friday 13 March
    DateTime Term1End = new DateTime(2020, 3, 13);                    
    DateTime Term2Start = new DateTime(2020, 3, 23);            // Monday 23 March to Monday 4 May 
    DateTime Term2End = new DateTime(2020, 5, 4);
    DateTime Term3Start = new DateTime(2020, 6, 2);             // Tuesday 2 June to Friday 4 September
    DateTime Term3End = new DateTime(2020, 9, 4);
    DateTime Term4Start = new DateTime(2020, 9, 14);            // Monday 14 September to *Friday 20 November
    DateTime Term4End = new DateTime(2020, 11, 20);

    // Academic Calender
    public int Term = 0;
    TimeSpan TimeSinceStartOfTerm;              // to calculate weeksFromStartOfTerm
    int DaysFromStartOfTerm = 0;                // to calculate weeksFromStartOfTerm
    public int WeeksFromStartOfTerm = 0;               // Will be used to unlock Revision Content based on the weeks into the term
    // Day of Week using DayOfWeek method, see documents on dayOfWeek for details
    bool holiday = false;           // indicator if it is currently the holidays or not

    public int currentWorkingDay;      // to check if a new day has arrived when the player is playing the game, 
                                         // if yes, refresh acad calender and unlock new quizzes for user (used in Scoring script)



    void Start()
    {
        // For start of School date
        SchoolStartYear = PlayerPrefs.GetInt("SchoolStartYear");
        SchoolStartMonth = PlayerPrefs.GetInt("SchoolStartMonth");
        SchoolStartDay = PlayerPrefs.GetInt("SchoolStartDay");

        Debug.Log("--------- ACAD CALENDAR STATS ---------");

        // DateTime(Int32, Int32, Int32)  Initializes a new instance of the DateTime structure to the specified year, month, and day.
        DateTime SchoolStartDate = new DateTime(SchoolStartYear, SchoolStartMonth, SchoolStartDay);
        Debug.Log("School Start Date: " + SchoolStartDate);

        if (SchoolStartDate >= newYear && SchoolStartDate <= ThreshHoldForUserInput)
            Term1Start = SchoolStartDate;               // LOL WE DONT WANT TO WASTE OUR EFFORT GETTING THIS FROM THE PLAYER HAHA
        else
            Term1Start = new DateTime(2020, 1, 2);

        Debug.Log("term 1 start date: " + Term1Start);


        now = DateTime.Now;         // get the current time in this context first (since start() occurs before update(), to prevent not knowing what now is when we calculate academic calendar)
        currentWorkingDay = now.Day;            // to signal if there is a need to refresh academic calendar or not
        Debug.Log("Current working day is: " + currentWorkingDay);

        Debug.Log("--------- ACAD CALENDAR STATS END ---------");


        // For academic calender
        refreshAcademicTerm();                   // for term
        refreshAcademicWeek();                   // for WeeksFromStartOfTerm
                                                 // unlockRevisionContent();


        Debug.Log("--------- QUIZ STATS ---------");
        Debug.Log(currentWorkingDay + " and " + PlayerPrefs.GetInt("DayOfLastQuizCompletion"));
        Debug.Log(Term + " and " + PlayerPrefs.GetInt("TermOfLastQuizCompletion"));
        Debug.Log(WeeksFromStartOfTerm + " and " + PlayerPrefs.GetInt("WeekOfLastQuizCompletion"));
        Debug.Log("--------- QUIZ STATS END ---------");

        // Decide if we should refresh quiz based on the last time the quiz was done.
        // Refresh if it is a new day - will definitely refresh when we first start the game for the first time!!
        if (currentWorkingDay != PlayerPrefs.GetInt("DayOfLastQuizCompletion") || Term != PlayerPrefs.GetInt("TermOfLastQuizCompletion") || WeeksFromStartOfTerm != PlayerPrefs.GetInt("WeekOfLastQuizCompletion"))
        {
            refreshQuiz();
            refreshQuizPassedCounter();

            // For trophy
            PlayerPrefs.SetInt("WaitTillNextDayQuiz", 0);
        }

        // testif refreshQuiz() works
        //refreshQuiz();
        //refreshQuizPassedCounter();
        // PlayerPrefs.SetInt("WaitTillNextDayQuiz", 0);


        // Decide if we should refresh the game availability based on the last time the game was played
        // Refresh if it is a new day - will definitely refresh when we first start the game for the first time
        if (currentWorkingDay != PlayerPrefs.GetInt("DayOfLastMiniGame") || Term != PlayerPrefs.GetInt("TermOfLastMiniGame") || WeeksFromStartOfTerm != PlayerPrefs.GetInt("WeekOfLastMiniGame"))
        {
            refreshHasPlayedGame();
        }


        // testif refreshHasPlayedGame() works
        //refreshHasPlayedGame();         // yep works.
        //PlayerPrefs.SetInt("WaitTillNextDayGame", 0);



    }

    // Update is called once per frame
    void Update()
    {
        // Time UI
        now = DateTime.Now;

       // Debug.Log("current working day: " + currentWorkingDay + " Versus today's day " + now.Day);
       // Debug.Log(currentWorkingDay > now.Day);

        // Only occurs when we enter a new day when the user is playing the game.
        // if yes, we refresh the academic calendar, as well as unlock new quiz and revision contents. 
        // When a new day comes, we should see the date being updated, and quiz contents being replenished
        if (now.Day != currentWorkingDay)
        {
            Debug.Log("We have entered a new day! Refreshing stuffs!");
            refreshAcademicTerm();
            refreshAcademicWeek();
            refreshQuiz();
            refreshQuizPassedCounter();
            refreshHasPlayedGame();

            // unlockRevisionContent();

            currentWorkingDay = now.Day;            // currentWorkingDay will be set to the new day
        }
        // else
        // Debug.Log("Still same day");

        DayAndDate.text = now.ToString("dd MMM yyyy   hh:mm tt");
        if (holiday)
        {
            AcademicCalender.text = "Holiday!";
        }
        else
        {
            AcademicCalender.text = "Term " + Term + " - Week " + WeeksFromStartOfTerm + " - " + now.DayOfWeek;

        }

    }

    public void refreshAcademicTerm() 
    {
        if (now >= Term4Start && now < Term4End)
        {
            Term = 4;
            holiday = false;
        }
        else if (now >= Term3Start && now < Term3End)
        {
            Term = 3;
            holiday = false;
        }
        else if (now >= Term2Start && now < Term2End)
        {
            Term = 2;
            holiday = false;
        }
        else if (now < Term1End)
        {
            Term = 1;
            holiday = false;
        }
        else
        {
            holiday = true;                 // if current time falls within the holiday period.
        }

        //Debug.Log(Term);
    }

    // ***The value of the constants in the DayOfWeek enumeration ranges from DayOfWeek.Sunday to DayOfWeek.Saturday. 
    // If cast to an integer, its value ranges from zero (which indicates DayOfWeek.Sunday) to six (which indicates DayOfWeek.Saturday).
    public void refreshAcademicWeek()
    {
        int DayOfStartOfTerm = 0;       // For sunday this variable should be 7, but school dont start on a Sunday, so we dont have to consider it
        int DaysTillWeek2OfTerm = 0;    // To allow us to just add week 1 into the count and not consider it since week 1 varies according to when school starts for that term

        if (!holiday)           // if current time is within a study term
        {
            // Refer to notes on Microsoft word on how we actually math it out. Its pretty straightforward
            if (Term == 4)
            {
                TimeSinceStartOfTerm = now - Term4Start;                                    
                DaysFromStartOfTerm = TimeSinceStartOfTerm.Days;                        // number of days since the start of the term
                //Debug.Log("Days since start of term: " + DaysFromStartOfTerm);

                // Since the start of the term can start on any day of the week, we need to math it out to get the correct weeks that have passed since start of term
                DayOfStartOfTerm = (int) Term4Start.DayOfWeek;                          // ***(follow notes above function definition)
                //Debug.Log("Day of start of term " + DayOfStartOfTerm);      
                DaysTillWeek2OfTerm = 7 - DayOfStartOfTerm;
                    
                if (DaysFromStartOfTerm <= DaysTillWeek2OfTerm)          // if only week 1 of the new term
                {
                    WeeksFromStartOfTerm = 1;
                } 
                else
                {
                    WeeksFromStartOfTerm++;                         // Take into account week 1
                    DaysFromStartOfTerm -= DaysTillWeek2OfTerm;         // remaining days responsible for the weeks' count except for week 1
                    WeeksFromStartOfTerm += (DaysFromStartOfTerm / 7);          // get the number of weeks since week 1
                    if (DaysFromStartOfTerm % 7 > 0)                    // if in the middle of the last week, we havent taken this into account, so we add one to the count.
                    {
                        WeeksFromStartOfTerm++;
                    }
                }
                //Debug.Log("Weeks since start of term: " + WeeksFromStartOfTerm);
            }
            else if (Term == 3)
            {
                TimeSinceStartOfTerm = now - Term3Start;
                DaysFromStartOfTerm = TimeSinceStartOfTerm.Days;                        // number of days since the start of the term

                // Since the start of the term can start on any day of the week, we need to math it out to get the correct weeks that have passed since start of term
                DayOfStartOfTerm = (int)Term3Start.DayOfWeek;                          // ***(follow notes above function definition)
                DaysTillWeek2OfTerm = 7 - DayOfStartOfTerm;

                if (DaysFromStartOfTerm <= DaysTillWeek2OfTerm)          // if only week 1 of the new term
                {
                    WeeksFromStartOfTerm = 1;
                }
                else
                {
                    WeeksFromStartOfTerm++;                         // Take into account week 1
                    DaysFromStartOfTerm -= DaysTillWeek2OfTerm;         // remaining days responsible for the weeks' count except for week 1
                    WeeksFromStartOfTerm += (DaysFromStartOfTerm / 7);          // get the number of weeks since week 1
                    if (DaysFromStartOfTerm % 7 > 0)                    // if in the middle of the last week, we havent taken this into account, so we add one to the count.
                    {
                        WeeksFromStartOfTerm++;
                    }

                }
            }

            else if (Term == 2)
            {
                TimeSinceStartOfTerm = now - Term2Start;
                DaysFromStartOfTerm = TimeSinceStartOfTerm.Days;                        // number of days since the start of the term

                // Since the start of the term can start on any day of the week, we need to math it out to get the correct weeks that have passed since start of term
                DayOfStartOfTerm = (int)Term2Start.DayOfWeek;                          // ***(follow notes above function definition)
                DaysTillWeek2OfTerm = 7 - DayOfStartOfTerm;

                if (DaysFromStartOfTerm <= DaysTillWeek2OfTerm)          // if only week 1 of the new term
                {
                    WeeksFromStartOfTerm = 1;
                }
                else
                {
                    WeeksFromStartOfTerm++;                         // Take into account week 1
                    DaysFromStartOfTerm -= DaysTillWeek2OfTerm;         // remaining days responsible for the weeks' count except for week 1
                    WeeksFromStartOfTerm += (DaysFromStartOfTerm / 7);          // get the number of weeks since week 1
                    if (DaysFromStartOfTerm % 7 > 0)                    // if in the middle of the last week, we havent taken this into account, so we add one to the count.
                    {
                        WeeksFromStartOfTerm++;
                    }

                }
            }

            else if (Term == 1)
            {
                TimeSinceStartOfTerm = now - Term1Start;
                DaysFromStartOfTerm = TimeSinceStartOfTerm.Days;                        // number of days since the start of the term

                // Since the start of the term can start on any day of the week, we need to math it out to get the correct weeks that have passed since start of term
                DayOfStartOfTerm = (int)Term1Start.DayOfWeek;                          // ***(follow notes above function definition)
                DaysTillWeek2OfTerm = 7 - DayOfStartOfTerm;

                if (DaysFromStartOfTerm <= DaysTillWeek2OfTerm)          // if only week 1 of the new term
                {
                    WeeksFromStartOfTerm = 1;
                }
                else
                {
                    WeeksFromStartOfTerm++;                         // Take into account week 1
                    DaysFromStartOfTerm -= DaysTillWeek2OfTerm;         // remaining days responsible for the weeks' count except for week 1
                    WeeksFromStartOfTerm += (DaysFromStartOfTerm / 7);          // get the number of weeks since week 1
                    if (DaysFromStartOfTerm % 7 > 0)                    // if in the middle of the last week, we havent taken this into account, so we add one to the count.
                    {
                        WeeksFromStartOfTerm++;
                    }

                }
            }

        }
        else
        {
            return;                         // if it is a holiday, we dont need to math anything
        }
    }


    // Refresh quiz availability
    public void refreshQuiz()
    {
        Debug.Log("REFRESHING QUIZ!");
        PlayerPrefs.SetInt("EnglishQuizDone", 0);
        PlayerPrefs.SetInt("MathQuizDone", 0);
        PlayerPrefs.SetInt("PhysicsQuizDone", 0);
        PlayerPrefs.SetInt("ChemQuizDone", 0);
        PlayerPrefs.SetInt("BioQuizDone", 0);

    }

    // reset quiz counter
    public void refreshQuizPassedCounter()
    {
        PlayerPrefs.SetInt("QuizPassedCounter", 0);
        QuizManager.instance.quizPassedCounter = 0;           // cuz we are not updating its value anywhere else other than start() in quizManager, so doing this will allow us to update the value when a new day arrives
    }


    // for mini game. Reset done game indicator
    public void refreshHasPlayedGame()
    {
        PlayerPrefs.SetInt("HasPlayedGame", 0);
        MiniGameManager.instance.hasPlayedGame = 0;         // cuz we are not updating its value anywhere else other than start() in quizManager, so doing this will allow us to update the value when a new day arrives
    }


    // ------------------------------- for Developers -------------------------------
    public void Devs_ResetQuiz()
    {
        refreshQuiz();
        refreshQuizPassedCounter();
        PlayerPrefs.SetInt("WaitTillNextDayQuiz", 0);
    }

    public void Devs_ResetMiniGame()
    {
        refreshHasPlayedGame();         
        PlayerPrefs.SetInt("WaitTillNextDayGame", 0);
    }
}







/*        
        // time of test: 20/6/2020, 12:24am


        DateTime now = DateTime.Now;
        Debug.Log(now);                             // output: 06/20/2020 00:23:25
        Debug.Log(now.TimeOfDay);                   // 00:23:25.1121232

        string time = DateTime.UtcNow.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss tt");
        Debug.Log(time);
*/
