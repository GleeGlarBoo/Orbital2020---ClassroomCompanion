using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI welcomeName;

    // Load game when play button is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit game when quit button is click in menu
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            welcomeName.text = "WELCOME " + PlayerPrefs.GetString("name") + "!";
        }
    }

}
