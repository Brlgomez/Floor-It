using UnityEngine;
using System.Collections;

public class BombAttributes : MonoBehaviour {

	static float radius = 4.5f;
	static float power = 1000;
	static float upwardForce = 50;
	ParticleSystem smoke;
	Renderer rend;
	Color colorStart;
	Color colorEnd;
	bool exploded;
	bool particlePlayed;
	float timer;
	float timeLimit;
	public bool placed;
	public bool isBombX;
	public bool isTransparent;
	Material XBombMaterial;
	Material invisibleMaterial;
	Material clearMaterial;

	void Awake () {
		smoke = gameObject.GetComponent<ParticleSystem> ();
		if (Random.Range (0, 2) < 1 && name != AllBlockNames.bombBlock) {
			GetComponent<Renderer> ().material = GameObject.Find("XBombPlaceHolder").GetComponent<Renderer>().material;
			isBombX = true;
		} else {
			isBombX = false;
		}
		invisibleMaterial = GameObject.Find ("InvisibleFloor").GetComponent<Renderer> ().material;
		clearMaterial = GameObject.Find ("ClearBlock").GetComponent<Renderer> ().material; 
		colorStart = GetComponent<Renderer> ().material.color;
		colorEnd = new Color (
			GetComponent<Renderer> ().material.color.r, 
			0f, 
			GetComponent<Renderer> ().material.color.b
		);
		rend = GetComponent<Renderer>();
		timeLimit = 3;
		isTransparent = false;
		exploded = false;
		particlePlayed = false;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && name != AllBlockNames.bombBlock) {
			if (placed) { 
				timer+=Time.deltaTime;
				if (!isTransparent) {
					rend.material.color = Color.Lerp (colorStart, colorEnd, (timer * 1.1f) / timeLimit);
				} else {
					rend.material = clearMaterial;
				}
			}
			if (timer > timeLimit - 0.2f && !particlePlayed) {
				smoke.Play ();
				ParticleSystem.EmissionModule em = smoke.emission;
				em.enabled = true;
				particlePlayed = true;
				Camera.main.GetComponent<Points> ().incrementPoints (3, gameObject);
				Camera.main.GetComponent<SoundEffects> ().playExplosionSound (transform.position);
				explosionForce ();
			}
			if(timer > timeLimit && !exploded){
				rend.material = invisibleMaterial;
				GetComponent<Collider> ().enabled = false;
				exploded = true;
				Camera.main.GetComponent<Vibration> ().vibrate();
				if (isBombX) {
					deleteBlocksX ();
				} else if (!isBombX) {
					deleteBlocksT ();
				}
			}
			if(timer > timeLimit && !smoke.isPlaying && particlePlayed){
				Destroy (gameObject);
			}
		}
	}

	void deleteBlocksX() {
		GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag (TagManagement.blockOnRoad);
		for (int i = 0; i < roadBlocks.Length; i++) {
			if (roadBlocks [i].transform.position.x == transform.position.x - 2 && 
				roadBlocks [i].transform.position.z == transform.position.z - 2) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x - 2 && 
				roadBlocks [i].transform.position.z == transform.position.z + 2) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x + 2 && 
				roadBlocks [i].transform.position.z == transform.position.z - 2) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x + 2 && 
				roadBlocks [i].transform.position.z == transform.position.z + 2) {
				Destroy (roadBlocks [i]);
				continue;
			}
		}
	}

	void deleteBlocksT() {
		GameObject[] roadBlocks = GameObject.FindGameObjectsWithTag (TagManagement.blockOnRoad);
		for (int i = 0; i < roadBlocks.Length; i++) {
			if (roadBlocks [i].transform.position.x == transform.position.x && 
				roadBlocks [i].transform.position.z == transform.position.z - 2) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x && 
				roadBlocks [i].transform.position.z == transform.position.z + 2) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x - 2 && 
				roadBlocks [i].transform.position.z == transform.position.z) {
				Destroy (roadBlocks [i]);
				continue;
			} else if (roadBlocks [i].transform.position.x == transform.position.x + 2 && 
				roadBlocks [i].transform.position.z == transform.position.z) {
				Destroy (roadBlocks [i]);
				continue;
			}
		}
	}

	void explosionForce(){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null) {
				rb.AddExplosionForce (power, explosionPos, radius, upwardForce);
			}
		}
	}
}
