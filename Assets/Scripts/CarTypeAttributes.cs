using UnityEngine;
using System.Collections;

public class CarTypeAttributes : MonoBehaviour {
	
	// 0 = sudan, 1 = limo, 2 = truck, 3 = sport, 4 = monster truck, 5 = cone, 6 = bus
	static int numOfCars = 7;
	public Material[] carTypeMaterial = new Material[numOfCars];
	public float[] massOfCars = new float[numOfCars];
	public float[] carAutoSteering = new float[numOfCars];
	public float[] carManSteering = new float[numOfCars];
	public float[] carAcceleration = new float[numOfCars];
	public float[] pointSpeed = new float[numOfCars];
	public float[] newCarSpawnDist = new float[numOfCars];

	void Awake(){
		carTypeMaterial [0] = GameObject.Find ("Car").GetComponent<Renderer> ().material;
		carTypeMaterial [1] = GameObject.Find (AllBlockNames.standardBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [2] = GameObject.Find (AllBlockNames.pointBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [3] = GameObject.Find (AllBlockNames.invisibleBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [4] = GameObject.Find (AllBlockNames.multiplierBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [5] = GameObject.Find (AllBlockNames.invisibleBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [6] = GameObject.Find (AllBlockNames.decelerateBlock).GetComponent<Renderer> ().material;
		massOfCars [0] = 5;
		massOfCars [1] = 10;
		massOfCars [2] = 8;
		massOfCars [3] = 4;
		massOfCars [4] = 18;
		massOfCars [5] = 1;
		massOfCars [6] = 40;
		carAutoSteering [0] = 75;
		carAutoSteering [1] = 50;
		carAutoSteering [2] = 65;
		carAutoSteering [3] = 100;
		carAutoSteering [4] = 35;
		carAutoSteering [5] = 100;
		carAutoSteering [6] = 20;
		carManSteering [0] = 0.75f;
		carManSteering [1] = 0.5f;
		carManSteering [2] = 0.65f;
		carManSteering [3] = 1.0f;
		carManSteering [4] = 0.35f;
		carManSteering [5] = 1.0f;
		carManSteering [6] = 0.2f;
		carAcceleration [0] = 0.01f;
		carAcceleration [1] = 0.008f;
		carAcceleration [2] = 0.009f;
		carAcceleration [3] = 0.012f;
		carAcceleration [4] = 0.007f;
		carAcceleration [5] = 0.01f;
		carAcceleration [6] = 0.002f;
		pointSpeed [0] = 2.0f;
		pointSpeed [1] = 3.0f;
		pointSpeed [2] = 2.8f;
		pointSpeed [3] = 1.6f;
		pointSpeed [4] = 3.8f;
		pointSpeed [5] = 1.0f;
		pointSpeed [6] = 3.2f;
		newCarSpawnDist [0] = -1.5f;
		newCarSpawnDist [1] = -2.0f;
		newCarSpawnDist [2] = -1.75f;
		newCarSpawnDist [3] = -1.5f;
		newCarSpawnDist [4] = -2.5f;
		newCarSpawnDist [5] = -1.4f;
		newCarSpawnDist [6] = -2.5f;
	}
}
