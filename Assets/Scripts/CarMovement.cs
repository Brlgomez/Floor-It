using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	private Rigidbody rb;

	public static float slowestSpeed = 0.5f;
	public static float fastestSpeed = 10.0f;
	public static float jumpHeight = 5.0f;

	static float yPosFallingBarrier = -1;
	static float distFromLeadForGameOver = -15;
	static float carFlippedLimit = 0f; //0 to -1;

	public float flyingTimer = 0;
	public static float flyingTime = 10; // in seconds;
	public float forceTimer = 0;
	public static float forceLimit = 0.5f;
	public float resizeCounter = 0;
	public static float resizeLimit = 10;

	public bool gameOver;
	public bool flying;
	public bool resized;
	public bool carFlipped;
	public bool evilCarWithinRange;
	static float evilCarRange = 5;
	public float speedometer;
	Vector3 lastPos;

	public float speed;
	public float acceleration;
	public float distToGround;

	string level;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		level = Camera.main.GetComponent<LevelManagement> ().level;
		gameOver = false;
		flying = false;
		resized = false;
		carFlipped = false;
		evilCarWithinRange = true;
		distToGround = transform.position.y;
		if (speed == 0) {
			speed = 0.5f;
		}
		acceleration = 0.01f;
	}

	void FixedUpdate () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && !gameOver) {
			float deltaTime = Time.deltaTime;
			forceTimer += deltaTime;
			if (tag.Equals (TagManagement.evilCar)) {
				if (!(Vector3.Dot (transform.up, Vector3.down) > carFlippedLimit) || flying) {
					Vector3 leadCarPos = Camera.main.GetComponent<FollowCar> ().leadCar.transform.position;
					if (Vector3.Distance (leadCarPos, transform.position) < evilCarRange) {
						evilCarWithinRange = true;
						carFlipped = false;
						rb.MovePosition (transform.position + transform.forward * deltaTime * speed);
					} else {
						evilCarWithinRange = false;
						carFlipped = true;
					}
				}
			} else {
				if (!(Vector3.Dot (transform.up, Vector3.down) > carFlippedLimit) || flying) {
					rb.MovePosition (transform.position + transform.forward * deltaTime * speed);
					carFlipped = false;
				} else {
					carFlipped = true;
				}
				if (gameObject == Camera.main.GetComponent<CarMangment> ().cars [0]) {
					speedometer = (transform.position - lastPos).magnitude / Time.smoothDeltaTime;
					lastPos = transform.position;
				}
				if (speed < fastestSpeed) {
					speed += deltaTime * acceleration;
				}
				if (speed < slowestSpeed) {
					speed = slowestSpeed;
				}
			}
			checkGameOverConditions ();
			if (resized) {
				Camera.main.GetComponent<CarAttributes> ().resizedTimer (gameObject);
			}
			if (flying) {
				Camera.main.GetComponent<CarAttributes> ().flyingTimer (gameObject);
			}
		}
	}

	void checkGameOverConditions(){
		// car is too far away form lead car
		if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
			float leadCarZ = Camera.main.GetComponent<FollowCar> ().leadCar.transform.position.z;
			if (transform.position.z - leadCarZ < distFromLeadForGameOver) {
				setToGameOver ();
			}
		}
		// car has fallen
		if (transform.position.y < yPosFallingBarrier) {
			setToGameOver ();
		}
		//car is stopped, evil cars can be stopped
		if (rb.IsSleeping () && !flying && tag != TagManagement.evilCar) {
			setToGameOver ();
		}
	}

	void setToGameOver () {
		gameOver = true;
		gameObject.tag = TagManagement.deadCar;
		Camera.main.GetComponent<SoundEffects> ().playCarDeathSound (gameObject.transform.position);
		if (!tag.Equals (TagManagement.evilCar) && level == LevelManagement.drive) {
			Renderer rend = gameObject.GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int i = 0; i < rend.materials.Length; i++) {
				mats [i] = rend.materials [i];
			}
			mats [2] = Camera.main.GetComponent<CarAttributes>().regularCarMaterial; 
			rend.materials = mats;
			Camera.main.GetComponent<CarAttributes> ().changeMaterialOfCars ();
		}
	}
		
	void OnCollisionEnter(Collision hit) {
		if (!gameOver) {
			Camera.main.GetComponent<CarAttributes> ().onBlock (hit, gameObject, rb);
		}
	}
}
