using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone_Enemy : MonoBehaviour
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
        position.y = -1.399f;
        rb.position = position;

        Destroy(this.gameObject, 15);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DamagePlayerAndDie();
        }
    }



    private void DamagePlayerAndDie()
    {
        Player_FL.instance.Health_Script.TakeDamage();
        Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
    