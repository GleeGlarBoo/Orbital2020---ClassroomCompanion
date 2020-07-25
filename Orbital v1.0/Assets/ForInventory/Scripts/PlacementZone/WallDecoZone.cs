using System.Collections;
using System.Collections.Generic;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


// Attached to the buttons - placement zones
public class WallDecoZone : MonoBehaviour
{

    #region Singleton


    // Singleton pattern, same as for inventory script
    public static WallDecoZone instance;

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

    // index 8
    public Item NormalWall_item;       // CHANGE ACCORDING TO WHAT OBJECTS WE HAVE IN FUTURE
    public GameObject NormalWall_object;

    // index 9
    public Item PikachuWall_item;
    public GameObject PikachuWall_object;

    // index 10
    public Item SickWallPainting_item;
    public GameObject SickWallPainting_object;

    // index 11
    public Item FairyWall_item;
    public GameObject FairyWall_object;

    // index 12
    public Item Ironman_item;
    public GameObject Ironman_object;

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
            case 8:
                itemManager.instance.placeItem(NormalWall_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }
                
                // will give objectPlaced a value
                objectPlaced = Instantiate(NormalWall_object, transform);
                removeFromInventory(NormalWall_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;     // close placement zone too.
                break;
            case 9:
                itemManager.instance.placeItem(PikachuWall_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                // will give objectPlaced a value
                objectPlaced = Instantiate(PikachuWall_object, transform);
                removeFromInventory(PikachuWall_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;     // close placement zone too.
                break;
            case 10:
                itemManager.instance.placeItem(SickWallPainting_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(SickWallPainting_object, transform);           // solving the 'not a variable' issue
                removeFromInventory(SickWallPainting_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            case 11:
                itemManager.instance.placeItem(FairyWall_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(FairyWall_object, transform);           // solving the 'not a variable' issue
                removeFromInventory(FairyWall_item);
                thisButton.interactable = false;              // hide the placement zone again

                itemManager.instance.placementZoneIsOpened = false;
                break;
            case 12:
                itemManager.instance.placeItem(Ironman_item);

                if (objectPlaced != null)
                {
                    Destroy(objectPlaced);
                }

                objectPlaced = Instantiate(Ironman_object, transform);           // solving the 'not a variable' issue
                removeFromInventory(Ironman_item);
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
