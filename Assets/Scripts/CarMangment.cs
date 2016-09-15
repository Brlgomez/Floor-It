using UnityEngine;
using System.Collections;

public class CarMangment : MonoBehaviour {

	public GameObject[] cars;
	public bool trueGameOver;
	string level;
	public bool allPinsStopped;
	public Mesh[] carMeshes;
	public GameObject[] carColliders;
	public Material[] carMaterial;
	public float carMass;
	public float carAutoSteering;
	public float carManSteering;
	public float carAcceleration;
	public float pointSpeed;
	public float newCarSpawnDist;

	void Start () {
		trueGameOver = false;
		cars = GameObject.FindGameObjectsWithTag ("Car");
		level = Camera.main.GetComponent<LevelManagement>().level;
		Camera.main.GetComponent<SoundEffects> ().playGameplayMusic ();
		allPinsStopped = true;
		int carNum = PlayerPrefs.GetInt ("Car Type", 0);
		GameObject.Find ("Car").GetComponent<MeshFilter> ().mesh = carMeshes [carNum];
		DestroyImmediate(GameObject.Find ("Car").GetComponent<MeshCollider>());
		MeshCollider collider = GameObject.Find ("Car").AddComponent<MeshCollider>();
		collider.sharedMesh = carColliders[carNum].GetComponent<MeshFilter> ().mesh;
		collider.convex = true;
		if (carNum == 0) {
			carMass = 5;
			carAutoSteering = 75;
			carManSteering = 0.75f;
			carAcceleration = 0.01f;
			pointSpeed = 2;
			newCarSpawnDist = -1.5f;
		} else if (carNum == 1) {
			carMass = 10;
			carAutoSteering = 50;
			carManSteering = 0.5f;
			carAcceleration = 0.008f;
			pointSpeed = 3;
			newCarSpawnDist = -2f;
		} else if (carNum == 2) {
			carMass = 8;
			carAutoSteering = 65;
			carManSteering = 0.65f;
			carAcceleration = 0.009f;
			pointSpeed = 2.8f;
			newCarSpawnDist = -1.75f;
		} else if (carNum == 3) {
			carMass = 4;
			carAutoSteering = 100;
			carManSteering = 1.0f;
			carAcceleration = 0.012f;
			pointSpeed = 1.6f;
			newCarSpawnDist = -1.5f;
		} else if (carNum == 4) {
			carMass = 18;
			carAutoSteering = 35;
			carManSteering = 0.35f;
			carAcceleration = 0.007f;
			pointSpeed = 3.8f;
			newCarSpawnDist = -2.5f;
		} else if (carNum == 5) {
			carMass = 1;
			carAutoSteering = 100;
			carManSteering = 1.0f;
			carAcceleration = 0.01f;
			pointSpeed = 1;
			newCarSpawnDist = -1.4f;
		}
		GameObject.Find ("Car").GetComponent<Rigidbody> ().mass = carMass;
		GameObject.Find ("Car").GetComponent<CarMovement> ().acceleration = carAcceleration;
		if (carNum != 5) {
			Renderer rend = GameObject.Find ("Car").GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int j = 0; j < rend.materials.Length; j++) {
				if (j == 1) {
					mats [j] = carMaterial [carNum]; 
				} else {
					mats [j] = rend.materials [j];
				}
			}
			rend.materials = mats;
		} else {
			Renderer rend = GameObject.Find ("Car").GetComponent<Renderer> ();
			rend.materials = new Material[1];
			Material[] mats = new Material[rend.materials.Length];
			mats [0] = carMaterial [carNum]; 
			rend.materials = mats;
		} 
	}

	void Update(){
		cars = GameObject.FindGameObjectsWithTag (TagManagement.car);
		if (cars.Length == 0 && level != LevelManagement.bowl && !trueGameOver) {
			trueGameOver = true;
			Camera.main.GetComponent<Points> ().checkScore ();
		}
		if (cars.Length == 0 && level == LevelManagement.bowl && !trueGameOver) {
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
			}
		}
	}
}
