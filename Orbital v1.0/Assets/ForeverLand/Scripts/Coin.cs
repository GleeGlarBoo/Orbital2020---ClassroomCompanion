using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Range(0, 20)]
    public float m_Speed = 20;

    public GameObject RewardedAnim;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-m_Speed, 0);
        Destroy(this.gameObject, 15);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Player_FL.instance.Health_Script.IsDead())           // if player is dead, he cant get any more coins
        {
            Rewarded();
        }
    }

    private void Rewarded()
    {
        Player_FL.instance.AddCoin(3);
        Instantiate(RewardedAnim, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
