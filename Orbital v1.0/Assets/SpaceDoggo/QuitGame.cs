using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{

    public LevelLoader loader;
    public GameObject EndPanel;

    // to invoke after a few seconds

    public void QuitOnClick()
    {
        Time.timeScale = 1;

        EndPanel.SetActive(true);

        Invoke("LoadRoomScene", 4f);


    }


    public void LoadRoomScene()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        loader.LoadLevel("Room");

    }
}
