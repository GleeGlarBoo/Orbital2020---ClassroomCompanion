using UnityEngine;

// Put into the canvas of the shop so that it is always enabled and it will call the start function to get everything initialized 

public class ShopUI : MonoBehaviour
{
    // reference to the shop (script in the form of a class) 
    Shop shop;

    
    public GameObject ShopItemsParent;      // Parent to all of the shop slots 
    ShopSlot[] slots;              // array of shop slots


    // Start is called before the first frame update
    void Start()
    {
        // Since shop was created as a singleton
        shop = Shop.instance;     // cache the instance into this script

        // When an item is added or removed as specified in Shop script
        shop.shopOnItemChangedCallback += UpdateUI;        // Suscribing to this event for the delegate

        // Must be plural since we want to find all of the components of the children
        slots = ShopItemsParent.GetComponentsInChildren<ShopSlot>();       // if slots are gonna change, put it in UpdateUI() method, but since now it is static, here is fine

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update items in the shop when we buy or sell stuff
    // Called using the callback method (delegate) we created in shop script
    void UpdateUI()
    {
        // Loop through all shop slot components, and add item if there is an item to add, else clear slot
        // Need to find all of the shop slots, and all of the shop slots are children of the itemsParent object, hence we get a reference to it
        // Executed everything we call UpdateUI, and it will 'refresh' all the slots in the shop
        // hence it is like a general thing where it can be used when we add an item to the shop or if we take out one, since it just checks everything
        for (int i = 0; i < slots.Length; i++)
        {
            // if there are more items to add
            if (i < shop.shopItems.Count)          // number of items already in the shop. Remember that shop.items is a list of items created in shop.cs
            {
                slots[i].AddItem(shop.shopItems[i]);       // take the i'th slot, call AddItem (a function of shop slot), and pass in the corresponding item in the shop items array
            } 
            else                // out of items to add
            {
                slots[i].ClearSlot();
            }
        }   
    }
}
