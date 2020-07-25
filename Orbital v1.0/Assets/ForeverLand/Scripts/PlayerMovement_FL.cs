using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_FL : MonoBehaviour
{
    int TapCount = 0;
    public float MaxDoubleTapTime = 0.2f;   // 0.2secs as threshold time for charge
    public float jumpValue = 20f;
    float NewTime;
    bool GameEnded = false;
    bool canJump = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DoggoMovements());

    }

    IEnumerator DoggoMovements()
    {
        Debug.Log("touches: " + Input.touchCount);

        while (!GameEnded)
        {
            // Ensure that doggo is not jumping or charging at all anymore
            if (Mathf.Abs(rb.velocity.y) < 0.01f && Mathf.Abs(rb.velocity.x) < 0.01f) {

                // Getting first touch to see if user will give the next within a threshold time limit 
                // to charge. If not, then just go ahead and jump
                if (Input.touchCount == 1)
                {

                    Touch touch = Input.GetTouch(0);

                    // has to check for the beginnings since our jump requires that
                    if (touch.phase == TouchPhase.Began)
                    {
                        canJump = true;
                        TapCount++;
                    }

                    // if one touch already and has ended that touch
                    if (TapCount == 1)
                    {
                        NewTime = Time.time + MaxDoubleTapTime;
                    }
                    else if (TapCount == 2 && Time.time <= NewTime)
                    {
                        Debug.Log("CHARGE!");

                        // play charge animation here as well as waiting for some time
                        yield return null;      // wait for sometime, edit later
                        TapCount = 0;
                    }
                    else if (canJump == true && Time.time > NewTime)            // jump otherwise
                    {
                        // play jump animation and sound
                        rb.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
                        // wait for awhile
                        TapCount = 0;
                        yield return null;
                    }
                        
                }


            }
            yield return null;


        }
    }


}
