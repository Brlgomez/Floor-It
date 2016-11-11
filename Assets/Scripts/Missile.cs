using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	float totalTime;
	int lifetime = 1;

	void Update () {
		totalTime += Time.deltaTime;
		transform.Translate (Vector3.forward * Time.deltaTime * 7.5f);
		if (totalTime > lifetime) {
			totalTime = 0;
			Vector2 missilePos = new Vector2 (transform.position.x, transform.position.z);
			Vector2 cameraPos = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
			if (Vector2.Distance(missilePos, cameraPos) > Camera.main.transform.position.y) {
				Destroy (gameObject);
			}
		}
	}
}
