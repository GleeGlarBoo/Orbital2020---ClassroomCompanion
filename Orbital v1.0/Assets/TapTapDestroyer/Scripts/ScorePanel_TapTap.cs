using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel_TapTap : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI coins;


    // Update is called once per frame
    void Update()
    {
        score.text = GameManagerTapTap.score.ToString() + "/100 = " + (GameManagerTapTap.score / 100).ToString("F2");
        coins.text = GameManagerTapTap.coinsEarned.ToString() + " X 5 = " + (GameManagerTapTap.coinsEarned * 5).ToString();
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

        currentMoney += (GameManagerTapTap.coinsEarned * 5);
        PlayerPrefs.SetInt("money", currentMoney);
        Debug.Log("now: " + currentMoney);

        float currentExp = PlayerPrefs.GetFloat("doggoExperience");
        Debug.Log("currently: " + currentExp);

        currentExp += (GameManagerTapTap.score / 100);     // convert to decimals first. Divide by 20 cuz we dont want them to earn too much exp. 5% each score
        PlayerPrefs.SetFloat("doggoExperience", currentExp);
        Debug.Log("now: " + currentExp);


    }
    /*
    // to invoke after a few seconds
    private void LoadRoomScene()
    {
        SceneManager.LoadScene("Room");

    }
    */
}
