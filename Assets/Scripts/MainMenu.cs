using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public ScrollRect scrollrect;
	public Scrollbar scrollbar;
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
	public Button buyButton;
	public Button soundButton;
	public Button musicButton;
	public Button vibrationButton;

	public Text title;
	public Text floorItText;
	public Text bowlingText;
	public Text driveText;
	public Text storeText;
	public Text settingsText;
	public Text sudanText;
	public Text limoText;
	public Text truckText;
	public Text sportText;
	public Text monsterTruckText;
	public Text buyText;
	public Text soundText;
	public Text musicText;
	public Text vibrationText;
	public Text cashText;

	int highScoreInfinite;
	int highScoreBowling;
	int highScoreDriving;
	int cash;
	int carNumber;

	bool viewSettings;
	bool viewStore;
	public Text loadingText;
	bool loading;

	static Vector4 carLocked = new Vector4 (0.25f, 0.25f, 0.25f, 1);
	static Vector4 buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
	static Vector4 textOn = Vector4.one;
	static Vector4 noColor = Vector4.zero;

	public GameObject[] cars;
	public GameObject car;

	static int truckAmount = 500;
	static int sportAmount = 5000;
	static int limoAmount = 10000;
	static int monsterTruckAmount = 25500;

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
		buyButton.onClick.AddListener (delegate { buyButtonClick (); });

		loading = false;
		viewSettings = false;
		viewStore = false;

		highScoreInfinite = PlayerPrefs.GetInt ("High Score Infinite", 0);
		highScoreBowling = PlayerPrefs.GetInt ("High Score Bowling", 0);
		highScoreDriving = PlayerPrefs.GetInt ("High Score Driving", 0);
		carNumber = PlayerPrefs.GetInt ("Car Type", 0);
		cash = PlayerPrefs.GetInt ("Cash", 0);
		floorItText.text = "Floor It\n\nHigh Score\n" + highScoreInfinite;
		bowlingText.text = "Bowl\n\nHigh Score\n" + highScoreBowling;
		driveText.text = "Drive\n\nHigh Score\n" + highScoreDriving;
		GameObject newCar = (GameObject)Instantiate(cars[carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		cashText.text = cash + " EXP";
		if (PlayerPrefs.GetInt ("Play Sound Effects", 0) == 0){
			soundText.text = "Sound Effects: On";
		} else {
			soundText.text = "Sound Effects: Off";
		}
		if (PlayerPrefs.GetInt ("Play Music", 0) == 0){
			musicText.text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		} else {
			musicText.text = "Music: Off";
		}
		if (PlayerPrefs.GetInt ("Play Vibrations", 0) == 0){
			vibrationText.text = "Vibration: On";
		} else {
			vibrationText.text = "Vibration: Off";
		}
		if (PlayerPrefs.GetInt ("Limo Unlocked", 0) == 0) {
			limoText.text = limoAmount + " EXP";
		}
		if (PlayerPrefs.GetInt ("Truck Unlocked", 0) == 0) {
			truckText.text = truckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt ("Sport Unlocked", 0) == 0) {
			sportText.text = sportAmount + " EXP";
		}
		if (PlayerPrefs.GetInt ("Monster Truck Unlocked", 0) == 0) {
			monsterTruckText.text = monsterTruckAmount + " EXP";
		}
		menuOn ();
		//PlayerPrefs.DeleteAll();
	}
		
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
		scrollbar.GetComponent<Scrollbar> ().enabled = true;
		scrollbar.GetComponent<Image> ().color = buttonOn/4;
		handle.GetComponent<Image> ().color = buttonOn;

		sudanButton.GetComponent<Button> ().enabled = true;
		sudanButton.GetComponent<Image> ().color = textOn;
		turnOnCarButton (limoButton, limoText, "Limo Unlocked");
		turnOnCarButton (truckButton, truckText, "Truck Unlocked");
		turnOnCarButton (sportButton, sportText, "Sport Unlocked");
		turnOnCarButton (monsterTruckButton, monsterTruckText, "Monster Truck Unlocked");
		//buyButton.GetComponent<Button> ().enabled = true;
		//buyText.GetComponent<Text> ().enabled = true;
		//buyButton.GetComponent<Image> ().color = carLocked;
		//buyText.GetComponent<Text> ().color = textOn;

		cashText.GetComponent<Text> ().color = textOn;
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
		scrollbar.GetComponent<Scrollbar> ().enabled = false;
		scrollbar.GetComponent<Image> ().color = noColor;
		handle.GetComponent<Image> ().color = noColor;

		turnOffButtonAndText (sudanButton, sudanText);
		turnOffButtonAndText (limoButton, limoText);
		turnOffButtonAndText (truckButton, truckText);
		turnOffButtonAndText (sportButton, sportText);
		turnOffButtonAndText (monsterTruckButton, monsterTruckText);
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

	public void playButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		loadingText.text = "Loading...";
		PlayerPrefs.SetString("Level", LevelManagement.floorIt);
		if (loading == false) {
			StartCoroutine(loadNewScene(LevelManagement.floorIt));
		}
		loading = true;
	}

	public void playBowlingButtonClick() {		
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();	
		loadingText.text = "Loading...";
		PlayerPrefs.SetString("Level", LevelManagement.bowl);
		if (loading == false) {
			StartCoroutine(loadNewScene(LevelManagement.bowl));
		}
		loading = true;
	}

	public void playDrivingButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		loadingText.text = "Loading...";
		PlayerPrefs.SetString("Level", LevelManagement.drive);
		if (loading == false) {
			StartCoroutine(loadNewScene(LevelManagement.drive));
		}
		loading = true;
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

	public void soundEffectsButtonClick() {
		if(PlayerPrefs.GetInt ("Play Sound Effects", 0) == 0){
			PlayerPrefs.SetInt ("Play Sound Effects", 1);
			soundText.text = "Sound Effects: Off";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt ("Play Sound Effects", 0);
		} else {
			PlayerPrefs.SetInt ("Play Sound Effects", 0);
			soundText.text = "Sound Effects: On";
			Camera.main.GetComponent<SoundEffects>().playSoundEffects = PlayerPrefs.GetInt ("Play Sound Effects", 0);
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		}
		PlayerPrefs.Save ();
	}

	public void musicButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt ("Play Music", 0) == 0){
			PlayerPrefs.SetInt ("Play Music", 1);
			musicText.text = "Music: Off";
			Camera.main.GetComponent<SoundEffects> ().stopMusic ();
		} else {
			PlayerPrefs.SetInt ("Play Music", 0);
			musicText.text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		}
		PlayerPrefs.Save ();
	}

	public void vibrationButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if(PlayerPrefs.GetInt ("Play Vibrations", 0) == 0){
			PlayerPrefs.SetInt ("Play Vibrations", 1);
			vibrationText.text = "Vibration: Off";
		} else {
			PlayerPrefs.SetInt ("Play Vibrations", 0);
			vibrationText.text = "Vibration: On";
			Camera.main.GetComponent<Vibration> ().vibrate ();
		}
		PlayerPrefs.Save ();
	}

	public void sudanButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		GameObject newCar = (GameObject)Instantiate(cars[0], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		PlayerPrefs.SetInt ("Car Type", 0);
		PlayerPrefs.Save ();
	}

	public void limoButtonClick() {
		buyCar ("Limo Unlocked", limoAmount, 1, limoButton, limoText);
	}

	public void truckButtonClick() {
		buyCar ("Truck Unlocked", truckAmount, 2, truckButton, truckText);
	}

	public void sportButtonClick() {
		buyCar ("Sport Unlocked", sportAmount, 3, sportButton, sportText);
	}

	public void monsterTruckButtonClick() {
		buyCar ("Monster Truck Unlocked", monsterTruckAmount, 4, monsterTruckButton, monsterTruckText);
	}

	void buyCar(string playerPref, int amount, int carIndex, Button carButton, Text priceText){
		if (PlayerPrefs.GetInt (playerPref, 0) == 0 && PlayerPrefs.GetInt ("Cash", 0) >= amount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			PlayerPrefs.SetInt(playerPref, 1);
			PlayerPrefs.SetInt ("Cash", PlayerPrefs.GetInt ("Cash", 0) - amount);
			GameObject newCar = (GameObject)Instantiate(cars[carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", carIndex);
			PlayerPrefs.Save ();
			carButton.GetComponent<Image> ().color = textOn;
			priceText.text = "";
			cashText.text = PlayerPrefs.GetInt ("Cash", 0) + " EXP";
		} else if (PlayerPrefs.GetInt (playerPref, 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			GameObject newCar = (GameObject)Instantiate(cars[carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", carIndex);
			PlayerPrefs.Save ();
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	public void buyButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
		PlayerPrefs.SetInt ("Cash", PlayerPrefs.GetInt ("Cash", 0) + 55555);
		PlayerPrefs.Save ();
		cashText.text = PlayerPrefs.GetInt ("Cash", 0) + " EXP";
	}

	IEnumerator loadNewScene(string level) {
		AsyncOperation async = SceneManager.LoadSceneAsync(level);
		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}
}
