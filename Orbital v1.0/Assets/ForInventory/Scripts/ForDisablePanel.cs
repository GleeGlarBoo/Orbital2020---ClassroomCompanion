using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDisablePanel : MonoBehaviour
{
 
    // Placed in the shop UI button too because we want to
    // Prevent reappearance of the panel if user click out of shop when the panel is showing. 
    // cuz problem we face right now is if panel shows and player clicks out before it is disabled, when player clicks into shop again it will appear once more and play the animation till it is disabled.
    public void DisablePanel()
    {
        Debug.Log("Panelled is disabled");
        this.gameObject.SetActive(false);
    }
}
