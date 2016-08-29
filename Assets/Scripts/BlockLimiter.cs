using UnityEngine;
using System.Collections;

public class BlockLimiter : MonoBehaviour {

	Color colorStart;
	Color colorEnd;
	float totalAmount;
	float totalLimit;
	public bool isTransparent;
	public bool disabled;
	Renderer rend;

	void Start () {
		totalLimit = 2;
		isTransparent = false;
		disabled = false;
		colorStart = GetComponent<Renderer> ().material.color;
		colorEnd = new Color (0, 0, 0);
		rend = GetComponent<Renderer>();
	}
		
	public void incrementTotal(){
		totalAmount += Time.deltaTime;
		if (!isTransparent && totalAmount < totalLimit) {
			rend.material.color = Color.Lerp (colorStart, colorEnd, totalAmount / totalLimit);
		}
		if (totalAmount >= totalLimit) {
			disabled = true;
		}
	}
}
