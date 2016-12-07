using UnityEngine;
using System.Collections;

public class MainMenuCameraMovement : MonoBehaviour {

	Vector3 lookPosition;
	bool loading;

	void Start(){
		loading = false;
		lookPosition = new Vector3 (0, 0, 0);
	}

	void Update () {
		if (!loading) {
			transform.LookAt (lookPosition);
			transform.Translate (Vector3.right * Time.smoothDeltaTime);
		}
	}

	public bool getLoading () {
		return loading;
	}

	public void setLoading (bool b) {
		loading = b;
	}
}
