using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button playButton;
	public Button playBowlingButton;
	public Button playDriveButton;
	public Button storeButton;
	public Button settingsButton;
	public Button sudanButton;
	public Button limoButton;
	public Button truckButton;
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
	public Text soundText;
	public Text musicText;
	public Text vibrationText;
	public Text highScore;
	public Text bowlingHighScore;
	public Text drivingHighScore;
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

	Vector4 buttonOn;
	Vector4 buttonOff;
	Vector4 textOn;
	Vector4 textOff;

	public GameObject[] cars;
	public GameObject car;

	static int truckAmount = 500;
	static int limoAmount = 5000;

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

		buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
		buttonOff = new Vector4 (0.5f, 0.5f, 0.5f, 0);
		textOn = new Vector4 (1, 1, 1, 1);
		textOff = new Vector4 (1, 1, 1, 0);
		loading = false;
		viewSettings = false;
		viewStore = false;

		highScoreInfinite = PlayerPrefs.GetInt ("High Score Infinite", 0);
		highScoreBowling = PlayerPrefs.GetInt ("High Score Bowling", 0);
		highScoreDriving = PlayerPrefs.GetInt ("High Score Driving", 0);
		carNumber = PlayerPrefs.GetInt ("Car Type", 0);
		cash = PlayerPrefs.GetInt ("Cash", 0);
		highScore.text = "High Score " + highScoreInfinite;
		bowlingHighScore.text = "High Score " + highScoreBowling;
		drivingHighScore.text = "High Score " + highScoreDriving;
		GameObject newCar = (GameObject)Instantiate(cars[carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		cashText.text = "$" + cash;
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
			limoText.text = "$" + limoAmount;
		}
		if (PlayerPrefs.GetInt ("Truck Unlocked", 0) == 0) {
			truckText.text = "$" + truckAmount;
		}
		menuOn ();
		//PlayerPrefs.DeleteAll();
	}
		
	void menuOn(){
		turnOffAll ();
		title.text = "Floor It";
		playButton.GetComponent<Button> ().enabled = true;
		playButton.GetComponent<Image> ().color = buttonOn;
		floorItText.GetComponent<Text> ().color = textOn;
		floorItText.GetComponent<Text> ().enabled = true;
		playBowlingButton.GetComponent<Button> ().enabled = true;
		playBowlingButton.GetComponent<Image> ().color = buttonOn;
		bowlingText.GetComponent<Text> ().color = textOn;
		bowlingText.GetComponent<Text> ().enabled = true;
		playDriveButton.GetComponent<Button> ().enabled = true;
		playDriveButton.GetComponent<Image> ().color = buttonOn;
		driveText.GetComponent<Text> ().color = textOn;
		driveText.GetComponent<Text> ().enabled = true;

		highScore.GetComponent<Text> ().color = textOn;
		bowlingHighScore.GetComponent<Text> ().color = textOn;
		drivingHighScore.GetComponent<Text> ().color = textOn;

		storeButton.GetComponent<Button> ().enabled = true;
		storeButton.GetComponent<Image> ().color = buttonOn;
		storeText.GetComponent<Text> ().color = textOn;
		storeText.text = "$";
		settingsButton.GetComponent<Button> ().enabled = true;
		settingsButton.GetComponent<Image> ().color = buttonOn;
		settingsText.GetComponent<Text> ().color = textOn;
		settingsText.text = "~\n^";

		cashText.GetComponent<Text> ().color = textOn;
	}

	void settingsOn(){
		turnOffAll();
		title.text = "Settings";
		settingsButton.GetComponent<Button> ().enabled = true;
		settingsButton.GetComponent<Image> ().color = buttonOn;
		settingsText.GetComponent<Text> ().color = textOn;
		settingsText.text = "<-";

		soundButton.GetComponent<Button> ().enabled = true;
		soundButton.GetComponent<Image> ().color = buttonOn;
		soundText.GetComponent<Text> ().enabled = true;
		soundText.GetComponent<Text> ().color = textOn;
		musicButton.GetComponent<Button> ().enabled = true;
		musicButton.GetComponent<Image> ().color = buttonOn;
		musicText.GetComponent<Text> ().enabled = true;
		musicText.GetComponent<Text> ().color = textOn;
		vibrationButton.GetComponent<Button> ().enabled = true;
		vibrationButton.GetComponent<Image> ().color = buttonOn;
		vibrationText.GetComponent<Text> ().enabled = true;
		vibrationText.GetComponent<Text> ().color = textOn;
	}

	void storeOn(){
		turnOffAll ();
		title.text = "Store";
		storeButton.GetComponent<Button> ().enabled = true;
		storeButton.GetComponent<Image> ().color = buttonOn;
		storeText.GetComponent<Text> ().color = textOn;
		storeText.text = "->";

		sudanButton.GetComponent<Button> ().enabled = true;
		sudanButton.GetComponent<Image> ().color = textOn;
		limoButton.GetComponent<Button> ().enabled = true;
		limoButton.GetComponent<Image> ().color = textOn;
		limoText.GetComponent<Text> ().enabled = true;
		limoText.GetComponent<Text> ().color = textOn;
		truckButton.GetComponent<Button> ().enabled = true;
		truckButton.GetComponent<Image> ().color = textOn;
		truckText.GetComponent<Text> ().enabled = true;
		truckText.GetComponent<Text> ().color = textOn;

		cashText.GetComponent<Text> ().color = textOn;
	}

	void turnOffAll(){
		playButton.GetComponent<Button> ().enabled = false;
		playButton.GetComponent<Image> ().color = buttonOff;
		floorItText.GetComponent<Text> ().color = textOff;
		floorItText.GetComponent<Text> ().enabled = false;
		playBowlingButton.GetComponent<Button> ().enabled = false;
		playBowlingButton.GetComponent<Image> ().color = buttonOff;
		bowlingText.GetComponent<Text> ().color = textOff;
		bowlingText.GetComponent<Text> ().enabled = false;
		playDriveButton.GetComponent<Button> ().enabled = false;
		playDriveButton.GetComponent<Image> ().color = buttonOff;
		driveText.GetComponent<Text> ().color = textOff;
		driveText.GetComponent<Text> ().enabled = false;

		highScore.GetComponent<Text> ().color = textOff;
		bowlingHighScore.GetComponent<Text> ().color = textOff;
		drivingHighScore.GetComponent<Text> ().color = textOff;

		storeButton.GetComponent<Button> ().enabled = false;
		storeButton.GetComponent<Image> ().color = buttonOff;
		storeText.GetComponent<Text> ().color = textOff;
		settingsButton.GetComponent<Button> ().enabled = false;
		settingsButton.GetComponent<Image> ().color = buttonOff;
		settingsText.GetComponent<Text> ().color = textOff;

		soundButton.GetComponent<Button> ().enabled = false;
		soundButton.GetComponent<Image> ().color = buttonOff;
		soundText.GetComponent<Text> ().enabled = false;
		soundText.GetComponent<Text> ().color = textOff;
		musicButton.GetComponent<Button> ().enabled = false;
		musicButton.GetComponent<Image> ().color = buttonOff;
		musicText.GetComponent<Text> ().enabled = false;
		musicText.GetComponent<Text> ().color = textOff;
		vibrationButton.GetComponent<Button> ().enabled = false;
		vibrationButton.GetComponent<Image> ().color = buttonOff;
		vibrationText.GetComponent<Text> ().enabled = false;
		vibrationText.GetComponent<Text> ().color = textOff;

		sudanButton.GetComponent<Button> ().enabled = false;
		sudanButton.GetComponent<Image> ().color = buttonOff;
		limoButton.GetComponent<Button> ().enabled = false;
		limoButton.GetComponent<Image> ().color = buttonOff;
		limoText.GetComponent<Text> ().enabled = false;
		limoText.GetComponent<Text> ().color = textOff;
		truckButton.GetComponent<Button> ().enabled = false;
		truckButton.GetComponent<Image> ().color = buttonOff;
		truckText.GetComponent<Text> ().enabled = false;
		truckText.GetComponent<Text> ().color = textOff;

		cashText.GetComponent<Text> ().color = textOff;
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
		if (PlayerPrefs.GetInt ("Limo Unlocked", 0) == 0 && PlayerPrefs.GetInt ("Cash", 0) >= limoAmount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			PlayerPrefs.SetInt("Limo Unlocked", 1);
			PlayerPrefs.SetInt ("Cash", PlayerPrefs.GetInt ("Cash", 0) - limoAmount);
			GameObject newCar = (GameObject)Instantiate(cars[1], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", 1);
			PlayerPrefs.Save ();
			limoText.text = "";
			cashText.text = "$" + PlayerPrefs.GetInt ("Cash", 0);
		} else if (PlayerPrefs.GetInt ("Limo Unlocked", 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			GameObject newCar = (GameObject)Instantiate(cars[1], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", 1);
			PlayerPrefs.Save ();
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	public void truckButtonClick() {
		if (PlayerPrefs.GetInt ("Truck Unlocked", 0) == 0 && PlayerPrefs.GetInt ("Cash", 0) >= truckAmount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			PlayerPrefs.SetInt("Truck Unlocked", 1);
			PlayerPrefs.SetInt ("Cash", PlayerPrefs.GetInt ("Cash", 0) - truckAmount);
			GameObject newCar = (GameObject)Instantiate(cars[2], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", 2);
			PlayerPrefs.Save ();
			truckText.text = "";
			cashText.text = "$" + PlayerPrefs.GetInt ("Cash", 0);
		} else if (PlayerPrefs.GetInt ("Truck Unlocked", 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			GameObject newCar = (GameObject)Instantiate(cars[2], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			PlayerPrefs.SetInt ("Car Type", 2);
			PlayerPrefs.Save ();
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	IEnumerator loadNewScene(string level) {
		AsyncOperation async = SceneManager.LoadSceneAsync(level);
		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}
}
