using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{

    private void OnMouseDown()
    {
        Debug.Log("Music change!");
        if (itemManager.instance.Customizing == false)
            ChangeSongs.instance.Change = true;

    }

}
