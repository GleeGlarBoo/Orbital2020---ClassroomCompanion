using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Stores player progress
public class UserInterface : MonoBehaviour
{

    #region Singleton

    public static UserInterface instance;

    void Awake()
    {
        // hence we Should only have one shop instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;



        // Checking if there is already saved data for the game, if yes, initialize to these values when game reopens
        // else, use GameManager's default values

        if (PlayerPrefs.HasKey("doggoLevel"))
        {
            GameManager.doggoLevel = PlayerPrefs.GetInt("doggoLevel");
        }

        // must get level first before we update the levels using the EXP, cuz otherwise the level will be 0 when we are trying to update levels with EXP
        // since game manager initialize it to 0 level at the start.
        if (PlayerPrefs.HasKey("doggoExperience"))
        {
            GameManager.doggoExperience = PlayerPrefs.GetFloat("doggoExperience");
            UpdateLevelAfterGames();
        }

        if (PlayerPrefs.HasKey("money"))
        {
            GameManager.money = PlayerPrefs.GetInt("money");
        }
    }

    #endregion

    // Level and coins
    public TextMeshProUGUI doggoLevel;
    public TextMeshProUGUI money;
    public Image experienceBar;

    // Initialize values for variables in GameManager between Game Openings and Game Closings
    private void Start()
    {
       // Placed GameManager data initialization in Awake() because our animations require the level during start()
    }

    // Update is called once per frame
    void Update()
    {
        // Update user's doggo level and money
        doggoLevel.text = GameManager.doggoLevel.ToString();
        money.text =  GameManager.money.ToString();

        experienceBar.fillAmount = GameManager.doggoExperience;

    }

    public void LevelUp(float exp)
    {
        // Update our doggo experience
        GameManager.doggoExperience += exp;

        // Allow doggo to level up and save up his remaining exp
        while (GameManager.doggoExperience >= 1.0f)
        {
            GameManager.doggoExperience -= 1.0f;
            GameManager.doggoLevel++;
        }

        // Save these data everytime we update it
        PlayerPrefs.SetFloat("doggoExperience", GameManager.doggoExperience);
        PlayerPrefs.SetInt("doggoLevel", GameManager.doggoLevel);

    }

    // CHECK IF WORKING WITH MULTIPLE GAME TESTS
    // since games cant call the levelUp or addmoney function, we can just add them to playerprefs. Hence we need check if pet actually level up already or not
    private void UpdateLevelAfterGames()
    {
        // Allow doggo to level up and save up his remaining exp
        while (GameManager.doggoExperience >= 1.0f)
        {
            GameManager.doggoExperience -= 1.0f;
            GameManager.doggoLevel++;
        }

        // Save these data everytime we update it
        PlayerPrefs.SetFloat("doggoExperience", GameManager.doggoExperience);
        PlayerPrefs.SetInt("doggoLevel", GameManager.doggoLevel);

    }

    public void AddMoney(int money)
    {
        GameManager.money += money;
        PlayerPrefs.SetInt("money", GameManager.money);
    }

    public void DeductMoney(int money)
    {
        GameManager.money -= money;
        PlayerPrefs.SetInt("money", GameManager.money);
    }

}
