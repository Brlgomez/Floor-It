using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	float totalTime;
	int lifetime = 5;

	void Update () {
		totalTime += Time.deltaTime;
		transform.Translate (Vector3.forward * Time.deltaTime * 7.5f);
		if (totalTime > lifetime) {
			Destroy (gameObject);
		}
	}
}
