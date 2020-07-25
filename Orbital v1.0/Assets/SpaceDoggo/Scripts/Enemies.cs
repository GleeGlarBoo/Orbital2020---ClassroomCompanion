using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 15f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -(Random.Range(minSpeed, maxSpeed)));
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(45, 180);
        Destroy(this.gameObject, 10);           // self destruct after 10 secs
    }

    public void Die()
    {
        // explosion with reward
        ExplosionManager.instance.SpawnExplosionWithRewardAt(transform.position, 1f);
        Destroy(this.gameObject);
    }


    // make it such that when collide with player, also has a chance to earn rewards
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Die();
            Player.instance.m_Health.TakeDamage(20);
        }
    }
}
