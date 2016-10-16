using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interfaceMainMenuMovement : MonoBehaviour {

	public GameObject levelSelect;
	public GameObject settings;
	public Image stats;
	public Image store;
	public Image bottomButtons;
	public GameObject backButton;
	public Image confirmation;
	public Image confirmationButtons;

	float deltaTime;

	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime * 5;
		if (Camera.main.GetComponent<InterfaceMainMenu>().viewLevelSelect) {
			levelSelect.transform.localScale = 
				Vector3.MoveTowards (levelSelect.transform.localScale, Vector3.one, deltaTime);
			bottomButtons.transform.localScale = 
				Vector3.MoveTowards (bottomButtons.transform.localScale, Vector3.one, deltaTime);
		} else {
			levelSelect.transform.localScale = 
				Vector3.MoveTowards (levelSelect.transform.localScale, Vector3.zero, deltaTime);
			bottomButtons.transform.localScale = 
				Vector3.MoveTowards (bottomButtons.transform.localScale, Vector3.zero, deltaTime);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu> ().viewSettings || 
			Camera.main.GetComponent<InterfaceMainMenu> ().viewStore || 
			Camera.main.GetComponent<InterfaceMainMenu> ().viewStats) {
			backButton.transform.localScale = 
				Vector3.MoveTowards (backButton.transform.localScale, Vector3.one, deltaTime);
		} else {
			backButton.transform.localScale = 
				Vector3.MoveTowards (backButton.transform.localScale, Vector3.zero, deltaTime);
		}
	
		if (Camera.main.GetComponent<InterfaceMainMenu>().viewSettings) {
			settings.transform.localScale = Vector3.MoveTowards (settings.transform.localScale, Vector3.one, deltaTime);
		} else {
			settings.transform.localScale = Vector3.MoveTowards (settings.transform.localScale, Vector3.zero, deltaTime);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewStore) {
			store.transform.localScale = Vector3.MoveTowards (store.transform.localScale, Vector3.one, deltaTime);
		} else {
			store.transform.localScale = Vector3.MoveTowards (store.transform.localScale, Vector3.zero, deltaTime);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewStats) {
			stats.transform.localScale = Vector3.MoveTowards (stats.transform.localScale, Vector3.one, deltaTime);
		} else {
			stats.transform.localScale = Vector3.MoveTowards (stats.transform.localScale, Vector3.zero, deltaTime);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewConfirmation) {
			confirmation.transform.localScale = 
				Vector3.MoveTowards (confirmation.transform.localScale, Vector3.one, deltaTime);
			confirmationButtons.transform.localScale = 
				Vector3.MoveTowards (confirmationButtons.transform.localScale, Vector3.one, deltaTime);
		} else {
			confirmation.transform.localScale = 
				Vector3.MoveTowards (confirmation.transform.localScale, Vector3.zero, deltaTime);
			confirmationButtons.transform.localScale = 
				Vector3.MoveTowards (confirmationButtons.transform.localScale, Vector3.zero, deltaTime);
		}
	}
}
