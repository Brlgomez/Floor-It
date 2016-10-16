using UnityEngine;
using System.Collections;

public class interfaceMainMenuMovement : MonoBehaviour {

	public GameObject levelSelect;

	Vector3 levelSelectPos = new Vector3 (-400, 0, 0);

	Vector3 middleOfScreen;

	float deltaTime;

	// Use this for initialization
	void Start () {
		middleOfScreen = new Vector3 (Screen.width/2, Screen.height/2, 0);
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime;
		if (Camera.main.GetComponent<InterfaceMainMenu>().viewLevelSelect == true) {
			levelSelect.transform.position = Vector3.MoveTowards (levelSelect.transform.position, Vector3.zero, deltaTime * 100);
		} else {
			levelSelect.transform.position = Vector3.MoveTowards (levelSelect.transform.position, levelSelectPos, deltaTime * 100);
		}
	}
}
