using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	public float total;	
	public int highscoreInfinite;
	public int highscoreBowling;
	public int highscoreDriving;
	public bool newHighScore;
	public float highestMulti;
	public float multiplier;
	string level;
	bool checkHighScore;
	float timerCount;
	float timerLimit;
	float aliveCars;
	static float multiplierLimit = 10;
	public float multiplierCount;
	bool checkedNoBlockAchievement = false;

	void Start(){
		total = 0;
		highscoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highscoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highscoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		newHighScore = false;
		highestMulti = 0;
		level = Camera.main.GetComponent<LevelManagement>().level;
		checkHighScore = false;
		aliveCars = 1;
		timerLimit = Camera.main.GetComponent<CarMangment> ().pointSpeed;
		multiplier = 1;
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver) {
			if (timerLimit == 0) {
				timerLimit = Camera.main.GetComponent<CarMangment> ().pointSpeed;
			}
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
			if (multiplier > 1) {
				multiplierCount += Time.deltaTime;
				if (multiplierCount > multiplierLimit) {
					multiplier = 1;
					multiplierCount = 0;
					Camera.main.GetComponent<SoundEffects> ().playMultiplierRevertSound (Camera.main.GetComponent<FollowCar>().leadCar.transform.position);
					Camera.main.GetComponent<Interface> ().multiplierOff ();
				}
			}
		}
	}

	public void checkScore () {
		if (Camera.main.GetComponent<CarMangment> ().trueGameOver && !checkHighScore) {
			if (level == LevelManagement.floorIt) {
				checkHighScore = true;
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseExp (Mathf.FloorToInt (total));
				Camera.main.GetComponent<OnlineServices> ().postScore (level, Mathf.FloorToInt(total));
				checkNoBlockAchievementAfterGame (Mathf.FloorToInt (total));
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
				float tempHighestMulti = highestMulti;
				if (tempHighestMulti == 0) {
					tempHighestMulti = 1;
				}
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseExp (Mathf.FloorToInt (total * tempHighestMulti));
				Camera.main.GetComponent<OnlineServices> ().postScore (level, Mathf.FloorToInt(total * tempHighestMulti));
				checkNoBlockAchievementAfterGame (Mathf.FloorToInt (total * tempHighestMulti));
				if (highestMulti >= 10) {
					Camera.main.GetComponent<OnlineServices> ().revealStrikeAchievement ();
				}
				if (Camera.main.GetComponent<AllBlockAttributes> ().blockActivated == 0 && Camera.main.GetComponent<FollowCar>().inPinArea) {
					Camera.main.GetComponent<OnlineServices> ().revealNoActivationBowlAchievement ();
				}
				if (Mathf.FloorToInt (total * tempHighestMulti) > highscoreBowling) {
					newHighScore = true;
					highscoreBowling = Mathf.FloorToInt (total * tempHighestMulti);
					PlayerPrefs.SetInt (PlayerPrefManagement.highScoreBowl, highscoreBowling);
					PlayerPrefs.Save ();
					Camera.main.GetComponent<SoundEffects> ().playHighScoreSound ();	
				} else {
					Camera.main.GetComponent<SoundEffects> ().stopMusic ();
				}
			} else if (level == LevelManagement.drive) {
				checkHighScore = true;
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseExp (Mathf.FloorToInt (total));
				Camera.main.GetComponent<OnlineServices> ().postScore (level, Mathf.FloorToInt(total));
				checkNoBlockAchievementAfterGame (Mathf.FloorToInt (total));
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

	public void incrementPoints(float point, GameObject obj){
		total += point * multiplier;
		Camera.main.GetComponent<Interface> ().changePointsText (point * multiplier, obj);
		checkNoBlockAchievementDuringGame ();
	}

	public void incrementPointsCars(float point){
		total += point * multiplier;
		Camera.main.GetComponent<Interface> ().changeCarPointsText (point * multiplier);
		checkNoBlockAchievementDuringGame ();
	}

	void checkNoBlockAchievementDuringGame () {
		if (total >= 250 && Camera.main.GetComponent<AllBlockAttributes>().blockActivated == 0 && !checkedNoBlockAchievement) {
			Camera.main.GetComponent<OnlineServices> ().revealNoActivationAchievement ();
			checkedNoBlockAchievement = true;
		}
	}

	void checkNoBlockAchievementAfterGame (int total) {
		if (total >= 250 && Camera.main.GetComponent<AllBlockAttributes>().blockActivated == 0 && !checkedNoBlockAchievement) {
			Camera.main.GetComponent<OnlineServices> ().revealNoActivationAchievement ();
		}
	}
}
