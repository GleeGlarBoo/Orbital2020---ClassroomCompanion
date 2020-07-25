using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Tied to the bullet and player as the explosion will just disappear and not produce any reward
public class DisappearAfterExplosion : MonoBehaviour
{

    // to remove the leftover 'debris' after explosion animation
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Call this at the end of the explosion animation
    public void DisappearAfterExploding()
    {
        Destroy(spriteRenderer);    // get rid of the remaining debris after asteroid exploded

    }
}
