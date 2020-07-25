using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Put in button canvas in scroll Rect parent
public class ForGameButton : MonoBehaviour
{
    // reference to this button
    private Button thisButton;

    // reference to animation
    private Animation blink;

    public GameObject SpaceDoggoPanel;
    public GameObject ForeverLandPanel;
    public GameObject TapTapDestroyerPanel;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        blink = gameObject.GetComponent<Animation>();
    }

    // Next time it should randomly select a game and load the scene of that game
    public void OnClick()
    {
        // include a prompt for user to turn their screen. Do it for 3 different games

        // TODO: if (probrability hits space doggo, then do ....) else if (other 2 games) do ....

        float probability = Random.value;
        Debug.Log(probability);

        if (probability <= 0.33f)
        {
            SpaceDoggoPanel.SetActive(true);    // show prompt to turn device to portrait mode first
            Invoke("loadSpaceDoggo", 4f);
        }
        else if (probability > 0.33f && probability <= 0.66f)
        {
            ForeverLandPanel.SetActive(true);    // show prompt to turn device to portrait mode first
            Invoke("loadForeverLand", 4f);
        }
        else
        {
            TapTapDestroyerPanel.SetActive(true);
            Invoke("loadTapTapDestroyer", 4f);
        }



        // Sets the date when the user last finished a quiz. 
        // We get the full date because otherwise if we only get the day, if user comes back and play at the exact same day but the next month, 
        // then there will be a bug
        PlayerPrefs.SetInt("DayOfLastMiniGame", TimeManager.instance.currentWorkingDay);            // having problems accessing time manager's values since it is in a different scene. hence our time manager
        PlayerPrefs.SetInt("TermOfLastMiniGame", TimeManager.instance.Term);                        // wont be destroyed on load now when we load the game scene, as we have included the code of DontDestroyOnLoad in TimeManager
        PlayerPrefs.SetInt("WeekOfLastMiniGame", TimeManager.instance.WeeksFromStartOfTerm);

        PlayerPrefs.SetInt("HasPlayedGame", 1);         // signal that we have played the game back into the room scene. Will reset every day
    }


    // So we can invoke after ssome time
    private void loadSpaceDoggo ()
    {
        SceneManager.LoadScene("SpaceDoggo");
    }

    private void loadForeverLand()
    {
        SceneManager.LoadScene("ForeverLand");
    }

    private void loadTapTapDestroyer()
    {
        SceneManager.LoadScene("TapTapDestroyer");
    }

    // Animation should not start playing in the background, when it is active but not interactable. SAME AS TABLE DECO ZONE SCRIPT
    private void Start()
    {
        blink.Stop();

        SpaceDoggoPanel.SetActive(false);       // extra safety measures to ensure those panels are not turned on cuz they cant be turned off unless we restart the game
        ForeverLandPanel.SetActive(false);
        // TODO: 1 more panels here
    }

    // play blink animation if it is clickable
    private void Update()
    {
        if (thisButton.interactable == true)
            blink.Play();
    }

}
