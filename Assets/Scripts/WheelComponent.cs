using UnityEngine;
using System.Collections;

public class WheelComponent : MonoBehaviour {

	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBL;
	public WheelCollider wheelBR;

	public float speed;
	public float acceleration;

	void Start () {
		if (speed == 0) {
			speed = 350f;
		}
	}
	
	void FixedUpdate () {
		wheelBL.motorTorque = Time.deltaTime * speed;
		wheelBR.motorTorque = Time.deltaTime * speed;

		wheelFL.steerAngle = Input.GetAxis ("Horizontal") * Time.deltaTime * 2000;
		wheelFR.steerAngle = Input.GetAxis ("Horizontal") * Time.deltaTime * 2000;

		speed += Time.deltaTime * 10;
	}
}
