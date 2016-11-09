using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * 7.5f);
		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime/5);
		if (transform.localScale.x < 0.01f) {
			Destroy (gameObject);
		}
	}
}
