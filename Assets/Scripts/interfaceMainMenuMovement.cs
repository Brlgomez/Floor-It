using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interfaceMainMenuMovement : MonoBehaviour {

	public GameObject levelSelect;
	public GameObject settings;
	public GameObject backButton;
	public Image stats;
	public Image store;
	public Image bottomButtons;
	public Image confirmation;
	public Image confirmationButtons;
	public Text title;
	public Text exp;

	static Vector3 zero = new Vector3 (0, 1, 0);

	float deltaTime;
	public bool titleShift = false;
	bool adjustHighlight = false;
	public string titleText;

	
	// Update is called once per frame
	void Update () {
		deltaTime = Time.deltaTime * 7.5f;

		titleMovement ();

		if (Camera.main.GetComponent<InterfaceMainMenu> ().viewSettings || 
			Camera.main.GetComponent<InterfaceMainMenu> ().viewStore || 
			Camera.main.GetComponent<InterfaceMainMenu> ().viewStats) {
			shiftUp (backButton.transform);
		} else {
			shiftDown (backButton.transform);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu> ().viewStore || 
			Camera.main.GetComponent<InterfaceMainMenu>().viewLevelSelect ||
			Camera.main.GetComponent<InterfaceMainMenu>().viewConfirmation) {
			shiftUp (exp.transform);
		} else {
			shiftDown (exp.transform);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewLevelSelect) {
			shiftUp (levelSelect.transform);
			shiftUp (bottomButtons.transform);
		} else {
			shiftDown (levelSelect.transform);
			shiftDown (bottomButtons.transform);
		}
	
		if (Camera.main.GetComponent<InterfaceMainMenu>().viewSettings) {
			shiftUp (settings.transform);
		} else {
			shiftDown (settings.transform);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewStore) {
			if (store.transform.localScale.x == 1 && adjustHighlight) {
				Camera.main.GetComponent<InterfaceMainMenuTools> ().setInitialHighlightPosition (PlayerPrefs.GetInt(PlayerPrefManagement.carType, 0));
				adjustHighlight = false;
			}
			shiftUp (store.transform);
		} else {
			shiftDown (store.transform);
			adjustHighlight = true;
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewStats) {
			shiftUp (stats.transform);
		} else {
			shiftDown (stats.transform);
		}

		if (Camera.main.GetComponent<InterfaceMainMenu>().viewConfirmation) {
			shiftUp (confirmation.transform);
			shiftUp (confirmationButtons.transform);
		} else {
			shiftDown (confirmation.transform);
			shiftDown (confirmationButtons.transform);
		}
	}

	void titleMovement () {
		if (titleShift) {
			if (title.transform.localScale.x != 0) {
				title.transform.localScale = Vector3.MoveTowards (title.transform.localScale, zero, deltaTime * 2);
			}
			if (title.transform.localScale.x == 0) { 
				titleShift = false;
				title.text = titleText;
			}
		} else {
			if (title.transform.localScale.x != 1) {
				title.transform.localScale = Vector3.MoveTowards (title.transform.localScale, Vector3.one, deltaTime * 2);
			}
		}
	}

	void shiftDown (Transform obj) {
		if (obj.localScale.x != 0) {
			obj.localScale = Vector3.MoveTowards (obj.localScale, zero, deltaTime);
		}
	}

	void shiftUp (Transform obj) {
		if (obj.localScale.x != 1) {
			obj.localScale = Vector3.MoveTowards (obj.localScale, Vector3.one, deltaTime);
		}
	}
}
