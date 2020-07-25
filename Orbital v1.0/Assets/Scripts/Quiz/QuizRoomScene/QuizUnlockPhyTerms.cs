using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.EventSystems;

public class QuizUnlockPhyTerms : MonoBehaviour
{

    // Term of the acad year we are in.
    int term;


    #region Singleton


    // Singleton pattern, same as for inventory script
    public static QuizUnlockPhyTerms instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;


        term = TimeManager.instance.Term;

    }


    #endregion

    public Button Term1topic1;
    public Button Term1topic2;
    public Button Term2topic1;
    public Button Term2topic2;
    public Button Term3topic1;
    public Button Term3topic2;
    public Button Term4topic1;

    // For quiz multiple topic selection
    public int[] selectedTopics = new int[3];
    public Button PHY_startButton;

    // to signal to the buttons if they can turn back to the normal color when we click on back or exit space
    public int numTimesEnterPage = 0;

    private bool rebounce = false;

    private void Start()
    {

        for(int i = 0; i < 3; i++)
        {
            selectedTopics[i] = 0;
        }


        // If we are at term 3 now, only buttons up to term 3 will be enabled 
        ActivateButtonsBasedOnTerms();
        
    }

    
    private void Update()
    {
        // if 0 topics selected, cant start the quiz
        if (selectedTopics[0] == 0 && selectedTopics[1] == 0 && selectedTopics[2] == 0)
        {
            PHY_startButton.interactable = false;

            // The case of us selecting 3 topics already, then leaving the page, and coming back afterwards.
            // without this, after selecting 3 topics, buttons will be disabled, but when we leave the page and come back in
            // only our selected array is cleared, but the buttons remain disabled.
            // so we need this to ensure the buttons are enabled as well too.
            if (rebounce == true)
            {
                // If we are at term 3 now, only buttons up to term 3 will be enabled 
                ActivateButtonsBasedOnTerms();
            }
        } 
        else if (selectedTopics[0] != 0 && selectedTopics[1] != 0 && selectedTopics[2] != 0)    // if 3 topics selected, cant choose anymore
        {
            Term1topic1.interactable = false;
            Term1topic2.interactable = false;

            Term2topic1.interactable = false;
            Term2topic2.interactable = false;

            Term3topic1.interactable = false;
            Term3topic2.interactable = false;

            Term4topic1.interactable = false;

        }
        else
        {
            PHY_startButton.interactable = true;         // if selected any1 of the topics, can start quiz already
        }
    }
    

    // whenever we click into the english topics selection page
    public void OnSubjectEnterPage()
    {
        // when we open up the english topic parent item, we will increment this value
        // for the individual buttons, they will check if the current value of this variable they have is the same as what this singleton has
        // if yes then it will maintain it selectedness, else it will be reset to be unselected since 
        // it just means we have opened up this for a new time.
        numTimesEnterPage++;

        // reset rebounce indicator
        rebounce = true;
    }


    // for the buttons, input in their topic
    public void onTopicSelection(int index)
    {
        bool OkayToAdd = true;
        for(int i = 0; i < 3; i++)
        {
            if (selectedTopics[i] == index)     // just to prevent double adding it into the array.
                OkayToAdd = false;              // Code below will take care of deselecting a topic
        }


        // checking against the selected topics array to see if we have already selected this topic, 
        // if yes, it means we have selected it and now when we click on it we want to deselect it
        // if no, then it means we want to select it
        for(int i = 0; i < 3; i++)
        {
            if (selectedTopics[i] == index)
                selectedTopics[i] = 0;
            else if (selectedTopics[i] == 0 && OkayToAdd)             // select an empty slot from the array. Able to double add, hence we need the indicator that none of the array element = him
            {      
                selectedTopics[i] = index;
                break;                          // so only 1 element gets changed
            }
        }
    }

    // to be placed in term 1's BACK button and exit space
    public void PhyClearSelectedArray()
    {
        for (int i = 0; i < 3; i++)
        {
            selectedTopics[i] = 0;
        }

        rebounce = false;       // might be unncessary but for safe measures
    }

    // to be placed in Physics start button
    public void PHY_OnStartingQuiz()
    {
        if (selectedTopics[0] != 0)
            PlayerPrefs.SetInt("selectedTopic1", selectedTopics[0]);
        if (selectedTopics[1] != 0)
            PlayerPrefs.SetInt("selectedTopic2", selectedTopics[1]);
        if (selectedTopics[2] != 0)
            PlayerPrefs.SetInt("selectedTopic3", selectedTopics[2]);

        Invoke("LoadPhysicsQuiz", 0.05f);   // delay to ensure playerprefs are set first. 

    }

    public void LoadPhysicsQuiz()
    {
        SceneManager.LoadScene("PhysicsQuiz");
    }



    private void ActivateButtonsBasedOnTerms()
    {
        switch (term)
        {
            case 1:
                Term1topic1.interactable = true;
                Term1topic2.interactable = true;
                break;
            case 2:
                Term1topic1.interactable = true;
                Term1topic2.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;
                break;
            case 3:
                Term1topic1.interactable = true;
                Term1topic2.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;

                Term3topic1.interactable = true;
                Term3topic2.interactable = true;
                break;
            case 4:
                Term1topic1.interactable = true;
                Term1topic2.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;

                Term3topic1.interactable = true;
                Term3topic2.interactable = true;

                Term4topic1.interactable = true;
                break;
        }
    }

}
