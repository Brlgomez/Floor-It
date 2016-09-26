using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {

	int playVibration;

	void Start () {
		playVibration = PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0);	
	}

	public void vibrate () {
		if (playVibration == 0) {
			// comment this out before porting to PC
			Handheld.Vibrate ();
		}
	}
}
