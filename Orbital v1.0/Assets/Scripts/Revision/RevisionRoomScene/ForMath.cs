using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Placed in Popup_contentRevision Canvas
// To decide which term's content we jump straight to
public class ForMath : MonoBehaviour
{
    // Term of the acad year we are in.
    int term;

    public GameObject Term1;
    public GameObject Term2;
    public GameObject Term3;
    public GameObject Term4;


    // Start is called before the first frame update
    void Start()
    {
        term = TimeManager.instance.Term;
       // Debug.Log(" WE ARE IN TERM " + term);
    }

    // We need to make sure that whenever we leave the revision page, all of the terms are disabled too. 
    // To ensure we dont get an overlap of the terms
    public void onClick()
    {
        switch (term)
        {
            case 1:
                Term1.SetActive(true);
                break;
            case 2:
                Term2.SetActive(true);
                break;
            case 3:
                Term3.SetActive(true);
                break;
            case 4:
                Term4.SetActive(true);
                break;
            default:
                Debug.Log("Noob");
                break;
        }
    }



}
