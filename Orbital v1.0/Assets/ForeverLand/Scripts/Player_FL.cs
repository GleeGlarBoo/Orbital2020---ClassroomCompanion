using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class Player_FL : MonoBehaviour
{

    #region Singleton
    public static Player_FL instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
    #endregion

    public float MaxDoubleTapTime = 0.2f;   // 0.2secs as threshold time for charge
    public float jumpValue = 20f;
    public float strikeValue = 8f;
    public float moveBackValue = 5f;
    public float strikeTime = 0.5f;
    private float startingXposition = -7.14f;
    // bool GameEnded = false;

    private Rigidbody2D rb;
    public Health_FL Health_Script;
    public AudioSource JumpSound;
    public AudioSource AttackSound;

    public Animator animator;

    public bool isStriking = false;

    private int rewardCoin = 0;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Health_Script = GetComponent<Health_FL>();
        JumpSound = GetComponent<AudioSource>();
        StartCoroutine(DoggoMovements());

    }

    IEnumerator DoggoMovements()
    {
        Debug.Log("touches: " + Input.touchCount);

        // if not dead yet
        while (!Player_FL.instance.Health_Script.IsDead())
        {
            // Ensure that doggo is not jumping or charging at all anymore
            if (Mathf.Abs(rb.velocity.y) < 0.01f && Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    // tap on left side of screen = jump
                    if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2)
                    {
                        // play jump animation and sound
                        rb.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
                        animator.SetTrigger("Jump");
                        JumpSound.Play();
                        yield return null;
                    }
                    // tap on right side of screen = charge
                    else if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
                    {
                        // To indicate if the player can destroy the frog in peace or not.
                        isStriking = true;

                        animator.SetTrigger("Strike");
                        var position = rb.position;
                        position.y += 0.01f;
                        rb.position = position;
                        rb.AddForce(Vector2.right * strikeValue, ForceMode2D.Impulse);
                        AttackSound.Play();

                        yield return new WaitForSeconds(strikeTime);
                        rb.velocity = Vector2.zero;

                        rb.AddForce(Vector2.left * (strikeValue - 2f), ForceMode2D.Impulse);
                        while (rb.position.x > startingXposition)
                        {
                            yield return null;
                        }

                        // means he is back to his original spot
                        rb.velocity = Vector2.zero;
                        position = rb.position;
                        position.y -= 0.01f;
                        rb.position = position;

                        isStriking = false;

                    }
                }

            }

            yield return null;
        }

        // if dead
        Die();

    }



    // For reward management
    public void AddCoin(int amt)
    {
        rewardCoin += amt;
    }

    public int GetCoin()                // tbh not currently used since our UI is in this script so even the private members can be accessed
    {
        return rewardCoin;
    }

    // stop spawning stuff and display fainted doggo
    private void Die()
    {
        animator.SetTrigger("Faint");
    }

}
