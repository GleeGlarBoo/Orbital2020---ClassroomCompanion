using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Placed in reward zone
public class getRewarded : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            Player.instance.AddCoin(2);
            //Debug.Log("Player's coin: " + Player.instance.rewardCoin.ToString());
        }
        else if (collision.tag == "bone")                        // tagged 'bone'
        {
            Player.instance.AddBone(1);
            //Debug.Log("Player's exp: " + Player.instance.rewardExp.ToString());
        }


    }
}
