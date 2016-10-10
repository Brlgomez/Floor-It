using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {

	public void vibrate () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0) {
			// comment this out before porting to PC
			Handheld.Vibrate ();
		}
	}
}
