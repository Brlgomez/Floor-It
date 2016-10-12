using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceMainMenuTools : MonoBehaviour {

	static Vector4 carLocked = new Vector4 (0.25f, 0.25f, 0.25f, 1);
	static Vector4 buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
	static Vector4 scrollBackgrounOn = new Vector4 (0.125f, 0.125f, 0.125f, 0.125f);
	static Vector4 statsBackgrounOn = new Vector4 (0.125f, 0.125f, 0.125f, 0.5f);
	static Vector4 textOn = Vector4.one;
	static Vector4 noColor = Vector4.zero;

	public void menuOn () {
		turnOffAll ();
		Camera.main.GetComponent<InterfaceMainMenu>().titleText.text = "Floor It";
		Camera.main.GetComponent<InterfaceMainMenu>().expText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playBowlingButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playDriveButton);

		turnOnButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().leaderboardButton);
		turnOnButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().achievementButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().statsButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().storeButton);
		turnOnButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().settingsButton);
	}

	public void settingsOn () {
		turnOffAll ();
		Camera.main.GetComponent<InterfaceMainMenu>().titleText.text = "Settings";
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().backButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().soundButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().musicButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().vibrationButton);
	}

	public void storeOn () {
		turnOffAll ();
		Camera.main.GetComponent<InterfaceMainMenu>().titleText.text = "Store";
		Camera.main.GetComponent<InterfaceMainMenu>().expText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().backButton);

		Camera.main.GetComponent<InterfaceMainMenu>().scrollrect.GetComponent<ScrollRect> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().viewport.GetComponent<Image> ().color = scrollBackgrounOn;
		Camera.main.GetComponent<InterfaceMainMenu>().scrollbarVert.GetComponent<Scrollbar> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().handle.GetComponent<Image> ().color = buttonOn;

		Camera.main.GetComponent<InterfaceMainMenu>().sudanButton.GetComponent<Button> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().sudanButton.GetComponent<Image> ().color = textOn;
		Camera.main.GetComponent<InterfaceMainMenu>().sudanButton.GetComponent<Image> ().raycastTarget = true;
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().limoButton, PlayerPrefManagement.limo);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().truckButton, PlayerPrefManagement.truck);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().sportButton, PlayerPrefManagement.sport);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().monsterTruckButton, PlayerPrefManagement.monsterTruck);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().coneButton, PlayerPrefManagement.cone);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().busButton, PlayerPrefManagement.bus);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().abstractButton, PlayerPrefManagement.abstractCar);
		Camera.main.GetComponent<InterfaceMainMenu>().normalVisual.GetComponent<Button> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().normalVisual.GetComponent<Image> ().color = textOn;
		Camera.main.GetComponent<InterfaceMainMenu>().normalVisual.GetComponent<Image> ().raycastTarget = true;
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().nightVisual, PlayerPrefManagement.nightVisual);
		turnOnCarButton (Camera.main.GetComponent<InterfaceMainMenu>().pixelVisual, PlayerPrefManagement.pixelVisual);

		Camera.main.GetComponent<InterfaceMainMenu>().buyButton.GetComponent<Button> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().buyButton.GetComponentInChildren<Text> ().enabled = true;
		Camera.main.GetComponent<InterfaceMainMenu>().buyButton.GetComponent<Image> ().color = carLocked;
		Camera.main.GetComponent<InterfaceMainMenu>().buyButton.GetComponentInChildren<Text> ().color = textOn;
	}

	public void statsOn () {
		turnOffAll ();
		Camera.main.GetComponent<InterfaceMainMenu>().titleText.text = "Stats";
		Camera.main.GetComponent<InterfaceMainMenu>().statsBackGround.GetComponent<Image> ().color = statsBackgrounOn;
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().backButton);
	}

	public void confirmationOn (string itemName, int amount, Sprite image) {
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationScreen.GetComponent<Image> ().color = statsBackgrounOn;
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationImage.sprite = image;
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationImage.GetComponent<Image> ().color = textOn;
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationText.GetComponent<Text> ().color = textOn;
		Camera.main.GetComponent<InterfaceMainMenu>().expText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().confirmYesButton);
		turnOnButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().confirmNoButton);
		if (PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) >= amount) { 
			Camera.main.GetComponent<InterfaceMainMenu>().confirmationText.text = "Get the " + itemName + " for " + 
				amount + " EXP?";
		} else {
			Camera.main.GetComponent<InterfaceMainMenu>().confirmationText.text = "You need " + 
				(amount - PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0)) + " more EXP to get the " + itemName + ".";
		}
	}

	public void turnOffAll () {
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().storeButton);
		turnOffButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().settingsButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().statsButton);
		turnOffButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().leaderboardButton);
		turnOffButtonAndImage (Camera.main.GetComponent<InterfaceMainMenu>().achievementButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().backButton);

		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playBowlingButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().playDriveButton);

		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().soundButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().musicButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().vibrationButton);

		Camera.main.GetComponent<InterfaceMainMenu>().scrollrect.GetComponent<ScrollRect> ().enabled = false;
		Camera.main.GetComponent<InterfaceMainMenu>().viewport.GetComponent<Image> ().color = noColor;
		Camera.main.GetComponent<InterfaceMainMenu>().scrollbarVert.GetComponent<Scrollbar> ().enabled = false;
		Camera.main.GetComponent<InterfaceMainMenu>().handle.GetComponent<Image> ().color = noColor;
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().sudanButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().limoButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().truckButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().sportButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().monsterTruckButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().coneButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().busButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().buyButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().abstractButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().normalVisual);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().nightVisual);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().pixelVisual);

		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().confirmYesButton);
		turnOffButtonAndText (Camera.main.GetComponent<InterfaceMainMenu>().confirmNoButton);
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationScreen.GetComponent<Image> ().color = noColor;
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationText.GetComponent<Text> ().color = noColor;
		Camera.main.GetComponent<InterfaceMainMenu>().confirmationImage.GetComponent<Image> ().color = noColor;
		Camera.main.GetComponent<InterfaceMainMenu>().expText.GetComponent<Text> ().color = noColor;

		Camera.main.GetComponent<InterfaceMainMenu>().statsBackGround.GetComponent<Image> ().color = noColor;
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.GetComponent<Text> ().color = noColor;
	}

	public void turnOnButtonAndText (Button b) {
		if (!b.GetComponent<Button> ().enabled) {
			b.GetComponent<Button> ().enabled = true;
			b.GetComponent<Image> ().color = buttonOn;
			b.GetComponentInChildren<Text> ().enabled = true;
			b.GetComponent<Image> ().raycastTarget = true;
		}
	}

	public void turnOffButtonAndText (Button b) {
		if (b.GetComponent<Button> ().enabled) {
			b.GetComponent<Button> ().enabled = false;
			b.GetComponent<Image> ().color = noColor;
			b.GetComponentInChildren<Text> ().enabled = false;
			b.GetComponent<Image> ().raycastTarget = false;
		}
	}

	public void turnOnButtonAndImage (Button b) {
		if (!b.GetComponent<Button> ().enabled) {
			b.GetComponent<Button> ().enabled = true;
			b.GetComponent<Image> ().color = buttonOn;
			b.GetComponentsInChildren<Image> () [1].enabled = true;
			b.GetComponentsInChildren<Image> () [1].color = textOn;
			b.GetComponent<Image> ().raycastTarget = true;
		}
	}

	public void turnOffButtonAndImage (Button b) {
		if (b.GetComponent<Button> ().enabled) {
			b.GetComponent<Button> ().enabled = false;
			b.GetComponent<Image> ().color = noColor;
			b.GetComponentsInChildren<Image> () [1].enabled = false;
			b.GetComponentsInChildren<Image> () [1].color = noColor;
			b.GetComponent<Image> ().raycastTarget = false;
		}
	}

	public void turnOnCarButton (Button button, string playerPref) {
		button.GetComponent<Button> ().enabled = true;
		button.GetComponentInChildren<Text> ().enabled = true;
		if (PlayerPrefs.GetInt (playerPref, 0) == 0) {
			button.GetComponent<Image> ().color = carLocked;
			button.GetComponentInChildren<Text> ().color = textOn;
		} else {
			button.GetComponent<Image> ().color = textOn;
		}
		button.GetComponent<Image> ().raycastTarget = true;
	}

	public void setCarPosition (int carNumber) {
		if (carNumber == 1) {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 4) {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 5) {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = Vector3.zero;
		} else if (carNumber == 6) {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 7) {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = new Vector3 (0, 0, 0.3f);
		} else {
			Camera.main.GetComponent<InterfaceMainMenu>().car.transform.position = new Vector3 (0, 0, 0.2f);
		}
	}

	public void setInitialHighlightPosition (int carNumber) {
		if (carNumber == 0) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().sudanButton.transform.position;
		} else if (carNumber == 1) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().limoButton.transform.position;
		} else if (carNumber == 2) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().truckButton.transform.position;
		} else if (carNumber == 3) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().sportButton.transform.position;
		} else if (carNumber == 4) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().monsterTruckButton.transform.position;
		} else if (carNumber == 5) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().coneButton.transform.position;
		} else if (carNumber == 6) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().busButton.transform.position;
		} else if (carNumber == 7) {
			GameObject.Find ("Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().abstractButton.transform.position;
		} 
		if (PlayerPrefs.GetInt (PlayerPrefManagement.visual, 0) == 0) {
			GameObject.Find ("Visual Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().normalVisual.transform.position;
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual, 0) == 1) {
			GameObject.Find ("Visual Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().nightVisual.transform.position;
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual, 0) == 2) {
			GameObject.Find ("Visual Highlight").transform.position = Camera.main.GetComponent<InterfaceMainMenu>().pixelVisual.transform.position;
		}
	}

	public void setInitialText () {
		int highScoreInfinite, highScoreBowling, highScoreDriving;
		int exp = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
		highScoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highScoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highScoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		Camera.main.GetComponent<InterfaceMainMenu>().playButton.GetComponentInChildren<Text> ().text = "Floor It\n\nHigh Score\n" + highScoreInfinite;
		Camera.main.GetComponent<InterfaceMainMenu>().playBowlingButton.GetComponentInChildren<Text> ().text = "Bowl\n\nHigh Score\n" + highScoreBowling;
		Camera.main.GetComponent<InterfaceMainMenu>().playDriveButton.GetComponentInChildren<Text> ().text = "Drive\n\nHigh Score\n" + highScoreDriving;
		Camera.main.GetComponent<InterfaceMainMenu>().expText.text = exp + " EXP";
		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: On";
		} else {
			Camera.main.GetComponent<InterfaceMainMenu>().soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().musicButton.GetComponentInChildren<Text> ().text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		} else {
			Camera.main.GetComponent<InterfaceMainMenu>().musicButton.GetComponentInChildren<Text> ().text = "Music: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: On";
		} else {
			Camera.main.GetComponent<InterfaceMainMenu>().vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().limoButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.limoAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().truckButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.truckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().sportButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.sportAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().monsterTruckButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.monsterTruckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().coneButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.coneAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.bus, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().busButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.busAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.abstractCar, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().abstractButton.GetComponentInChildren<Text> ().text = InterfaceMainMenu.abstractAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.nightVisual, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().nightVisual.GetComponentInChildren<Text> ().text = InterfaceMainMenu.nightVisualAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.pixelVisual, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenu>().pixelVisual.GetComponentInChildren<Text> ().text = InterfaceMainMenu.pixelVisualAmount + " EXP";
		}
		setUpStats ();
	}

	public void setUpStats () {
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "\nFloor It High Score: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Drive High Score: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Drive High Score: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0) + "\n\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Floor It Farthest Dist: " + 
			string.Format("{0:F1}", PlayerPrefs.GetFloat (PlayerPrefManagement.farthestDistFloorIt, 0)) + " units \n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Drive Farthest Dist: " + 
			string.Format("{0:F1}", PlayerPrefs.GetFloat (PlayerPrefManagement.farthestDistDrive, 0)) + " units \n\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total Cars MIA: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.totalCarDeaths, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total Game Overs: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.totalGameOvers, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total EXP Earned: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.totalExp, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total Dist: " + 
			string.Format("{0:F1}",PlayerPrefs.GetFloat (PlayerPrefManagement.totalDistance, 0)) + " units \n";

		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total Blocks Activated: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.totalBlocksActivated, 0) + "\n";
		Camera.main.GetComponent<InterfaceMainMenu>().statsText.text += "Total Bomb Cars Exploded: " + 
			PlayerPrefs.GetInt (PlayerPrefManagement.totalBombCarsBlownUp, 0) + "\n\n";
	}
}
