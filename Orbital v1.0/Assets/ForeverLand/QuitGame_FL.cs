using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame_FL : MonoBehaviour
{

    public LevelLoader loader;

    // to invoke after a few seconds

    public void QuitOnClick()
    {
        Time.timeScale = 1;

        LoadRoomScene();
    }


    public void LoadRoomScene()
    {

        loader.LoadLevel("Room");

    }
}
