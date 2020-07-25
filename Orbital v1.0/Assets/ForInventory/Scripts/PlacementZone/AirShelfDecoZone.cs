using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


// Attached to the buttons - placement zones
public class AirShelfDecoZone : MonoBehaviour
{

    #region Singleton


    // Singleton pattern, same as for inventory script
    public static AirShelfDecoZone instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;

        blink = gameObject.GetComponent<Animation>();

    }

    #endregion


    // reference to this button
    public Button thisButton;

    // reference to animation
    private Animation blink;        // initialized in awake

    // index 13
    public Item cuckooclock_item;       // CHANGE ACCORDING TO WHAT OBJECTS WE HAVE IN FUTURE
    public GameObject cuckooclock_object;

    // index 14
    public Item rainbow_item;
    public GameObject rainbow_object;

    // index 15
    public Item fatbatman_item;
    public GameObject fatbatman_object;

    // index 16
    public Item couplePokemon_item;
    public GameObject couplePokemon_object;


    // variable to take note of the object we are working with
    int index;

    // When placement refresh is called in itemManager, this will be set, because we are calling a placement function from this 
    // script itself during the placement refresh function.
    GameObject objectPlaced = null;      // Take note of any existing objects already placed. Set in code below

    public void OnClickToPlaceObject()
    {
        index = itemManager.instance.FocusedItemIndex;
        Debug.Log("PLACE ITEM NOW");

        switch (index)
        {
            case 13:
                itemManager.instance.placeItem(cuckooclock_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }
                
                // will give objectPlaced a value
                objectPlaced = Instantiate(cuckooclock_object, transform);
                removeFromInventory(cuckooclock_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;     // close placement zone too.
                break;
            case 14:
                itemManager.instance.placeItem(rainbow_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                // will give objectPlaced a value
                objectPlaced = Instantiate(rainbow_object, transform);
                removeFromInventory(rainbow_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;     // close placement zone too.
                break;
            case 15:
                itemManager.instance.placeItem(fatbatman_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(fatbatman_object, transform);           // solving the 'not a variable' issue
                removeFromInventory(fatbatman_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            case 16:
                itemManager.instance.placeItem(couplePokemon_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(couplePokemon_object, transform);           // solving the 'not a variable' issue
                removeFromInventory(couplePokemon_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            default:
                Debug.Log("PLACING WRONG ITEM");
                break;

        }
    }

    public void removeFromInventory(Item item)
    {
        // Same as clicking on the 'x' button on the item slot
        Inventory.instance.Remove(item);
    }




    // Awake() defined with singleton
    /*
    // For blinking animation. Firstly we get reference to the animation component on the button
    private void Awake()
    {
        blink = gameObject.GetComponent<Animation>();
    }
    */

    // Animation should not start playing in the background
    private void Start()
    {
        blink.Stop();
    }

    // If button gets activated, then we play the animation
    private void Update()
    {
        // if button is not interactable, we do not play the animation at all, so animation will not be played in the background
        if (thisButton.interactable == true)
        {
            blink.Play();
        }
    }
}
