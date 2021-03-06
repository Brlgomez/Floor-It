﻿// base code from: http://www.theappguruz.com/blog/drag-and-drop-any-game-object

using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	GameObject highlight;
	GameObject target;
	bool isMouseDrag;
	Vector3 screenPosition;
	Vector3 offset;
	public bool onPauseButton;
	bool gotDragged;
	static int yPosition = -1;
	float dragTime = 0;
	static float dragTimeToMove = 0.1f;
	bool highlightOn = false;

	void Start () {
		highlight = GameObject.Find ("Highlight");
		onPauseButton = false;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && !onPauseButton) {
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
		dragTime = 0;
		isMouseDrag = true;
		if (target != null) {
			//isMouseDrag = true;
			if (target.tag.Equals (TagManagement.blockOnRoad)) {
				target.tag = TagManagement.pickedUp;
			}
			// convert world position to screen position
			screenPosition = Camera.main.WorldToScreenPoint (target.transform.position);
			Vector3 screen = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
			offset = target.transform.position - Camera.main.ScreenToWorldPoint (screen);
			offset = new Vector3 (offset.x, offset.y, offset.z);
		}
	}

	// letting go of the dragged object
	void mouseUp () {
		isMouseDrag = false;
		if (target != null) {
			if (target.tag.Equals (TagManagement.pickedUp)) {
				target.tag = TagManagement.blockOnRoad;
				if (highlightOn) {
					Camera.main.GetComponent<SoundEffects> ().playDropRoadBlockSound (target.transform.position);
				}
			}
			target.transform.position = roundVector (target.transform.position);
			highlight.transform.position = new Vector3 (0, yPosition, -10);
			highlightOn = false;
		}
	}

	// dragging the object
	void mouseDrag () {
		if (target != null) {
			dragTime += Time.deltaTime;
			if (dragTime > dragTimeToMove) {
				Vector3 currentPosition = target.transform.position;
				if (dragTime > dragTimeToMove * 1.25f) {
					dragTime = dragTimeToMove;
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
						if (hit.collider.tag == TagManagement.invisibleFloor) {
							currentPosition = hit.point;
						}
					}
					// will update target gameobject's rounded current postion.
					currentPosition = roundVector (currentPosition);
					// will create a sphere and will determine if object can be dragged
					// if no object is where the mouse it, the object will drag to it
					Vector3 spawnPos = new Vector3 (currentPosition.x, -1f, currentPosition.z);
					Vector3 targetPos = target.transform.position;
					highlight.transform.position = new Vector3 (currentPosition.x, yPosition, currentPosition.z);
					if (!highlightOn) {
						Camera.main.GetComponent<SoundEffects> ().playPickUpRoadBlockSound (target.transform.position);
						highlight.transform.localScale = new Vector3 (
							0.1f + target.GetComponent<Renderer> ().bounds.size.x / 2,
							0.15f + target.GetComponent<Renderer> ().bounds.size.y / 2,
							0.1f + target.GetComponent<Renderer> ().bounds.size.z / 2
						);
						highlightOn = true;
					}
					if (!Physics.CheckSphere (spawnPos, 0.5f) || (spawnPos.x == targetPos.x && spawnPos.z == targetPos.z)) {
						target.transform.position = new Vector3 (currentPosition.x, yPosition, currentPosition.z);
					}
				}
			}
		} else {
			/*
			dragTime += Time.deltaTime;
			if (dragTime > dragTimeToMove/2) {
				dragTime = 0;
				RaycastHit hitInfo;
				returnClickedObject (out hitInfo);
			}
			*/
		}
	}

	// return clicked object unless car then the object is null and car jumps
	GameObject returnClickedObject (out RaycastHit hit) {
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			target = hit.collider.gameObject;
			if (target.tag.Equals (TagManagement.invisibleFloor)) {
				Vector3 spawnPos = roundVector (hit.point);
				spawnPos = new Vector3 (spawnPos.x, -1, spawnPos.z);
				if (!Physics.CheckSphere (spawnPos, 0.5f)) {
					addNewPieceByClick (hit.point);
					Camera.main.GetComponent<SoundEffects> ().playeDropBlockSound ();
				}
			} else if (target.tag.Equals (TagManagement.moveableObject) || target.tag.Equals (TagManagement.blockOnRoad)) {
				return target;
			} 
		}
		return null;
	}
		
	// add a new piece to the board with a click and that wasn't from the hud block
	void addNewPieceByClick (Vector3 hit) {
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
