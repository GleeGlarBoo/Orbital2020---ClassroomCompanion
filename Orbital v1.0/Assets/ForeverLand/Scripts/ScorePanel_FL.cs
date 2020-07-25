using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// GAME ENDS WHEN PLAYER DIE, SO SCORE PANEL IS SHOWN THEN. CODE TO ACIVATE SCOREPANEL IS IN PLAYER.cs
public class ScorePanel_FL : MonoBehaviour
{

    public TextMeshProUGUI FinalCoinScore;


    private void Update()
    {
        FinalCoinScore.text = Player_FL.instance.GetCoin().ToString();
    }

    public void ForButtonOnClick()
    {

        // update real rewards
        Debug.Log("Adding rewards");

        // UserInterface.instance.AddMoney(Player.instance.GetCoin());           // cant call since not same scene
        // UserInterface.instance.LevelUp(Player.instance.GetBone());            // cant call

        int currentMoney = PlayerPrefs.GetInt("money");
        Debug.Log("currently: " + currentMoney);

        currentMoney += Player_FL.instance.GetCoin();
        PlayerPrefs.SetInt("money", currentMoney);
        Debug.Log("now: " + currentMoney);



        // Invoke("LoadRoomScene", 4f);
    }

    /* 
    // to invoke after a few seconds
    private void LoadRoomScene()
    {
        SceneManager.LoadScene("Room");

    }
    */
}
