using UnityEngine;
using System.Collections;

public class MakeCarsTurn : MonoBehaviour {

	float time;
	float timeReset;
	float randomAmount;
	float turningTime;

	int timeResetMin, timeResetMax;
	float randomAngleMin, randomAngleMax;
	float minTurningTime, maxTurningTime;
	float maxAngle;

	float turnSpeed;

	float rotationCurr;
	float rotationPrev;
	float maxDiffAngle;
	bool needsToBeRecalibrated;

	string level;

	public bool leftButtonPressed;
	public bool rightButtonPressed;

	float aiTurnCount;
	float aiTurnLimit = 0.1f;

	void Start () {
		time = 0;
		timeResetMin = 5;
		timeResetMax = 10;
		randomAngleMin = -0.001f;
		randomAngleMax = 0.001f;
		minTurningTime = 1.0f;
		maxTurningTime = 3.5f;
		maxAngle = 0.25f;

		turnSpeed = Camera.main.GetComponent<CarMangment>().carAutoSteering;

		timeReset = Random.Range (timeResetMin, timeResetMax);
		randomAmount = Random.Range (randomAngleMin, randomAngleMax);
		turningTime = Random.Range (minTurningTime, maxTurningTime);

		rotationPrev = 0;
		rotationCurr = 0;
		maxDiffAngle = 0.1f;
		needsToBeRecalibrated = false;

		level = Camera.main.GetComponent<LevelManagement>().level;

		leftButtonPressed = false;
		rightButtonPressed = false;
	}
	
	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			if (level == LevelManagement.floorIt) {
				autoTurnCars ();
			}
			if (level == LevelManagement.drive) {
				manualRotate ();
			}
		}
	}

	void autoTurnCars() {
		time += Time.deltaTime;
		rotationCurr = 0;
		rotationPrev = 0;
		if (time > timeReset + turningTime) {
			time = 0;
			timeReset = Random.Range (timeResetMin, timeResetMax);
			randomAmount = Random.Range (randomAngleMin, randomAngleMax);
			turningTime = Random.Range (minTurningTime, maxTurningTime);
		}
		if ((time > timeReset) && (time < (timeReset + turningTime))) {
			for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
				if (Camera.main.GetComponent<CarMangment> ().cars [i] != null) {
					rotationCurr = Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.y;
					if (Mathf.Abs (rotationCurr) > maxAngle) {
						needsToBeRecalibrated = true;	
						recalibrateRotation ();
					} else {
						needsToBeRecalibrated = false;
					}
					if (rotationPrev != 0) {
						if (Mathf.Abs (rotationCurr - rotationPrev) > maxDiffAngle) {
							needsToBeRecalibrated = true;	
							recalibrateRotation ();
						} else {
							needsToBeRecalibrated = false;
						}
					}
					rotationPrev = Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.y;
					if (!needsToBeRecalibrated) {
						Quaternion newRoation = new Quaternion (
							Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.x, 
							Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.y + randomAmount, 
							Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.z, 
							Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.w
						);
						Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation = Quaternion.Slerp (
							Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation, 
							newRoation, 
							Time.deltaTime * Camera.main.GetComponent<CarMangment>().carAutoSteering
						);
					}
				} 
			}
		}
	}

	void recalibrateRotation(){
		for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
			if (Camera.main.GetComponent<CarMangment> ().cars [i] != null) {
				Quaternion newRoation = new Quaternion (
					Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.x, 
					0, 
					Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.z, 
					Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation.w
				);
				Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation = Quaternion.Slerp (
					Camera.main.GetComponent<CarMangment> ().cars [i].transform.rotation, 
					newRoation, 
					Time.deltaTime * Camera.main.GetComponent<CarMangment>().carAutoSteering/10
				);
			}
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
		GameObject leadCar = GameObject.FindGameObjectsWithTag(TagManagement.car)[0];
		for (int i = 1; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
			GameObject aiCar = Camera.main.GetComponent<CarMangment> ().cars [i];
			if (aiCar!= null) {
				if (!(Vector3.Dot (aiCar.transform.up, Vector3.down) > -0.50f)) {
					if (Mathf.Abs (aiCar.transform.rotation.y - leadCar.transform.rotation.y) > maxDiffAngle / 2) {
						Vector3 targetPosition = leadCar.transform.position;
						targetPosition.y = aiCar.transform.position.y;
						Quaternion targetRotation = Quaternion.LookRotation (targetPosition - aiCar.transform.position);
						aiCar.transform.rotation = Quaternion.Slerp (
							aiCar.transform.rotation, 
							targetRotation, 
							Time.deltaTime * 7
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
