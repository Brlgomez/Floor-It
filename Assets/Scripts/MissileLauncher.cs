using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour {

	GameObject missile;
	int timeBetweenFire = 3;
	float timer;

	void Start () {
		missile = GameObject.Find ("Missile");
		timeBetweenFire = Random.Range (1, 6);
		timer = Random.Range (0, timeBetweenFire);
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > timeBetweenFire) {
			timer = 0;
			GameObject missileRight;
			missileRight = Instantiate (missile);
			missileRight.transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z + 0.4f);
			missileRight.transform.Rotate(0, transform.eulerAngles.y + 90, 0);
			missileRight.AddComponent<Missile> ();
			GameObject missileLeft;
			missileLeft = Instantiate (missile);
			missileLeft.transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z - 0.4f);
			missileLeft.transform.Rotate(0, transform.eulerAngles.y + 270, 0);
			missileLeft.AddComponent<Missile> ();
			GetComponentsInChildren<ParticleSystem> () [0].Play ();
			GetComponentsInChildren<ParticleSystem> () [1].Play ();
			Camera.main.GetComponent<SoundEffects> ().playMissileSound (transform.position);
		}
	}
}
