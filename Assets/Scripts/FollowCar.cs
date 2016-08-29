using UnityEngine;
using System.Collections;

public class FollowCar : MonoBehaviour {

	public GameObject leadCar;
	public GameObject lastCar;
	float yPositionOfCam;
	float xPositionOfCam;
	float pinZPosition;
	public bool inPinArea;

	string level;

	void Start (){
		level = Camera.main.GetComponent<LevelManagement>().level;
		if (level == LevelManagement.bowl) {
			pinZPosition = GameObject.Find ("Pins").transform.position.z;
		}
		inPinArea = false;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			GameObject[] aliveCars = Camera.main.GetComponent<CarMangment> ().cars;
			float tempX = getXPositionOfCam (aliveCars);
			if (tempX != 0) {
				xPositionOfCam = tempX;
			}
			for (int i = 0; i < aliveCars.Length; i++) {
				leadCar = getLeadCar (leadCar, aliveCars [i]);
				lastCar = getLastCar (lastCar, aliveCars [i]);
			}
			if (leadCar != null) {
				yPositionOfCam = getYPositionOfCam (leadCar, lastCar);
				int yPosShift = 10;
				if (Screen.width < Screen.height) {
					yPositionOfCam = yPositionOfCam + 5;
					yPosShift = 15;
				}
				Vector3 end = new Vector3 (
					xPositionOfCam + transform.rotation.y * 1000, 
					yPositionOfCam, 
					leadCar.transform.position.z + 2 - ((yPositionOfCam - yPosShift) / 2)
				);
				if (level == LevelManagement.bowl && leadCar.transform.position.z > pinZPosition - 5) {
					end = new Vector3 (0, 20, pinZPosition);
					inPinArea = true;
				} 

				float speed = leadCar.GetComponent<CarMovement> ().speed;
				transform.position = Vector3.Lerp (
					transform.position, 
					end, Time.deltaTime * speed
				);
			}
		}
	}

	GameObject getLeadCar (GameObject a, GameObject b) {
		if (a != null && b != null) {
			if (!a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				if (a.transform.position.z >= b.transform.position.z) {
					return a;
				} else {
					return b;
				}
			}
			if (!a.GetComponent<CarMovement> ().gameOver && b.GetComponent<CarMovement> ().gameOver) {
				return a;
			}
			if (a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				return b;
			}
			return null;
		}
		if (a == null) {
			return b;
		} else {
			return a;
		}
	}

	public GameObject getLastCar (GameObject a, GameObject b) {
		if (a != null && b != null) {
			if (!a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				if (a.transform.position.z <= b.transform.position.z) {
					return a;
				} else {
					return b;
				}
			}
			if (!a.GetComponent<CarMovement> ().gameOver && b.GetComponent<CarMovement> ().gameOver) {
				return a;
			}
			if (a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				return b;
			}
			return null;
		}
		if (a == null) {
			return b;
		} else {
			return a;
		}
	}

	float getYPositionOfCam (GameObject a, GameObject b) {
		float yPos = Mathf.Abs(a.transform.position.z - b.transform.position.z) + 10;
		if (yPos <= 10) {
			return 10.0f;
		} else {
			return yPos;
		}
	}

	float getXPositionOfCam (GameObject[] aliveCars) { 
		int amountOfAliveCars = 0;
		float xPosCombined = 0;

		for (int i = 0; i < aliveCars.Length; i++) {
			if (aliveCars[i] != null && !aliveCars [i].GetComponent<CarMovement> ().gameOver) {
				xPosCombined += aliveCars [i].transform.position.x;
				amountOfAliveCars++;
			}
		}
		if (amountOfAliveCars != 0) {
			return xPosCombined / amountOfAliveCars;
		} else {
			return 0;
		}
	}
}
	