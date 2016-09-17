﻿using UnityEngine;
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

	void Start () {
		trueGameOver = false;
		cars = GameObject.FindGameObjectsWithTag ("Car");
		GameObject initialCar = cars [0];
		level = Camera.main.GetComponent<LevelManagement>().level;
		Camera.main.GetComponent<SoundEffects> ().playGameplayMusic ();
		allPinsStopped = true;
		int carNum = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);

		initialCar.GetComponent<MeshFilter> ().mesh = carMeshes [carNum];
		DestroyImmediate(initialCar.GetComponent<MeshCollider>());
		MeshCollider collider = initialCar.AddComponent<MeshCollider>();
		collider.sharedMesh = carColliders[carNum].GetComponent<MeshFilter> ().mesh;
		collider.convex = true;

		carMass = Camera.main.GetComponent<CarTypeAttributes> ().massOfCars [carNum];
		carAutoSteering = Camera.main.GetComponent<CarTypeAttributes> ().carAutoSteering [carNum];
		carManSteering = Camera.main.GetComponent<CarTypeAttributes> ().carManSteering [carNum];
		carAcceleration = Camera.main.GetComponent<CarTypeAttributes> ().carAcceleration [carNum];
		pointSpeed = Camera.main.GetComponent<CarTypeAttributes> ().pointSpeed [carNum];
		newCarSpawnDist = Camera.main.GetComponent<CarTypeAttributes> ().newCarSpawnDist [carNum];

		initialCar.GetComponent<Rigidbody> ().mass = carMass;
		initialCar.GetComponent<CarMovement> ().acceleration = carAcceleration;

		if (carNum != 5) {
			Renderer rend = initialCar.GetComponent<Renderer> ();
			Material[] mats = new Material[rend.materials.Length];
			for (int j = 0; j < rend.materials.Length; j++) {
				if (j == 1) {
					mats [j] = Camera.main.GetComponent<CarTypeAttributes>().carTypeMaterial[carNum]; 
				} else {
					mats [j] = rend.materials [j];
				}
			}
			rend.materials = mats;
		} else {
			Renderer rend = initialCar.GetComponent<Renderer> ();
			rend.materials = new Material[1];
			Material[] mats = new Material[rend.materials.Length];
			mats [0] = Camera.main.GetComponent<CarTypeAttributes>().carTypeMaterial[carNum];  
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
