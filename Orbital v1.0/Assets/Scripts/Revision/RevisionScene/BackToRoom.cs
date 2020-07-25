using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToRoom : MonoBehaviour
{

    public void OnCLickToRoom()
    {
        SceneManager.LoadScene("Room");
    }
}
