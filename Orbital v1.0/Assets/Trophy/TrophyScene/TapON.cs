using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapON : MonoBehaviour
{
    public GameObject Description;

    // if still holding down on the object
    private void OnMouseDown()
    {
        Description.SetActive(true);
    }

    private void OnMouseExit()
    {
        Description.SetActive(false);
    }

}
