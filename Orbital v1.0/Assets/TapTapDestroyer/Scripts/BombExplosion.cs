using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When bomb explodes, it sends the other rocks flying away
public class BombExplosion : MonoBehaviour
{

    public GameObject BombBigExplosion;

    public float power = 50.0f;         // Force of explosion that sets gameobjects flying
    public float radius = 500.0f;         // radius of the explosion
    public float upForce = 1.0f;        // adds dramatic effect to let gameobject fly up too

    public bool detonateIndicator = false;

    private const float yDie = -30.0f;      // if stone falls down and is beyond this boundary, will be destroyed

    private void Detonate()
    {

        // explosion position = bomb's position. Script is attached to bomb so we use 'this'
        Vector3 explosionPosition = transform.position;

        // hold the colliders of gameobjects around the bomb within the bomb that exploded
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        // run this code for each collider in the array above
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }
        }

        Destroy(Instantiate(BombBigExplosion, transform.position, transform.rotation), 3f);

        Destroy(gameObject);        // audio for explosion is played when explosion is instantiated
    }

    private void OnMouseDown()              // called automatically if someone is touching the object
    {
        detonateIndicator = true;


        //Destroy(gameObject.GetComponentInChildren<MeshRenderer>());               // doesnt work for some  reason

    }

    // if user clicked on bomb
    private void FixedUpdate()
    {
        if (transform.position.y < yDie)
        {
            Destroy(gameObject);            // Destroying the game object that this script is attached to
        }

        if (detonateIndicator)
        {
            Detonate();
            // detonateIndicator = false;

            MainLoop.instance.BombExplodedTimeOut = true;
            Invoke("ResetDetonateIndicator", 2.0f);        
        }
    }

    // needs time for fixedUpdate to create the explosion effects
    private void ResetDetonateIndicator()
    {
        detonateIndicator = false;
    }

}
