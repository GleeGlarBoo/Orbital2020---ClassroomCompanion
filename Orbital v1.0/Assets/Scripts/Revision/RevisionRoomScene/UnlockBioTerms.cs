using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBioTerms : MonoBehaviour
{
    public Button Term1topic1;
    public Button Term2topic1;
    public Button Term2topic2;
    public Button Term3topic1;
    public Button Term4topic1;


    // Term of the acad year we are in.
    int term;

    // Start is called before the first frame update
    void Awake()
    {
        term = TimeManager.instance.Term;
    }

    private void Start()
    {
        switch(term)
        {
            case 1:
                Term1topic1.interactable = true;
                break;
            case 2:
                Term1topic1.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;
                break;
            case 3:
                Term1topic1.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;

                Term3topic1.interactable = true;
                break;
            case 4:
                Term1topic1.interactable = true;

                Term2topic1.interactable = true;
                Term2topic2.interactable = true;

                Term3topic1.interactable = true;

                Term4topic1.interactable = true;
                break;

        }
    }


}
