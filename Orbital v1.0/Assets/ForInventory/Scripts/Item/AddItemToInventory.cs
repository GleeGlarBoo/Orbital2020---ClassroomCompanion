using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines the behaviour of the items, how they can be added to the inventory.
// Should be used for when we buy something from the shop
public class AddItemToInventory : MonoBehaviour
{
    public Item item;

    private void OnMouseDown()
    {
        // REMOVING ITEM FROM SCENE AND PUTTING IT BACK INTO THE INVENTORY
        // Cant do it when the placement zone is out or if we are not in customize mode

        Debug.Log("Pressing on item!");

        if (itemManager.instance.placementZoneIsOpened == false && itemManager.instance.Customizing == true)
        {
            // Need this instead for instant changes to the UI
            bool backToInv = Inventory.instance.Add(item);     // Add to inventory provided that there is space in the inventory or if player have enough money to buy?
            if (backToInv)                                     // if no space in the inventory, dont destroy the object too
                Destroy(gameObject);



            // Since we want to be able to take down the object during customize mode too,
            // we want to make sure that the object stored within the currentlyplaced array in itemManager is cleared too
            // otherwise after we remove it from the scene once, it will still remain in the array, and because of how placeItem
            // function works in itemManager, we will add an additional icon into the inventory
            itemManager.instance.removeItemFromScene(item);
            //Debug.Log("-----------");     // it does come to this point
        }
    }


    /*
    // cant make the sprite act as a button tho, need to create an image from UI and then give it the source image of the item
    // He used button in his example, but our work around is to use onMouseDown, as seen above. 
    public void forButton()
    {
        bool wasBought = Inventory.instance.Add(item);     // Add to inventory provided that there is space in the inventory
        if (wasBought)                                     // if no space in the inventory, dont destroy the object too
            Destroy(gameObject);
    }
    */
}
