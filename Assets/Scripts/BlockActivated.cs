using UnityEngine;
using System.Collections;

public class BlockActivated : MonoBehaviour {

	Material newMaterialRef;
	public bool hasActivated = false;
	public bool isTransparent = false;

	void Start() {
		newMaterialRef = GameObject.Find (AllBlockNames.standardBlock).GetComponent<Renderer> ().material;
	}

	public void activated(){
		if (!hasActivated && GetComponent<Renderer> ().material != newMaterialRef && !isTransparent) {
			GetComponent<Renderer> ().material = newMaterialRef;
		}
		hasActivated = true;
	}
}
