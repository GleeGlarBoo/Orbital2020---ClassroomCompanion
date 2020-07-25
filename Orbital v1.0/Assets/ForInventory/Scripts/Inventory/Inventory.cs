using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// similar to shop.cs
// Added to GameManger which controls the general state of the game
// SCRIPT HERE IS THE INVENTORY STORAGE AND TAKES CARE OF ADDING AND REMOVING ITEMS
public class Inventory : MonoBehaviour
{

    #region Singleton

    // Using Singleton pattern so that it can be accessible from other scripts - just like NUS CDG example
    // Singleton instance – creating a variable with the same type as our class 
    // Static variable = variable shared by all instances of a class 
    public static Inventory instance;

    void Awake ()
    {
        // Since we set instance as this particular component, can access it using Inventory.instance
        // hence we Should only have one inventory instance at all time
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    #endregion

    // see notes on delegate - can have different methods suscribed to this event (delegate), stuff that affects the UI 
    public delegate void OnItemChanged();       // Definition of delegate type, next statement is to implement it
    public OnItemChanged onItemChangedCallback;     // Just follow this syntax, allows us to do great stuff


    // Real definition for Inventory
    public List<Item> items = new List<Item>();     // backend logic for what we have in the inventory
    public int space = 20;
    
    public bool Add(Item item)
    {
        // Adding item to items list using list's own method 'add'
        // Can add in conditions to determine if we can add the item to the inv or not. 
        // For example, can check if user has enough money to buy the object, then allow him to buy, else cant buy
        if (items.Count >= space)
        {
            Debug.Log("Not enough room!");
            // # Can add something else like display a message to the user that his inventory is full
            return false;                   // flag that we cant add the item, so player cant buy the object
        }

        items.Add(item);            // Up to this point, we updated the backend storage but we also want to update the UI, hence the next few lines

        // Call the delegate - whenever something changes in our inventory, we trigger this 
        if (onItemChangedCallback != null)      // make sure we have methods suscribed to this delegate
            onItemChangedCallback.Invoke();     // triggerring the event - updates our UI through some methods suscribed in it

        return true;

    }

    public void Remove (Item item)
    {
        items.Remove(item);

        // Call the delegate 
        if (onItemChangedCallback != null)      // make sure we have methods suscribed to this delegate
            onItemChangedCallback.Invoke();     // triggerring the event - updates our UI through some methods suscribed in it

    }

}
