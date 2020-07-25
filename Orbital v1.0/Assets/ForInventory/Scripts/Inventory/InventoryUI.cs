using UnityEngine;

// Put into the canvas of the inventory stuff so that it is always enabled and it will call the start function to get everything initialized 

public class InventoryUI : MonoBehaviour
{
    // reference to the inventory (script in the form of a class) 
    Inventory inventory;
    
    public GameObject itemsParent;      // Parent to all of the inventory slots 
    InventorySlot[] slots;              // array of inventory slots

    public GameObject SellPrompt;
    Item item;


    // Start is called before the first frame update
    void Start()
    {
        // Since inventory was created as a singleton
        inventory = Inventory.instance;     // cache the instance into this script

        // When an item is added or removed as specified in Inventory script
        inventory.onItemChangedCallback += UpdateUI;        // Suscribing to this event for the delegate

        // Must be plural since we want to find all of the components of the children
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();       // if slots are gonna change, put it in UpdateUI() method, but since now it is static, here is fine
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update items in the inventory when we buy or sell stuff
    // Called using the callback method (delegate) we created in Inventory script
    void UpdateUI()
    {
        // Loop through all inventory slot components, and add item if there is an item to add, else clear slot
        // Need to find all of the inventory slots, and all of the inventory slots are children of the itemsParent object, hence we get a reference to it
        // Executed everything we call UpdateUI, and it will 'refresh' all the slots in the inventory
        // hence it is like a general thing where it can be used when we add an item to the inventory or if we take out one, since it just checks everything
        for (int i = 0; i < slots.Length; i++)
        {
            // if there are more items to add
            if (i < inventory.items.Count)          // number of items already in the inventory. Remember that inventory.items is a list of items created in inventory.cs
            {
                slots[i].AddItem(inventory.items[i]);       // take the i'th slot, call AddItem (a function of inventory slot), and pass in the corresponding item in the inventory items array
            } 
            else                // out of items to add
            {
                slots[i].ClearSlot();
            }
        }
        
        // Debug.Log("Updating UI");
    }




    // For SellPrompt! 
    // removeButton opening up sellPrompt is defined in inventorySlot script

    // get item reference from each's slots removeButton since each slot already has reference to its own item. 
    public void getItemReference(Item itemToBeSold)
    {
        item = itemToBeSold;
    }

    // For both Yes and No buttons
    public void onAnyOption()          
    {
        SellPrompt.SetActive(false);
    }

    public void onSellConfirm()
    {
        Shop.instance.Add(item);

        // adds to user's balance after selling
        UserInterface.instance.AddMoney(item.cost);
        TrophyManager.instance.DecreaseSpentAmount(item.cost);
        Debug.Log("Amount Spent: " + TrophyManager.instance.SpentAmount);

        // Update its status in the status list - go back to shop
        itemManager.instance.itemStatus[item.itemIndex] = 0;

        Inventory.instance.Remove(item);

        // SAVE itemStatus
        SaveManager.instance.SaveData();
        Debug.Log("SAVING");


    }

}
