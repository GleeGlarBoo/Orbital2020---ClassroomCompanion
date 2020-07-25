using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bio_ForTopicButtons : MonoBehaviour
{
    public Button thisButton;
    public bool selected = false;
    public Image thisImage;
    private int numTimesMainPageStarted = 0;        // How we decide if we have reopened the topics.

    // Only called when we open up the popupQuizMenu since it is the parent of all of these already activated buttons
    void Start()
    {
        //Debug.Log("I DO START");
        thisButton = GetComponent<Button>();
        thisImage = GetComponent<Image>();

    }

    public void onClick()
    {
        // just toggle
        Debug.Log(selected);
        selected = !selected;

    }

    private void Update()
    {
        // How we decide if we have reopened the topics.
        if (numTimesMainPageStarted != QuizUnlockBioTerms.instance.numTimesEnterPage)
        {
            selected = false;
            numTimesMainPageStarted = QuizUnlockBioTerms.instance.numTimesEnterPage;
        }
        // else { if we are still navigate within the same topic, we wont be changing its selected status, a behaviour that we want }


        // How we decide to change the color according to if they are selected or not
        if (selected == true)
        {
            thisImage.color = Color.green;
        }
        else
        {
            thisImage.color = new Color32(0xC8, 0x7F, 0x00, 0xFF);
        }

    }
}
