using UnityEngine;
using System.Collections;

public class CarMangment : MonoBehaviour {

	public GameObject[] cars;
	public bool trueGameOver;
	string level;
	public bool allPinsStopped;
	public Mesh[] carMeshes;
	public GameObject[] carColliders;
	public float carMass;
	public float carAutoSteering;
	public float carManSteering;
	public float carAcceleration;
	public float pointSpeed;
	public float newCarSpawnDist;
	public float initialSpeed;
	public float carJumpDist;
	public int carNum;

	void Start () {
		trueGameOver = false;
		cars = GameObject.FindGameObjectsWithTag ("Car");
		GameObject initialCar = cars [0];
		level = Camera.main.GetComponent<LevelManagement>().level;
		Camera.main.GetComponent<SoundEffects> ().playGameplayMusic ();
		allPinsStopped = true;
		carNum = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);

		initialCar.GetComponent<MeshFilter> ().mesh = carMeshes [carNum];
		DestroyImmediate(initialCar.GetComponent<MeshCollider>());
		MeshCollider collider = initialCar.AddComponent<MeshCollider>();
		collider.sharedMesh = carColliders[carNum].GetComponent<MeshFilter> ().mesh;
		collider.convex = true;

		carMass = Camera.main.GetComponent<CarTypeAttributes> ().getMassOfCar(carNum);
		carAutoSteering = Camera.main.GetComponent<CarTypeAttributes> ().getCarAutoSteering (carNum);
		carManSteering = Camera.main.GetComponent<CarTypeAttributes> ().getCarManSteering (carNum);
		carAcceleration = Camera.main.GetComponent<CarTypeAttributes> ().getCarAcceleration (carNum);
		pointSpeed = Camera.main.GetComponent<CarTypeAttributes> ().getPointSpeed (carNum);
		newCarSpawnDist = Camera.main.GetComponent<CarTypeAttributes> ().getNewCarSpawnDist (carNum);
		initialSpeed = Camera.main.GetComponent<CarTypeAttributes> ().getInitialSpeed (carNum);
		carJumpDist = Camera.main.GetComponent<CarTypeAttributes> ().getJumpDistance (carNum);

		initialCar.GetComponent<Rigidbody> ().mass = carMass;
		if (level == LevelManagement.floorIt) {
			initialCar.GetComponent<CarMovement> ().acceleration = carAcceleration * 0.67f;
		} else {
			initialCar.GetComponent<CarMovement> ().acceleration = carAcceleration;
		}
		initialCar.GetComponent<CarMovement> ().slowestSpeed = initialSpeed;
		if (level == LevelManagement.drive) {
			initialCar.GetComponent<CarMovement> ().driveSpeedIncrease = initialSpeed * 0.5f;
		} else {
			initialCar.GetComponent<CarMovement> ().driveSpeedIncrease = 0;
		}

		if (carNum != 5) {
			Renderer rend = initialCar.GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int j = 0; j < rend.materials.Length; j++) {
				if (j == 1) {
					mats [j] = Camera.main.GetComponent<CarTypeAttributes>().getCarTypeMaterial(carNum); 
				} else {
					mats [j] = rend.materials [j];
				}
			}
			rend.materials = mats;
		} else {
			Renderer rend = initialCar.GetComponent<Renderer> ();
			rend.materials = new Material[1];
			Material[] mats = new Material[rend.materials.Length];
			mats [0] = Camera.main.GetComponent<CarTypeAttributes>().getCarTypeMaterial(carNum); 
			rend.materials = mats;
		} 
	}

	void Update(){
		if (!trueGameOver) {
			cars = GameObject.FindGameObjectsWithTag (TagManagement.car);
			if (cars.Length == 0) {
				if (level == LevelManagement.drive || level == LevelManagement.floorIt) {
					trueGameOver = true;
					Camera.main.GetComponent<Points> ().checkScore ();
					Camera.main.GetComponent<Interface> ().gameOverInterface ();
					Camera.main.GetComponent<PlayerPrefManagement> ().increaseDistance (checkDistance (), level);
				} else if (level == LevelManagement.bowl) {
					GameObject[] pins = GameObject.FindGameObjectsWithTag (TagManagement.pin);
					foreach (GameObject pin in pins) {
						allPinsStopped = true;
						if (!pin.GetComponent<Rigidbody> ().IsSleeping () && pin.transform.position.y > 0) {
							allPinsStopped = false;
							break;
						}
					}
					if (allPinsStopped == true) {
						trueGameOver = true;
						Camera.main.GetComponent<Points> ().checkScore ();
						Camera.main.GetComponent<Interface> ().gameOverInterface ();
						Camera.main.GetComponent<PlayerPrefManagement> ().increaseDistance (checkDistance (), level);
					}
				}
			}
		}
	}

	float checkDistance () {
		Vector3 currentPosition = Camera.main.transform.position;
		Vector3 startingPosition = new Vector3 (0, 15, 8);
		return (Vector3.Distance (startingPosition, currentPosition)/2);
	}
}
