using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        Invoke("FlyForward", 0.1f);
        Destroy(this.gameObject, 5f);           // self destruct after 5 seconds
    }


    void FlyForward()
    {
        rb.velocity += new Vector2(0, speed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemies e = other.GetComponent<Enemies>();

        // we can safely assume that an object is an enemy if it contains the Enemy.cs component.
        if (e)
        {
            e.Die();
            ExplosionManager.instance.SpawnExplosionAt(transform.position, 0.2f);    // explosion without reward
            Destroy(this.gameObject);
        }
    }


}
