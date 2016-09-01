using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	private Rigidbody rb;
	GameObject invisibleFloor;
	public Material leadCarMaterial;
	public Material regularCarMaterial;

	public bool gameOver;
	public bool flying;
	public bool resized;
	public bool carFlipped;
	public bool evilCarWithinRange;
	public float speedometer;
	Vector3 lastPos;

	public float speed;
	public float acceleration;
	public float distToGround;
	public static float slowestSpeed = 0.5f;
	public static float fastestSpeed = 10.0f;
	public static float jumpHeight = 5.0f;

	float yPosFallingBarrier;
	static float carFlippedLimit = 0f; //0 to -1;

	public float flyingTimer = 0;
	public static float flyingTime = 10; // in seconds;
	public float forceTimer = 0;
	public static float forceLimit = 0.5f;
	public float resizeCounter = 0;
	public static float resizeLimit = 10;

	string level;

	void Start () {
		level = Camera.main.GetComponent<LevelManagement>().level;
	
		rb = GetComponent<Rigidbody> ();
		invisibleFloor = GameObject.Find ("InvisibleFloor");
		Physics.IgnoreCollision (invisibleFloor.GetComponent<Collider> (), GetComponent<MeshCollider> ());
		Physics.IgnoreCollision (invisibleFloor.GetComponent<Collider> (), GetComponent<BoxCollider> ());

		gameOver = false;
		flying = false;
		resized = false;
		carFlipped = false;
		evilCarWithinRange = true;

		if (speed == 0) {
			speed = 0.5f;
		}
		acceleration = 0.01f;
		distToGround = transform.position.y;
		yPosFallingBarrier = invisibleFloor.transform.position.y;

		if (name != "Car1") {
			changeMaterial ();
		}
	}

	void FixedUpdate () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && !gameOver) {
			float deltaTime = Time.deltaTime;
			forceTimer += deltaTime;
			if (tag.Equals ("Evil Car")) {
				if (!(Vector3.Dot (transform.up, Vector3.down) > carFlippedLimit) || flying) {
					if (Vector3.Distance (Camera.main.GetComponent<FollowCar> ().leadCar.transform.position, transform.position) < 5) {
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

	public void changeMaterial(){
		if (gameObject.name == GameObject.FindGameObjectsWithTag("Car")[0].name) {
			changeLeadMat ();
		} else {
			changeAIMat ();
		}
	}

	public void changeLeadMat(){
		if (level == LevelManagement.drive) {
			Renderer rend = gameObject.GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int i = 0; i < rend.materials.Length; i++) {
				mats [i] = rend.materials [i];
			}
			mats [2] = leadCarMaterial; 
			rend.materials = mats;
		}
	}

	public void changeAIMat(){
		if (level == LevelManagement.drive) {
			Renderer rend = gameObject.GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int i = 0; i < rend.materials.Length; i++) {
				mats [i] = rend.materials [i];
			}
			mats [2] = regularCarMaterial; 
			rend.materials = mats;
		}
	}

	void checkGameOverConditions(){
		// car is too far away form lead car
		if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
			if (transform.position.z - Camera.main.GetComponent<FollowCar> ().leadCar.transform.position.z < -15) {
				setToGameOver ();
			}
		}
		// car has fallen
		if (transform.position.y < yPosFallingBarrier) {
			setToGameOver ();
		}
		//car is stopped, evil cars can be stopped
		if (rb.IsSleeping () && !flying && tag != "Evil Car") {
			setToGameOver ();
		}
	}

	void setToGameOver () {
		gameOver = true;
		gameObject.tag = "Dead Car";
		Camera.main.GetComponent<SoundEffects> ().playCarDeathSound (gameObject.transform.position);

		Renderer rend = gameObject.GetComponent<Renderer> ();
		Material[] mats = new Material[rend.materials.Length];
		for (int i = 0; i < rend.materials.Length; i++) {
			mats [i] = rend.materials [i];
		}
		mats [2] = regularCarMaterial; 
		rend.materials = mats;

		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Car").Length; i++) {
			GameObject.FindGameObjectsWithTag ("Car") [i].GetComponent<CarMovement> ().changeMaterial ();
		}
	}
		
	void OnTriggerEnter(Collider hit) {
		if (!gameOver) {
			Camera.main.GetComponent<CarAttributes> ().onBlock (hit, gameObject, rb);
		}
	}
}
