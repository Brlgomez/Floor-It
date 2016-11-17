using UnityEngine;
using System.Collections;

public class PlayerPrefManagement : MonoBehaviour {

	// player preference
	static public string soundEffects = "Play Sound Effects";
	static public string music = "Play Music";
	static public string vibration = "Play Vibrations";
	static public string carType = "Car Type";
	static public string sudan = "sudan";
	static public string limo = "limo";
	static public string truck = "truck";
	static public string sport = "sport car";
	static public string monsterTruck = "monster truck";
	static public string cone = "cone";
	static public string bus = "bus";
	static public string abstractCar = "abstract car";
	static public string level = "Level";
	static public string exp = "Cash";
	static public string visual = "Visual Setting";
	static public string normalVisual = "normal visual";
	static public string nightVisual = "night visual";
	static public string outlineVisual = "outline visual";
	static public string pixelVisual = "classic handheld visual";
	static public string usingOnlineServices = "online services";

	// stats
	static public string highScoreFloorIt = "High Score Infinite";
	static public string highScoreBowl = "High Score Bowling";
	static public string highScoreDrive = "High Score Driving";
	static public string totalExp = "totalExp";
	static public string totalDistance = "totalDistance";
	static public string farthestDistFloorIt = "farhestDistFloorIt";
	static public string farthestDistDrive = "farhestDistDrive";
	static public string totalBombCarsBlownUp = "totalBombCarsBlownUp";
	static public string totalBlocksActivated = "totalBlocksActivated";
	static public string totalCarDeaths = "totalCarDeaths";
	static public string totalGameOvers = "totalGameOvers";

	// social
	static public string googlePlayEnabled = "googlePlayEnabled";

	public void increaseExp (int experiece) {
		PlayerPrefs.SetInt (exp, PlayerPrefs.GetInt(exp, 0) + experiece);
		PlayerPrefs.SetInt (totalExp, PlayerPrefs.GetInt(totalExp, 0) + experiece);
	}

	public void increaseDistance (float distance, string level) {
		PlayerPrefs.SetFloat (totalDistance, PlayerPrefs.GetFloat(totalDistance, 0) + distance);
		Camera.main.GetComponent<OnlineServices> ().postDistance (level, Mathf.FloorToInt(distance));
		if (level == LevelManagement.floorIt) {
			if (distance > PlayerPrefs.GetFloat(farthestDistFloorIt, 0)) {
				PlayerPrefs.SetFloat (farthestDistFloorIt, distance);
			}
		}
		else if (level == LevelManagement.drive) {
			if (distance > PlayerPrefs.GetFloat(farthestDistDrive, 0)) {
				PlayerPrefs.SetFloat (farthestDistDrive, distance);
			}
		}
		increaseGameOver ();
		PlayerPrefs.Save ();
	}

	public void increaseGameOver () {
		PlayerPrefs.SetInt (totalGameOvers, PlayerPrefs.GetInt(totalGameOvers, 0) + 1);
	}

	public void increaseCarDeaths () {
		PlayerPrefs.SetInt (totalCarDeaths, PlayerPrefs.GetInt(totalCarDeaths, 0) + 1);
	}

	public void increaseBombCars () {
		PlayerPrefs.SetInt (totalBombCarsBlownUp, PlayerPrefs.GetInt(totalBombCarsBlownUp, 0) + 1);
	}

	public void increaseBlocksActivated () {
		PlayerPrefs.SetInt (totalBlocksActivated, PlayerPrefs.GetInt(totalBlocksActivated, 0) + 1);
	}
}
