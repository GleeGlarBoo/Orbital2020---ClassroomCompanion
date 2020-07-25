using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class Scoring : MonoBehaviour
{

    public TextMeshProUGUI score;

    public void AddScore()
    {
        // GAME MANAGER IS A STATIC CLASS SO IT CAN BE ASSESSED FROM ANY SCRIPT FROM ANY SCENE.

        GameManager.score++;        // correct buttons will have this function being called, wrong buttons wont have

        /*
        if (this.gameObject.CompareTag("Correct"))
        {
            GameManager.score++;
        }
        */
    }


    public void ClearScore()
    {
        GameManager.score = 0;
    }

    private void Update()
    {
        score.text = GameManager.score.ToString() + "/5";
    }


    // Reset everyday to 0 - cleared in TimeManager
    public void EndQuiz (int Index)                       
    {
        switch(Index)
        {
            case 1:
                PlayerPrefs.SetInt("EnglishQuizDone", 1);       // value of 1 signifies that it is done today
                break;
            case 2:
                PlayerPrefs.SetInt("MathQuizDone", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("PhysicsQuizDone", 1);
                break;
            case 4:
                PlayerPrefs.SetInt("ChemQuizDone", 1);
                break;
            case 5:
                PlayerPrefs.SetInt("BioQuizDone", 1);
                break;
            default:
                Debug.Log("Error LOLOLOLOLOLOLOLOLOLOLOLOLOLOL you suck");
                break;
        }


        // Sets the date when the user last finished a quiz. 
        // We get the full date because otherwise if we only get the day, if user comes back and play at the exact same day but the next month, 
        // then there will be a bug
        PlayerPrefs.SetInt("DayOfLastQuizCompletion", TimeManager.instance.currentWorkingDay);
        PlayerPrefs.SetInt("TermOfLastQuizCompletion", TimeManager.instance.Term);
        PlayerPrefs.SetInt("WeekOfLastQuizCompletion", TimeManager.instance.WeeksFromStartOfTerm);


        // Taking note of how many quizzes we have done with a passing grade
        if (GameManager.score >= 3)
        {
            int quizPassedCounter = PlayerPrefs.GetInt("QuizPassedCounter");
            quizPassedCounter++;          // add to the count
            PlayerPrefs.SetInt("QuizPassedCounter", quizPassedCounter);
        }

        /*
        if (SceneManager.GetActiveScene().name == "EnglishQuiz")
        {
            FindObjectOfType<PopUpMenu>().SendMessage("FinishedQuiz", QuizIndex);
        }
        */
    }

}
