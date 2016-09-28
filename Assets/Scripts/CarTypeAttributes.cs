using UnityEngine;
using System.Collections;

public class CarTypeAttributes : MonoBehaviour {
	
	// 0 = sudan, 1 = limo, 2 = truck, 3 = sport, 4 = monster truck, 5 = cone, 6 = bus, 7 = abstract
	static int numOfCars = 8;
	Material[] carTypeMaterial = new Material[numOfCars];
	float[] massOfCars = new float[numOfCars];
	float[] carAutoSteering = new float[numOfCars];
	float[] carManSteering = new float[numOfCars];
	float[] carAcceleration = new float[numOfCars];
	float[] pointSpeed = new float[numOfCars];
	float[] newCarSpawnDist = new float[numOfCars];

	void Awake(){
		carTypeMaterial [0] = GameObject.Find ("Car").GetComponent<Renderer> ().materials[1];
		carTypeMaterial [1] = GameObject.Find (AllBlockNames.hillBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [2] = GameObject.Find (AllBlockNames.pointBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [3] = GameObject.Find (AllBlockNames.shuffleBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [4] = GameObject.Find (AllBlockNames.multiplierBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [5] = GameObject.Find (AllBlockNames.invisibleBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [6] = GameObject.Find (AllBlockNames.decelerateBlock).GetComponent<Renderer> ().material;
		carTypeMaterial [7] = GameObject.Find ("Car").GetComponent<Renderer> ().materials[1];
		massOfCars [0] = 5;
		massOfCars [1] = 10;
		massOfCars [2] = 8;
		massOfCars [3] = 4;
		massOfCars [4] = 18;
		massOfCars [5] = 1;
		massOfCars [6] = 40;
		massOfCars [7] = 2;
		carAutoSteering [0] = 75;
		carAutoSteering [1] = 50;
		carAutoSteering [2] = 65;
		carAutoSteering [3] = 100;
		carAutoSteering [4] = 35;
		carAutoSteering [5] = 100;
		carAutoSteering [6] = 20;
		carAutoSteering [7] = 50;
		carManSteering [0] = 0.75f;
		carManSteering [1] = 0.5f;
		carManSteering [2] = 0.65f;
		carManSteering [3] = 1.0f;
		carManSteering [4] = 0.35f;
		carManSteering [5] = 1.0f;
		carManSteering [6] = 0.2f;
		carManSteering [7] = 0.50f;
		carAcceleration [0] = 0.01f;
		carAcceleration [1] = 0.008f;
		carAcceleration [2] = 0.009f;
		carAcceleration [3] = 0.012f;
		carAcceleration [4] = 0.007f;
		carAcceleration [5] = 0.01f;
		carAcceleration [6] = 0.002f;
		carAcceleration [7] = 0.0075f;
		pointSpeed [0] = 2.0f;
		pointSpeed [1] = 3.0f;
		pointSpeed [2] = 2.8f;
		pointSpeed [3] = 1.6f;
		pointSpeed [4] = 3.5f;
		pointSpeed [5] = 1.0f;
		pointSpeed [6] = 3.25f;
		pointSpeed [7] = 1.25f;
		newCarSpawnDist [0] = -1.5f;
		newCarSpawnDist [1] = -2.0f;
		newCarSpawnDist [2] = -1.75f;
		newCarSpawnDist [3] = -1.5f;
		newCarSpawnDist [4] = -2.5f;
		newCarSpawnDist [5] = -1.4f;
		newCarSpawnDist [6] = -2.0f;
		newCarSpawnDist [7] = -1.75f;
	}

	public Material getCarTypeMaterial(int index){
		return carTypeMaterial [index];
	}

	public float getMassOfCar(int index) {
		return massOfCars [index];
	}

	public float getCarAutoSteering(int index) {
		return carAutoSteering [index];
	}

	public float getCarManSteering(int index) {
		return carManSteering [index];
	}

	public float getCarAcceleration(int index) {
		return carAcceleration [index];
	}

	public float getPointSpeed(int index) {
		return pointSpeed [index];
	}

	public float getNewCarSpawnDist(int index) {
		return newCarSpawnDist [index];
	}
}
