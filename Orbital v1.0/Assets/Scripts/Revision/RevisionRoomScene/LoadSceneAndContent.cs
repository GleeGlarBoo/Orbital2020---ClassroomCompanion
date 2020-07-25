using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// CAN BE USED FOR ALL SUBJECTS
public class LoadSceneAndContent : MonoBehaviour
{
    public void LoadScene (int index)       // 1 for english, 2 for math ....
    {
        switch (index)
        {
            case 1:
                SceneManager.LoadScene("EnglishRev");
                break;
            case 2:
                SceneManager.LoadScene("MathRev");
                break;
            case 3:
                SceneManager.LoadScene("PhysicsRev");
                break;
            case 4:
                SceneManager.LoadScene("ChemRev");
                break;
            case 5:
                SceneManager.LoadScene("BioRev");
                break;

        }
    }

    public void LoadContentInScene (int index)
    {
        PlayerPrefs.SetInt("topicIndex", index);        // for individual topic selector scripts to open up the appropriate content
    }

}
