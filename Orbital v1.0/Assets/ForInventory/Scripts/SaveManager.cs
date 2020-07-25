using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    // Singleton pattern, same as for inventory script
    public static SaveManager instance;


    // We want to load the data before the start() of the game 
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }


    // Put it in a button for now, next time we can make this entier class a singleton class and then call this save function everytime we change something in the game
    public void SaveData()
    {
        SaveItemsData.SaveItems();
    }

    public void LoadData()
    {
        SaveItemsData.LoadItems();
    }
}
