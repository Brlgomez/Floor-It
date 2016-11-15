using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	float totalTime;
	int lifetime = 1;
	bool exploded = false;
	ParticleSystem smoke;
	int explosionForce = 1000;
	int explosionRadius = 1;
	int explosionForceUp = 25;

	void Start () {
		smoke = gameObject.GetComponentInChildren<ParticleSystem> ();
	}

	void Update () {
		if (!exploded) {
			totalTime += Time.deltaTime;
			transform.Translate (Vector3.forward * Time.deltaTime * 7.5f);
			if (totalTime > lifetime) {
				totalTime = 0;
				Vector2 missilePos = new Vector2 (transform.position.x, transform.position.z);
				Vector2 cameraPos = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);
				if (Vector2.Distance (missilePos, cameraPos) > Camera.main.transform.position.y) {
					Destroy (gameObject);
				}
			} 
		} else if (exploded && !smoke.isPlaying) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter (Collision obj) {
		if (!exploded) {
			Camera.main.GetComponent<SoundEffects> ().playExplosionSound (transform.position);
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, explosionRadius);
			foreach (Collider hit in colliders) {
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				if (rb != null) {
					rb.AddExplosionForce (explosionForce, explosionPos, explosionRadius, explosionForceUp);
				}
			}
			smoke.Play ();
			GetComponent<Collider> ().enabled = false;
			GetComponent<Renderer> ().material = GameObject.Find ("InvisibleFloor").GetComponent<Renderer> ().material;
			exploded = true;
			if (obj.transform.tag == TagManagement.evilCar) {
				obj.transform.GetComponent<EvilCarAttributes> ().explodeNow = true;
			}
		}
	}
}
