using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    #region Singleton

    public static MiniGameManager instance;

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

    public GameObject PlayGameButton;

    public int hasPlayedGame = 0;

    public TextMeshProUGUI GamePlayedIndicator;

    // Start is called before the first frame update
    void Start()
    {
        // Played game indicator 
        // Set to see if the user has already played the game or not
        // Reset every new day
        if (!PlayerPrefs.HasKey("HasPlayedGame"))
            PlayerPrefs.SetInt("HasPlayedGame", 0);                 // 0 = havent played game, 1 = played the game for the day already. Resets everyday
        else
            hasPlayedGame = PlayerPrefs.GetInt("HasPlayedGame");

    }

    // Manage whether the game can be played or not, showcasing to the user in text too along with the button blinking / disabled. 
    void Update()
    {
        if (hasPlayedGame == 1)     // if user has played the game already, the button will show but it will not be interactable. Resets everyday
        {
            PlayGameButton.SetActive(true);              // in order to make it be seen and to be not interactable - clearer for players to know they have played the game already
            PlayGameButton.GetComponent<Button>().interactable = false;
            GamePlayedIndicator.color = Color.yellow;
            GamePlayedIndicator.text = "Mini Game Played!";                 // tbh like daily game sia lol 
        }
        else if (QuizManager.instance.quizPassedCounter >= 2)         // User has done more than 2 quizzes and havent played the game
        {
            PlayGameButton.SetActive(true);
            PlayGameButton.GetComponent<Button>().interactable = true;
            GamePlayedIndicator.color = Color.green;
            GamePlayedIndicator.text = "Mini Game Unlocked!";                 // tbh like daily game sia lol 
        }
        else                                                        // user havent done 2 quizzes yet
        {
            PlayGameButton.SetActive(false);
            GamePlayedIndicator.color = Color.red;
            GamePlayedIndicator.text = "Mini Game Locked!";                 // tbh like daily game sia lol 
        }
    }
}
