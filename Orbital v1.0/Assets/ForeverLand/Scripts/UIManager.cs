using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;

    public GameObject InstructionButton;

    private void Start()
    {
        InstructionButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        coins.text = Player_FL.instance.GetCoin().ToString();

        int LivesLeft = Player_FL.instance.Health_Script.GetHealth();

        switch (LivesLeft)
        {
            case 0:
                Heart1.enabled = false;
                Heart2.enabled = false;
                Heart3.enabled = false;
                FLGameManager.instance.DisplayScorePanel();
                break;
            case 1:
                Heart2.enabled = false;
                Heart3.enabled = false;
                break;
            case 2:
                Heart3.enabled = false;
                break;
            
        }
    }
}
