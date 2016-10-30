using UnityEngine;
using System.Collections;

public class BlockActivated : MonoBehaviour {

	Material newMaterialRef;
	public bool hasActivated = false;
	public bool isTransparent = false;


	void Start() {
		newMaterialRef = GameObject.Find (AllBlockNames.standardBlock).GetComponent<Renderer> ().material;
	}

	public void activated(bool changeMaterial){
		if (!hasActivated && !isTransparent && GetComponent<Renderer> ().material != newMaterialRef && changeMaterial) {
			GetComponent<Renderer> ().material = newMaterialRef;
		}
		hasActivated = true;
		GetComponentInChildren<ParticleSystem> ().Play ();
	}
}
