using UnityEngine;
using System.Collections;

public class BlockActivated : MonoBehaviour {

	public Material newMaterialRef;
	public bool hasActivated = false;
	public bool isTransparent = false;

	public void activated(){
		if (!hasActivated && GetComponent<Renderer> ().material != newMaterialRef && !isTransparent) {
			GetComponent<Renderer> ().material = newMaterialRef;
		}
		hasActivated = true;
	}
}
