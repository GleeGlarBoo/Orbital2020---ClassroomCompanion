using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rev_PopUpMenu : MonoBehaviour
{

    public Button[] buttons;

    // Load all the topics within the terms
    public void LoadTextEditiing()
    {
        SceneManager.LoadScene("TextEditing");
    }

    // and more...



    private void Start()
    {


    }

    // PlayerPrefs values Set to 1 whenever user completes quiz - so this is defined in Scoring.cs
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
