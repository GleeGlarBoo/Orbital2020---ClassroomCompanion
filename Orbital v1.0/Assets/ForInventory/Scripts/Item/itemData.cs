using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOT USING FOR NOW

[System.Serializable]           // just means that we can save it in a file
public class itemData 
{
    // Data we are trying to save and load
    public List<Item> inventoryList = new List<Item>();     // since we are dealing with list in inventory script too
    // public List<Item> shopList = new List<Item>();
    public Item[] placedList;

    // Constructor for this class which specifies how we get the data for the above fields
    // specify which scripts you want to access the data from as arguments
    // not dealing with shop yet, but remember to add in all the fields for shop when we have a shop
    public itemData (Inventory inventory, itemManager placedItems)
    {
        inventoryList = inventory.items;        // simply take the list of inventory items in inventory and store it here. Will call the add function on inventory to add it back to inventory at the start of the game
        // shopList = shop.shopItems;

        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;   // make it the same sized array as currentlyPlaced
        placedList = new Item[numSlots];
        placedList = placedItems.currentlyPlaced;           // not sure if we can just equate them, but if this fails, we use a for loop 
    }

}
