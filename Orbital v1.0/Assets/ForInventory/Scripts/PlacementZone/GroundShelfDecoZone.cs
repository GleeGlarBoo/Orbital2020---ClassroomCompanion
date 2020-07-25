using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


// Attached to the buttons - placement zones
public class GroundShelfDecoZone : MonoBehaviour
{

    #region Singleton


    // Singleton pattern, same as for inventory script
    public static GroundShelfDecoZone instance;

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
    private Animation blink;     // initialized in awake

    // index 4
    public Item GroundPot_item;       // CHANGE ACCORDING TO WHAT OBJECTS WE HAVE IN FUTURE
    public GameObject GroundPot_Object;

    // index 5
    public Item CuteCactus_item;
    public GameObject CuteCactus_Object;

    // index 6
    public Item Snowball_item;
    public GameObject Snowball_object;

    // index 7
    public Item Pikachu_item;
    public GameObject Pikachu_object;

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
            case 4:
                // Add item to currently placed items (currentlyPlace array) and swap out any exisiting item there
                // Deals with the items only and not the object
                itemManager.instance.placeItem(GroundPot_item);

                // Remove the exisiting item's object so that it doesnt appear there. Code above already place the old
                // item into the inventory. This is for the object itself.
                if (objectPlaced != null)
                {
                    Debug.Log("REMOVING CURRENT OBJECT");
                    Destroy(objectPlaced);
                }
                
                // will give objectPlaced a value
                objectPlaced = Instantiate(GroundPot_Object, transform);           // solving the 'not a variable' issue (for the lines of code below)
                var ObjectScale0 = objectPlaced.transform.localScale;           // adding number behind variable names in order to prevent repeated declaration in other parts of code
                ObjectScale0.x = 14;
                ObjectScale0.y = 14;
                objectPlaced.transform.localScale = ObjectScale0;
                var objectPosition0 = objectPlaced.transform.localPosition;                  // USE LOCALSCALE AND LOCALPOSITION CUZ WE ARE MEASURING WITH RESPECT TO THE PARENT
                objectPosition0.y = 6.7f;
                objectPosition0.x = 2;
                objectPlaced.transform.localPosition = objectPosition0;

                // Remove item from inventory 
                removeFromInventory(GroundPot_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;     // close placement zone too.

                // blink.Stop();        // dont need this as our update method checks if it is interactable or not, and if it is not, animation will not play

                // not working to disable the button here cuz if we do that then we cant place the object there anymore since button is gone at the same time
                // thisButton.gameObject.SetActive(false);              // hide the placement zone again
                break;
            case 5:
                itemManager.instance.placeItem(CuteCactus_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(CuteCactus_Object, transform);         
                var ObjectScale1 = objectPlaced.transform.localScale;
                ObjectScale1.x = 15;
                ObjectScale1.y = 15;
                objectPlaced.transform.localScale = ObjectScale1;
                var objectPosition1 = objectPlaced.transform.localPosition;                  // USE LOCALSCALE AND LOCALPOSITION CUZ WE ARE MEASURING WITH RESPECT TO THE PARENT
                objectPosition1.y = 12.8f;
                objectPlaced.transform.localPosition = objectPosition1;
                // Instantiate(Doggo_Object, transform.position, Quaternion.identity, Parent.transform, false);
                removeFromInventory(CuteCactus_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            case 6:
                itemManager.instance.placeItem(Snowball_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(Snowball_object, transform);
                removeFromInventory(Snowball_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            case 7:
                itemManager.instance.placeItem(Pikachu_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(Pikachu_object, transform);
                removeFromInventory(Pikachu_item);
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
