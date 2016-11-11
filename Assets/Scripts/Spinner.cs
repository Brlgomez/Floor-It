using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	int speed = 0;

	void Start () {
		speed = Random.Range (25, 75);
		if (speed % 2 == 0) {
			speed *= -1;
		}
	}

	void Update () {
		transform.Rotate (0, Time.deltaTime * speed, 0);
	}
}
