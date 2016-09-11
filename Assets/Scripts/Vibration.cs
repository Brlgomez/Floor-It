using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {

	int playVibration;

	void Start () {
		playVibration = PlayerPrefs.GetInt ("Play Vibrations", 0);	
	}
	
	public void vibrate(){
		if (playVibration == 0) {
			Handheld.Vibrate();
		}
	}
}
