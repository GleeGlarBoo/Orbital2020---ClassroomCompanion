using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quiz_PopUpMenu : MonoBehaviour
{

    public Button[] buttons;
    // public int subjectSelected;          // gonna use individual topic's start button now

        /*
    // To differentiate which subject the user has selected in order to tell the start button to load which scene
    public void SubjectSelected(int index)
    {
        subjectSelected = index;
    }

    
    public void onStartButton()
    {
        switch(subjectSelected)
        {
            case 1:
                LoadEnglishQuiz();
                break;
            case 2:
                LoadMathQuiz();
                break;
            case 3:
                LoadPhysicsQuiz();
                break;
            case 4:
                LoadChemQuiz();
                break;
            case 5:
                LoadBiologyQuiz();
                break;
            default:
                Debug.Log("Selected subject is out of index. error");
                break;
        }
    }
    */

    public void LoadEnglishQuiz ()
    {
        SceneManager.LoadScene("EnglishQuiz");
    }

    public void LoadMathQuiz ()
    {
        SceneManager.LoadScene("MathQuiz");
    }

    public void LoadPhysicsQuiz ()
    {
        SceneManager.LoadScene("PhysicsQuiz");
    }

    public void LoadChemQuiz()
    {
        SceneManager.LoadScene("ChemQuiz");
    }

    public void LoadBiologyQuiz()
    {
        SceneManager.LoadScene("BioQuiz");
    }




    private void Start()
    {
        FinishedOrRefreshQuiz();

        // Clear previous traces of topics selected.
        if (PlayerPrefs.HasKey("selectedTopic1"))
            PlayerPrefs.DeleteKey("selectedTopic1");      
        if (PlayerPrefs.HasKey("selectedTopic2"))
            PlayerPrefs.DeleteKey("selectedTopic2");      
        if (PlayerPrefs.HasKey("selectedTopic3"))
            PlayerPrefs.DeleteKey("selectedTopic3");      

    }

    // PlayerPrefs values Set to 1 whenever user completes quiz - this is defined in Scoring.cs
    // PlayerPrefs values set to reset to 0 everytime we enter a new day - so this is defined in TimeManger.
    public void FinishedOrRefreshQuiz()
    {
        if (PlayerPrefs.GetInt("EnglishQuizDone") == 1)         
            buttons[0].interactable = false;                    // lock the quiz if user has already done it
        else
            buttons[0].interactable = true;                     // if a new day comes and the playerPrefs key gets sets to 0, re-enable the quiz

        if (PlayerPrefs.GetInt("MathQuizDone") == 1)
            buttons[1].interactable = false;
        else
            buttons[1].interactable = true;

        if (PlayerPrefs.GetInt("PhysicsQuizDone") == 1)
            buttons[2].interactable = false;
        else
            buttons[2].interactable = true;

        if (PlayerPrefs.GetInt("ChemQuizDone") == 1)
            buttons[3].interactable = false;
        else
            buttons[3].interactable = true;

        if (PlayerPrefs.GetInt("BioQuizDone") == 1)
            buttons[4].interactable = false;
        else
            buttons[4].interactable = true;

    }



}
