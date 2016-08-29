using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class PauseButton :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public void OnPointerEnter (PointerEventData eventData) {
		Camera.main.GetComponent<Draggable> ().inPause = true;
	}

	public void OnPointerExit (PointerEventData eventData) {
		Camera.main.GetComponent<Draggable> ().inPause = false;
	}
}
