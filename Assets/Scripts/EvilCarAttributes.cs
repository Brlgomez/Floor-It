using UnityEngine;
using System.Collections;

public class EvilCarAttributes : MonoBehaviour {

	ParticleSystem smoke;
	GameObject invisibleFloor;
	static float maxDiffAngle = 0.05f;
	float aiTurnCount;
	static float aiTurnLimit = 0.1f;
	static float explodedDist = 1.25f;

	static float radius = 1.75f;
	static int power = 750;
	static int upwardForce = 50;
	bool exploded;
	bool particlePlayed;
	public bool explodeNow;

	GameObject leadCar = null;

	void Start () {
		invisibleFloor = GameObject.Find ("InvisibleFloor");
		smoke = gameObject.GetComponent<ParticleSystem> ();
		transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
		GetComponent<Rigidbody> ().useGravity = true;
		exploded = false;
		particlePlayed = false;
		explodeNow = false;
	}
	
	void Update () {
		aiTurnCount += Time.deltaTime;
		if (aiTurnCount > aiTurnLimit && !GetComponent<CarMovement>().gameOver) {
			aiTurnCount = 0;
			lookAtLeadCar ();
		}
		if (leadCar != null && !exploded) {
			if (Vector3.Distance (transform.position, leadCar.transform.position) < (explodedDist * transform.localScale.x) || GetComponent<CarMovement> ().gameOver || explodeNow) {
				explosionForce ();
				smoke.Play ();
				ParticleSystem.EmissionModule em = smoke.emission;
				Camera.main.GetComponent<SoundEffects> ().playExplosionSound (transform.position);
				ChangeMaterial (invisibleFloor.GetComponent<Renderer>().material);
				GetComponent<Collider> ().enabled = false;
				em.enabled = true;
				exploded = true;
				particlePlayed = true;
				int playVibration = PlayerPrefs.GetInt ("Play Vibrations", 0);
				if (playVibration == 0) {
					Handheld.Vibrate();
				}
			}
		}
		if (!smoke.isPlaying && particlePlayed) {
			Camera.main.GetComponent<Points> ().incrementPoints (5);
			Destroy (gameObject);
		}
	}

	void ChangeMaterial(Material newMat) {
		Renderer rend = GetComponent<Renderer>();
		Material[] mats = new Material[rend.materials.Length];
		for (int j = 0; j < rend.materials.Length; j++) {
			mats[j] = newMat; 
		}
		rend.materials = mats;
	}


	void lookAtLeadCar(){
		if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
			if (!GetComponent<CarMovement> ().carFlipped && GetComponent<CarMovement>().evilCarWithinRange) {
				leadCar = Camera.main.GetComponent<FollowCar> ().leadCar;
				if (Mathf.Abs (transform.rotation.y - leadCar.transform.rotation.y) > maxDiffAngle) {
					Vector3 targetPosition = leadCar.transform.position;
					targetPosition.y = transform.position.y;
					Quaternion targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
					transform.rotation = Quaternion.Slerp (
						transform.rotation, 
						targetRotation, 
						Time.deltaTime * 7
					);
				}
			}
		}
	}

	void explosionForce(){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null) {
				rb.AddExplosionForce (
					power * transform.localScale.x,
					explosionPos * transform.localScale.x, 
					radius * transform.localScale.x, 
					upwardForce * transform.localScale.x
				);
			}
		}
	}
}
