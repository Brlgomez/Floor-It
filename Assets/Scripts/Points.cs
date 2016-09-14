using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	public float total;	
	public int highscoreInfinite;
	public int highscoreBowling;
	public int highscoreDriving;
	public bool newHighScore;
	public float highestMulti;
	string level;
	bool checkHighScore;
	float timerCount;
	static float timerLimit = 2;
	float aliveCars;

	void Start(){
		total = 0;
		highscoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highscoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highscoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		newHighScore = false;
		highestMulti = 1;
		level = Camera.main.GetComponent<LevelManagement>().level;
		checkHighScore = false;
		aliveCars = 1;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver) {
			timerCount += Time.deltaTime;
			if (timerCount > timerLimit) {
				timerCount = 0;
				aliveCars = 0;
				for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
					if (!Camera.main.GetComponent<CarMangment> ().cars [i].GetComponent<CarMovement> ().gameOver) {
						aliveCars++;
					}
				}
				incrementPointsCars (aliveCars);
			}
		}
	}

	public void checkScore () {
		if (Camera.main.GetComponent<CarMangment> ().trueGameOver && !checkHighScore) {
			if (level == LevelManagement.floorIt) {
				checkHighScore = true;
				PlayerPrefs.SetInt (PlayerPrefManagement.exp, (PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + Mathf.FloorToInt(total)));
				if (Mathf.Floor (total) > highscoreInfinite) {
					newHighScore = true;
					highscoreInfinite = Mathf.FloorToInt (total);
					PlayerPrefs.SetInt (PlayerPrefManagement.highScoreFloorIt, highscoreInfinite);
					PlayerPrefs.Save ();
					Camera.main.GetComponent<SoundEffects> ().playHighScoreSound ();
				} else {
					Camera.main.GetComponent<SoundEffects> ().stopMusic ();
				}
			} else if (level == LevelManagement.bowl) {
				GameObject[] pins = GameObject.FindGameObjectsWithTag (TagManagement.pin);
				foreach (GameObject pin in pins) {
					if ((pin.transform.up.y < 0.5f) || (pin.transform.position.y < 0)) {
						highestMulti++;
					}
				}
				checkHighScore = true;
				PlayerPrefs.SetInt (PlayerPrefManagement.exp, (PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + Mathf.FloorToInt(total * highestMulti)));
				if (Mathf.FloorToInt (total * highestMulti) > highscoreBowling) {
					newHighScore = true;
					highscoreBowling = Mathf.FloorToInt (total * highestMulti);
					PlayerPrefs.SetInt (PlayerPrefManagement.highScoreBowl, highscoreBowling);
					PlayerPrefs.Save ();
					Camera.main.GetComponent<SoundEffects> ().playHighScoreSound ();				
				} else {
					Camera.main.GetComponent<SoundEffects> ().stopMusic ();
				}
			} else if (level == LevelManagement.drive) {
				checkHighScore = true;
				PlayerPrefs.SetInt (PlayerPrefManagement.exp, (PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + Mathf.FloorToInt(total)));
				if (Mathf.Floor (total) > highscoreDriving) {
					newHighScore = true;
					highscoreDriving = Mathf.FloorToInt (total);
					PlayerPrefs.SetInt (PlayerPrefManagement.highScoreDrive, highscoreDriving);
					PlayerPrefs.Save ();
					Camera.main.GetComponent<SoundEffects> ().playHighScoreSound ();
				} else {
					Camera.main.GetComponent<SoundEffects> ().stopMusic ();
				}
			}
		}
	}

	public void incrementPoints(float multiplier, GameObject obj){
		total += multiplier;
		Camera.main.GetComponent<Interface> ().changePointsText (multiplier, obj);
	}

	public void incrementPointsCars(float multiplier){
		total += multiplier;
		Camera.main.GetComponent<Interface> ().changeCarPointsText (multiplier);
	}
}
