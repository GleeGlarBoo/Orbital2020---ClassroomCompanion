using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

// Place in GameManager in Room scene
// Quite useless for now lol, good thing is at least we get the keys when the game starts, and not when user clicks on the popup menu
public class QuizManager : MonoBehaviour
{
    #region Singleton

    public static QuizManager instance;

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

    public int quizPassedCounter = 0;

    public TextMeshProUGUI quizzesDoneUI;

    // Start is called before the first frame update
    void Start()
    {
        // ONLY INITIALIZE ONCE. Afterwards, just adjust the value within it through quizzes

        if (!PlayerPrefs.HasKey("EnglishQuizDone"))         // if no key, we create one, if have key, we keep its current value as whatever it is
            PlayerPrefs.SetInt("EnglishQuizDone", 0);

        if (!PlayerPrefs.HasKey("MathQuizDone"))
            PlayerPrefs.SetInt("MathQuizDone", 0);

        if (!PlayerPrefs.HasKey("PhysicsQuizDone"))
            PlayerPrefs.SetInt("PhysicsQuizDone", 0);

        if (!PlayerPrefs.HasKey("ChemQuizDone"))
            PlayerPrefs.SetInt("ChemQuizDone", 0);

        if (!PlayerPrefs.HasKey("BioQuizDone"))
            PlayerPrefs.SetInt("BioQuizDone", 0);


        // Record the date where user last finish the quiz. Will use this to check when to reset the quiz availbility
        if (!PlayerPrefs.HasKey("DayOfLastQuizCompletion"))            // if no key, we create one, if have key, we keep its current value as whatever it is
            PlayerPrefs.SetInt("DayOfLastQuizCompletion", 0);

        if (!PlayerPrefs.HasKey("TermOfLastQuizCompletion"))           
            PlayerPrefs.SetInt("TermOfLastQuizCompletion", 0);

        if (!PlayerPrefs.HasKey("WeekOfLastQuizCompletion"))
            PlayerPrefs.SetInt("WeekOfLastQuizCompletion", 0);



        // Quiz counter. Create one if no key exists
        // Set to the number of quizzes done for the day if user has already done some quizzes
        // Reset every new day
        if (!PlayerPrefs.HasKey("QuizPassedCounter"))
            PlayerPrefs.SetInt("QuizPassedCounter", 0);
        else
            quizPassedCounter = PlayerPrefs.GetInt("QuizPassedCounter");
    }


    // Managing the play game button over here too.
    private void Update()
    {
        quizzesDoneUI.text = "Quizzes Passed Today: " + quizPassedCounter.ToString();

        /*  done in quizManager now
        if (QuizDoneCounter >= 2)
        {
            PlayGameButton.SetActive(true);
        }
        */
    }
}
