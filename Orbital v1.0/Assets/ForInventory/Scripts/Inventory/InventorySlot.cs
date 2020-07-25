using UnityEngine;
using UnityEngine.UI;


// Keep track of everything happening on a particular SLOT 
// Update UI on the slot, and the function that define what happens when we press it or press remove
public class InventorySlot : MonoBehaviour
{
    // Keeps track of the current item in the slot 
    public Item item;

    // Reference to the image object (child of 'inventory button')
    public Image icon;
    public Image itemTypeIndicator;
    public Button removeButton;

    // reference to placement zones
    public Button[] PlacementZones;         // 0 = table, 1 = ground shelf, 2 = air shelf, 3 = wall deco, 4 = large floor

   /*
    public Button TableDecoZoneButton;
    public Button GroundShelfDecoZoneButton;
    public Button AirShelfDecoZoneButton;
    public Button WallDecoZoneButton;
    public Button LargeFloorDecoZoneButton;
    */


    // reference to sell panel
    public GameObject SellPrompt;
    public InventoryUI inventoryUI;     // reference to Canvas_Inventory since it has the inventoryUI script attached to it
                                        // Required to tell the sellPrompt what item we are dealing 
    //bool onPromptOut = false;        // making it global (in itemmanager)   // flag to prevent user from opening any more sell confirm pop ups when dealing with one
    // private bool UsingItem = false;      // LOL

    // Adding an item into the SLOT. 
    public void AddItem (Item newItem)
    {
        item = newItem;

        // To update the graphics of the slot, we need to access the image component of the 'icon' object (child of 'inventory button')
        icon.sprite = item.icon;         // item.icon is defined in Item.cs which is to be defined individually for each item
        icon.enabled = true;             // enable the image to be shown since by default it is not enabled

        itemTypeIndicator.color = newItem.color;
        itemTypeIndicator.enabled = true;

        removeButton.interactable = true;       // show the remove button

        // Update its status in the status list - add to inventory
        // itemManager.instance.itemStatus[item.itemIndex] = 1;
    }


    // Method for cleaning out the slot, just the opposite of add
    // To update the UI
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        itemTypeIndicator.enabled = false;
    }

    // Sell to shop function
    // This function will bring out the prompt.
    // The prompt will enable the user to buy the item
    public void onRemoveButton()
    {        
        if (SellPrompt.activeSelf == false)           // so not more than 1 pop up appears.
        {
            SellPrompt.SetActive(true);
            inventoryUI.getItemReference(item);     // pass in the item reference to inventoryUI script for it to sell the correct item
            SetEverythingInactive();
        }
        
    }

    // WHEN USER CLICKS ON THE INVENTORY SLOT BUTTON
    // In our case, we want the user to be able to customize his room when he click on an item
    // So in our case, lets use this function to enable customize mode, where coloured spots will show where users can place the objects
    public void UseItem ()
    {
        if (item != null && itemManager.instance.placementZoneIsOpened == false && SellPrompt.activeSelf == false)
        {
            itemManager.instance.placementZoneIsOpened = true;

            // Sets item index to itemManager(singleton) as the item properties are all defined in item script.
            item.UseToDecorate();
            // Debug.Log(itemManager.instance.FocusedItemIndex.ToString());

            // Remove any exisiting placement zone in the scene first.
            SetEverythingInactive();

            // enable placement zone to appear depending on what object was selected based on its itemType (enum)
            // The buttons will then take care of instantiating the objects
            if (item.itemType == ItemType.TableDeco)                            // will be disabled after placing object
            {
                PlacementZones[0].interactable = true;
            }
            else if (item.itemType == ItemType.GroundShelfDeco)
            {
                PlacementZones[1].interactable = true;
            }
            else if (item.itemType == ItemType.AirShelfDeco)
            {
                PlacementZones[2].interactable = true;
            }
            else if (item.itemType == ItemType.WallDeco )
            {
                PlacementZones[3].interactable = true;
            }
            else if (item.itemType == ItemType.LargeFloorDeco)
            {
                PlacementZones[4].interactable = true;
            }

        }
        else if (item != null && itemManager.instance.placementZoneIsOpened == true)         // when user wants to cancel placing object
        {
            // Remove any exisiting placement zone in the scene first.
            SetEverythingInactive();

            itemManager.instance.placementZoneIsOpened = false;

        }

    }

    // To disable everything else when one thing is selected
    // Also called when user clicks on the 'x' button on the inventory
    public void SetEverythingInactive()
    {
        foreach (Button butt in PlacementZones)
        {
            butt.interactable = false;
        }
    }

    /* Mistake Souvenir :D
    private void Update()
    {
        if (itemManager.instance.placementZoneIsOpened == false && UsingItem == true)
        {
            UsingItem = false;
        }
    }
    */
}
