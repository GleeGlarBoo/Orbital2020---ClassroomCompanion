using System.Collections;
using System.Collections.Generic;
using System.Linq;                  // for the method that converts array to list
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Host the logic for the quiz. Load the questions according to what topics the user has selected, 
// check if the answer is correct, and then load a new question 
public class ChemQuizGameManager : MonoBehaviour
{
    // UI and interfaces
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionNumber;
    public TextMeshProUGUI questionTopic;
    public Image questionImageDisplay;
    public TextMeshProUGUI Option1text;
    public TextMeshProUGUI Option2text;
    public TextMeshProUGUI Option3text;
    public TextMeshProUGUI Option4text;
    public GameObject Option3Obj;               // will be enabling or disabling option 3 and 4 if not needed or needed
    public GameObject Option4Obj;


    public GameObject QuestionPage;
    public GameObject ScorePage;
    public GameObject QuitButton;
    public GameObject NextButton;       // only enable the next button after user completed the quiz after 2.5 secs have passed, so that doggo can bark


    public TextMeshProUGUI score;

    /* use arrays when we dont need to resize the list, while we use a list if we need to resize at run time. */
    /* idea here is to use a huge array to store all of the questions first, then select a few based on what topics the user has selected,
     * then afterwards we randomly select 5 from there to be put into the main array. We then have an unanswered list to take note of which qns
     * are not done yet*/

    // Array for our entire question bank
    public ChemQuestion[] questionBank;

    // all the questions related to the topics (max 3) that the user has selected in room scene 
    public List<ChemQuestion> relatedQuestions;

    // Array for the 5 questions we have selected based on the topics that the user have selected. 
    public ChemQuestion[] selectedQuestions = new ChemQuestion[5];

    // Allows us to pick a random question but wont allow us to pick the same question twice 
    // Using a list to contain all of the questions in the beginning and then remove them 1 by 1 as we answer questions
    // static because we want it to persist between scenes, so when we reload the scene to load in new qn, it will rmb what is stored
    private static List<ChemQuestion> unansweredQuestions;
    private static int questionCount = 0;       // count and display the question number, will be reset whenever user leaves the quiz


    private ChemQuestion currentQuestion;

    public List<ChemQuestion> answeredQuestions;       // to be used for the summary page
    private int[] CorrectOrWrong = new int[5];      // to be used for summary page too. Will display if the player had the qn correct or not 
                                                    // 0 = wrong, 1 = right

    public Sprite correctImage;
    public Sprite wrongImage;

    // load 5 questions related to the topic the user has selected from questionBank into selectedQuestions
    private void Awake()
    {
        int topic1 = -1;        // -1 so that we wont get questionBank[i].topic == 0 because then it will definitely be true
        int topic2 = -1;
        int topic3 = -1;

        if (PlayerPrefs.HasKey("selectedTopic1"))
            topic1 = PlayerPrefs.GetInt("selectedTopic1") - 1;      // -1 since our topic enum here is 0-indexed, while we didnt do it for the room scene
        else
            Debug.Log("NO TOPIC 1");
        if (PlayerPrefs.HasKey("selectedTopic2"))
            topic2 = PlayerPrefs.GetInt("selectedTopic2") - 1;
        else
            Debug.Log("NO TOPIC 2");
        if (PlayerPrefs.HasKey("selectedTopic3"))
            topic3 = PlayerPrefs.GetInt("selectedTopic3") - 1;
        else
            Debug.Log("NO TOPIC 3");

        for (int i = 0; i < questionBank.Length; i++)
        {
            if ((int)questionBank[i].topic == topic1 || (int)questionBank[i].topic == topic2 || (int)questionBank[i].topic == topic3)
            {
                relatedQuestions.Add(questionBank[i]);
            } 
        }

        // Safety Measures for summary page
        if (answeredQuestions != null)
        {
            while (answeredQuestions.Count > 0)
            {
                answeredQuestions.RemoveAt(1);        // keep removing the top most element bahs. like a stack / queue
            }

        }

        for (int i = 0; i < 5; i++)
        {
            CorrectOrWrong[i] = 0;          // by default all are wrong first
        }

    }

    private void Start()
    {
        // Also handles the case if user exits a previous quiz halfway using the quit button
        questionCount = 0;

       // Debug.Log(relatedQuestions.Count);

        // Randomly choose 5 questions from related questions to be displayed to the player
        for (int i = 0; i < 5; i++)
        {
            int randomQnIndex = Random.Range(0, relatedQuestions.Count);
            
            selectedQuestions[i] = relatedQuestions[randomQnIndex];
            relatedQuestions.RemoveAt(randomQnIndex);       // to ensure no repeat of question
            
        }

        // Just to make sure that we have nothing in unansweredQuestions. Game might crash and stuff that causes user to 
        // come to this scene without prior clearance, so we do this just for in case
        if (unansweredQuestions != null)
        {
            while (unansweredQuestions.Count > 0)
            {
                unansweredQuestions.RemoveAt(0);        // keep removing the top most element bahs. like a stack / queue
            }

        }

        // load in the qns into unansweredQuestions. But we only want to do it when we first enter the scene from room
        // possible also for it to have 0 elements in the list but not null because we have initialized it before. Might be
        // the case after user done a quiz before for that subject, or simply quit the quiz and try again. 
        // Hence we reinitialize everything to 0 in the code above
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = selectedQuestions.ToList<ChemQuestion>();
        }
        

        // selects a random question from unansweredQuestions for the user. 
        // NOW, We put this on each option button and start quiz button
        // SetCurrentQuestion();
        // Debug.Log(currentQuestion.question + " and answer is: " + currentQuestion.answer);
    }

    // instead of reloading the scene, we can simply place this whenever the user selected an option, 
    // because that just means he has done answering that question and we will be moving on to another question
    // hence, there is no need to reload the scene
    // place it at the start button too
    // So everytime we click on an answer or the start button, it will refresh the question
    public void SetNextQuestionOrEnd()
    {
        // if end quiz
        if (questionCount == 5)
        {
            // transition to end quiz after answering 5 questions
            QuestionPage.SetActive(false);
            ScorePage.SetActive(true);

            // Disable quit button too cuz we want them to read through the summary of the questions
            QuitButton.SetActive(false);
            Invoke("EnableNextButton", 3f);

            // for doggo to transition to end quiz behaviour
            QuizDoggo.instance.EndQuizSignal();


            EndQuiz(4);                                             // CHANGE ACCORDING TO SUBJECT

            // implement anything to clear // or if things are retinitiated at the start then its fine
        }
        else
        {
            questionCount++;

            // Based on the number of unaswered questions, we select 1 from the list
            int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
            currentQuestion = unansweredQuestions[randomQuestionIndex];

            // Updating UI
            questionNumber.text = "QUESTION " + questionCount.ToString();
            questionText.text = currentQuestion.question;
            DisplayQuestionTopic(-1, questionTopic);

            // Display any image picture if there is, else we wont show any
            if (currentQuestion.questionImage != null)
            {
                questionImageDisplay.enabled = true;
                questionImageDisplay.sprite = currentQuestion.questionImage;
                questionImageDisplay.preserveAspect = true;
            }
            else
            {
                questionImageDisplay.enabled = false;
            }

            switch (currentQuestion.numberOfOptions)
            {
                case 2:
                    Option3Obj.SetActive(false);
                    Option4Obj.SetActive(false);
                    break;
                case 3:
                    Option3Obj.SetActive(true);
                    Option4Obj.SetActive(false);
                    Option3text.text = currentQuestion.option3;
                    break;
                case 4:
                    Option3Obj.SetActive(true);
                    Option4Obj.SetActive(true);
                    Option3text.text = currentQuestion.option3;
                    Option4text.text = currentQuestion.option4;
                    break;

            }

            // Always will have 2 options at least. 
            Option1text.text = currentQuestion.option1;
            Option2text.text = currentQuestion.option2;


            // Add it in the correct order to the answerQuestion list to display to the user in the end
            answeredQuestions.Add(currentQuestion);

            // once it is loaded in, we remove it from the list. Make sure we dont select the same question twice
            // unansweredQuestions.RemoveAt(randomQuestionIndex);            // use this if any thing fails
            unansweredQuestions.Remove(currentQuestion);


        }
    }

    private void EnableNextButton()
    {
        NextButton.SetActive(true);
    }

    // can be placed into any option button. General correct checker
    // Each option will input their corresponding option index - 1, 2, 3 or 4, and we will check with the question answer
    public void correctnessChecker(int index)
    {
        // checking if user has input the correct answer
        if (currentQuestion.answer == index)
        {
           // Debug.Log("Correct!");
            AddScore();
            CorrectOrWrong[questionCount - 1] = 1;      // if user got it correct, we take note of it
        }
        else
        {
           // Debug.Log("Wrong!");
            CorrectOrWrong[questionCount - 1] = 0;       // if user got it wrong, we take note of it

        }
    }



    /* --------------------- From the original scoring script ---------------------- */
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

    // place in start button to ensure we always reset the score before we start on any quiz
    public void ClearScore()
    {
        GameManager.score = 0;
    }


    private void Update()
    {
        score.text = GameManager.score.ToString() + "/5";
    }

    // CALL IT BEFORE THEY SEE THE SUMMARY, TO ENSURE NO LOOPHOLE OF THEM KEEP BEING ABLE TO TRY THE QUIZ AGAIN IF THEY RESTART THE GAME
    // BEFORE LOOKING THRU THE SUMMARY
    // Reset everyday to 0 - cleared in TimeManager
    public void EndQuiz(int Index)
    {
        // add in clear out unansweredQuestions list
        // reset anything that has to be reset. maybe nth else too la

        switch (Index)
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


    // FOR UI
    void DisplayQuestionTopic(int questionTopicNum, TextMeshProUGUI topicTextToBeDisplayed)
    {
        if (questionTopicNum == -1)
        {
            questionTopicNum = (int)currentQuestion.topic;          // The argument is to allow us to use it below to for summary page
        }

        switch (questionTopicNum)
        {
            case 0:
                topicTextToBeDisplayed.text = "(Topic: Elements, compounds and mixtures)";
                break;
            case 1:
                topicTextToBeDisplayed.text = "(Topic: Separating techniques)";
                break;
            case 2:
                topicTextToBeDisplayed.text = "(Topic: Chemical changes)";
                break;
            case 3:
                topicTextToBeDisplayed.text = "(Topic: Kinetic particle theory)";
                break;
            case 4:
                topicTextToBeDisplayed.text = "(Topic: Atoms 1)";
                break;
            case 5:
                topicTextToBeDisplayed.text = "(Topic: Atoms 2)";
                break;
            default:
                Debug.Log("WRONG TOPIC LOL");
                break;

        }
    }



    // ----------------------------- FOR SUMMARY PAGE -----------------------------


    public TextMeshProUGUI summary1QnText;
    public TextMeshProUGUI summary1Topic;
    public TextMeshProUGUI summary1CorrectAnswer;
    public TextMeshProUGUI summary1Explanation;
    public Image summary1QnImageDisplay;
    public Image CorrectOrWrong1;

    public TextMeshProUGUI summary2QnText;
    public TextMeshProUGUI summary2Topic;
    public TextMeshProUGUI summary2CorrectAnswer;
    public TextMeshProUGUI summary2Explanation;
    public Image summary2QnImageDisplay;
    public Image CorrectOrWrong2;

    public TextMeshProUGUI summary3QnText;
    public TextMeshProUGUI summary3Topic;
    public TextMeshProUGUI summary3CorrectAnswer;
    public TextMeshProUGUI summary3Explanation;
    public Image summary3QnImageDisplay;
    public Image CorrectOrWrong3;

    public TextMeshProUGUI summary4QnText;
    public TextMeshProUGUI summary4Topic;
    public TextMeshProUGUI summary4CorrectAnswer;
    public TextMeshProUGUI summary4Explanation;
    public Image summary4QnImageDisplay;
    public Image CorrectOrWrong4;

    public TextMeshProUGUI summary5QnText;
    public TextMeshProUGUI summary5Topic;
    public TextMeshProUGUI summary5CorrectAnswer;
    public TextMeshProUGUI summary5Explanation;
    public Image summary5QnImageDisplay;
    public Image CorrectOrWrong5;


    // Sets the UI after user finish his attempt of the quiz. When user clicks on Next at the score page
    // no choice but to hardcode since our design isnt dynamic
    public void onFinishingQuizAttempt()
    {
        // -------------------------------- For one question in the summary page -------------------------//
        summary1QnText.text = answeredQuestions[0].question;
        DisplayQuestionTopic((int)answeredQuestions[0].topic, summary1Topic);

        switch (answeredQuestions[0].answer)
        {
            case 1:
                summary1CorrectAnswer.text = "Correct answer: " + answeredQuestions[0].option1;
                break;
            case 2:
                summary1CorrectAnswer.text = "Correct answer: " + answeredQuestions[0].option2;
                break;
            case 3:
                summary1CorrectAnswer.text = "Correct answer: " + answeredQuestions[0].option3;
                break;
            case 4:
                summary1CorrectAnswer.text = "Correct answer: " + answeredQuestions[0].option4;
                break;

        }

        summary1Explanation.text = "Explanation: " + answeredQuestions[0].explanation;

        if (answeredQuestions[0].questionImage != null)
        {
            summary1QnImageDisplay.enabled = true;
            summary1QnImageDisplay.sprite = answeredQuestions[0].questionImage;
            summary1QnImageDisplay.preserveAspect = true;
        }
        else
        {
            summary1QnImageDisplay.enabled = false;
        }

        // if correct, show tick
        if (CorrectOrWrong[0] == 1)
        {
            CorrectOrWrong1.sprite = correctImage;
        }
        else
        {
            CorrectOrWrong1.sprite = wrongImage;
        }

        // -------------------------------- For one question in the summary page -------------------------//

        summary2QnText.text = answeredQuestions[1].question;
        DisplayQuestionTopic((int)answeredQuestions[1].topic, summary2Topic);

        switch (answeredQuestions[1].answer)
        {
            case 1:
                summary2CorrectAnswer.text = "Correct answer: " + answeredQuestions[1].option1;
                break;
            case 2:
                summary2CorrectAnswer.text = "Correct answer: " + answeredQuestions[1].option2;
                break;
            case 3:
                summary2CorrectAnswer.text = "Correct answer: " + answeredQuestions[1].option3;
                break;
            case 4:
                summary2CorrectAnswer.text = "Correct answer: " + answeredQuestions[1].option4;
                break;

        }

        summary2Explanation.text = "Explanation: " + answeredQuestions[1].explanation;

        if (answeredQuestions[1].questionImage != null)
        {
            summary2QnImageDisplay.enabled = true;
            summary2QnImageDisplay.sprite = answeredQuestions[1].questionImage;
            summary2QnImageDisplay.preserveAspect = true;
        }
        else
        {
            summary2QnImageDisplay.enabled = false;
        }

        // if correct, show tick
        if (CorrectOrWrong[1] == 1)
        {
            CorrectOrWrong2.sprite = correctImage;
        }
        else
        {
            CorrectOrWrong2.sprite = wrongImage;
        }


        // -------------------------------- For one question in the summary page -------------------------//

        summary3QnText.text = answeredQuestions[2].question;
        DisplayQuestionTopic((int)answeredQuestions[2].topic, summary3Topic);

        switch (answeredQuestions[2].answer)
        {
            case 1:
                summary3CorrectAnswer.text = "Correct answer: " + answeredQuestions[2].option1;
                break;
            case 2:
                summary3CorrectAnswer.text = "Correct answer: " + answeredQuestions[2].option2;
                break;
            case 3:
                summary3CorrectAnswer.text = "Correct answer: " + answeredQuestions[2].option3;
                break;
            case 4:
                summary3CorrectAnswer.text = "Correct answer: " + answeredQuestions[2].option4;
                break;

        }

        summary3Explanation.text = "Explanation: " + answeredQuestions[2].explanation;

        if (answeredQuestions[2].questionImage != null)
        {
            summary3QnImageDisplay.enabled = true;
            summary3QnImageDisplay.sprite = answeredQuestions[2].questionImage;
            summary3QnImageDisplay.preserveAspect = true;
        }
        else
        {
            summary3QnImageDisplay.enabled = false;
        }

        // if correct, show tick
        if (CorrectOrWrong[2] == 1)
        {
            CorrectOrWrong3.sprite = correctImage;
        }
        else
        {
            CorrectOrWrong3.sprite = wrongImage;
        }


        // -------------------------------- For one question in the summary page -------------------------//

        summary4QnText.text = answeredQuestions[3].question;
        DisplayQuestionTopic((int)answeredQuestions[3].topic, summary4Topic);

        switch (answeredQuestions[3].answer)
        {
            case 1:
                summary4CorrectAnswer.text = "Correct answer: " + answeredQuestions[3].option1;
                break;
            case 2:
                summary4CorrectAnswer.text = "Correct answer: " + answeredQuestions[3].option2;
                break;
            case 3:
                summary4CorrectAnswer.text = "Correct answer: " + answeredQuestions[3].option3;
                break;
            case 4:
                summary4CorrectAnswer.text = "Correct answer: " + answeredQuestions[3].option4;
                break;

        }

        summary4Explanation.text = "Explanation: " + answeredQuestions[3].explanation;

        if (answeredQuestions[3].questionImage != null)
        {
            summary4QnImageDisplay.enabled = true;
            summary4QnImageDisplay.sprite = answeredQuestions[3].questionImage;
            summary4QnImageDisplay.preserveAspect = true;
        }
        else
        {
            summary4QnImageDisplay.enabled = false;
        }

        // if correct, show tick
        if (CorrectOrWrong[3] == 1)
        {
            CorrectOrWrong4.sprite = correctImage;
        }
        else
        {
            CorrectOrWrong4.sprite = wrongImage;
        }


        // -------------------------------- For one question in the summary page -------------------------//

        summary5QnText.text = answeredQuestions[4].question;
        DisplayQuestionTopic((int)answeredQuestions[4].topic, summary5Topic);

        switch (answeredQuestions[4].answer)
        {
            case 1:
                summary5CorrectAnswer.text = "Correct answer: " + answeredQuestions[4].option1;
                break;
            case 2:
                summary5CorrectAnswer.text = "Correct answer: " + answeredQuestions[4].option2;
                break;
            case 3:
                summary5CorrectAnswer.text = "Correct answer: " + answeredQuestions[4].option3;
                break;
            case 4:
                summary5CorrectAnswer.text = "Correct answer: " + answeredQuestions[4].option4;
                break;

        }

        summary5Explanation.text = "Explanation: " + answeredQuestions[4].explanation;

        if (answeredQuestions[4].questionImage != null)
        {
            summary5QnImageDisplay.enabled = true;
            summary5QnImageDisplay.sprite = answeredQuestions[4].questionImage;
            summary5QnImageDisplay.preserveAspect = true;
        }
        else
        {
            summary5QnImageDisplay.enabled = false;
        }

        // if correct, show tick
        if (CorrectOrWrong[4] == 1)
        {
            CorrectOrWrong5.sprite = correctImage;
        }
        else
        {
            CorrectOrWrong5.sprite = wrongImage;
        }


        // -------------------------------- For one question in the summary page -------------------------//

    }

}
