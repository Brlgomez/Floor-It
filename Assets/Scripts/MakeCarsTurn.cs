using UnityEngine;
using System.Collections;

public class MakeCarsTurn : MonoBehaviour {

	float time;
	float timeReset;
	float randomAngle;
	float turningTime;
	float aiTurnCount;
	static float aiTurnLimit = 0.1f;

	static int timeResetMin = 5;
	static int timeResetMax = 10;
	static float randomAngleMin = -0.002f;
	static float randomAngleMax = 0.002f;
	static float minTurningTime = 1.0f;
	static float maxTurningTime = 4.0f;
	static float maxAngle = 0.4f;
	static float maxDiffAngle = 0.04f;

	float turnSpeed;
	bool needsToBeRecalibrated = false;

	public bool leftButtonPressed = false;
	public bool rightButtonPressed = false;
	string level;

	void Start () {
		resestValues ();
		level = Camera.main.GetComponent<LevelManagement>().level;
		turnSpeed = Camera.main.GetComponent<CarMangment>().carAutoSteering;
	}
	
	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			if (level == LevelManagement.floorIt) {
				if (turnSpeed == 0) {
					turnSpeed = Camera.main.GetComponent<CarMangment>().carAutoSteering;
				}
				autoRotate ();
			} else if (level == LevelManagement.drive) {
				manualRotate ();
			}
		}
	}

	void autoRotate() {
		time += Time.deltaTime;
		if (time > timeReset + turningTime) {
			resestValues ();
		}
		if ((time > timeReset) && (time < (timeReset + turningTime))) {
			if (Camera.main.GetComponent<CarMangment> ().cars [0] != null) {
				float rotationCurr = Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation.y;
				if (Mathf.Abs (rotationCurr) > maxAngle) {
					needsToBeRecalibrated = true;	
				} else {
					needsToBeRecalibrated = false;
				}
				if (needsToBeRecalibrated) {
					recalibrateRotation ();
				} else {
					regularRotation ();
				}
			}
		}
		if (Camera.main.GetComponent<CarMangment> ().cars.Length > 1) {
			aiTurnCount += Time.deltaTime;
			if (aiTurnCount > aiTurnLimit) {
				aiTurnCount = 0;
				lookAtLeadCar ();
			}
		}
	}

	void resestValues () {
		time = 0;
		timeReset = Random.Range (timeResetMin, timeResetMax);
		randomAngle = Random.Range (randomAngleMin, randomAngleMax);
		turningTime = Random.Range (minTurningTime, maxTurningTime);
	}

	void regularRotation () {
		Quaternion newRoation;
		newRoation = new Quaternion (
			Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation.x, 
			Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation.y + randomAngle, 
			Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation.z, 
			Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation.w);
		Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation = Quaternion.Slerp (
			Camera.main.GetComponent<CarMangment> ().cars [0].transform.rotation, 
			newRoation, 
			Time.deltaTime * turnSpeed
		);
	}

	void recalibrateRotation () {
		for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
			Quaternion newRoation = new Quaternion (
				Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.x, 
				0, 
				Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.z, 
				Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.w
			);
			Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation = Quaternion.Slerp (
				Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation, 
				newRoation, 
				Time.deltaTime
			);
		}
	}

	void manualRotate() {
		if ((Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) || leftButtonPressed) {
			turnLeft ();
		}
		if ((Input.GetButton ("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) || rightButtonPressed) {
			turnRight ();
		}
		if (Camera.main.GetComponent<CarMangment> ().cars.Length > 1) {
			aiTurnCount += Time.deltaTime;
			if (aiTurnCount > aiTurnLimit) {
				aiTurnCount = 0;
				lookAtLeadCar ();
			}
		}
	}

	void lookAtLeadCar(){
		GameObject leadCar = GameObject.FindGameObjectsWithTag (TagManagement.car) [0];
		for (int i = 1; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
			GameObject aiCar = Camera.main.GetComponent<CarMangment> ().cars [i];
			if (aiCar != null && !(Vector3.Dot (aiCar.transform.up, Vector3.down) > -0.50f)) {
				if (aiCar.transform.position.z < leadCar.transform.position.z) {
					if (Mathf.Abs (aiCar.transform.rotation.y - leadCar.transform.rotation.y) > maxDiffAngle) {
						Vector3 targetPosition = leadCar.transform.position;
						targetPosition.y = aiCar.transform.position.y;
						Quaternion targetRotation = Quaternion.LookRotation (targetPosition - aiCar.transform.position);
						aiCar.transform.rotation = Quaternion.Slerp (
							aiCar.transform.rotation, 
							targetRotation, 
							Time.deltaTime * turnSpeed / 7.5f
						);
					}
				}
			}
		}
	}

	public void turnLeft(){
		GameObject leadCar = GameObject.FindGameObjectsWithTag(TagManagement.car)[0];
		if (!leadCar.GetComponent<CarMovement>().carFlipped) {
			float turnPos = -Camera.main.GetComponent<CarMangment>().carManSteering * Time.deltaTime;
			if (leadCar.transform.rotation.w < 0) {
				turnPos *= -1;
			}
			float newY = leadCar.transform.rotation.y + turnPos;
			newY = Mathf.Clamp (newY, -0.65f, 0.65f);
			Quaternion newRotation = new Quaternion (
				leadCar.transform.rotation.x,
				newY,
				leadCar.transform.rotation.z,
				leadCar.transform.rotation.w
			);
			leadCar.transform.rotation = newRotation;
		}
	}

	public void turnRight(){
		GameObject leadCar = GameObject.FindGameObjectsWithTag(TagManagement.car)[0];
		if (!leadCar.GetComponent<CarMovement>().carFlipped) {
			float turnPos = Camera.main.GetComponent<CarMangment>().carManSteering * Time.deltaTime;
			if (leadCar.transform.rotation.w < 0) {
				turnPos *= -1;
			}
			float newY = leadCar.transform.rotation.y + turnPos;
			newY = Mathf.Clamp (newY, -0.65f, 0.65f);
			Quaternion newRotation = new Quaternion (
				leadCar.transform.rotation.x,
				newY,
				leadCar.transform.rotation.z,
				leadCar.transform.rotation.w
			);
			leadCar.transform.rotation = newRotation;
		}
	}
}
