using UnityEngine;
using System.Collections;

public class SphereActions : MonoBehaviour {

	void Start () {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.velocity += Vector3.forward * Random.Range(-0.25f, 0.25f);
		rb.velocity += Vector3.right * Random.Range(-0.25f, 0.25f);
		transform.Rotate(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
		GetComponent<Rigidbody> ().useGravity = true;
		GetComponent<Rigidbody> ().isKinematic = false;
	}
}
