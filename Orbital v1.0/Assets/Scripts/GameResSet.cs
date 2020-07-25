using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameResSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        Screen.orientation = ScreenOrientation.Landscape;
        Debug.Log("it should be landscape now");


        // reset doggo level if needed
       // GameManager.doggoLevel = 1;
       // PlayerPrefs.SetInt("doggoLevel", GameManager.doggoLevel);

    }


    // Latched onto the reset button (hidden in bottom left of settings screen)
    public void ResetGame()
    {
        QuizManager.instance.quizPassedCounter = 0;           // cuz we are not updating its value anywhere else other than start() in quizManager, so doing this will allow us to update the value when a new day arrives
        MiniGameManager.instance.hasPlayedGame = 0;         // cuz we are not updating its value anywhere else other than start() in quizManager, so doing this will allow us to update the value when a new day arrives
        GameManager.doggoLevel = 1;
        GameManager.doggoExperience = 0f;
        GameManager.money = 100;

        // set every item to go back to the shop
        for (int i = 0; i < itemManager.instance.AllItems.Length; i++)
        {
            itemManager.instance.itemStatus[i] = 0;
        }
        // SAVE itemStatus
        SaveManager.instance.SaveData();
        Debug.Log("SAVING");

        ResetTrophies();
        PlayerPrefs.DeleteAll();

        Invoke("goToWelcome", 0.5f);
    }

    public void FreeLevelUP()
    {
        GameManager.doggoLevel++;
        PlayerPrefs.SetInt("doggoLevel", GameManager.doggoLevel);
    }

    private void goToWelcome()
    {

        PlayerPrefs.SetInt("money", 100);
        PlayerPrefs.SetInt("doggoLevel", 1);

        SceneManager.LoadScene("WelcomePage");
    }

    public void ResetTrophies()
    {
        TrophyManager.instance.Reset();
    }


    public void AddMoneyHack()
    {
        UserInterface.instance.AddMoney(100);       // saved to player prefs too
    }
}
