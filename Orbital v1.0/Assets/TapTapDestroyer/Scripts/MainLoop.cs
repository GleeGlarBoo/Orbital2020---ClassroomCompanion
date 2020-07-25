using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;		// Since we have to jump to the final scene

// different levels could be implemented, values below could change or we could change the timescale of the game

public class MainLoop: MonoBehaviour {

	#region Singleton
	public static MainLoop instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(instance);

		instance = this;
	}
	#endregion

	// All public so that we can edit them in the editor
	public GameObject[] stonesAndBomb = new GameObject[8];		// an array of 8 game objects for the 3 stones (2x each for higher chance than bomb) and 1 bombs, which will be randomly chosen to be instantiated.
	public float torque = 5.0f;							// Make our object rotate / spin

	// Range of values so that we can generate random numbers in this range, to make our game less predictable
	public float minUpForce = 20.0f, maxUpForce = 40.0f;			// greater than 9.81 since this is the force we use to throw our stones upwards
	public float minLateralForce = -15.0f, maxLateralForce = 15.0f;			// give our stones some left and right tendency. -ve = left, +ve = right
	public float minTimeBetweenStones = 1f, maxTimeBetweenStones = 3f;		// Random time interval before creating next stone
	public float minX = -30.0f, maxX = 30.0f;							// Determines which part of the screen (left or right), the stone is going to be created  
	public float minZ = -10.0f, maxZ = 20.0f;							// Determins if the stone is closer to the camera or further away - smaller or bigger
	
	public bool enableStones = true;				// Condition for infinite loop 
	private Rigidbody rb;

	public bool BombExplodedTimeOut = false;		// 10 seconds timeout of no spawn if bomb exploded

	public float TimeScaleNow = 2;	// follows time scale of game so that we can control the time with real world 1 sec
	
	// Use this for initialization
	void Start () {

		Screen.orientation = ScreenOrientation.Landscape;

		Time.timeScale = 2;
		TimeScaleNow = 2;
		StartCoroutine(ThrowStones());

		Invoke("TimeScale_3", 40);			// since 20s in 2x timescale is 40s
		Invoke("TimeScale_4", 60);          // same reason above, but 3x now
		// Invoke("TimeScale_5", 80);          // same reason above, but 4x now

	}

	// Update is called once per frame
	void Update () {
	}

	// Similar coroutine as the previous example. 
	IEnumerator ThrowStones()
	{
		// Initial delay
		// yield return new WaitForSeconds(2.0f);		// wait abit when the level starts - allow user to be prepared
		
		while(enableStones) {

			// if user clicked on bomb, we dont spanw anything for 10 seconds
			if (BombExplodedTimeOut)
			{
				yield return new WaitForSeconds(10f);
				BombExplodedTimeOut = false;
			}
			else
			{
				// wait for a random amount of time between 1 and 3 seconds - to achieve random behavior for game 
				yield return new WaitForSeconds(Random.Range(minTimeBetweenStones, maxTimeBetweenStones));

			}

			/*
			if (GameManagerTapTap.currentNumberStonesThrown == 19)		// increase difficulty
			{
				Time.timeScale = 10;
			}
			else if (GameManagerTapTap.currentNumberStonesThrown == 49)		// his example was just 20 then end the game
			{
				SceneManager.LoadScene("Final");
			}
			*/

			// put this condition here so we dont spawn a new object right after the player hits the bomb. Because if this is below, 1 item still spawns
			if (!BombExplodedTimeOut && Timer_TapTap.instance.GameStarted == true)
			{
				// randomly geenerate one of the 3 prefabs. 0 is included but stones.Length = 3 is excluded	
				GameObject stone = (GameObject)Instantiate(stonesAndBomb[Random.Range(0, stonesAndBomb.Length)]);           // Instantiate returns the gameObject created 
				stone.transform.position = new Vector3(Random.Range(minX, maxX), -30.0f, Random.Range(minZ, maxZ));     // change stones position with variables we defined above. Stones created below the screen
				stone.transform.rotation = Random.rotation;

				rb = stone.GetComponent<Rigidbody>();

				// Adding angular force so that our stone will rotate around the 3 axis
				// Impulse force since we want from the beginning to have this angular velocity
				// Could make torque a random value too
				rb.AddTorque(Vector3.up * torque, ForceMode.Impulse);           // up = vertical, in the y-axis
				rb.AddTorque(Vector3.right * torque, ForceMode.Impulse);        // right = horizontal, in the x-axis
				rb.AddTorque(Vector3.forward * torque, ForceMode.Impulse);      // in the z-axis 

				// Force that is responsible for throwing up the stone - Vector3.up = (0, 1, 0)
				// and Force that is responsible for making stones go to the right or left - Vector3.right = (1, 0, 0)    - so that stones are not only flying up
				// Impulse mode since we Want an initial velocity in the respective direction - like 'kicking' the stones
				rb.AddForce(Vector3.up * Random.Range(minUpForce, maxUpForce), ForceMode.Impulse);
				rb.AddForce(Vector3.right * Random.Range(minLateralForce, maxLateralForce), ForceMode.Impulse);         // a random number from -15 (left) to 15 (right)

			}


		}
		
	}

	void TimeScale_3()
	{
		Time.timeScale = 2.5f;
		TimeScaleNow = 2.5f;
	}

	void TimeScale_4()
	{
		Time.timeScale = 3f;
		TimeScaleNow = 3f;
	}

	void TimeScale_5()
	{
		Time.timeScale = 5;
		TimeScaleNow = 5;
	}


}

