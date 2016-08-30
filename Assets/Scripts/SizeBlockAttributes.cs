using UnityEngine;
using System.Collections;

public class SizeBlockAttributes : MonoBehaviour {

	public Material smallMaterial;
	public bool big = true;

	void Awake () {
		if (Random.Range (0, 10) < 5 && name != AllBlockNames.sizeBlock) {
			GetComponent<Renderer> ().material = smallMaterial;
			big = false;
		} else {
			big = true;
		}
	}
}

