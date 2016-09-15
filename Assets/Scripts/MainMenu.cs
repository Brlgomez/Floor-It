using UnityEngine;
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
	public Button buyButton;
	public Button soundButton;
	public Button musicButton;
	public Button vibrationButton;

	public Text title;
	public Text cashText;
	public Text loadingText;
	Text floorItText, bowlingText, driveText, storeText, settingsText;
	Text sudanText, limoText, truckText, sportText, monsterTruckText, coneText, buyText;
	Text soundText, musicText, vibrationText;

	int highScoreInfinite, highScoreBowling ,highScoreDriving;
	int cash;
	int carNumber;

	bool viewSettings;
	bool viewStore;
	bool loading;

	static Vector4 carLocked = new Vector4 (0.25f, 0.25f, 0.25f, 1);
	static Vector4 buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
	static Vector4 scrollBackgrounOn = new Vector4 (0.5f, 0.5f, 0.5f, 0.125f);
	static Vector4 textOn = Vector4.one;
	static Vector4 noColor = Vector4.zero;

	static int truckAmount = 500;
	static int sportAmount = 5000;
	static int limoAmount = 12195;
	static int monsterTruckAmount = 25500;
	static int coneAmount = 52015;

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
		buyButton.onClick.AddListener (delegate { buyButtonClick (); });

		floorItText = playButton.GetComponentInChildren<Text> ();
		bowlingText = playBowlingButton.GetComponentInChildren<Text> ();
		driveText = playDriveButton.GetComponentInChildren<Text> ();
		storeText = storeButton.GetComponentInChildren<Text> ();
		settingsText = settingsButton.GetComponentInChildren<Text> ();
		sudanText = sudanButton.GetComponentInChildren<Text> ();
		limoText = limoButton.GetComponentInChildren<Text> ();
		truckText = truckButton.GetComponentInChildren<Text> ();
		sportText = sportButton.GetComponentInChildren<Text> ();
		monsterTruckText = monsterTruckButton.GetComponentInChildren<Text> ();
		coneText = coneButton.GetComponentInChildren<Text> ();
		buyText = buyButton.GetComponentInChildren<Text> ();
		soundText = soundButton.GetComponentInChildren<Text> ();
		musicText = musicButton.GetComponentInChildren<Text> ();
		vibrationText = vibrationButton.GetComponentInChildren<Text> ();

		loading = false;
		viewSettings = false;
		viewStore = false;

		highScoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highScoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highScoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		carNumber = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);
		cash = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
		floorItText.text = "Floor It\n\nHigh Score\n" + highScoreInfinite;
		bowlingText.text = "Bowl\n\nHigh Score\n" + highScoreBowling;
		driveText.text = "Drive\n\nHigh Score\n" + highScoreDriving;
		GameObject newCar = (GameObject)Instantiate(cars[carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		cashText.text = cash + " EXP";
		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0){
			soundText.text = "Sound Effects: On";
		} else {
			soundText.text = "Sound Effects: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0){
			musicText.text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		} else {
			musicText.text = "Music: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0){
			vibrationText.text = "Vibration: On";
		} else {
			vibrationText.text = "Vibration: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 0) {
			limoText.text = limoAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 0) {
			truckText.text = truckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 0) {
			sportText.text = sportAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 0) {
			monsterTruckText.text = monsterTruckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 0) {
			coneText.text = coneAmount + " EXP";
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
			soundText.text = "Sound Effects: Off";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 0);
			soundText.text = "Sound Effects: On";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		}
		PlayerPrefs.Save ();
	}

	public void musicButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0){
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 1);
			musicText.text = "Music: Off";
			Camera.main.GetComponent<SoundEffects> ().stopMusic ();
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 0);
			musicText.text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		}
		PlayerPrefs.Save ();
	}

	public void vibrationButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0){
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 1);
			vibrationText.text = "Vibration: Off";
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 0);
			vibrationText.text = "Vibration: On";
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
	}

	public void limoButtonClick() {
		buyCar (PlayerPrefManagement.limo, limoAmount, 1, limoButton, limoText);
	}

	public void truckButtonClick() {
		buyCar (PlayerPrefManagement.truck, truckAmount, 2, truckButton, truckText);
	}

	public void sportButtonClick() {
		buyCar (PlayerPrefManagement.sport, sportAmount, 3, sportButton, sportText);
	}

	public void monsterTruckButtonClick() {
		buyCar (PlayerPrefManagement.monsterTruck, monsterTruckAmount, 4, monsterTruckButton, monsterTruckText);
	}

	public void coneButtonClick() {
		buyCar (PlayerPrefManagement.cone, coneAmount, 5, coneButton, coneText);
	}

	public void buyButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
		PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + 55555);
		PlayerPrefs.Save ();
		cashText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
	}

	void buyCar(string playerPref, int amount, int carIndex, Button carButton, Text priceText){
		if (PlayerPrefs.GetInt (playerPref, 0) == 0 && PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) >= amount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			GameObject newCar = (GameObject)Instantiate(cars[carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt(playerPref, 1);
			PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) - amount);
			PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
			PlayerPrefs.Save ();
			carButton.GetComponent<Image> ().color = textOn;
			priceText.text = "";
			cashText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
		} else if (PlayerPrefs.GetInt (playerPref, 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			GameObject newCar = (GameObject)Instantiate(cars[carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
			PlayerPrefs.Save ();
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
		turnOnButtonAndText (playButton, floorItText);
		turnOnButtonAndText (playBowlingButton, bowlingText);
		turnOnButtonAndText (playDriveButton, driveText);

		turnOnButtonAndText (storeButton, storeText);
		storeText.text = "$";
		turnOnButtonAndText (settingsButton, settingsText);
		settingsText.text = "~\n^";

		cashText.GetComponent<Text> ().color = textOn;
	}

	void settingsOn(){
		turnOffAll();
		title.text = "Settings";
		turnOnButtonAndText (settingsButton, settingsText);
		settingsText.text = "<-";

		turnOnButtonAndText (soundButton, soundText);
		turnOnButtonAndText (musicButton, musicText);
		turnOnButtonAndText (vibrationButton, vibrationText);
	}

	void storeOn(){
		turnOffAll ();
		title.text = "Store";
		turnOnButtonAndText (storeButton, storeText);
		storeText.text = "->";

		scrollrect.GetComponent<ScrollRect> ().enabled = true;
		view.GetComponent<Image> ().color = scrollBackgrounOn;
		scrollbar.GetComponent<Scrollbar> ().enabled = true;
		scrollbar.GetComponent<Image> ().color = buttonOn/4;
		handle.GetComponent<Image> ().color = buttonOn;

		sudanButton.GetComponent<Button> ().enabled = true;
		sudanButton.GetComponent<Image> ().color = textOn;
		turnOnCarButton (limoButton, limoText, PlayerPrefManagement.limo);
		turnOnCarButton (truckButton, truckText, PlayerPrefManagement.truck);
		turnOnCarButton (sportButton, sportText, PlayerPrefManagement.sport);
		turnOnCarButton (monsterTruckButton, monsterTruckText, PlayerPrefManagement.monsterTruck);
		turnOnCarButton (coneButton, coneText, PlayerPrefManagement.cone);

		buyButton.GetComponent<Button> ().enabled = true;
		buyText.GetComponent<Text> ().enabled = true;
		buyButton.GetComponent<Image> ().color = carLocked;
		buyText.GetComponent<Text> ().color = textOn;

		cashText.GetComponent<Text> ().color = textOn;
	}

	void turnOffAll(){
		turnOffButtonAndText (storeButton, storeText);
		turnOffButtonAndText (settingsButton, settingsText);
		cashText.GetComponent<Text> ().color = noColor;

		turnOffButtonAndText (playButton, floorItText);
		turnOffButtonAndText (playBowlingButton, bowlingText);
		turnOffButtonAndText (playDriveButton, driveText);

		turnOffButtonAndText (soundButton, soundText);
		turnOffButtonAndText (musicButton, musicText);
		turnOffButtonAndText (vibrationButton, vibrationText);

		scrollrect.GetComponent<ScrollRect> ().enabled = false;
		view.GetComponent<Image> ().color = noColor;
		scrollbar.GetComponent<Scrollbar> ().enabled = false;
		scrollbar.GetComponent<Image> ().color = noColor;
		handle.GetComponent<Image> ().color = noColor;

		turnOffButtonAndText (sudanButton, sudanText);
		turnOffButtonAndText (limoButton, limoText);
		turnOffButtonAndText (truckButton, truckText);
		turnOffButtonAndText (sportButton, sportText);
		turnOffButtonAndText (monsterTruckButton, monsterTruckText);
		turnOffButtonAndText (coneButton, coneText);
		turnOffButtonAndText (buyButton, buyText);
	}

	void turnOnButtonAndText(Button b, Text t){
		b.GetComponent<Button> ().enabled = true;
		b.GetComponent<Image> ().color = buttonOn;
		t.GetComponent<Text> ().enabled = true;
		t.GetComponent<Text> ().color = textOn;
	}

	void turnOffButtonAndText(Button b, Text t){
		b.GetComponent<Button> ().enabled = false;
		b.GetComponent<Image> ().color = noColor;
		t.GetComponent<Text> ().enabled = false;
		t.GetComponent<Text> ().color = noColor;
	}

	void turnOnCarButton (Button button, Text text, string playerPref){
		button.GetComponent<Button> ().enabled = true;
		text.GetComponent<Text> ().enabled = true;
		if (PlayerPrefs.GetInt (playerPref, 0) == 0) {
			button.GetComponent<Image> ().color = carLocked;
			text.GetComponent<Text> ().color = textOn;
		} else {
			button.GetComponent<Image> ().color = textOn;
		}
	}
}
