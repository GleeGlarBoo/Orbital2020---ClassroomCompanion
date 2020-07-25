using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    // drag and drop the prefab explosion here
    public GameObject explosion;
    public GameObject coin;
    public float coinTorque = 8;

    private const float yDie = -30.0f;      // if stone falls down and is beyond this boundary, will be destroyed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yDie)
        {
            Destroy(gameObject);            // Destroying the game object that this script is attached to
        }
    }

    // will be called when the player presses the left mouse button while over the object - which must have a collider 
    // same behaviour when we touch a screen on a mobile device
    // achieved using the ray casting technique in Unity - where Unity generate a ray from the mouse or our finger
    // and try to calculate the intersection of this ray with the game objects of the scene.
    // If there is one or several intersections, the object closer to the camera (player) will be the one receiving the OnMouseDown()
    private void OnMouseDown()              // called automatically if someone is touching the object
    {
        // Instantiating explosion must come before destroying the object because we want to instantiate explosion with the 
        // position of the gameObject, means we have to consult the gameObject first
        // Quaternion.identity is so that we do not rotate particle system, so using identity of the rotations
        // Destroying it so that the instances created will not remain in the game

        // Got a chance to spawn a coin too - at 30% chance
        float probability = Random.value;
        if (probability < 0.30f)
        {
            GameObject CoinSpawned = Instantiate(coin, transform.position, Quaternion.identity);
            CoinSpawned.GetComponent<Rigidbody>().AddForce(Vector3.up * 15f, ForceMode.Impulse);
            CoinSpawned.GetComponent<Rigidbody>().AddTorque(Vector3.right * coinTorque, ForceMode.Impulse);
            /*
             var CoinRotation = CoinSpawned.transform.rotation;
             CoinRotation.x = 90f;
             CoinSpawned.transform.rotation = CoinRotation;
             */
        }

        Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3.0f);

        GameManagerTapTap.score++;         // update score

        Destroy(gameObject);                // if clicking on stone, destroy it after a delay so that sound could be played.
    }
    
    
}