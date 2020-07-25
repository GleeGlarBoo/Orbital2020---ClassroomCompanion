using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// GAME ENDS WHEN PLAYER DIE, SO SCORE PANEL IS SHOWN THEN. CODE TO ACIVATE SCOREPANEL IS IN PLAYER.cs
public class scorePanel : MonoBehaviour
{

    public TextMeshProUGUI FinalCoinScore;
    public TextMeshProUGUI FinalBoneScore;

    public GameObject EndPanel;

    public LevelLoader loader;

    void Start()
    {
        // final score should show up when the panel shows up
        FinalCoinScore.text = Player.instance.GetCoin().ToString();
        FinalBoneScore.text = Player.instance.GetBone().ToString();
    }

    private void Update()
    {
        FinalCoinScore.text = Player.instance.GetCoin().ToString();
        FinalBoneScore.text = Player.instance.GetBone().ToString();
    }

    public void ForButtonOnClick()
    {
        // Can add in a panel that advices players to turn their screen to landscape mode


        // update real rewards
        Debug.Log("Adding rewards");

        // UserInterface.instance.AddMoney(Player.instance.GetCoin());           // cant call
        // UserInterface.instance.LevelUp(Player.instance.GetBone());            // cant call

        int currentMoney = PlayerPrefs.GetInt("money");
        Debug.Log("currently: " + currentMoney);
        
        currentMoney += Player.instance.GetCoin();
        PlayerPrefs.SetInt("money", currentMoney);
        Debug.Log("now: " + currentMoney);

        float currentExp = PlayerPrefs.GetFloat("doggoExperience");
        Debug.Log("currently: " + currentExp);

        currentExp += (Player.instance.GetBone() / 5);     // convert to decimals first. Divide by 20 cuz we dont want them to earn too much exp
        PlayerPrefs.SetFloat("doggoExperience", currentExp);
        Debug.Log("now: " + currentExp);



        EndPanel.SetActive(true);

        Invoke("LoadRoomScene", 4f);
    }

    // to invoke after a few seconds
    private void LoadRoomScene()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        loader.LoadLevel("Room");

    }
}
