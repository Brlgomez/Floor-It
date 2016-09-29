﻿using UnityEngine;
using System.Collections;

public class BlockManagment : MonoBehaviour {

	float timerCount;
	static float timerLimit = 5;
	static float maxDistAway = -10f;
	Vector3 lastCarPosition;
	public bool carStill = false;

	void Start () {
		lastCarPosition = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
	}
	
	void Update () {
		timerCount += Time.deltaTime;
		if (timerCount > timerLimit) {
			timerCount = 0;
			GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag (TagManagement.blockOnRoad);
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject> ();
			// destroy objects that have fallen
			foreach (GameObject go in allObjects) {
				if (go.transform.position.y < -250 && go.tag != TagManagement.pin && go.layer == 2) {
					Destroy (go);
				}
			}
			if (Camera.main.GetComponent<FollowCar> ().lastCar != null) {
				lastCarPosition = Camera.main.GetComponent<FollowCar> ().lastCar.transform.position;
			}
			for (int i = 0; i < roadBlocks.Length; i++) {
				float distanceToLastCar = 0;
				distanceToLastCar = roadBlocks [i].transform.position.z - lastCarPosition.z;
				if (distanceToLastCar < maxDistAway && !Camera.main.GetComponent<FollowCar> ().inPinArea) {
					if (roadBlocks [i] != null && !Camera.main.GetComponent<CarMangment> ().trueGameOver) {
						Destroy (roadBlocks [i]);
					}
				}
				if (carStill) { 
					if (i % 3 == 0) {
						Camera.main.GetComponent<AllBlockAttributes> ().spawnEvilCar (roadBlocks [i], 10);
					}
				}
			}
		}
	}
}
