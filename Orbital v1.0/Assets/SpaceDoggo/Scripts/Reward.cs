using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Tied to the explosion prefabs because they are the one who will spawn the coin or bone
public class Reward : MonoBehaviour
{

    // idea is that when the user clears 1 asteroid, there is a probability of him getting one of these 2 rewards
    // animation will be playing the xplosion first, then followed by the fading in of the reward (if any),
    // then followed by the reward flying up and at the end of it, register the reward to player's count. 


    public GameObject coins;
    public GameObject bone;
    private float probability;

    // to remove the leftover 'debris' after explosion animation
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Call this at the end of the explosion animation
    // Created two different animation - one for asteroids and one for player and bullets. Only asteroids can spawn rewards now
    public void SpawnRewards()
    {
        Destroy(spriteRenderer);    // get rid of the remaining debris after asteroid exploded

        probability = Random.value;

        if (probability < 0.40f)
        {
            Instantiate(coins, transform.position, Quaternion.identity);
        }
        else if (probability < 0.5f)                // only 10% bah, cuz we dont want to earn alot of bones
        {
            Instantiate(bone, transform.position, Quaternion.identity);
        }


    }
}
