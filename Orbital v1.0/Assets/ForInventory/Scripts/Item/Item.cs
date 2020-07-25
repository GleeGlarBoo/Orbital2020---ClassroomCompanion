using UnityEngine;
using UnityEngine.UI;
using System;

/*  Scriptable object does not have to sit on an object. 
    Custom assets that we can create inside our project and then easily set properties for the objects we create from this asset
    Script will be the blueprint for all of the scriptable objects we create
*/


// Item assets we are creating
// Tell unity how we want to create them and how to navigate to create one
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // a dropdown to choose what type of equipment this is
    public ItemType itemType;
    public int itemIndex;

    new public string name = "Decoration";
    public Sprite icon = null;
    public Color color;
    public int cost;


    // He used virtual because he want to derive different functionalities based on what item he is selecting. 
    // But make sure to call this method for all of them inside the inventory slot 
    // See next videos to see how this work out
    // Might not have to follow him and implement different functionalities.... But then again might need to because
    // we have different objects and we want user to place different objects in different areas. 
    // idea: A painting doesnt go to the ground
    // Update: We placed the main chunk of logic in inventorySlot script instead because we couldnt get a reference to the buttons
    // if this is just a scriptable object. It has to be placed on a gameObject
    public virtual void UseToDecorate()         // im not sure how we are using virtual la but just leave it lor
    {

        Debug.Log("Placing " + name);

        itemManager.instance.FocusedItemIndex = itemIndex;

    }
}

// Specify what type of decoration this is.
// Written outside of class because we dont want it to be encapsulated in the item class, 
// we want to be able to use this in multiple places
public enum ItemType { TableDeco, GroundShelfDeco, AirShelfDeco, LargeFloorDeco, WallDeco }



