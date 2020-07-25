using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;        // to reference audio mixer
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    // reference to the audio mixer where we have exposed the parameter we want to change
    public AudioMixer audioMixer;

    // Takes in a value ('volume') from the slider
    public void SetVolume (float volume)
    {
        // Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);      // first param is the exposed parameter name, 2nd param is the value of the slider
    }

    // public so that we can call it from dropdown
    // takes in an integer - INDEX of the element that we have chosen, Low = 0, Med = 1, High = 2
    public void SetQuality (int qualityIndex)
    {
        // Simple. Method takes in an index, so we just pass in the index that the user chose
        // Hence the indexes that our user interface has have to match up with that of the project settings options
        // So then just hook up the graphics dropdown to call this function here.
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.GetQualityLevel());
    }

    public void ReturnToRoom()
    {
        SceneManager.LoadScene("Room");
    }

    // Quit game when quit button is click in menu
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


}
