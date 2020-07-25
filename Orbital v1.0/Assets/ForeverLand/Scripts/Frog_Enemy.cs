using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_Enemy : MonoBehaviour
{
    [Range(0, 20)]
    public float m_Speed = 20;

    public GameObject DeathExplosion;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-m_Speed, 0);

        var position = rb.position;
        position.y = -1.421513f;
        rb.position = position;

        Destroy(this.gameObject, 15);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If hit player and he is not striking, damage player
        if (collision.CompareTag("Player") && !Player_FL.instance.isStriking)
            DamagePlayerAndDie();
        else if (collision.CompareTag("Player") && Player_FL.instance.isStriking)       // if he is striking, no damage, frog just dies, no damage to player
            FrogJustDie();
    }


    private void DamagePlayerAndDie()
    {
        Player_FL.instance.Health_Script.TakeDamage();
        Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void FrogJustDie()
    {
        Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
