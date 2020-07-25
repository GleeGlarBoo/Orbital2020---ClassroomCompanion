using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Just to toggle between opening the inventory menu and closing it.
public class ForShopButton : MonoBehaviour
{
    public GameObject ShopUI;
    
    public void OnClick()
    {
        // Toggling with more concise code - taking inverse of its current active state
        ShopUI.SetActive(!ShopUI.activeSelf);

        /*
        // 'activeSelf' This returns the local active state of this GameObject, which is set using GameObject.SetActive. Note that a GameObject may be inactive because a parent is not active, even if this returns true. This state will then be used once all parents are active. 
        // Use GameObject.activeInHierarchy if you want to check if the GameObject is actually treated as active in the Scene.
        if (InventoryUI.activeSelf == false)
        {
            InventoryUI.SetActive(true);
        }
        else
        {
            InventoryUI.SetActive(false);
        }
        */

       itemManager.instance.ShopRefresh();
    }
}
