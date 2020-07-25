using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBehaviour : MonoBehaviour
{

    // We want to add an animation to the reward such that they fade in, then after awhile it will fly up and eventually register the rewards

    public float Speed = 10f;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        Invoke("FlyUpward", 0.8f);
        Destroy(this.gameObject, 8f);           // self destruct after 8 seconds
    }


    void FlyUpward()
    {
        rb.velocity += new Vector2(0, Speed);
    }

    /* lets use a trigger above the screen to add rewards, cuz we want to add rewards at the right time when the reward fly to the top
     * 
     * 
    // only called if either of the reward is being instantiated, if no reward come up then nth happens
    void AddReward()
    {
        if (this.tag == "coin")
        {
            Player.instance.rewardCoin += 2;
            Debug.Log("Player's coin: " + Player.instance.rewardCoin.ToString());
        } 
        else                        // tagged 'bone'
        {
            Player.instance.rewardExp += 1;
            Debug.Log("Player's coin: " + Player.instance.rewardExp.ToString());
        }
    }

    */

}
