using UnityEngine;
using System.Collections;

public class MultiplierBlockAttributes : MonoBehaviour {

	public Material timesThree;
	public bool timesTwo = true;
	public int multiplier = 2;

	void Awake () {
		if (Random.Range (0, 10) < 2 && name != AllBlockNames.multiplierBlock) {
			GetComponent<Renderer> ().material = timesThree;
			multiplier = 3;
			timesTwo = false;
		} else {
			multiplier = 2;
			timesTwo = true;
		}
	}
}
