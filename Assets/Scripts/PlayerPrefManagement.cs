using UnityEngine;
using System.Collections;

public class PlayerPrefManagement : MonoBehaviour {

	// player preference
	static public string soundEffects = "Play Sound Effects";
	static public string music = "Play Music";
	static public string vibration = "Play Vibrations";
	static public string carType = "Car Type";
	static public string sudan = "Sudan Unlocked";
	static public string limo = "Limo Unlocked";
	static public string truck = "Truck Unlocked";
	static public string sport = "Sport Unlocked";
	static public string monsterTruck = "Monster Truck Unlocked";
	static public string cone = "Cone Unlocked";
	static public string bus = "Bus Unlocked";
	static public string abstractCar = "Abstract Unlocked";
	static public string level = "Level";
	static public string exp = "Cash";

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
		Camera.main.GetComponent<GooglePlayServices> ().postDistance (level, Mathf.FloorToInt(distance));
		if (level == LevelManagement.floorIt) {
			if (distance > PlayerPrefs.GetFloat(farthestDistFloorIt, 0)) {
				PlayerPrefs.SetFloat (farthestDistFloorIt, PlayerPrefs.GetFloat(totalDistance, 0) + distance);
			}
		}
		else if (level == LevelManagement.drive) {
			if (distance > PlayerPrefs.GetFloat(farthestDistDrive, 0)) {
				PlayerPrefs.SetFloat (farthestDistDrive, PlayerPrefs.GetFloat(totalDistance, 0) + distance);
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
