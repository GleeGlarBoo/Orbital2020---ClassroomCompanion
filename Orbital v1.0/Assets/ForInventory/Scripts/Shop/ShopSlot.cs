using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Keep track of everything happening on a particular SLOT 
// Update UI on the slot, and the function that define what happens when we press it or press remove
public class ShopSlot : MonoBehaviour
{
    // Keeps track of the current item in the slot 
    Item item;

    // Reference to the image object 
    public Image icon;
    public Image itemTypeIndicator;


    // Reference to price tag
    public TextMeshProUGUI priceTag;

    // reference for caching userInterface static instance
    UserInterface userInterface;

    // reference to NotEnoughMoney panel
    public GameObject NotEnoughMoneyPanel;

    private void Start()
    {
        // cache the instance
        userInterface = UserInterface.instance;
    }

    // Adding an item into the SLOT. 
    public void AddItem(Item newItem)
    {
        item = newItem;

        // To update the graphics of the slot, we need to access the image component of the 'icon' object (child of 'inventory button')
        icon.sprite = item.icon;         // item.icon is defined in Item.cs which is to be defined individually for each item
        icon.enabled = true;             // enable the image to be shown since by default it is not enabled  

        itemTypeIndicator.color = newItem.color;
        itemTypeIndicator.enabled = true;

        // Update its status in the status list - add to shop
        // itemManager.instance.itemStatus[item.itemIndex] = 0;
    }


    // Method for cleaning out the slot, just the opposite of add
    // To update the UI
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        itemTypeIndicator.enabled = false;
    }



    // WHEN USER CLICKS ON THE BUY BUTTON. Lets not have a prompt, because user can easily sell the item back for the same price
    public void OnBuyItem()
    {
        if (item != null)
        {
            if (GameManager.money < item.cost)
            {
                NotEnoughMoneyPanel.SetActive(true);
                return;
            } 
            else
            {
                // deducts user's money after buying
                userInterface.DeductMoney(item.cost);
                TrophyManager.instance.IncreaseSpentAmount(item.cost);
                Debug.Log("Amount Spent: " + TrophyManager.instance.SpentAmount);


                // add it to the inventory
                Inventory.instance.Add(item);

                // Update its status in the status list - go back to shop
                itemManager.instance.itemStatus[item.itemIndex] = 1;

                // SAVE itemStatus
                SaveManager.instance.SaveData();
                Debug.Log("SAVING");

                // Remove bought item from shop panel
                Shop.instance.Remove(item);

            }


        }
        else
        {
            Debug.Log("WHAT ARE U BUYING?");
        }


    }


    // Update pricetag
    private void Update()
    {
        if (item != null)
            priceTag.text = "$" + item.cost.ToString();
        else
            priceTag.text = "Mesos";
    }

}
