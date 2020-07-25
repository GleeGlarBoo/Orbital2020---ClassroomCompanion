using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    private Rigidbody2D m_Rigidbody2D;
    public Animator animator;

    // General Game variables
    public bool isPaused = false;               // yet to be implemented
    float animationSelector = 0f;               // Probability. divided between parts of the many different animations to decide what the doggo will do
    private int currentDoggoLevel;
    private bool doggoRight = true;             // 1 for right, 0 for left

    // Movement variables
    private bool canMove = false;                // STATE VARIABLE
    float Speed_raw = 0f;                       // To configure Speed parameter in Animator
    float horizontalMove = 0f;                // argument for Move function
    public float runSpeed = 40f;                // changeable multiplier
    private float directionDetermine = 0f;      // for direction selection
    public float minCycles = 3.0f, maxCycles = 8.0f;           // Randomly select number of cycles to run, will be floored to give an int. Gives how long the doggo can run
    private int numCycles = 0;
    private float animationTime = 0.54f;              // for one cycle
    private float intervalTime = 0f;                    // Delay before next animation selection. Set in code below
    private float RunRollFly = 0f;           // Probability.

    // Flying variables
    public float UpForce = 35f;
    public float DownForce = 50f;
    public float flyTime = 7f;
    private bool flying = false;      // to determine length of animation in code below, since flying is only for a fixed 5s
    private float startingYaxis = -2.558043f;


    // Idle variables
    private bool isIdling = false;               // STATE VARIABLE
    public float minTimeIdling = 2f, maxTimeIdling = 5f;     // Random time interval for idling


    // Sitting variables
    private bool canSit = false;                // STATE VARIABLE
    private float IdleOrStand = 0f;     // Probability.

    // Barking variables
    private bool canBark = false;
    private int JustStartedGame = 0;                // only want to set this to be 1 when we come from the welcome page
    public float doggoBarkTime = 4.5f;
    private float speechBubbleSelector = 0f;        // Probability.
    private float contentSelector = 0f;             // Probability.

    public GameObject speechbubble_Stronk;              // could make another script for this, but we figured it's fine since theres only 7 different quotes
    public GameObject LoveQuiz;                    // All are gameobject cuz we will just setActive them instead of enabling / disabling them cuz their instances may still exists hence more things to load
    public GameObject Awesome;
    public GameObject StudyTogether;

    public GameObject speechbubble_Lovely;
    public TextMeshProUGUI WelcomeHome;                // these 2 are TMP because we need to change their words. Will enable / disable in replace of setActive    
    public TextMeshProUGUI GoodMorning;
    public GameObject WelcomeHomeObj;                   // need these 2 objects either way since we need to turn them around when doggo is facing left
    public GameObject GoodMorningObj;   
    public GameObject LubYou;
    public GameObject GoodBoi;
    public GameObject heart_image;
    public GameObject henlo;

    // Faint 1 variables
    private bool canFaint1 = false;
    public float faint1Time = 3f;

    // dizzy variables
    private bool canDizzy = false;
    public float minTimeDizzy = 2f, maxTimeDizzy = 5f;     // Random time interval for Dizzling

    // Faint 2 variables
    private bool canFaint2 = false;
    public float faint2Time = 3f;

    // Electric shock variables
    private bool canShock = false;
    public float minTimeShock = 2f, maxTimeShock = 5f;     // Random time interval for Shocking


    // Probabilities of selecting each animations  - We want to reduce repetition of animations. For example, avoid doggo sitting back down after standing up from sitting posture. Able to math it
    // Set in start because we want to take into account the levels of the doggo
    private float sitHi;                                // sit state - 20% chance
    private float runLow;                               // run state - 30% chance
    private float runHi;                               // idle state - 50% chance 
    private float barkLow;
    private float barkHigh;
    private float faint1Low;
    private float faint1High;
    private float dizzyLow;
    private float dizzyHigh;
    private float faint2Low;
    private float faint2High;
    private float shockLow;
    private float shockHigh;

    // Boundaries variables
    private float leftAllowed = -7.5f;
    private float rightAllowed = 8.0f;
    private bool doggoTooLeft = false;
    private bool doggoTooRight = false;



    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


        // Randomnize Doggo's initial x-axis value
        var positionOfDoggo = m_Rigidbody2D.position;
        positionOfDoggo.x = Random.Range(-6.2f, 4.0f);      // Within the range of the center viewing area of the screen
        m_Rigidbody2D.position = positionOfDoggo;


        currentDoggoLevel = GameManager.doggoLevel;

        // Set probabilities for doggo's animations based on the levels. Will decide if some tricks can be played anot corresponding to what doggo has learnt
        switch (currentDoggoLevel)
        {
            case 1:                                         // level 1 - basic animations
                sitHi = 0.20f;      // sit 20%
                runLow = 0.20f;     // run 30%
                runHi = 0.50f;      // idle 50%
                break;
            case 2:                                         // level 2 - bark
                sitHi = 0.20f;      // sit 20%
                runLow = 0.20f;     // run 25%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;   // idle 40%
                break;
            case 3:                                         // level 3 - faint 1
                sitHi = 0.20f;      // sit 20%
                runLow = 0.20f;     // run 20%
                runHi = 0.40f;
                barkLow = 0.40f;    // bark 15%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f; // idle 35%
                break;
            case 4:                                         
                sitHi = 0.20f;      // sit 20%
                runLow = 0.20f;     // run 20%
                runHi = 0.40f;
                barkLow = 0.40f;    // bark 15%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f; // idle 35%
                break;
            case 5:                                         // level 5 - flying (included in run)
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;
                faint1Low = 0.60f;  // faint 10%
                faint1High = 0.70f; // idle 30%
                break;
            case 6:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;
                faint1Low = 0.60f;  // faint 10%
                faint1High = 0.70f; // idle 30%
                break;
            case 7:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;
                faint1Low = 0.60f;  // faint 10%
                faint1High = 0.70f; // idle 30%
                break;
            case 8:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;
                faint1Low = 0.60f;  // faint 10%
                faint1High = 0.70f; // idle 30%
                break;
            case 9:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 15%
                barkHigh = 0.60f;
                faint1Low = 0.60f;  // faint 10%
                faint1High = 0.70f; // idle 30%
                break;
            case 10:                                            // level 10 - dizzy
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f; 
                dizzyLow = 0.65f;   // dizzy 10%
                dizzyHigh = 0.75f;  // idle 25%
                break;
            case 11:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f;
                dizzyLow = 0.65f;   // dizzy 10%
                dizzyHigh = 0.75f;  // idle 25%
                break;
            case 12:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f;
                dizzyLow = 0.65f;   // dizzy 10%
                dizzyHigh = 0.75f;  // idle 25%
                break;
            case 13:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f;
                dizzyLow = 0.65f;   // dizzy 10%
                dizzyHigh = 0.75f;  // idle 25%
                break;
            case 14:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint 10%
                faint1High = 0.65f;
                dizzyLow = 0.65f;   // dizzy 10%
                dizzyHigh = 0.75f;  // idle 25%
                break;
            case 15:                                                // level 15 - faint 2
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint1 7%
                faint1High = 0.62f;
                dizzyLow = 0.62f;   // dizzy 8%
                dizzyHigh = 0.70f;
                faint2Low = 0.70f;   // faint2 10%
                faint2High = 0.80f;  // idle 20%
                break;
            case 16:                                                
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint1 7%
                faint1High = 0.62f;
                dizzyLow = 0.62f;   // dizzy 8%
                dizzyHigh = 0.70f;
                faint2Low = 0.70f;   // faint2 10%
                faint2High = 0.80f;  // idle 20%
                break;
            case 17:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint1 7%
                faint1High = 0.62f;
                dizzyLow = 0.62f;   // dizzy 8%
                dizzyHigh = 0.70f;
                faint2Low = 0.70f;   // faint2 10%
                faint2High = 0.80f;  // idle 20%
                break;
            case 18:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint1 7%
                faint1High = 0.62f;
                dizzyLow = 0.62f;   // dizzy 8%
                dizzyHigh = 0.70f;
                faint2Low = 0.70f;   // faint2 10%
                faint2High = 0.80f;  // idle 20%
                break;
            case 19:
                sitHi = 0.15f;      // sit 15%
                runLow = 0.15f;     // run 30%
                runHi = 0.45f;
                barkLow = 0.45f;    // bark 10%
                barkHigh = 0.55f;
                faint1Low = 0.55f;  // faint1 7%
                faint1High = 0.62f;
                dizzyLow = 0.62f;   // dizzy 8%
                dizzyHigh = 0.70f;
                faint2Low = 0.70f;   // faint2 10%
                faint2High = 0.80f;  // idle 20%
                break;
            case 20:
                sitHi = 0.10f;      // sit 15%
                runLow = 0.10f;     // run 30%
                runHi = 0.40f;
                barkLow = 0.40f;    // bark 10%
                barkHigh = 0.50f;
                faint1Low = 0.50f;  // faint1 7%
                faint1High = 0.57f;
                dizzyLow = 0.57f;   // dizzy 8%
                dizzyHigh = 0.65f;
                faint2Low = 0.65f;   // faint2 10%
                faint2High = 0.75f;
                shockLow = 0.75f;   // shock 10%
                shockHigh = 0.85f;  // idle 15%
                break;
            default:                                // default follows the highest level configuration that we have now.
                sitHi = 0.10f;      // sit 15%
                runLow = 0.10f;     // run 30%
                runHi = 0.40f;
                barkLow = 0.40f;    // bark 10%
                barkHigh = 0.50f;
                faint1Low = 0.50f;  // faint1 7%
                faint1High = 0.57f;
                dizzyLow = 0.57f;   // dizzy 8%
                dizzyHigh = 0.65f;
                faint2Low = 0.65f;   // faint2 10%
                faint2High = 0.75f;
                shockLow = 0.75f;   // shock 10%
                shockHigh = 0.85f;  // idle 15%
                break;
        }





        /*                                          // Tried to randomnize his direction, but will cause him to move in the opposite direction lol
        // Randomnize Doggo's initial direction
        directionDetermine = Random.value;      // Returns a random number between 0 and 1 inclusive  
        
        // 50% to face in either way
        if (directionDetermine < 0.49f)
        {
            var currentDirection = m_Rigidbody2D.transform.localScale;
            currentDirection.x *= -1;
            m_Rigidbody2D.transform.localScale = currentDirection;
        }
        */

        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        // Will idle at the start of the game 
        isIdling = true;

        // Starting animation
        // if havent learn any tricks yet (including barking)
        if (currentDoggoLevel == 1)
        {
            // idle for a random interval 
            yield return new WaitForSeconds(Random.Range(minTimeIdling, maxTimeIdling));          // When user enters game, doggo will idle for random amount of seconds. Can be changed - maybe to greet the user or something. 
        }
        else                        // if learnt to bark
        {
            yield return new WaitForSeconds(0.2f);            // just wait for 0.2 second before barking.

            isIdling = false;
            canBark = true;

            // This is set in LevelLoader script which is activated using the 'play' button during the welcome page
            if (PlayerPrefs.HasKey("FromWelcomePage"))
                JustStartedGame = PlayerPrefs.GetInt("FromWelcomePage");             // so doggo will only use that 2 specific speech bubble

            animator.SetBool("IsBarking", true);        // animation will handle the speechbubble
            yield return new WaitForSeconds(doggoBarkTime);

            PlayerPrefs.SetInt("FromWelcomePage", 0);       // so when we come back from other scenes, we wont trigger the starting game behaviour. Only have that behaviour if we arrive from welcome page
            JustStartedGame = 0;
        }


        AnimationSelection();

        // Main Game Loop                                                                  ------- Comes in with a preset initial animation already ----------
        // As long as game is not paused by the user, we animate our doggo.
        // I think by now we have decided that it will forever be unpaused lol.
        while (!isPaused)
        {
            // Debug.Log(m_Rigidbody2D.position.x);
            // Debug.Log("canMove: " + canMove);
            // Debug.Log("canSit: " + canSit);
            // Debug.Log("isIdling:" + isIdling);

            // Movement for doggo - to be used by running, rolling and flying animation.
            while (canMove)
            {
                RunRollFly = Random.value;   // Returns a random number between 0 and 1 inclusive  

                // levels consideration for flying. If doggo is level 5 and below, he wont fly
                if (currentDoggoLevel < 5)
                {
                    if (RunRollFly <= 0.70f)               // 70% chance of running
                    {
                        animator.SetBool("IsRunning", true);
                        animator.SetBool("IsRolling", false);
                    }
                    else                              // 30% chance of rolling 
                    {
                        animator.SetBool("IsRolling", true);
                        animator.SetBool("IsRunning", false);
                    }
                }
                else            // if doggo is level 5 and above, he has a chance of flying
                {
                    if (RunRollFly <= 0.50f)               // 50% chance of running
                    {
                        animator.SetBool("IsRunning", true);
                        animator.SetBool("IsRolling", false);
                        animator.SetBool("IsFlying", false);
                        flying = false;                                     // to determine length of animation in code below
                    }
                    else if (RunRollFly > 0.50f && RunRollFly <= 0.70f)                              // 20% chance of rolling
                    {
                        animator.SetBool("IsRolling", true);
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsFlying", false);
                        flying = false;
                    }
                    else                                                                        // 30% chance of flying
                    {
                        flying = true;
                        animator.SetBool("IsFlying", true);
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsRolling", false);
                    }

                }


                // Can be adjusted to see how left you want doggo to be before turning the other way. 
                // Have to consider he might move some distance in that exreme-end way too if not chosen to turn around
                if (m_Rigidbody2D.position.x < leftAllowed || doggoTooLeft)               // If going to the extreme left already
                {
                    Debug.Log("IM TOO LEFT");
                    if (flying)
                        Speed_raw = Random.Range(0.5f, 0.65f);           // require this variable instead of a one liner for horizontalMove as we want to decide which animation to play
                    else
                        Speed_raw = Random.Range(0.5f, 1.0f);           // require this variable instead of a one liner for horizontalMove as we want to decide which animation to play

                    horizontalMove = Speed_raw * runSpeed;       // Random speed to the right
                    doggoRight = true;
                }
                else if (m_Rigidbody2D.position.x > rightAllowed || doggoTooRight)           // If going to the extreme right already
                {
                    Debug.Log("IM TOO RIGHT");
                    if (flying)
                        Speed_raw = Random.Range(-0.65f, -0.5f);
                    else
                        Speed_raw = Random.Range(-1.0f, -0.5f);
                    horizontalMove = Speed_raw * runSpeed;     // Random speed to the left
                    doggoRight = false;
                }
                else                                                // When doggo is anywhere else
                {
                    // To achieve randomness in his chosen direction
                    directionDetermine = Random.value;      // Returns a random number between 0 and 1 inclusive  

                    // And then we move him in that direction
                    if (directionDetermine < 0.50f)
                    {
                        if (flying)
                            Speed_raw = Random.Range(0.5f, 0.65f);           // require this variable instead of a one liner for horizontalMove as we want to decide which animation to play
                        else
                            Speed_raw = Random.Range(0.5f, 1.0f);            // to the right
                        horizontalMove = Speed_raw * runSpeed;
                        doggoRight = true;

                    }
                    else
                    {
                        if (flying)
                            Speed_raw = Random.Range(-0.65f, -0.5f);
                        else
                            Speed_raw = Random.Range(-1.0f, -0.5f);
                        horizontalMove = Speed_raw * runSpeed;
                        doggoRight = false;

                    }

                    // to decide if doggo run slow or run normal
                    // If doggo is going left, will have a negative value even if he is running fast in the left direction.
                    // hence, We use absolute values to make sure the fast running animation is played 
                    animator.SetFloat("Speed", Mathf.Abs(Speed_raw));

                    // initially wanted to do this ONLY, but we might get a small value like 0.1f which makes him very slow
                    // horizontalMove = Random.Range(-1.0f, 1.0f) * runSpeed;
                }

                if (flying)
                {
                    Debug.Log("Waiting for " + flyTime + "seconds");
                    yield return new WaitForSeconds(flyTime);
                }
                else
                {
                    numCycles = (int)Mathf.Floor(Random.Range(minCycles, maxCycles));
                    intervalTime = numCycles * animationTime;
                    yield return new WaitForSeconds(intervalTime);              // Allow doggo to run for a random amount of time
                }


                AnimationSelection();        // Select another animation to play and disable those not using

                // If doggo is not going to run anymore, we make him go back to idle state first before transitioning to more idling / sit
                // We also take away his speed
                if (canMove == false)
                {
                    Speed_raw = 0f;
                    horizontalMove = 0f;
                    animator.SetBool("IsRunning", false);           // No need to set isIdling to true since if idling was chosen, it would have been set in AnimationSelection()
                    animator.SetBool("IsRolling", false);           // These 2 statements will ensure doggo goes to idle animation.
                    animator.SetBool("IsFlying", false);
                    flying = false;
                    yield return new WaitForSeconds(Random.Range(minTimeIdling, maxTimeIdling));            // idle for awhile before going to the next animation selected by AnimationSelection()
                }
            }

            // If doggo is meant to sit
            if (canSit)          // possible to stand up and straight away sit back down
            {
                animator.SetBool("IsSitting", true);
                animator.SetBool("IsSittingIdle", true);        // true first.

                yield return new WaitForSeconds(1f);          // wait for doggo to sit first

                animator.SetBool("IsSitting", false);                   // maybe can make this into a trigger

                IdleOrStand = Random.value;   // Returns a random number between 0 and 1 inclusive 

                // Debug.Log(IdleOrStand);

                while (IdleOrStand <= 0.70f)         // 70% chance of sit idling                                                                // REMEMBER FLOATS NEEDS an f: GOOD = 0.80f, BAD = 0.8
                {
                    animator.SetBool("IsSittingIdle", true);        // continues to be true
                    IdleOrStand = Random.value;
                    yield return new WaitForSeconds(2.5f);          // So we dont keep repeating this loop and eventually getting an IdleOrStand > 0.8f which makes us exit this loop prematurely
                                                                    // Debug.Log(IdleOrStand);
                }


                // 20% chance of standing up
                animator.SetBool("IsSittingIdle", false);       // stand up
                yield return new WaitForSeconds(6.0f);          // goes back to idle

                sitHi = 0f;         // no chance of sitting back down STRAIGHT AWAY - still have chance of sitting back down after idling or running for awhile
                runLow = 0f;        // increase chance of running
                AnimationSelection();       // Select another animation to play and disable those not using

                sitHi = 0.20f;          // change back to normal status
                runLow = 0.20f;
            }


            // If doggo is meant to bark. 
            // Simple states, just bark and go back to other animation. Possible to bark again, and even bark the same content
            if (canBark)
            {
                animator.SetBool("IsBarking", true);
                yield return new WaitForSeconds(doggoBarkTime);
                AnimationSelection();
            }

            // When doggo is idling
            if (isIdling)
            {
                yield return new WaitForSeconds(Random.Range(minTimeIdling, maxTimeIdling));
                AnimationSelection();
            }

            // when doggo is fainting 1
            if (canFaint1)
            {
                animator.SetBool("IsFainting1", true);
                yield return new WaitForSeconds(faint1Time);
                AnimationSelection();
            }

            // when doggo is dizzy
            if (canDizzy)
            {
                animator.SetBool("IsDizzy", true);
                yield return new WaitForSeconds(Random.Range(minTimeDizzy, maxTimeDizzy));
                AnimationSelection();
            }

            // when doggo is fainting 2
            if (canFaint2)
            {
                animator.SetBool("IsFainting2", true);
                yield return new WaitForSeconds(faint2Time);
                AnimationSelection();
            }

            // When doggo is electrocuted
            if (canShock)
            {
                animator.SetBool("IsShocking", true);
                yield return new WaitForSeconds(Random.Range(minTimeShock, maxTimeShock));
                AnimationSelection();
            }
        }

    }


    private void FixedUpdate()
    {
        // Move our character
        // fixedDeltaTime is the amount of time elapsed since the last time the FixedUpdate() was called.
        // It will ensure that we move the same amount no matter how often the function is called 
        //  Debug.Log(horizontalMove);
        //  horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;                  // For testing - move doggo using user inputs
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }


    // Only disable animator's parameters, and do not set them. In the respective sections above, the parameters will then be set
    private void AnimationSelection()
    {
        // selection of another animation 
        animationSelector = Random.value;   // Returns a random number between 0 and 1 inclusive

        // Based on what tricks we have learnt
        switch (currentDoggoLevel)
        {
            case 1:                                         // level 1
                AnimationLevel1(animationSelector);
                break;
            case 2:                                         // level 2
                AnimationLevel2(animationSelector);
                break;
            case 3:
                AnimationLevel3(animationSelector);         // level 3
                break;
            case 4:
                AnimationLevel3(animationSelector);         // same as level 3
                break;
            case 5:
                AnimationLevel5(animationSelector);         // level 5
                break;
            case 6:
                AnimationLevel5(animationSelector);         // same as level 5
                break;
            case 7:
                AnimationLevel5(animationSelector);         // same as level 5
                break;
            case 8:
                AnimationLevel5(animationSelector);         // same as level 5
                break;
            case 9:
                AnimationLevel5(animationSelector);         // same as level 5
                break;
            case 10:
                AnimationLevel10(animationSelector);         // level 10
                break;
            case 11:
                AnimationLevel10(animationSelector);         // same as level 10
                break;
            case 12:
                AnimationLevel10(animationSelector);         // same as level 10
                break;
            case 13:
                AnimationLevel10(animationSelector);         // same as level 10
                break;
            case 14:
                AnimationLevel10(animationSelector);         // same as level 10
                break;
            case 15:
                AnimationLevel15(animationSelector);         // level 15
                break;
            case 16:
                AnimationLevel15(animationSelector);         // same as level 15
                break;
            case 17:
                AnimationLevel15(animationSelector);         // same as level 15
                break;
            case 18:
                AnimationLevel15(animationSelector);         // same as level 15
                break;
            case 19:
                AnimationLevel15(animationSelector);         // same as level 15
                break;
            case 20:
                AnimationLevel20(animationSelector);         // level 20
                break;
            default:                                        // go with the highest level
                AnimationLevel20(animationSelector);
                break;

        }

    }

    // Doggo level 1's learnt animations - run / roll, idle, sit
    private void AnimationLevel1 (float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            isIdling = false;
        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);
        }
    }


    // Doggo level 2's learnt animations - run / roll, idle, sit, bark
    private void AnimationLevel2(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);


            canBark = false;
            animator.SetBool("IsBarking", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);

            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);
            canBark = false;
            animator.SetBool("IsBarking", false);

        }
    }

    // Doggo level 3's learnt animations - run / roll, idle, sit, bark, faint1                   
    private void AnimationLevel3(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);


            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            canSit = false;
            animator.SetBool("IsSitting", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;

        }
        else if (Selector > faint1Low && Selector <= faint1High)
        {
            canFaint1 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);
            canBark = false;
            animator.SetBool("IsBarking", false);

            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);
            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

        }
    }


    // Doggo level 5's learnt animations - run / roll, idle, sit, bark, faint1, flying
    private void AnimationLevel5(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;


            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            isIdling = false;

        }
        else if (Selector > faint1Low && Selector <= faint1High)
        {
            canFaint1 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);

            canSit = false;
            animator.SetBool("IsSitting", false);
            canBark = false;
            animator.SetBool("IsBarking", false);

            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);
            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

        }
    }

    // Doggo level 10's learnt animations - run / roll / flying, idle, sit, bark, faint1, dizzy
    private void AnimationLevel10(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;


            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            isIdling = false;

        }
        else if (Selector > faint1Low && Selector <= faint1High)
        {
            canFaint1 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            isIdling = false;

        }
        else if (Selector > dizzyLow && Selector <= dizzyHigh)
        {
            canDizzy = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);


            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);


        }
    }

    // Doggo level 15's learnt animations - run / roll / flying, idle, sit, bark, faint1, dizzy, faint2
    private void AnimationLevel15(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);


            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);


            isIdling = false;

        }
        else if (Selector > faint1Low && Selector <= faint1High)
        {
            canFaint1 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);


            isIdling = false;

        }
        else if (Selector > dizzyLow && Selector <= dizzyHigh)
        {
            canDizzy = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);


            isIdling = false;

        }
        else if (Selector > faint2Low && Selector <= faint2High)
        {
            canFaint2 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);


        }
    }


    // Doggo level 20's learnt animations - run / roll / flying, idle, sit, bark, faint1, dizzy, faint2, electic shock
    private void AnimationLevel20(float Selector)
    {
        if (Selector <= sitHi)
        {
            canSit = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canMove = false;
            animator.SetBool("IsRunning", false);                       // For movement - HAVE TO ALWAYS CONSIDER THAT DOGGO CAN EITHER RUN OR ROLL
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;
        }
        else if (Selector > runLow && Selector <= runHi)
        {
            canMove = true;

            // Disable all else that is not supposed to be running ----- needs to add on to make sure everything else is disabled
            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;
        }
        else if (Selector > barkLow && Selector <= barkHigh)
        {
            canBark = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;

        }
        else if (Selector > faint1Low && Selector <= faint1High)
        {
            canFaint1 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;

        }
        else if (Selector > dizzyLow && Selector <= dizzyHigh)
        {
            canDizzy = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;

        }
        else if (Selector > faint2Low && Selector <= faint2High)
        {
            canFaint2 = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canShock = false;
            animator.SetBool("IsShocking", false);

            isIdling = false;

        }
        else if (Selector > shockLow && Selector <= shockHigh)
        {
            canShock = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            isIdling = false;

        }
        else                                                        // For idle state, we disable everything
        {
            isIdling = true;

            canMove = false;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsRolling", false);
            animator.SetBool("IsFlying", false);
            flying = false;

            canSit = false;
            animator.SetBool("IsSitting", false);

            canBark = false;
            animator.SetBool("IsBarking", false);

            canFaint1 = false;
            animator.SetBool("IsFainting1", false);

            canDizzy = false;
            animator.SetBool("IsDizzy", false);

            canFaint2 = false;
            animator.SetBool("IsFainting2", false);

            canShock = false;
            animator.SetBool("IsShocking", false);


        }
    }



    public void FixPositionSit()
    {
        var positionOfDoggo = m_Rigidbody2D.position;
        positionOfDoggo.y = -2.70f;
        //positionOfDoggo.y = -55.0f;
        m_Rigidbody2D.position = positionOfDoggo;
    }

    public void FixPositionStand()     // not used in sit_idle now since we dont want him to be jerking when sit_idle animation is repeated. used in standup animation instead
    {
        var positionOfDoggo = m_Rigidbody2D.position;
        positionOfDoggo.y = -2.57f;
        // positionOfDoggo.y = -52.94f;
        m_Rigidbody2D.position = positionOfDoggo;
    }


    /*
    // Update is called once per frame
    void Update()
    {
        // left or A key = -1, right or D key = 1
        // Mulitplied by runSpeed, so -1 * runSpeed or 1 * runSpeed
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Can be made to give a random -1 or 1 integer value later on, and invoke it at a correct time interval
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftTrigger"))
        {
            Debug.Log("IM IN LEFT TRIGGER");
            doggoTooLeft = true;

            // UserInterface.instance.AddMoney(10);                     
            // UserInterface.instance.LevelUp(0.60f);

            // Before we made UserInterface into a singleton, we used the 2 statements below
            // FindObjectOfType<UserInterface>().SendMessage("AddMoney", 10);
            // FindObjectOfType<UserInterface>().SendMessage("LevelUp", 0.60f);
        }
        else if (collision.CompareTag("RightTrigger"))
        {
            Debug.Log("IM IN RIGHT TRIGGER");
            doggoTooRight = true;

            // UserInterface.instance.AddMoney(10);
            //   UserInterface.instance.LevelUp(0.60f);

            // FindObjectOfType<UserInterface>().SendMessage("AddMoney", 10);
            // FindObjectOfType<UserInterface>().SendMessage("LevelUp", 0.60f);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftTrigger"))
        {
            Debug.Log("I HAVE EXITED THE LEFT TRIGGER");
            doggoTooLeft = false;
        }
        else if (collision.CompareTag("RightTrigger"))
        {
            Debug.Log("I HAVE EXITED THE RIGHT TRIGGER");
            doggoTooRight = false;
        }
    }


    // Manage Speech bubbles. Called within the barking animation
    public void SpeechBubblePopUp()
    {
        Debug.Log("JUST STARTED GAME VAR: " + JustStartedGame);
        Debug.Log("Should be 0 unless we come from welcome page");

        if (doggoRight)
            TextForRight();             // convert all text to be for right direction
        else
            TextForLeft();              // convert all text to be for left direction

        // If just came from welcome page
        if (JustStartedGame == 1)
        {
            speechbubble_Lovely.SetActive(true);
            contentSelector = Random.value;

            if (contentSelector < 0.50f)
            {
                WelcomeHome.text = "Welcome Home " + PlayerPrefs.GetString("name") + "!";
                WelcomeHome.enabled = true;
            }
            else
            {
                if (TimeManager.instance.now.Hour < 12)                         // morning
                {
                    GoodMorning.text = "Good Morning " + PlayerPrefs.GetString("name") + "!";
                    GoodMorning.enabled = true;
                }
                else if (TimeManager.instance.now.Hour < 17)                    // afternoon
                {
                    GoodMorning.text = "Good Afternoon " + PlayerPrefs.GetString("name") + "!";
                    GoodMorning.enabled = true;
                }
                else                                                            // night
                {
                    GoodMorning.text = "Good Evening " + PlayerPrefs.GetString("name") + "!";
                    GoodMorning.enabled = true;
                }
            }
        }
        else                        // if during normal barking or came in from other scenes
        {
            speechBubbleSelector = Random.value;            // select speech bubble to use

            if (speechBubbleSelector < 0.50f)               // activate first type of speech bubble  - stronk
            {
                speechbubble_Stronk.SetActive(true);

                contentSelector = Random.value;             // select content within that speech bubble

                if (contentSelector < 0.33f)
                {
                    LoveQuiz.SetActive(true);
                }
                else if (contentSelector < 0.66f)
                {
                    Awesome.SetActive(true);
                }
                else
                {
                    StudyTogether.SetActive(true);
                }
            }
            else                // activate second type of speech bubble  - lovely
            {
                speechbubble_Lovely.SetActive(true);

                contentSelector = Random.value;             // select content within that speech bubble

                if (contentSelector < 0.25f)
                {
                    LubYou.SetActive(true);
                }
                else if (contentSelector < 0.50f)
                {
                    GoodBoi.SetActive(true);
                }
                else if (contentSelector < 0.75f)
                {
                    heart_image.SetActive(true);
                }
                else
                {
                    henlo.SetActive(true);
                }
            }

        }
    }

    public void DisableSpeechBubbleAndEverything()
    {
        // Since we do not know which one was randomly chosen, we will just disable everything related to speech bubble
        speechbubble_Stronk.SetActive(false);
        LoveQuiz.SetActive(false);
        Awesome.SetActive(false);
        StudyTogether.SetActive(false);

        speechbubble_Lovely.SetActive(false);
        WelcomeHome.enabled = false;
        GoodMorning.enabled = false;
        LubYou.SetActive(false);
        GoodBoi.SetActive(false);
        heart_image.SetActive(false);
        henlo.SetActive(false);
    }


    private void TextForLeft()
    {
        LoveQuiz.transform.localScale = new Vector3(-1, 1, 1);          // turn it oppposite for when doggo is facing the left side
        Awesome.transform.localScale = new Vector3(-1, 1, 1);         
        StudyTogether.transform.localScale = new Vector3(-1, 1, 1);          
        WelcomeHomeObj.transform.localScale = new Vector3(-1, 1, 1);          
        GoodMorningObj.transform.localScale = new Vector3(-1, 1, 1);         
        LubYou.transform.localScale = new Vector3(-1, 1, 1);       
        GoodBoi.transform.localScale = new Vector3(-1, 1, 1);
        henlo.transform.localScale = new Vector3(-1, 1, 1);

    }

    private void TextForRight()
    {
        LoveQuiz.transform.localScale = new Vector3(1, 1, 1);          // turn it oppposite for when doggo is facing the left side
        Awesome.transform.localScale = new Vector3(1, 1, 1);
        StudyTogether.transform.localScale = new Vector3(1, 1, 1);
        WelcomeHomeObj.transform.localScale = new Vector3(1, 1, 1);
        GoodMorningObj.transform.localScale = new Vector3(1, 1, 1);
        LubYou.transform.localScale = new Vector3(1, 1, 1);
        GoodBoi.transform.localScale = new Vector3(1, 1, 1);
        henlo.transform.localScale = new Vector3(1, 1, 1);
    }



    // for flying movement
    public void flyUp()
    {
        m_Rigidbody2D.AddForce(Vector2.up * UpForce);
    }

    public IEnumerator LandDown()
    {
        m_Rigidbody2D.AddForce(Vector2.down * DownForce);
        while (m_Rigidbody2D.position.y > startingYaxis)
        {
            yield return null;
        }
        m_Rigidbody2D.velocity = Vector2.zero;
    }

    public void YaxisStationary()
    {
        var rbVelocity = m_Rigidbody2D.velocity;
        rbVelocity.y = 0;
        m_Rigidbody2D.velocity = rbVelocity;
    }
}


