using UnityEngine;
using System.Collections;

public class BlockManagment : MonoBehaviour {

	float timerCount;
	float timerLimit;
	float maxDistAway;
	Vector3 lastCarPosition;

	void Start () {
		lastCarPosition = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
		maxDistAway = -8.5f;
		timerLimit = 5;
	}
	
	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && 
			!Camera.main.GetComponent<FollowCar> ().inPinArea) {
			timerCount += Time.deltaTime;
			if (timerCount > timerLimit) {
				timerCount = 0;
				GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag ("On road");
				if (Camera.main.GetComponent<FollowCar> ().lastCar != null) {
					lastCarPosition = Camera.main.GetComponent<FollowCar> ().lastCar.transform.position;
				}
				for (int i = 0; i < roadBlocks.Length; i++) {
					float distanceToLastCar = 0;
					if (roadBlocks[i].transform.position.y != -1) {
						roadBlocks[i].transform.position = new Vector3 (
							roadBlocks[i].transform.position.x, 
							-1, 
							roadBlocks[i].transform.position.z
						);
					}
					distanceToLastCar = roadBlocks[i].transform.position.z - lastCarPosition.z;
					if (distanceToLastCar < maxDistAway) {
						if (roadBlocks [i] != null) {
							Destroy (roadBlocks [i]);
						}
					}
				}
			}
		}
	}
}
