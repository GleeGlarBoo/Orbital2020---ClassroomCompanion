using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disppear : MonoBehaviour
{
    public GameObject thisPanel;

    public void Gonez()
    {
        thisPanel.SetActive(false);
    }
}
