using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	int speed = 0;

	void Start () {
		speed = Random.Range (50, 200);
		if (speed % 2 == 0) {
			speed = -speed;
		}
	}

	void Update () {
		transform.Rotate (Time.deltaTime * speed, 0 , 0);
	}
}
