using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{

    #region Singleton
    public static Player instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
    #endregion

    [Range(0, 50)]
    public float m_MovementSpeed = 0.5f;        // speed which rocket will follow the finger
    private Rigidbody2D m_Rigidbody;

    // For shooting
    public GameObject bullets;
    public float TimeBetweenShots = 0.2f;
    private float TimeOfLastShot = 0;

    // Controlling the movement of the player by dragging the finger
    private Vector3 touchPosition;
    private Vector3 direction;      // direction from rocket position to touch position


    public Health m_Health;

    // softward practices - private member because we want to encapsulate the value to prevent ourselves from changing the value arbitrarily
    // Has add and get function below
    private int rewardCoin = 0;         
    private int rewardExp = 0;           // will be converted to a number within 0 to 1. Hence we divide by 10 to this int value

    // Managing UI here since these data are tied to the player.
    public TextMeshProUGUI coinScore;
    public TextMeshProUGUI boneScore;
    public Image healthbar;

    private void Start()
    {
       m_Rigidbody = GetComponent<Rigidbody2D>();
       m_Health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();           // move by dragging finger
        HandleWeapons();            // fire by pressing on screen or holding down onto the screen

        if (m_Health.IsDead())
        {
            Die();
        }

        // just put UI here bah since our variables are here. Dont need create any other script bahs
        if (coinScore != null)
            coinScore.text = rewardCoin.ToString();
        if (boneScore != null)
            boneScore.text = rewardExp.ToString();

        if (healthbar != null)
            healthbar.fillAmount = m_Health.GetPercentageHealth();

    }

    // With this, we dont have to clamp the player within the screen's boundaries cuz the finger will never go pass that
    private void HandleMovement()
    {
        if (Input.touchCount > 0)       // if we touch the screen
        {
            Touch touch = Input.GetTouch(0);            // assess the information of the touch
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;                        // to help in moving in 2D space even though things works in 3D
            direction = (touchPosition - transform.position);
            m_Rigidbody.velocity = new Vector2(direction.x, direction.y) * m_MovementSpeed;     // movement in the direction towards the finger

            if (touch.phase == TouchPhase.Ended)
                m_Rigidbody.velocity = Vector2.zero;

        }
    }


    private void HandleWeapons()
    {
        if (bullets == null)
            return;

        if (Input.GetAxis("Fire1") > 0)
        {
            float TimeSinceLastShot = Time.time - TimeOfLastShot;
            if (TimeSinceLastShot > TimeBetweenShots)
            {
                Instantiate(bullets, (Vector2) transform.position + new Vector2(0f, 0.55f), Quaternion.identity);
                TimeOfLastShot = Time.time;

            }
        }
    }


    // Dies after 5 hits. We then display the rewards the player earned and allow them to load back into the room scene with the added rewards (playerPrefs)
    void Die()
    {
        // explosion without reward
        ExplosionManager.instance.SpawnExplosionAt(transform.position, 1.5f);
        Destroy(gameObject);


        // End the game by showing the score panel and reset to normal speed first
        SpaceDoggoGameManager.instance.ResetToNormalSpeed();
        SpaceDoggoGameManager.instance.DisplayScorePanel();
    }


    // For reward management
    public void AddCoin(int amt)
    {
        rewardCoin += 2;
    }

    public int GetCoin()                // tbh not currently used since our UI is in this script so even the private members can be accessed
    {
        return rewardCoin;
    }

    public void AddBone(int amt)
    {
        rewardExp += 1;
    }

    public float GetBone()
    {
        return rewardExp;
    }


}
