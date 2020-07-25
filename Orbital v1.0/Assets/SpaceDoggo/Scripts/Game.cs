using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Switch to 1080 x 1920 full-screen
        // Screen.SetResolution(1080, 1920, true);

        Screen.orientation = ScreenOrientation.Portrait;
        Debug.Log("it should be portrait now");
    }


}
