using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to inventory.cs
// Added to GameManger which controls the general state of the game
// SCRIPT HERE IS THE SHOP STORAGE AND TAKES CARE OF ADDING AND REMOVING ITEMS
public class Shop : MonoBehaviour
{

    #region Singleton

    public static Shop instance;

    void Awake ()
    {
        // hence we Should only have one shop instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    #endregion

    // see notes on delegate - can have different methods suscribed to this event (delegate), stuff that affects the UI 
    public delegate void ShopOnItemChanged();       // Definition of delegate type, next statement is to implement it
    public ShopOnItemChanged shopOnItemChangedCallback;     // Just follow this syntax, allows us to do great stuff


    // Real definition for Inventory
    public List<Item> shopItems = new List<Item>();     // backend logic for what we have in the inventory
    public int space = 20;
    
    public bool Add(Item item)
    {
        if (shopItems.Count >= space)
        {
            Debug.Log("Not enough room!");        
            return false;                   // flag that we cant add the item
        }

        shopItems.Add(item);            // Up to this point, we updated the backend storage but we also want to update the UI, hence the next few lines

        // Call the delegate - whenever something changes in our inventory, we trigger this 
        if (shopOnItemChangedCallback != null)      // make sure we have methods suscribed to this delegate
            shopOnItemChangedCallback.Invoke();     // triggerring the event - updates our UI through some methods suscribed in it

        return true;

    }

    public void Remove (Item item)
    {
        shopItems.Remove(item);

        // Call the delegate 
        if (shopOnItemChangedCallback != null)      // make sure we have methods suscribed to this delegate
            shopOnItemChangedCallback.Invoke();     // triggerring the event - updates our UI through some methods suscribed in it

    }


}
