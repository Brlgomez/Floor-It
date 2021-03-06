﻿using UnityEngine;
using System.Collections;

public class EvilCarAttributes : MonoBehaviour {

	ParticleSystem smoke;
	GameObject invisibleFloor;
	float aiTurnCount;
	static float aiTurnLimit = 0.1f;
	static float explodedDist = 1.25f;

	static int radius = 2;
	static int power = 1000;
	static int upwardForce = 25;
	bool exploded;
	bool particlePlayed;
	public bool explodeNow;

	GameObject followCar = null;
	float shortestDist;
	bool playedClip = false;

	private AudioSource source;

	void Start () {
		source = GetComponent<AudioSource> ();
		invisibleFloor = GameObject.Find ("InvisibleFloor");
		smoke = gameObject.GetComponentsInChildren<ParticleSystem>()[2];
		GetComponent<Rigidbody> ().useGravity = true;
		exploded = false;
		particlePlayed = false;
		explodeNow = false;
		getShortestDistance ();
		if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 1) {
			GetComponentsInChildren<Light> ()[0].enabled = true;
		}
	}
	
	void Update () {
		aiTurnCount += Time.deltaTime;
		if (aiTurnCount > aiTurnLimit && !GetComponent<CarMovement>().gameOver) {
			aiTurnCount = 0;
			lookAtLeadCar ();
		}
		if (followCar != null && !exploded) {
			if (Vector3.Distance (transform.position, followCar.transform.position) < (explodedDist * transform.localScale.x) || GetComponent<CarMovement> ().gameOver || explodeNow) {
				explosionForce ();
				smoke.Play ();
				source.Stop ();
				Camera.main.GetComponent<SoundEffects> ().playExplosionSound (transform.position);
				ChangeMaterial (invisibleFloor.GetComponent<Renderer>().material);
				Behaviour halo = (Behaviour)transform.GetChild (0).GetComponent ("Halo");
				halo.enabled = false;
				GetComponent<Collider> ().enabled = false;
				exploded = true;
				particlePlayed = true;
				Camera.main.GetComponent<Vibration> ().vibrate();
			}
		}
		if (!smoke.isPlaying && particlePlayed) {
			Camera.main.GetComponent<Points> ().incrementPoints (5, gameObject);
			Camera.main.GetComponent<PlayerPrefManagement> ().increaseBombCars ();
			Destroy (gameObject);
		}
		if (Camera.main.GetComponent<CarMangment> ().trueGameOver) {
			source.Stop ();
		}
	}

	void ChangeMaterial (Material newMat) {
		Renderer rend = GetComponent<Renderer>();
		Material[] mats = new Material[rend.materials.Length];
		for (int j = 0; j < rend.materials.Length; j++) {
			mats[j] = newMat; 
		}
		rend.materials = mats;
	}


	void lookAtLeadCar () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver) {
			getShortestDistance ();
			if (shortestDist < 5) {
				GetComponent<CarMovement> ().evilCarWithinRange = true;
				if (!GetComponent<CarMovement> ().carFlipped) {
					if (!playedClip) {
						Camera.main.GetComponent<SoundEffects> ().playEvilCarSound (source);
						playedClip = true;
					}
					Vector3 targetPosition = followCar.transform.position;
					targetPosition.y = transform.position.y;
					Quaternion targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
					transform.rotation = Quaternion.Slerp (
						transform.rotation, 
						targetRotation, 
						Time.deltaTime * 10f
					);
				}
			} else {
				GetComponent<CarMovement> ().evilCarWithinRange = false;
				source.Stop ();
				playedClip = false;
			}
		}
	}

	void getShortestDistance () {
		followCar = Camera.main.GetComponent<FollowCar>().leadCar;
		shortestDist = Vector3.Distance (followCar.transform.position, transform.position);
		for (int i = 0; i < GameObject.FindGameObjectsWithTag (TagManagement.car).Length; i++) {
			float distance = Vector3.Distance (GameObject.FindGameObjectsWithTag (TagManagement.car) [i].transform.position, transform.position);
			if (distance < shortestDist) {
				shortestDist = distance;
				followCar = GameObject.FindGameObjectsWithTag (TagManagement.car) [i];
			}
		}
	}

	void explosionForce () {
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null) {
				rb.AddExplosionForce (
					power * transform.localScale.x,
					explosionPos,
					radius * transform.localScale.x, 
					upwardForce * transform.localScale.x
				);
			}
		}
	}
}
