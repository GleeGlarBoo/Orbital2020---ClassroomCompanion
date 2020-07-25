using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_TapTap : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI coins;


    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + GameManagerTapTap.score;
        coins.text = "Coins earned: " + GameManagerTapTap.coinsEarned;
    }
}
