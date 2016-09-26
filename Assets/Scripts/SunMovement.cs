using UnityEngine;
using System.Collections;

public class SunMovement : MonoBehaviour {

	static float dayLength = 2f;
	float rotationSpeed;

	void Update () {
		rotationSpeed = Time.deltaTime / dayLength;
		transform.Rotate (0, rotationSpeed, 0);
	}
}