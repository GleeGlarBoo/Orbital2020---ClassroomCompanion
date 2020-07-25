using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    // function to be played on the 'play' button that allows us to load room scene, by specifying the name of the scene
    public void LoadLevel(string SceneName)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneName));
    }

    // Since these are running asynchronously, we have to do these things in coroutines
    IEnumerator LoadAsynchronously(string SceneName)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("FROM WELCOME PAGE");
            PlayerPrefs.SetInt("FromWelcomePage", 1);
        }
        else
        {
            Debug.Log("NOT FROM WELCOME PAGE");
            PlayerPrefs.SetInt("FromWelcomePage", 0);
        }


        // normal load scene method pauses the whole game, spending all of its resources trying to load a new scene.
        // but loadSceneAsync loads the scene asynchronously in the background. 
        // means it keeps the current scene and all of its behaviours running while loading a new scene into memory.
        // We can get information about the progress of our loading from the scenemanager below
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneName);      // returns an AsyncOperation object with information about how our operation is going. 


        // while not done loading
        while (!loadingOperation.isDone)
        {
            // Quick maths to clamp the value between 0 and 1 using the range of 0 to 0.9.
            float progress = Mathf.Clamp01(loadingOperation.progress / .9f);

            slider.value = progress;


            // float going from 0 to 1 indicating the current state of the process. Use UI to reflect this progress variable
            // loadingOperation.progress


            // wait a frame
            yield return null;
        }


    }
}
