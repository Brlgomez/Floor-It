using UnityEngine;
using System.Collections;

public class RigidbodySounds : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		if (collision.relativeVelocity.magnitude > 1) {
			if (name.Split('_')[0] == "Sphere") {
				Camera.main.GetComponent<SoundEffects> ().playBallBounceSound (
					collision.transform.position, collision.relativeVelocity.magnitude / 5
				);
			} else {
				Camera.main.GetComponent<SoundEffects> ().playMetalSound (
					collision.transform.position, collision.relativeVelocity.magnitude / 5
				);
			}
		}
	}
}
