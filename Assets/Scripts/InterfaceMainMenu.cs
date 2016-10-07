using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class InterfaceMainMenu : MonoBehaviour {

	public GameObject[] carModels;
	public GameObject car;

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

	public Text titleText;
	public Text expText;
	public Text loadingText;

	public ScrollRect scrollrect;
	public Scrollbar scrollbarVert;
	public Image viewport;
	public Image handle;

	bool viewSettings = false;
	bool viewStore = false;
	bool loading = false;

	static Vector4 carLocked = new Vector4 (0.25f, 0.25f, 0.25f, 1);
	static Vector4 buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
	static Vector4 scrollBackgrounOn = new Vector4 (0.125f, 0.125f, 0.125f, 0.125f);
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
		//PlayerPrefs.DeleteAll();
		playButton.onClick.AddListener (delegate { playButtonClick (); });
		playBowlingButton.onClick.AddListener (delegate { playBowlingButtonClick (); });
		playDriveButton.onClick.AddListener (delegate { playDrivingButtonClick (); });
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

		PlayerPrefs.SetInt (PlayerPrefManagement.sudan, 1);
		int carNumber = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);
		GameObject newCar = (GameObject)Instantiate (carModels [carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
		setCarPosition (carNumber);
		setInitialText ();
		setInitialHighlightPosition (carNumber);
		menuOn ();
	}
		
	/*
	 *  main menu buttons
	 */

	public void playButtonClick () {
		loadLevel (LevelManagement.floorIt);
	}

	public void playBowlingButtonClick () {		
		loadLevel (LevelManagement.bowl);
	}

	public void playDrivingButtonClick () {
		loadLevel (LevelManagement.drive);
	}

	void loadLevel (string level) {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		loadingText.text = "Loading...";
		PlayerPrefs.SetString (PlayerPrefManagement.level, level);
		if (loading == false) {
			StartCoroutine (loadNewScene (level));
		}
		loading = true;
	}

	IEnumerator loadNewScene (string level) {
		AsyncOperation async = SceneManager.LoadSceneAsync (level);
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

	public void soundEffectsButtonClick () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 1);
			soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: Off";
			Camera.main.GetComponent<SoundEffects> ().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 0);
			soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: On";
			Camera.main.GetComponent<SoundEffects> ().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		}
		PlayerPrefs.Save ();
	}

	public void musicButtonClick () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 1);
			musicButton.GetComponentInChildren<Text> ().text = "Music: Off";
			Camera.main.GetComponent<SoundEffects> ().stopMusic ();
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 0);
			musicButton.GetComponentInChildren<Text> ().text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		}
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		PlayerPrefs.Save ();
	}

	public void vibrationButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 1);
			vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: Off";
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 0);
			vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: On";
			Camera.main.GetComponent<Vibration> ().vibrate ();
		}
		PlayerPrefs.Save ();
	}

	/*
	 *  store buttons
	 */

	public void sudanButtonClick () {
		buyCar (PlayerPrefManagement.sudan, 0, 0, sudanButton);
	}

	public void limoButtonClick () {
		buyCar (PlayerPrefManagement.limo, limoAmount, 1, limoButton);
	}

	public void truckButtonClick () {
		buyCar (PlayerPrefManagement.truck, truckAmount, 2, truckButton);
	}

	public void sportButtonClick () {
		buyCar (PlayerPrefManagement.sport, sportAmount, 3, sportButton);
	}

	public void monsterTruckButtonClick () {
		buyCar (PlayerPrefManagement.monsterTruck, monsterTruckAmount, 4, monsterTruckButton);
	}

	public void coneButtonClick () {
		buyCar (PlayerPrefManagement.cone, coneAmount, 5, coneButton);
	}

	public void busButtonClick () {
		buyCar (PlayerPrefManagement.bus, busAmount, 6, busButton);
	}

	public void abstractButtonClick () {
		buyCar (PlayerPrefManagement.abstractCar, abstractAmount, 7, abstractButton);
	}

	public void buyButtonClick () {
		Camera.main.GetComponent<InAppPurchases> ().BuyConsumable ();
	}

	void buyCar (string carPlayerPref, int amount, int carIndex, Button carButton) {
		if (PlayerPrefs.GetInt (carPlayerPref, 0) == 0 && PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) >= amount) {
			Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
			GameObject newCar = (GameObject)Instantiate (carModels [carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
			PlayerPrefs.SetInt (carPlayerPref, 1);
			PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) - amount);
			PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
			PlayerPrefs.Save ();
			carButton.GetComponent<Image> ().color = textOn;
			carButton.GetComponentInChildren<Text> ().text = "";
			expText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
			GameObject.Find ("Highlight").transform.position = carButton.transform.position;
			setCarPosition (carIndex);
		} else if (PlayerPrefs.GetInt (carPlayerPref, 0) == 1) {
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
			if (PlayerPrefs.GetInt (PlayerPrefManagement.carType, carIndex) != carIndex) {
				GameObject newCar = (GameObject)Instantiate (carModels [carIndex], car.transform.position, car.transform.rotation);
				Destroy (car);
				car = newCar;
				car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
				PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
				PlayerPrefs.Save ();
				GameObject.Find ("Highlight").transform.position = carButton.transform.position;
				setCarPosition (carIndex);
			}
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	/*
	 * image and text manipulation
	 */

	void menuOn () {
		turnOffAll ();
		titleText.text = "Floor It";
		expText.GetComponent<Text> ().color = textOn;
		turnOnButtonAndText (playButton);
		turnOnButtonAndText (playBowlingButton);
		turnOnButtonAndText (playDriveButton);

		turnOnButtonAndText (storeButton);
		storeButton.GetComponentInChildren<Text> ().text = "$";
		turnOnButtonAndText (settingsButton);
		settingsButton.GetComponentInChildren<Text> ().text = "~\n^";
	}

	void settingsOn () {
		turnOffAll ();
		titleText.text = "Settings";
		turnOnButtonAndText (settingsButton);
		settingsButton.GetComponentInChildren<Text> ().text = "<-";

		turnOnButtonAndText (soundButton);
		turnOnButtonAndText (musicButton);
		turnOnButtonAndText (vibrationButton);
	}

	void storeOn () {
		turnOffAll ();
		titleText.text = "Store";
		expText.GetComponent<Text> ().color = textOn;
		storeButton.GetComponentInChildren<Text> ().text = "->";
		turnOnButtonAndText (storeButton);

		scrollrect.GetComponent<ScrollRect> ().enabled = true;
		viewport.GetComponent<Image> ().color = scrollBackgrounOn;
		scrollbarVert.GetComponent<Scrollbar> ().enabled = true;
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

		buyButton.GetComponent<Button> ().enabled = true;
		buyButton.GetComponentInChildren<Text> ().enabled = true;
		buyButton.GetComponent<Image> ().color = carLocked;
		buyButton.GetComponentInChildren<Text> ().color = textOn;
	}

	void turnOffAll () {
		turnOffButtonAndText (storeButton);
		turnOffButtonAndText (settingsButton);
		expText.GetComponent<Text> ().color = noColor;

		if (viewSettings || viewStore) {
			turnOffButtonAndText (playButton);
			turnOffButtonAndText (playBowlingButton);
			turnOffButtonAndText (playDriveButton);
		}

		if (!viewSettings) {
			turnOffButtonAndText (soundButton);
			turnOffButtonAndText (musicButton);
			turnOffButtonAndText (vibrationButton);
		}

		if (!viewStore) {
			scrollrect.GetComponent<ScrollRect> ().enabled = false;
			viewport.GetComponent<Image> ().color = noColor;
			scrollbarVert.GetComponent<Scrollbar> ().enabled = false;
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
		}
	}

	void turnOnButtonAndText (Button b) {
		b.GetComponent<Button> ().enabled = true;
		b.GetComponent<Image> ().color = buttonOn;
		b.GetComponentInChildren<Text> ().enabled = true;
	}

	void turnOffButtonAndText (Button b) {
		b.GetComponent<Button> ().enabled = false;
		b.GetComponent<Image> ().color = noColor;
		b.GetComponentInChildren<Text> ().enabled = false;
	}

	void turnOnCarButton (Button button, string playerPref) {
		button.GetComponent<Button> ().enabled = true;
		button.GetComponentInChildren<Text> ().enabled = true;
		if (PlayerPrefs.GetInt (playerPref, 0) == 0) {
			button.GetComponent<Image> ().color = carLocked;
			button.GetComponentInChildren<Text> ().color = textOn;
		} else {
			button.GetComponent<Image> ().color = textOn;
		}
	}

	/*
	 * Other
	 */

	void setCarPosition (int carNumber) {
		if (carNumber == 1) {
			car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 4) {
			car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 5) {
			car.transform.position = Vector3.zero;
		} else if (carNumber == 6) {
			car.transform.position = new Vector3 (0, 0, 0.4f);
		} else if (carNumber == 7) {
			car.transform.position = new Vector3 (0, 0, 0.3f);
		} else {
			car.transform.position = new Vector3 (0, 0, 0.2f);
		}
	}

	void setInitialHighlightPosition (int carNumber) {
		if (carNumber == 0) {
			GameObject.Find ("Highlight").transform.position = sudanButton.transform.position;
		} else if (carNumber == 1) {
			GameObject.Find ("Highlight").transform.position = limoButton.transform.position;
		} else if (carNumber == 2) {
			GameObject.Find ("Highlight").transform.position = truckButton.transform.position;
		} else if (carNumber == 3) {
			GameObject.Find ("Highlight").transform.position = sportButton.transform.position;
		} else if (carNumber == 4) {
			GameObject.Find ("Highlight").transform.position = monsterTruckButton.transform.position;
		} else if (carNumber == 5) {
			GameObject.Find ("Highlight").transform.position = coneButton.transform.position;
		} else if (carNumber == 6) {
			GameObject.Find ("Highlight").transform.position = busButton.transform.position;
		} else if (carNumber == 7) {
			GameObject.Find ("Highlight").transform.position = abstractButton.transform.position;
		}
	}

	void setInitialText () {
		int highScoreInfinite, highScoreBowling, highScoreDriving;
		int exp = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
		highScoreInfinite = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreFloorIt, 0);
		highScoreBowling = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreBowl, 0);
		highScoreDriving = PlayerPrefs.GetInt (PlayerPrefManagement.highScoreDrive, 0);
		playButton.GetComponentInChildren<Text> ().text = "Floor It\n\nHigh Score\n" + highScoreInfinite;
		playBowlingButton.GetComponentInChildren<Text> ().text = "Bowl\n\nHigh Score\n" + highScoreBowling;
		playDriveButton.GetComponentInChildren<Text> ().text = "Drive\n\nHigh Score\n" + highScoreDriving;
		expText.text = exp + " EXP";
		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0) {
			soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: On";
		} else {
			soundButton.GetComponentInChildren<Text> ().text = "Sound Effects: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0) {
			musicButton.GetComponentInChildren<Text> ().text = "Music: On";
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		} else {
			musicButton.GetComponentInChildren<Text> ().text = "Music: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0) {
			vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: On";
		} else {
			vibrationButton.GetComponentInChildren<Text> ().text = "Vibration: Off";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 0) {
			limoButton.GetComponentInChildren<Text> ().text = limoAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 0) {
			truckButton.GetComponentInChildren<Text> ().text = truckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 0) {
			sportButton.GetComponentInChildren<Text> ().text = sportAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 0) {
			monsterTruckButton.GetComponentInChildren<Text> ().text = monsterTruckAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 0) {
			coneButton.GetComponentInChildren<Text> ().text = coneAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.bus, 0) == 0) {
			busButton.GetComponentInChildren<Text> ().text = busAmount + " EXP";
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.abstractCar, 0) == 0) {
			abstractButton.GetComponentInChildren<Text> ().text = abstractAmount + " EXP";
		}
	}
}
