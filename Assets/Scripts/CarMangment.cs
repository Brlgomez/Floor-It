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
	public float carSteering;

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
			carSteering = 0.75f;
		} else {
			carMass = 10;
			carSteering = 0.5f;
		}
		GameObject.Find ("Car").GetComponent<Rigidbody> ().mass = carMass;
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
