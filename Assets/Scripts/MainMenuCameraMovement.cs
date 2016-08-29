using UnityEngine;
using System.Collections;

public class MainMenuCameraMovement : MonoBehaviour {

	Vector3 lookPosition;

	void Start(){
		lookPosition = new Vector3 (0, 0.25f, 0);
	}

	void Update () {
		transform.LookAt(lookPosition);
		transform.Translate(Vector3.right * Time.smoothDeltaTime);
	}
}
