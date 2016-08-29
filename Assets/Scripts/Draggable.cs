﻿// base code from: http://www.theappguruz.com/blog/drag-and-drop-any-game-object

using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	GameObject highlight;
	GameObject target;
	bool isMouseDrag;
	Vector3 screenPosition;
	Vector3 offset;
	public bool inPause;
	bool gotDragged;
	int yPosition = -1;

	void Start () {
		highlight = GameObject.Find ("Highlight");
		inPause = false;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && !inPause) {
			// initial touching an object or the invisible board
			if (Input.GetMouseButtonDown (0)) {
				mouseDown ();
			}
			// letting go of the object
			if (Input.GetMouseButtonUp (0)) {
				mouseUp ();
			}
			// dragging the object
			if (isMouseDrag) {
				mouseDrag ();
			}
		}
	}

	// initially touching an object or moving the object directly to the road
	void mouseDown () {
		RaycastHit hitInfo;
		target = returnClickedObject (out hitInfo);
		if (target != null) {
			isMouseDrag = true;
			if (target.tag.Equals ("On hud") || target.tag.Equals ("Moveable")) {
				Camera.main.GetComponent<AddBlock> ().touchedPiece ();
			}
			if (target.tag.Equals ("On road")) {
				target.tag = "Picked Up";
			}
			// convert world position to screen position
			screenPosition = Camera.main.WorldToScreenPoint (target.transform.position);
			Vector3 screen = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
			offset = target.transform.position - Camera.main.ScreenToWorldPoint (screen);
		}
	}

	// letting go of the dragged object
	void mouseUp () {
		isMouseDrag = false;
		if (target != null) {
			if (target.tag == "Selected") {
				Camera.main.GetComponent<AddBlock> ().addNewPiece ();
			}
			if (target.tag.Equals ("Picked Up")) {
				target.tag = "On road";
			}
			target.transform.position = roundVector (target.transform.position);
			Camera.main.GetComponent<SoundEffects> ().playeDropBlockSound (target.transform.position);
			highlight.transform.position = new Vector3 (0, yPosition, -10);
		}
	}

	// dragging the object
	void mouseDrag () {
		target.tag = "Picked Up";
		// track mouse position.
		Vector3 currentScreenSpace = new Vector3 (
			Input.mousePosition.x, 
			Input.mousePosition.y, 
			screenPosition.z
		);
		// convert screen position to world position with offset changes.
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenSpace) + offset;
		// will update target gameobject's rounded current postion.
		currentPosition = roundVector (currentPosition);
		// will create a sphere and will determine if object can be dragged
		// if no object is where the mouse it, the object will drag to it
		Vector3 spawnPos = new Vector3 (currentPosition.x, -2f, currentPosition.z);
		float radius = 0.5f;
		Vector3 targetPos = target.transform.position;
		if (!Physics.CheckSphere (spawnPos, radius) || (spawnPos.x == targetPos.x && spawnPos.z == targetPos.z)) {
			highlight.transform.position = new Vector3 (currentPosition.x, yPosition, currentPosition.z);
			target.transform.position = new Vector3 (currentPosition.x, yPosition, currentPosition.z);
		}
	}

	// return clicked object unless car then the object is null and car jumps
	GameObject returnClickedObject (out RaycastHit hit) {
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			target = hit.collider.gameObject;
			if (target.tag.Equals ("Invisible Floor")) {
				addNewPieceByClick (hit.point);
				Camera.main.GetComponent<SoundEffects> ().playeDropBlockSound (hit.point);
			} else if (target.tag.Equals ("Car")) {
				//target.GetComponent<CarMovement> ().jump ();
			} else if (target.tag.Equals ("Moveable") || target.tag.Equals ("On road") || target.tag.Equals ("On hud")) {
				return target;
			} 
		}
		return null;
	}
		
	// add a new piece to the board with a click and that wasn't from the hud block
	void addNewPieceByClick (Vector3 hit) {
		Camera.main.GetComponent<AddBlock> ().touchedPiece ();
		GameObject hudObject = Camera.main.GetComponent<AddBlock> ().hudBlock;
		hit = roundVector (hit);
		hudObject.transform.position = new Vector3 (hit.x, yPosition, hit.z);
		Camera.main.GetComponent<AddBlock> ().addNewPiece ();
	}

	Vector3 roundVector (Vector3 position) {
		int x = 0;
		int z = 0;
		if (position.x > 0) {
			x = Mathf.CeilToInt (position.x);
		} else {
			x = Mathf.FloorToInt (position.x);
		}
		x = nearestMultipleOf (x, 2);
		z = Mathf.CeilToInt (position.z);
		z = nearestMultipleOf (z, 2);
		return new Vector3 (x,yPosition, z);
	}

	// returns the nearest multiple of x
	int nearestMultipleOf (int x, int multiple) {
		int mod = x % multiple;
		float midPoint = multiple / 2.0f;
		if (mod > midPoint) {
			return x + (multiple - mod);
		} else {
			return x - mod;
		}
	}
}
