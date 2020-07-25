using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSongs : MonoBehaviour
{
    #region Singleton


    // Singleton pattern, same as for inventory script
    public static ChangeSongs instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;

    }

    #endregion


    public AudioSource[] Songs;

    public int i = 0;

    public bool Change = false;


    // Update is called once per frame
    void Update()
    {
        if (Change == true)
        {
            // if havent reach the last song before this touch
            if (i < 3)
            {
                Songs[i].enabled = false;   // disable current song
                i++;                        // go to the next song
                Songs[i].enabled = true;    // enable next song in the list
            }
            else                    // if already at the last song, we go back to square 1
            {
                Songs[i].enabled = false;    // i == 3 here
                i = 0;
                Songs[i].enabled = true;
            }

            Change = false;
        }

    }
}
