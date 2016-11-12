using UnityEngine;
using System.Collections;

public class Cylinder : MonoBehaviour {

	int speed = 0;

	void Start () {
		speed = Random.Range (25, 75);
	}

	void Update () {
		transform.Rotate (Time.deltaTime * -speed, 0 , 0);
	}

	public int getSpeed () {
		return speed;
	}

	void OnCollisionStay (Collision hit) {
		Vector3 rotationDirection = new Vector3 (0, 0, (speed / 25));
		hit.transform.position = (hit.transform.position + rotationDirection * Time.deltaTime); 
	}
}
