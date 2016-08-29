using UnityEngine;
using System.Collections;

public class SizeBlockAttributes : MonoBehaviour {

	public Material smallMaterial;
	public bool big = true;

	void Start () {
		if (Random.Range (0, 10) < 3) {
			GetComponent<Renderer> ().material = smallMaterial;
			big = false;
		} else {
			big = true;
		}
	}
}

