using UnityEngine;
using System.Collections;

public class Hydraulic : MonoBehaviour {

	bool enlarge = true;
	int maxSize;
	Vector3 enlarged;

	void Start () {
		maxSize = Random.Range (100, 250);
		enlarged = new Vector3 (1, maxSize, 1);
		transform.localScale = new Vector3 (1, Random.Range (1, maxSize), 1);
		if (maxSize % 2 == 0) {
			enlarge = false;
		}
	}
	
	void Update () {
		if (enlarge) {
			transform.localScale = Vector3.Lerp (transform.localScale, enlarged, Time.deltaTime * 4);
			if (transform.localScale.y > maxSize - 0.01f) {
				enlarge = false;
				Camera.main.GetComponent<SoundEffects> ().playHydraulicDownSound (transform.position);
			}
		} else {
			transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one, Time.deltaTime * 5);
			if (transform.localScale.y < 1.01f) {
				enlarge = true;
				Camera.main.GetComponent<SoundEffects> ().playHydraulicUpSound (transform.position);
			}
		}
	}
}
