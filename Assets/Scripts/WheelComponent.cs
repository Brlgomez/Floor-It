using UnityEngine;
using System.Collections;

public class WheelComponent : MonoBehaviour {

	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBL;
	public WheelCollider wheelBR;

	void Start () {
		
	}
	
	void FixedUpdate () {
		wheelBL.motorTorque = Time.deltaTime * 400;
		wheelBR.motorTorque = Time.deltaTime * 400;

		wheelFL.steerAngle = Input.GetAxis ("Horizontal") * Time.deltaTime * 2000;
		wheelFR.steerAngle = Input.GetAxis ("Horizontal") * Time.deltaTime * 2000;
	}
}
