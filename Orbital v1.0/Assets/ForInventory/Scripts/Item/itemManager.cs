using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Allows us to place the item to the room or put back into the inventory 
[Serializable]
public class itemManager : MonoBehaviour
{
    #region Singleton


    // Singleton pattern, same as for inventory script
    public static itemManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;

    }

    #endregion

    // MAKE SURE THEIR INDEX CORRESPONDS TO THEIR ITEMINDEX!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public Item[] AllItems;         // stores the list of items that we can use to update our inventory
   // public GameObject[] AllItemObjects;     // stores the list of gameobjects that we will use to deploy into the scene. Dont think we are using it though, since each placement zone got their own objects references alr
    public int[] itemStatus = new int[22];      // 0 = in the shop, 1 = in inventory, 2 = placed in scene

    // an array of items currently placed in the game
    public Item[] currentlyPlaced;

    Inventory inventory;
    Shop shop;

    // item we are currently interested in placing into the game
    // Since there are multiple items we can place into a slot, this will be used to distinguish them 
    public int FocusedItemIndex = 0;

    // Just some global variables as flags
    public bool placementZoneIsOpened = false;          // to allow user to cancel placement mode when pressed on the object again. Used in inventory slot and buttons script
    public bool Customizing = false;                    // to allow user to only remove item from scene if user opens up inventory

    // CURRENTLY NOT IN USE
    // Whenever placeItem is called when we try to place a new object into the scene
    // This variable will be set to true if there is already something in the place. Remember we have an array of currently placed objects
    // public bool alreadyOccupied = false;            // global static variable and accessible elsewhere

    // need to initialize this array based on the number of possible objects we can place in the scene, same as the length of enum in item script
    private void Start()
    {
        inventory = Inventory.instance;     // cache the instance so we dont always have to reference it manually again
        shop = Shop.instance;

        // String array of all of the elelemts in ItemType enum. Use the length to get the number of elements
        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;

        // initialize the size of this array
        currentlyPlaced = new Item[numSlots];       // Need debug mode in inspector to see this array

        // LOAD DATA BEFORE START()
        SaveManager.instance.LoadData();


        // Hopefully can load data + use data at the same time
        // Yay we can! nvm we will use the safer method 
        // PlacementRefresh();

        // Safer maybe. Because currently we are loading the data and using them straight. 
        Invoke("PlacementRefresh", 0.02f);
    }

    // placing item and swapping out exisitng item on that spot
    public void placeItem(Item newItem)
    {
        // Placement of item matters when inserting into the currentlyPlace array matters
        // because we want different elements of our array to correspond to different positions in the room
        // Luckily when we create an enum, every element is associate with an index 
        // So if we are placing a Wall deco, the index returned here will be 3
        int positionIndex = (int)newItem.itemType;     // item type is the ItemType object we declared in Item script


        Item oldItem = null;

        // swap out the item within the inventory if something is already occupying that position
        if (currentlyPlaced[positionIndex] != null)
        {
            // alreadyOccupied = true;
            oldItem = currentlyPlaced[positionIndex];       // set it to the item that currently sits in the currentlyPlaced slot
            inventory.Add(oldItem);                 // add old item back to inventory, just like swapping the objects 
            itemStatus[oldItem.itemIndex] = 1;      // since placed back into inventory
        }

        currentlyPlaced[positionIndex] = newItem;


        // Update its status in the status list - go to the room scene
        // Only 1 item of each catergory at max will have an item status of 2 because we always swap items out
        itemStatus[newItem.itemIndex] = 2;

        // SAVE itemStatus
        SaveManager.instance.SaveData();
        Debug.Log("SAVING");

    }

    // For the functionality of removing an object from the scene and making sure the position in the scene is cleared of object too
    // Might face with a bug if we are trying to 'buy' an object of the same type and put it into the inventory because what we are doing now 
    // is that we call removeItem when we 'buy'.
    // Solution for future: create a buy function that doesnt make use of removeItem. 
    public void removeItemFromScene(Item itemToBeRemoved)
    {
        int positionIndex = (int)itemToBeRemoved.itemType;

        currentlyPlaced[positionIndex] = null;

        // Update its status in the status list - go back to inventory
        itemStatus[itemToBeRemoved.itemIndex] = 1;

        // SAVE itemStatus
        SaveManager.instance.SaveData();
        Debug.Log("SAVING");

    }



    // Cant call this functionality in start() as well because we need to wait for every other script to finish their start() method during initialization
    // Fpr example, UpdateUI() in inventory UI only fully initialized at start(), hence we need to be later than start()
    // We will call this refresh function when user opens inventory or opens the shop
    // Make use of the itemStatus array, and update where the item should be - in the shop / in inventory / placed in scene
    public void InventoryRefresh()
    {
        // use a for loop to decide which should be in inventory
        // Need to configure how we add object to the scene, since they have custom functions according to their sizes
        for (int i = 0; i < AllItems.Length; i++)
        {
            if (itemStatus[i] == 1)
            {
                // Checking if an object is already in the list of items we have in the inventory,
                // if not, but status is 1, then we add it into the inventory.
                // Prevent double adding into the inventory
                if (!inventory.items.Contains(AllItems[i]))         // since index are all the same throughout item's itemIndex, AllItems index, AllItemsObjectsIndex
                    inventory.Add(AllItems[i]);
            }
        }
    }

    public void ShopRefresh()
    {
        // use a for loop to decide which should be in shop
        for (int i = 0; i < AllItems.Length; i++)
        {
            if (itemStatus[i] == 0)
            {
                if (!shop.shopItems.Contains(AllItems[i]))  // since index are all the same throughout item's itemIndex, AllItems index, AllItemsObjectsIndex
                {
                    shop.Add(AllItems[i]);
                    //Debug.Log("ADDED TO SHOPPO");
                    //Debug.Log(AllItems.Length);
                }
            }
        }
    }


    private void PlacementRefresh()
    {
        // use a for loop to decide which should be in shop
        for (int i = 0; i < AllItems.Length; i++)
        {
            if (itemStatus[i] == 2)
            {
                // So each DecoZone script know what to place into the scene
                FocusedItemIndex = i;

                // Pretend as if we press on the placement zone button and place the item
                if (AllItems[i].itemType == ItemType.TableDeco)                           
                {
                    TableDecoZone.instance.OnClickToPlaceObject();
                }
                else if (AllItems[i].itemType == ItemType.GroundShelfDeco)
                {
                    GroundShelfDecoZone.instance.OnClickToPlaceObject();
                }
                else if (AllItems[i].itemType == ItemType.WallDeco)
                {
                    WallDecoZone.instance.OnClickToPlaceObject();
                }
                else if (AllItems[i].itemType == ItemType.AirShelfDeco)
                {
                    AirShelfDecoZone.instance.OnClickToPlaceObject();
                }
                else if (AllItems[i].itemType == ItemType.LargeFloorDeco)
                {
                    FloorDecoZone.instance.OnClickToPlaceObject();
                }


            }
        }
    }

    // Attached onto big 'X' button in inventory
    public void CustomizeModeOff()
    {
        Customizing = false;           // toggling too.
    }


}
