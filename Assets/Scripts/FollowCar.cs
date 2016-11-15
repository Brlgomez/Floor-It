using UnityEngine;
using System.Collections;

public class FollowCar : MonoBehaviour {

	public GameObject leadCar;
	public GameObject lastCar;
	GameObject leftMostCar;
	GameObject rightMostCar;
	float yPositionOfCam;
	float xPositionOfCam;
	float pinZPosition;
	public bool inPinArea;
	string level;
	float timeLapsed;
	static float timeLimit = 0.075f;
	static float cameraHeight = 15;

	void Start (){
		level = Camera.main.GetComponent<LevelManagement>().level;
		if (level == LevelManagement.bowl) {
			pinZPosition = GameObject.Find ("Pins").transform.position.z;
		}
		inPinArea = false;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			timeLapsed += Time.deltaTime;
			if (timeLapsed > timeLimit) {
				timeLapsed = 0;
				GameObject[] aliveCars = Camera.main.GetComponent<CarMangment> ().cars;
				float tempX = getXPositionOfCam (aliveCars);
				if (tempX != 0) {
					xPositionOfCam = tempX;
				}
				if (aliveCars.Length == 1) {
					leadCar = aliveCars [0];
					lastCar = aliveCars [0];
					leftMostCar = aliveCars [0];
					rightMostCar = aliveCars [0];
				} else {
					for (int i = 0; i < aliveCars.Length; i++) {
						leadCar = getLeadCar (leadCar, aliveCars [i]);
						lastCar = getLastCar (lastCar, aliveCars [i]);
						leftMostCar = getLeftMostCar (leadCar, aliveCars [i]);
						rightMostCar = getRightMostCar (leadCar, aliveCars [i]);
					}
				}
			}
			if (leadCar != null) {
				yPositionOfCam = getYPositionOfCam (leadCar, lastCar, leftMostCar, rightMostCar);
				Vector3 end = new Vector3 (
					xPositionOfCam, 
					yPositionOfCam, 
					leadCar.transform.position.z - ((yPositionOfCam - cameraHeight) / 2) - 1
				);
				if (level == LevelManagement.bowl && leadCar.transform.position.z > pinZPosition - 6) {
					end = new Vector3 (0, 25, pinZPosition - 6);
					inPinArea = true;
				} 
				float distanceFromLead = Vector2.Distance (
					new Vector2 (transform.position.x, transform.position.z), 
					new Vector2 (leadCar.transform.position.x, leadCar.transform.position.z)
				);
				distanceFromLead = Mathf.Clamp (distanceFromLead, 1, 10);
				transform.position = Vector3.Slerp (
					transform.position, 
					end, Time.deltaTime * distanceFromLead
				);
			}
			if (level == LevelManagement.bowl && transform.rotation.x < 0.61f) {
				Quaternion newRotation = new Quaternion (0.65f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime / 2);
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

	GameObject getLeftMostCar (GameObject a, GameObject b) {
		if (a != null && b != null) {
			if (!a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				if (a.transform.position.x <= b.transform.position.x) {
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

	public GameObject getRightMostCar (GameObject a, GameObject b) {
		if (a != null && b != null) {
			if (!a.GetComponent<CarMovement> ().gameOver && !b.GetComponent<CarMovement> ().gameOver) {
				if (a.transform.position.x >= b.transform.position.x) {
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

	float getYPositionOfCam (GameObject a, GameObject b, GameObject c, GameObject d) {
		float zPosDiff = Mathf.Abs (a.transform.position.z - b.transform.position.z) + cameraHeight;
		float xPosDiff = Mathf.Abs (c.transform.position.x - d.transform.position.x) + cameraHeight;
		if (zPosDiff <= cameraHeight && xPosDiff <= cameraHeight) {
			return cameraHeight;
		} else {
			if (xPosDiff > zPosDiff) {
				return xPosDiff;
			} else {
				return zPosDiff;
			}
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
	