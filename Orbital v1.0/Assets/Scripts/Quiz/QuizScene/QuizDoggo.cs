using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuizDoggo : MonoBehaviour
{
    #region Singleton

    // Using Singleton pattern so that it can be accessible from other scripts - just like NUS CDG example
    // Singleton instance – creating a variable with the same type as our class 
    // Static variable = variable shared by all instances of a class 
    public static QuizDoggo instance;

    void Awake()
    {
        // Since we set instance as this particular component, can access it using Inventory.instance
        // hence we Should only have one inventory instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    #endregion

    public Animator animator;
    private float BarkOrIdle;           // probability
    private float BarkHigh = 0.20f;             // to be increased after every idle. Will go back to 20% again after barking

    public float minTimeIdling = 1f, maxTimeIdling = 2.5f;     // Random time interval for idling

    // Barking variables
    private int JustStartedQuiz = 1;                // only want to set this to be 1 when we first arrived into the scene
    public float doggoBarkTime = 2f;            // decides when we want to select the next animation
    private float contentSelector = 0f;             // Probability.

    // speech bubble, only 1 type
    public GameObject speechbubble_Stronk;   
    
    // initial welcome message - before pressing on start button
    public GameObject Leggo;              
    public GameObject Exciting;                    
    public GameObject AllTheBest;
    
    // quiz messages - after pressing on start button
    public GameObject LoveQuizzes;
    public GameObject CanDoIt;                   
    public GameObject Fun;
    public GameObject AceQuiz;
    public GameObject Together;

    // end quiz messages 
    public GameObject GreatJob;
    public GameObject Awesome;
    public GameObject WellDone;
    public GameObject Improvement;
    public GameObject AceNext;
    public GameObject ReviseMore;
    public GameObject AlmostThere;


    // End quiz indicator. IMPORTANT INDICATOR. dictates alot of the behavior and distinguishes when doggo do last bark . 
    private bool QuizDone = false;          // so if user finish 5 questions, doggo will bark once with specific msg on results of player, then sit down


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(QuizDoggoCoroutine());
    }

    IEnumerator QuizDoggoCoroutine()
    {

        yield return new WaitForSeconds(0.2f);            // just wait for 0.2 second before barking.

        animator.SetBool("Bark", true);                 // firstly, bark to motivate player to start the quiz
        yield return new WaitForSeconds(doggoBarkTime);            // just wait for 0.2 second before barking.

        while (!QuizDone)            // repeat all the time if quiz is not done yet
        {
            BarkOrIdle = Random.value;
            Debug.Log("BarkHigh: " + BarkHigh);
            Debug.Log("Bark or Idle: " + BarkOrIdle);

            if (BarkOrIdle < BarkHigh)                     // 20% will bark at first, every idle will increase it so we can bark more
            {
                animator.SetBool("Bark", true);
                yield return new WaitForSeconds(doggoBarkTime);            // just wait for 0.2 second before barking.

                BarkHigh = 0.20f;               // goes back to 20%
            }
            else                                        
            {
                animator.SetBool("Bark", false);
                yield return new WaitForSeconds(Random.Range(minTimeIdling, maxTimeIdling));

                BarkHigh += 0.03f;              // idling increases the chance for doggo to bark by 3%
            }
        }

        // if quiz is over, doggo bark once with results of player and then go on to sit down
        animator.SetBool("Bark", true);
        yield return new WaitForSeconds(doggoBarkTime - 0.5f);
        animator.SetBool("Bark", false);
        animator.SetBool("Sit", true);          // after doggo sits down, he will sit down forever and not bark or what. 


    }

    // placed in code when user just finished 5 quizzes. 
    public void EndQuizSignal ()
    {
        QuizDone = true;
    }


    // To be attached to the quiz's start button
    public void DisableStartingAnimation()
    {
        JustStartedQuiz = 0;
    }


    // attached to barking animation
    // Manage Speech bubbles. Called within the barking animation
    public void SpeechBubblePopUpForQuiz()
    {
        Debug.Log("JUST STARTED QUIZ VAR: " + JustStartedQuiz);
        Debug.Log("Should be 0 after we pressed the start button");


        // If just came from welcome page
        if (JustStartedQuiz == 1)
        {
            speechbubble_Stronk.SetActive(true);
            contentSelector = Random.value;
            Debug.Log("content selector: " + contentSelector);

            if (contentSelector < 0.33f)
            {
                Leggo.SetActive(true);
            }
            else if (contentSelector < 0.66f)
            {
                Exciting.SetActive(true);
            }
            else
            {
                AllTheBest.SetActive(true);
            }
        }
        else if (!QuizDone)                        
        {
            speechbubble_Stronk.SetActive(true);
            contentSelector = Random.value;
            Debug.Log("content selector: " + contentSelector);


            if (contentSelector < 0.20f)
            {
                LoveQuizzes.SetActive(true);
            }
            else if (contentSelector < 0.40f)
            {
                CanDoIt.SetActive(true);
            }
            else if (contentSelector < 0.60f)
            {
                Fun.SetActive(true);
            }
            else if (contentSelector < 0.80f)
            {
                AceQuiz.SetActive(true);
            }
            else
            {
                Together.SetActive(true);
            }


        }
        else                // quiz done, bark about results
        {
            speechbubble_Stronk.SetActive(true);
            contentSelector = Random.value;
            Debug.Log("content selector: " + contentSelector);

            if (GameManager.score >= 4)
            {
                if (contentSelector < 0.33f)
                {
                    GreatJob.SetActive(true);
                }
                else if (contentSelector < 0.66f)
                {
                    Awesome.SetActive(true);
                }
                else
                {
                    WellDone.SetActive(true);
                }
            }
            else if (GameManager.score == 3)            // barely passed
            {
                if (contentSelector < 0.50f)
                {
                    Improvement.SetActive(true);
                }
                else
                {
                    AceNext.SetActive(true);
                }

            }
            else                                          // fail quiz
            {
                if (contentSelector < 0.50f)
                {
                    ReviseMore.SetActive(true);
                }
                else
                {
                    AlmostThere.SetActive(true);
                }

            }

        }
    }

    public void DisableSpeechBubbleAndEverythingForQuiz()
    {
        // Since we do not know which one was randomly chosen, we will just disable everything related to speech bubble
        speechbubble_Stronk.SetActive(false);
        Leggo.SetActive(false);
        Exciting.SetActive(false);
        AllTheBest.SetActive(false);

        LoveQuizzes.SetActive(false);
        CanDoIt.SetActive(false);
        Fun.SetActive(false);
        AceQuiz.SetActive(false);
        Together.SetActive(false);

        GreatJob.SetActive(false);
        Awesome.SetActive(false);
        WellDone.SetActive(false);
        Improvement.SetActive(false);
        AceNext.SetActive(false);
        ReviseMore.SetActive(false);
        AlmostThere.SetActive(false);

    }




}