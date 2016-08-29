using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManagement : MonoBehaviour {

	public string level;
	public static string mainMenu = "MainMenu";
	public static string floorIt = "BuildARoad01";
	public static string bowl = "Bowling";
	public static string drive = "Driving";

	void Awake () {
		level = SceneManager.GetActiveScene ().name;
	}
}
