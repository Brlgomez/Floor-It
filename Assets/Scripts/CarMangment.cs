using UnityEngine;
using System.Collections;

public class CarMangment : MonoBehaviour {

	public GameObject[] cars;
	public bool trueGameOver;
	string level;
	public bool allPinsStopped;

	void Start () {
		trueGameOver = false;
		cars = GameObject.FindGameObjectsWithTag ("Car");
		level = Camera.main.GetComponent<LevelManagement>().level;
		Camera.main.GetComponent<SoundEffects> ().playGameplayMusic ();
		allPinsStopped = true;
	}

	void Update(){
		cars = GameObject.FindGameObjectsWithTag ("Car");
		if (cars.Length == 0 && level != LevelManagement.bowl && !trueGameOver) {
			trueGameOver = true;
			Camera.main.GetComponent<Points> ().checkScore ();
		}
		if (cars.Length == 0 && level == LevelManagement.bowl && !trueGameOver) {
			GameObject[] pins = GameObject.FindGameObjectsWithTag ("Pin");
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
