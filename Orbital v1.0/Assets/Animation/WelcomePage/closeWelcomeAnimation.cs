using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeWelcomeAnimation : MonoBehaviour
{
    public GameObject WelcomeAnimation;
    public void closeAnimation ()
    {
        WelcomeAnimation.SetActive(false);
    }
}
