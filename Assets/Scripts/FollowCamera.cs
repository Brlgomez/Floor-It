using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	float timerCount;
	float timerLimit;

	void Start () {
		timerLimit = 5;
	}
	
	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			timerCount += Time.deltaTime;
			if (timerCount > timerLimit) {
				timerCount = 0;
				transform.position = new Vector3 (
					Camera.main.transform.position.x, 
					transform.position.y, 
					Camera.main.transform.position.z
				);
			}
		}
	}
}
