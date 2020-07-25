using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Placed in main camera so that the script will run even when our signin page is not active. Otherwise, script wont run when signin page is disabled from the start
public class SignIn : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField dayInput;
    public TMP_InputField monthInput;
    public TMP_InputField yearInput;

    // for buttons
    public Button ContinueButton_name;
    public TextMeshProUGUI Continue_name;
    public Button ContinueButton_date;
    public TextMeshProUGUI Continue_date;

    public GameObject MainMenuPanel;
    public GameObject SignInPanel;
    public GameObject GetSchoolStartDatePanel;

    // for new player 
    public GameObject infoPage1;                    // come to here from date page if user is a new user
    public GameObject infoPage1BackButton;          // and disable the back button so that he has to see all the info pages
    public GameObject MainMenu;                     // else come here if he is not a new player


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Debug.Log("it should be landscape now");

        if (!PlayerPrefs.HasKey("name"))
        {
            Debug.Log("No name yet");
        } 
        else
        {
            Debug.Log("Has name: " + PlayerPrefs.GetString("name"));
        }

        if (!PlayerPrefs.HasKey("SchoolStartYear"))
        {
            Debug.Log("No date yet");
        }
        else
        {
            // See in our console to make sure date is indeed specified
            Debug.Log(PlayerPrefs.GetInt("SchoolStartDay"));
            Debug.Log(PlayerPrefs.GetInt("SchoolStartMonth"));
            Debug.Log(PlayerPrefs.GetInt("SchoolStartYear"));
        }

        // Check if user has already signed up or not and filled in start of school date, if yes, we skip sign up page
        if (PlayerPrefs.HasKey("name") && PlayerPrefs.HasKey("SchoolStartYear"))
        {
            MainMenuPanel.SetActive(true);
            SignInPanel.SetActive(false);
            GetSchoolStartDatePanel.SetActive(false);
        }
        else if (PlayerPrefs.HasKey("name"))        // if got name only and no school start date, happens if user just reset date
        {
            MainMenuPanel.SetActive(false);
            SignInPanel.SetActive(false);
            GetSchoolStartDatePanel.SetActive(true);             
            ContinueButton_date.interactable = false;
        }
        else                                                   // if got date only and no name or if no name and date.
        {                                                      // ForNameContinueOnClick() will take into account if have date or not and act accordingly
            MainMenuPanel.SetActive(false);
            GetSchoolStartDatePanel.SetActive(false);
            SignInPanel.SetActive(true);
            ContinueButton_name.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Makes sure user's name at least has a length greater than or equal to 4 before allowing him to continue
        if (nameInput.text.Length >= 3)
        {
            ContinueButton_name.interactable = true;
            
            // Increase alpha of Continue button to signal to the user that it is clickable now that the name has at least 4 characters
            var alpha = Continue_name.color;
            alpha.a = 1f;
            Continue_name.color = alpha;
            // Continue.color.a = 1f;      // Cannot modify because it is not a variable lol. fix this like how we do for similar errors
        } 
        else                // if user erase current input
        {
            ContinueButton_name.interactable = false;

            var alpha = Continue_name.color;
            alpha.a = 0.3f;
            Continue_name.color = alpha;
        }


        // Make sure user has input something into the date fields in order for him to continue
        if (dayInput.text.Length >= 1 && monthInput.text.Length >= 1 && yearInput.text.Length == 4)
        {
            ContinueButton_date.interactable = true;

            var alpha = Continue_date.color;
            alpha.a = 1f;
            Continue_date.color = alpha;

        }
        else                // if user erase current input
        {
            ContinueButton_date.interactable = false;

            var alpha = Continue_date.color;
            alpha.a = 0.3f;
            Continue_date.color = alpha;
        }

    }

    // In order to skip date input if user already has a date inputted, or to go to the date input if user has not specified already
    public void ForNameContinueOnClick()
    {
        // Skip date input if already has a date specified
        if (PlayerPrefs.HasKey("SchoolStartYear"))
        {
            GetSchoolStartDatePanel.SetActive(false);
            SignInPanel.SetActive(false);

            MainMenuPanel.SetActive(true);

            // See in our console to make sure date is indeed specified
            Debug.Log(PlayerPrefs.GetInt("SchoolStartDay"));
            Debug.Log(PlayerPrefs.GetInt("SchoolStartMonth"));
            Debug.Log(PlayerPrefs.GetInt("SchoolStartYear"));

        }
        else                        // enter start of school date if not already specified
        {
            MainMenuPanel.SetActive(false);
            SignInPanel.SetActive(false);

            GetSchoolStartDatePanel.SetActive(true);
            ContinueButton_date.interactable = false;
        }
    }


    // Clicking on Continue button will call this function. 
    public void SaveName()
    {
        PlayerPrefs.SetString("name", nameInput.text);
        Debug.Log(PlayerPrefs.GetString("name"));
    }


    public void SaveSchoolStartDate()
    {
        // Will be converted to a date object later on in the game
        PlayerPrefs.SetInt("SchoolStartDay", int.Parse(dayInput.text));
        PlayerPrefs.SetInt("SchoolStartMonth", int.Parse(monthInput.text));
        PlayerPrefs.SetInt("SchoolStartYear", int.Parse(yearInput.text));

        Debug.Log(PlayerPrefs.GetInt("SchoolStartDay"));
        Debug.Log(PlayerPrefs.GetInt("SchoolStartMonth"));
        Debug.Log(PlayerPrefs.GetInt("SchoolStartYear"));

    }

    // Auto corrects user if he / she inputs a number bigger than an actual possible day
    public void dayLimit()
    {
        if (int.Parse(dayInput.text) > 31)
        {
            dayInput.text = "31";
        }
    }

    // Auto corrects user if he / she inputs a number bigger than an actual possible month
    public void monthLimit()
    {
        if (int.Parse(monthInput.text) > 12)
        {
            monthInput.text = "12";
        }
    }



    // For a reset name button found in main menu page. Reloads the same scene to get user's name
    public void ResetName()
    {
        PlayerPrefs.DeleteKey("name");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // after restarting scene, since 'name' key is gone, user will receive a prompt for his name
    }

    public void ResetDate()
    {
        PlayerPrefs.DeleteKey("SchoolStartDay");
        PlayerPrefs.DeleteKey("SchoolStartMonth");
        PlayerPrefs.DeleteKey("SchoolStartYear");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // after restarting scene, since 'name' key is gone, user will receive a prompt for his name
    }




    // Newly added on 18/7/2020 - FOR new players, they have to look through the info pages at least once before they can start the game
    public void SeeIfNewPlayerOrNot()
    {
        // if player is a new player, cuz new player will not have this playerprefs value. Reset all delete all playerprefs keys too.
        if (!PlayerPrefs.HasKey("NotNew"))
        {
            infoPage1.SetActive(true);
            infoPage1BackButton.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(true);
        }

        // to indicate that he is not a new player anymore after clicking on this button
        PlayerPrefs.SetInt("NotNew", 1);
    }










    /* Not needed, just need to build and run the app on the phone, instead of unity remote
    public void showKeyBoard()
    {
        Debug.Log("Keyboard should show");
        // public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, bool autocorrection = true, bool multiline = false, bool secure = false, bool alert = false, string textPlaceholder = "", int characterLimit = 0);
        // No initial text, default keyboard, no autocorrect, only allow one line, no need for secure since not password, no need for alert, placeholder is empty, char limit of 12
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "", 12);
    }
    */

}
