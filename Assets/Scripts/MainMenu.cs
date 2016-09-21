﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject[] cars;
	public GameObject car;

	public ScrollRect scrollrect;
	public Scrollbar scrollbar;
	public Image view;
	public Image handle;

	public Button playButton;
	public Button playBowlingButton;
	public Button playDriveButton;
	public Button storeButton;
	public Button settingsButton;
	public Button sudanButton;
	public Button limoButton;
	public Button truckButton;
	public Button sportButton;
	public Button monsterTruckButton;
	public Button coneButton; 
	public Button busButton; 
	public Button abstractButton; 
	public Button buyButton;
	public Button soundButton;
	public Button musicButton;
	public Button vibrationButton;

	public Text title;
	public Text cashText;
	public Text loadingText;

	int highScoreInfinite, highScoreBowling, highScoreDriving;
	int cash;
	int carNumber;

	bool viewSettings = false;
	bool viewStore = false;
	bool loading = false;

	static Vector4 carLocked = new Vector4 (0.25f, 0.25f, 0.25f, 1);
	static Vector4 buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
	static Vector4 scrollBackgrounOn = new Vector4 (0.5f, 0.5f, 0.5f, 0.125f);
	static Vector4 textOn = Vector4.one;
	static Vector4 noColor = Vector4.zero;

	static int truckAmount = 500;
	static int sportAmount = 4000;
	static int limoAmount = 8000;
	static int busAmount = 12000;
	static int monsterTruckAmount = 20000;
	static int abstractAmount = 30000;
	static int coneAmount = 52427;

	void Start () {
		playButton.onClick.AddListener(delegate { playButtonClick(); });
		playBowlingButton.onClick.AddListener(delegate { playBowlingButtonClick(); });
		playDriveButton.onClick.AddListener(delegate { playDrivingButtonClick(); });
		storeButton.onClick.AddListener (delegate { storeButtonClick (); });
		settingsButton.onClick.AddListener (delegate { settingsButtonClick (); });
		soundButton.onClick.AddListener (delegate { soundEffectsButtonClick (); });
		musicButton.onClick.AddListener (delegate { musicButtonClick (); });
		vibrationButton.onClick.AddListener (delegate { vibrationButtonClick (); });
		sudanButton.onClick.AddListener (delegate { sudanButtonClick (); });
		limoButton.onClick.AddListener (delegate { limoButtonClick (); });
		truckButton.onClick.AddListener (delegate { truckButtonClick (); });
		sportButton.onClick.AddListener (delegate { sportButtonClick (); });
		monsterTruckButton.onClick.AddListener (delegate { monsterTruckButtonClick (); });
		coneButton.onClick.AddListener (delegate { coneButtonClick (); });
		busButton.onClick.AddListener (delegate { busButtonClick (); });
		abstractButton.onClick.AddListener (delegate { abstractButtonClick (); });
		buyButton.onClick.AddListener (delegate { buyButtonClick (); });

		highScoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highScoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highScoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		carNumber = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);
		cash = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
		playButton.GetComponentInChildren<Text>().text = "Floor It\n\nHigh Score\n" + highScoreInfinite;
		playBowlingButton.GetComponentInChildren<Text>().text = "Bowl\n\nHigh Score\n" + highScoreBowling;
		playDriveButton.GetComponentInChildren<Text>().text = "Drive\n\nHigh Score\n" + highScoreDriving;
		cashText.text = cash + " EXP";
		GameObject newCar = (GameObject)Instantiate(cars[carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;

		if (carNumber == 0) {
			GameObject.Find ("Highlight").transform.position = sudanButton.transform.position;
		}
		if (carNumber == 1) {
			GameObject.Find ("Highlight").transform.position = limoButton.transform.position;
		}
		if (carNumber == 2) {
			GameObject.Find ("Highlight").transform.position = truckButton.transform.position;
		}
		if (carNumber == 3) {
			GameObject.Find ("Highlight").transform.position = sportButton.transform.position;
		}
		if (carNumber == 4) {
			GameObject.Find ("Highlight").transform.position = monsterTruckButton.transform.position;
		}
		if (carNumber == 5) {
			GameObject.Find ("Highlight").transform.position = coneButton.transform.position;
		}
		if (carNumber == 6) {
			GameObject.Find ("Highlight").transform.position = busButton.transform.position;
		}
		if (carNumber == 7) {
			GameObject.Find ("Highlight").transform.position = abstractButton.transform.position;
		}

		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0){
			soundButton.GetComponentInChildren<Text>().text = "Sound Effects: On";
		} else {
			soundButton.GetComponentInChildren<Text>().text = "Sound Effects: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0){
			musicButton.GetComponentInChildren<Text>().text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		} else {
			musicButton.GetComponentInChildren<Text>().text = "Music: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0){
			vibrationButton.GetComponentInChildren<Text>().text = "Vibration: On";
		} else {
			vibrationButton.GetComponentInChildren<Text>().text = "Vibration: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 0) {
			limoButton.GetComponentInChildren<Text>().text = limoAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 0) {
			truckButton.GetComponentInChildren<Text>().text = truckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 0) {
			sportButton.GetComponentInChildren<Text>().text = sportAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 0) {
			monsterTruckButton.GetComponentInChildren<Text>().text = monsterTruckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 0) {
			coneButton.GetComponentInChildren<Text>().text = coneAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.bus, 0) == 0) {
			busButton.GetComponentInChildren<Text>().text = busAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.abstractCar, 0) == 0) {
			abstractButton.GetComponentInChildren<Text>().text = abstractAmount + " EXP";
		}

		menuOn ();
		//PlayerPrefs.DeleteAll();
	}
		
	/*
	 *  main menu buttons
	 */

	public void playButtonClick() {
		loadLevel (LevelManagement.floorIt);
	}

	public void playBowlingButtonClick() {		
		loadLevel (LevelManagement.bowl);
	}

	public void playDrivingButtonClick() {
		loadLevel (LevelManagement.drive);
	}

	void loadLevel(string level) {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		loadingText.text = "Loading...";
		PlayerPrefs.SetString(PlayerPrefManagement.level, level);
		if (loading == false) {
			StartCoroutine(loadNewScene(level));
		}
		loading = true;
	}

	IEnumerator loadNewScene(string level) {
		AsyncOperation async = SceneManager.LoadSceneAsync(level);
		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}

	public void settingsButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		viewSettings = !viewSettings;
		if (viewSettings) {
			settingsOn ();
		} else {
			menuOn ();
		}
	}

	public void storeButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		viewStore = !viewStore;
		if (viewStore) {
			storeOn ();
		} else {
			menuOn ();
		}
	}

	/*
	 *  settings buttons
	 */

	public void soundEffectsButtonClick() {
		if(PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0){
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 1);
			soundButton.GetComponentInChildren<Text>().text = "Sound Effects: Off";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 0);
			soundButton.GetComponentInChildren<Text>().text = "Sound Effects: On";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		}
		PlayerPrefs.Save ();
	}

	public void musicButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0){
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 1);
			musicButton.GetComponentInChildren<Text>().text = "Music: Off";
			Camera.main.GetComponent<SoundEffects> ().stopMusic ();
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 0);
			musicButton.GetComponentInChildren<Text>().text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		}
		PlayerPrefs.Save ();
	}

	public void vibrationButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0){
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 1);
			vibrationButton.GetComponentInChildren<Text>().text = "Vibration: Off";
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 0);
			vibrationButton.GetComponentInChildren<Text>().text = "Vibration: On";
			Camera.main.GetComponent<Vibration> ().vibrate ();
		}
		PlayerPrefs.Save ();
	}

	/*
	 *  store buttons
	 */

	public void sudanButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		GameObject newCar = (GameObject)Instantiate(cars[0], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		PlayerPrefs.SetInt (PlayerPrefManagement.carType, 0);
		PlayerPrefs.Save ();
		GameObject.Find ("Highlight").transform.position = sudanButton.transform.position;
	}

	public void limoButtonClick() {
		buyCar (PlayerPrefManagement.limo, limoAmount, 1, limoButton);
	}

	public void truckButtonClick() {
		buyCar (PlayerPrefManagement.truck, truckAmount, 2, truckButton);
	}

	public void sportButtonClick() {
		buyCar (PlayerPrefManagement.sport, sportAmount, 3, sportButton);
	}

	public void monsterTruckButtonClick() {
		buyCar (PlayerPrefManagement.monsterTruck, monsterTruckAmount, 4, monsterTruckButton);
	}

	public void coneButtonClick() {
		buyCar (PlayerPrefManagement.cone, coneAmount, 5, coneButton);
	}

	public void busButtonClick() {
		buyCar (PlayerPrefManagement.bus, busAmount, 6, busButton);
	}

	public void abstractButtonClick() {
		buyCar (PlayerPrefManagement.abstractCar, abstractAmount, 7, abstractButton);
	}

	public void buyButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
		PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + 55555);
		PlayerPrefs.Save ();
		cashText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
	}

	void buyCar(string carPlayerPref, int amount, int carIndex, Button carButton){
		if (PlayerPrefs.GetInt (carPlayerPref, 0) == 0 && PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) >= amount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			GameObject newCar = (GameObject)Instantiate(cars[carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt(carPlayerPref, 1);
			PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) - amount);
			PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
			PlayerPrefs.Save ();
			carButton.GetComponent<Image> ().color = textOn;
			carButton.GetComponentInChildren<Text>().text = "";
			cashText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
			GameObject.Find ("Highlight").transform.position = carButton.transform.position;
		} else if (PlayerPrefs.GetInt (carPlayerPref, 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			if (PlayerPrefs.GetInt (PlayerPrefManagement.carType, carIndex) != carIndex) {
				GameObject newCar = (GameObject)Instantiate (cars [carIndex], car.transform.position, car.transform.rotation);
				Destroy (car);
				car = newCar;
				PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
				PlayerPrefs.Save ();
				GameObject.Find ("Highlight").transform.position = carButton.transform.position;
			}
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	/*
	 * image and text manipulation
	 */ 

	void menuOn(){
		turnOffAll ();
		title.text = "Floor It";
		cashText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (playButton);
		turnOnButtonAndText (playBowlingButton);
		turnOnButtonAndText (playDriveButton);

		turnOnButtonAndText (storeButton);
		storeButton.GetComponentInChildren<Text>().text = "$";
		turnOnButtonAndText (settingsButton);
		settingsButton.GetComponentInChildren<Text>().text = "~\n^";
	}

	void settingsOn(){
		turnOffAll();
		title.text = "Settings";
		turnOnButtonAndText (settingsButton);
		settingsButton.GetComponentInChildren<Text>().text = "<-";

		turnOnButtonAndText (soundButton);
		turnOnButtonAndText (musicButton);
		turnOnButtonAndText (vibrationButton);
	}

	void storeOn(){
		turnOffAll ();
		title.text = "Store";
		cashText.GetComponent<Text> ().color = textOn;
		storeButton.GetComponentInChildren<Text>().text = "->";
		turnOnButtonAndText (storeButton);

		scrollrect.GetComponent<ScrollRect> ().enabled = true;
		view.GetComponent<Image> ().color = scrollBackgrounOn;
		scrollbar.GetComponent<Scrollbar> ().enabled = true;
		handle.GetComponent<Image> ().color = buttonOn;

		sudanButton.GetComponent<Button> ().enabled = true;
		sudanButton.GetComponent<Image> ().color = textOn;
		turnOnCarButton (limoButton, PlayerPrefManagement.limo);
		turnOnCarButton (truckButton, PlayerPrefManagement.truck);
		turnOnCarButton (sportButton, PlayerPrefManagement.sport);
		turnOnCarButton (monsterTruckButton, PlayerPrefManagement.monsterTruck);
		turnOnCarButton (coneButton, PlayerPrefManagement.cone);
		turnOnCarButton (busButton, PlayerPrefManagement.bus);
		turnOnCarButton (abstractButton, PlayerPrefManagement.abstractCar);
		GameObject.Find ("Highlight").GetComponent<Image> ().color = new Color (0.5f, 0.5f, 0.5f, 0.5f);

		//buyButton.GetComponent<Button> ().enabled = true;
		//buyButton.GetComponentInChildren<Text> ().enabled = true;
		//buyButton.GetComponent<Image> ().color = carLocked;
		//buyButton.GetComponentInChildren<Text> ().color = textOn;
	}

	void turnOffAll(){
		turnOffButtonAndText (storeButton);
		turnOffButtonAndText (settingsButton);
		cashText.GetComponent<Text> ().color = noColor;

		turnOffButtonAndText (playButton);
		turnOffButtonAndText (playBowlingButton);
		turnOffButtonAndText (playDriveButton);

		turnOffButtonAndText (soundButton);
		turnOffButtonAndText (musicButton);
		turnOffButtonAndText (vibrationButton);

		scrollrect.GetComponent<ScrollRect> ().enabled = false;
		view.GetComponent<Image> ().color = noColor;
		scrollbar.GetComponent<Scrollbar> ().enabled = false;
		handle.GetComponent<Image> ().color = noColor;

		turnOffButtonAndText (sudanButton);
		turnOffButtonAndText (limoButton);
		turnOffButtonAndText (truckButton);
		turnOffButtonAndText (sportButton);
		turnOffButtonAndText (monsterTruckButton);
		turnOffButtonAndText (coneButton);
		turnOffButtonAndText (busButton);
		turnOffButtonAndText (buyButton);
		turnOffButtonAndText (abstractButton);
		GameObject.Find ("Highlight").GetComponent<Image> ().color = noColor;
	}

	void turnOnButtonAndText(Button b){
		b.GetComponent<Button> ().enabled = true;
		b.GetComponent<Image> ().color = buttonOn;
		b.GetComponentInChildren<Text> ().enabled = true;
		b.GetComponentInChildren<Text> ().color = textOn;
	}

	void turnOffButtonAndText(Button b){
		b.GetComponent<Button> ().enabled = false;
		b.GetComponent<Image> ().color = noColor;
		b.GetComponentInChildren<Text> ().enabled = false;
		b.GetComponentInChildren<Text> ().color = noColor;
	}

	void turnOnCarButton (Button button, string playerPref){
		button.GetComponent<Button> ().enabled = true;
		button.GetComponentInChildren<Text>().enabled = true;
		if (PlayerPrefs.GetInt (playerPref, 0) == 0) {
			button.GetComponent<Image> ().color = carLocked;
			button.GetComponentInChildren<Text>().color = textOn;
		} else {
			button.GetComponent<Image> ().color = textOn;
		}
	}
}
