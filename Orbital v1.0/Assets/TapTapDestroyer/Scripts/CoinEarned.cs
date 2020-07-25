using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEarned : MonoBehaviour
{
    public GameObject ForSound;
    
    private const float yDie = -30.0f;      // if stone falls down and is beyond this boundary, will be destroyed

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Rigidbody>().rotation.Set(90, 0, 0, 0);
        // gameObject.transform.rotation.Set(90, 0, 0, 0);
        //        var CoinRotation = gameObject.transform.rotation;
        //      CoinRotation.x = 90;
        //    gameObject.transform.rotation = CoinRotation;

       //  var dkwhatswrong = gameObject.GetComponent<Rigidbody>().velocity;
       // dkwhatswrong.x = 9;
       // gameObject.GetComponent<Rigidbody>().velocity = dkwhatswrong;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yDie)
        {
            Destroy(gameObject);            // Destroying the game object that this script is attached to
        }
    }

    private void OnMouseDown()              // called automatically if someone is touching the object
    {
       // gameObject.GetComponent<Rigidbody>().rotation.Set(90, 0 , 0, 0);      // all not working lol

        GameManagerTapTap.coinsEarned++;         // update score

        /*  Does not have meshrenderer on this gameobject for some reason
        gameObject.GetComponent<AudioSource>().Play();

        // Do this so that the sound could be played before coin is destroyed
        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject, 2.0f);                // if clicking on stone, destroy it after a delay so that sound could be played.
        */


        // since we can do the above as there is 2 situations - destroy gameobject before the sound clip play finish, OR no meshrenderer to destroy, hence the coin will seem as tho it is delayed destroyed
        Destroy(Instantiate(ForSound, transform.position, Quaternion.identity), 3.0f);

        Destroy(gameObject);

    }
    
    
}