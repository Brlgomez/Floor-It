using UnityEngine;
using System.Collections;

public class MainMenuCameraMovement : MonoBehaviour {

	Vector3 lookPosition;
	public bool loading;

	void Start(){
		lookPosition = new Vector3 (0, 0, 0);
	}

	void Update () {
		if (!loading) {
			transform.LookAt (lookPosition);
			transform.Translate (Vector3.right * Time.smoothDeltaTime);
		}
	}
}
