using UnityEngine;
using System.IO;        // to work with files in operating system
using System.Runtime.Serialization.Formatters.Binary;       // allow us to access the binary formatter
using System;


// ONLY SAVING AND LOADING THE INTEGER ARRAY: itemManager.instance.itemStatus
// since playerprefs cant store int array i think


// static class cant be instantiated, dont want to accidentally create multiple version of our save system
// static also allow us to call it from anywhere without creating an instance
public static class SaveItemsData 
{
    public static void SaveItems ()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        // where the file will be saved to. 
        // Here unity has a nice default save location that can be used across any device like ios, android and computers
        // then add in the subfile name. Can use anything since we are just creating our own binary file, can use any file type.
        string path = Application.persistentDataPath + "/items.BestGame";           // was items.shag

        // Creating a new file - using the concept of a file stream (steam of data contained in a file)
        // use it to read and write
        FileStream fileStream = new FileStream(path, FileMode.Create);      // have options for filemode but now we just gonna create


        // write data to the file in binary form
        // Just storing item status in this case. 
        formatter.Serialize(fileStream, itemManager.instance.itemStatus);
        fileStream.Close();         // could use using (...) so we dont have to close but this is more logical to follow

    }


    // Load the data and return the data
    public static void LoadItems ()
    {
        string path = Application.persistentDataPath + "/items.BestGame";

        // check if file exist in the path
        if (File.Exists(path))          
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open); // read from exisiting saved file

            // Read data from file by changing it from binary to normal format
            // need to cast the information to the correct type too
            itemManager.instance.itemStatus = formatter.Deserialize(fileStream) as int[];

            // always remember to close the filestream that we open
            fileStream.Close();
        } 
        else
        {
            Debug.Log("Save file not found in " + path);
        }
    }

}
