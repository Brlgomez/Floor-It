using UnityEngine;
using System.Collections;

public class BlockManagment : MonoBehaviour {

	float timerCount;
	static float timerLimit = 5;
	static float maxDistAway = -10f;
	Vector3 lastCarPosition;
	bool carStill = false;
	Vector3 prevLastCarPosition;

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
				if (go.transform.position.y < -200 && (go.tag != TagManagement.pin && (go.layer == 2 || go.layer == 8))) {
					Destroy (go);
				}
			}
			prevLastCarPosition = lastCarPosition;
			if (Camera.main.GetComponent<FollowCar> ().lastCar != null) {
				lastCarPosition = Camera.main.GetComponent<FollowCar> ().lastCar.transform.position;
				if (Vector3.Distance (prevLastCarPosition, lastCarPosition) < 0.5f) {
					carStill = true;
				} else {
					carStill = false;
				}
			}
			if (!Camera.main.GetComponent<CarMangment> ().trueGameOver) {
				for (int i = 0; i < roadBlocks.Length; i++) {
					float distanceToLastCar = 0;
					distanceToLastCar = roadBlocks [i].transform.position.z - lastCarPosition.z;
					if (distanceToLastCar < maxDistAway && !Camera.main.GetComponent<FollowCar> ().inPinArea) {
						if (roadBlocks [i] != null) {
							if (roadBlocks [i].name.Split ('_') [0] == AllBlockNames.chainBlock) {
								Camera.main.GetComponent<AddBlock> ().canSpawnChain = true;
								if (!roadBlocks [i].GetComponent<BlockActivated> ().hasActivated) {
									if (Camera.main.GetComponent<AllBlockAttributes> ().chainCount > 0) {
										Camera.main.GetComponent<SoundEffects> ().playChainOffSound (roadBlocks [i].transform.position);
									}
									Camera.main.GetComponent<AllBlockAttributes> ().chainCount = 0;
								}
							}
							Destroy (roadBlocks [i]);
						}
					}
					if (carStill && Camera.main.GetComponent<CarMangment>().cars.Length == 1) { 
						if (i % 3 == 0) {
							Camera.main.GetComponent<AllBlockAttributes> ().spawnEvilCar (roadBlocks [i], 10);
							if (i == 3) {
								Camera.main.GetComponent<OnlineServices> ().revealHordeAchievement ();
							}
						}
					}
				}
			}
		}
	}
}
