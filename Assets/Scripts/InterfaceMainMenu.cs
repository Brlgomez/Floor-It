using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Pixelation.Scripts;
using UnityStandardAssets.ImageEffects;

public class InterfaceMainMenu : MonoBehaviour {

	public GameObject[] carModels;
	public GameObject car;

	public Button playButton, playBowlingButton, playDriveButton;
	public Button storeButton, settingsButton, statsButton, leaderboardButton, achievementButton;
	public Button soundButton, musicButton, vibrationButton;
	public Button sudanButton, limoButton, truckButton, sportButton, monsterTruckButton, coneButton, busButton, abstractButton;
	public Button normalVisual, nightVisual, outlineVisual, pixelVisual;
	public Button confirmYesButton, confirmNoButton, backButton;
	public Button buyButton;

	public Text titleText, expText;

	public ScrollRect scrollrect;
	public Scrollbar scrollbarVert;
	public Image viewport;
	public Image handle;

	public Text statsText;
	public Image statsBackGround;
	public Image confirmationScreen;
	public Text confirmationText;
	public Image confirmationImage;

	public bool viewLevelSelect = false;
	public bool viewSettings = false;
	public bool viewStore = false;
	public bool viewStats = false;
	public bool viewConfirmation = false;
	bool loading = false;
	bool carConfirmation = false;

	public static int truckAmount = 500;
	public static int sportAmount = 2000;
	public static int busAmount = 6000;
	public static int limoAmount = 12000;
	public static int monsterTruckAmount = 20000;
	public static int abstractAmount = 30000;
	public static int coneAmount = 52427;

	string globalCarPlayerPref;
	int globalAmount;
	int globalCarIndex;
	Button globalCarButton; 

	public ParticleSystem particles;

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
		statsButton.onClick.AddListener (delegate { statsButtonClick (); });
		leaderboardButton.onClick.AddListener (delegate { leaderboardButtonClick (); });
		achievementButton.onClick.AddListener (delegate { achievementButtonClick (); });
		sudanButton.onClick.AddListener (delegate { sudanButtonClick (); });
		limoButton.onClick.AddListener (delegate { limoButtonClick (); });
		truckButton.onClick.AddListener (delegate { truckButtonClick (); });
		sportButton.onClick.AddListener (delegate { sportButtonClick (); });
		monsterTruckButton.onClick.AddListener (delegate { monsterTruckButtonClick (); });
		coneButton.onClick.AddListener (delegate { coneButtonClick (); });
		busButton.onClick.AddListener (delegate { busButtonClick (); });
		abstractButton.onClick.AddListener (delegate { abstractButtonClick (); });
		buyButton.onClick.AddListener (delegate { buyButtonClick (); });
		backButton.onClick.AddListener (delegate { backButtonClick (); });
		confirmYesButton.onClick.AddListener (delegate { confirmYes (); });
		confirmNoButton.onClick.AddListener (delegate { confirmNo (); });
		normalVisual.onClick.AddListener (delegate { normalVisualButtonClick (); });
		nightVisual.onClick.AddListener (delegate { nightVisualButtonClick (); });
		outlineVisual.onClick.AddListener (delegate { outlineVisualButtonClick (); });
		pixelVisual.onClick.AddListener (delegate { pixelVisualButtonClick (); });

		setVisual ();
		PlayerPrefs.SetInt (PlayerPrefManagement.normalVisual, 1);
		PlayerPrefs.SetInt (PlayerPrefManagement.sudan, 1);
		int carNumber = PlayerPrefs.GetInt (PlayerPrefManagement.carType, 0);
		GameObject newCar = (GameObject)Instantiate (carModels [carNumber], car.transform.position, car.transform.rotation);
		Destroy (car);
		car = newCar;
		car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
		Camera.main.GetComponent<InterfaceMainMenuTools>().setCarPosition (carNumber);
		Camera.main.GetComponent<InterfaceMainMenuTools>().setInitialText ();
		Camera.main.GetComponent<InterfaceMainMenuTools>().setInitialHighlightPosition (carNumber);
		Camera.main.GetComponent<InterfaceMainMenuTools>().menuOn ();
		viewLevelSelect = true;
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
		Camera.main.GetComponent<MainMenuCameraMovement> ().loading = true;
		titleText.text = "Loading...";
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
		viewSettings = true;
		viewLevelSelect = false;
		if (viewSettings) {
			Camera.main.GetComponent<InterfaceMainMenuTools>().settingsOn ();
		} else {
			Camera.main.GetComponent<InterfaceMainMenuTools>().menuOn ();
		}
		Camera.main.GetComponent<interfaceMainMenuMovement> ().titleShift = true;
	}

	public void storeButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		viewStore = true;
		viewLevelSelect = false;
		if (viewStore) {
			Camera.main.GetComponent<InterfaceMainMenuTools>().storeOn ();
			Camera.main.GetComponent<InAppPurchases> ().checkReceipts ();
		} else {
			Camera.main.GetComponent<InterfaceMainMenuTools>().menuOn ();
		}
		Camera.main.GetComponent<interfaceMainMenuMovement> ().titleShift = true;
	}

	public void statsButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		viewStats = true;
		viewLevelSelect = false;
		if (viewStats) {
			Camera.main.GetComponent<InterfaceMainMenuTools>().statsOn ();
		} else {
			Camera.main.GetComponent<InterfaceMainMenuTools>().menuOn ();
		}
		Camera.main.GetComponent<interfaceMainMenuMovement> ().titleShift = true;
	}

	public void achievementButtonClick () {
		Camera.main.GetComponent<OnlineServices> ().revealUnlockAchievements ();
		Camera.main.GetComponent<OnlineServices> ().activatedAchievements ();
	}

	public void leaderboardButtonClick () {
		Camera.main.GetComponent<OnlineServices> ().activateLeaderBoards ();
	}

	public void backButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		viewStats = false;
		viewStore = false;
		viewSettings = false;
		viewConfirmation = false;
		viewLevelSelect = true;
		Camera.main.GetComponent<InterfaceMainMenuTools>().menuOn ();
		Camera.main.GetComponent<interfaceMainMenuMovement> ().titleShift = true;
	}
		
	/*
	 *  settings buttons
	 */

	public void soundEffectsButtonClick () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 1);
			soundButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().soundButtonOff;
			Camera.main.GetComponent<SoundEffects> ().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.soundEffects, 0);
			soundButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().soundButtonOn;
			Camera.main.GetComponent<SoundEffects> ().playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
			Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		}
		PlayerPrefs.Save ();
	}

	public void musicButtonClick () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.music, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 1);
			musicButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().musicButtonOff;
			Camera.main.GetComponent<SoundEffects> ().stopMusic ();
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.music, 0);
			musicButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().musicButtonOn;
			Camera.main.GetComponent<SoundEffects> ().playMenuMusic ();
		}
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		PlayerPrefs.Save ();
	}

	public void vibrationButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (PlayerPrefs.GetInt (PlayerPrefManagement.vibration, 0) == 0) {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 1);
			vibrationButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().vibrationButtonOff;
		} else {
			PlayerPrefs.SetInt (PlayerPrefManagement.vibration, 0);
			vibrationButton.GetComponentInChildren<Image> ().sprite = Camera.main.GetComponent<InterfaceMainMenuTools>().vibrationButtonOn;
			Camera.main.GetComponent<Vibration> ().vibrate ();
		}
		PlayerPrefs.Save ();
	}

	/*
	 *  store buttons
	 */

	public void sudanButtonClick () {
		confirmationPopUp (PlayerPrefManagement.sudan, 0, 0, sudanButton);
	}

	public void limoButtonClick () {
		confirmationPopUp (PlayerPrefManagement.limo, limoAmount, 1, limoButton);
	}

	public void truckButtonClick () {
		confirmationPopUp (PlayerPrefManagement.truck, truckAmount, 2, truckButton);
	}

	public void sportButtonClick () {
		confirmationPopUp (PlayerPrefManagement.sport, sportAmount, 3, sportButton);
	}

	public void monsterTruckButtonClick () {
		confirmationPopUp (PlayerPrefManagement.monsterTruck, monsterTruckAmount, 4, monsterTruckButton);
	}

	public void coneButtonClick () {
		confirmationPopUp (PlayerPrefManagement.cone, coneAmount, 5, coneButton);
	}

	public void busButtonClick () {
		confirmationPopUp (PlayerPrefManagement.bus, busAmount, 6, busButton);
	}

	public void abstractButtonClick () {
		confirmationPopUp (PlayerPrefManagement.abstractCar, abstractAmount, 7, abstractButton);
	}
		
	public void normalVisualButtonClick () {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		setVisualPref (PlayerPrefManagement.normalVisual);
	}

	public void nightVisualButtonClick () {
		visualConfirmationPopUp (PlayerPrefManagement.nightVisual, nightVisual);
	}

	public void outlineVisualButtonClick () {}

	public void pixelVisualButtonClick () {
		visualConfirmationPopUp (PlayerPrefManagement.pixelVisual, pixelVisual);
	}

	public void buyButtonClick () {
		buyConfirmationPopUp ("buy", buyButton);
	}

	void buyConfirmationPopUp (string playerPref, Button button) {
		carConfirmation = false;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		globalCarPlayerPref = playerPref;
		globalAmount = 0;
		Camera.main.GetComponent<InterfaceMainMenuTools>().turnOffAll ();
		Camera.main.GetComponent<InterfaceMainMenuTools>().confirmationOn (playerPref, 0, button.GetComponent<Image>().sprite);
	}

	void visualConfirmationPopUp (string playerPref, Button button) {
		carConfirmation = false;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (PlayerPrefs.GetInt (playerPref, 0) == 0) {
			globalCarPlayerPref = playerPref;
			globalAmount = 0;
			Camera.main.GetComponent<InterfaceMainMenuTools>().turnOffAll ();
			Camera.main.GetComponent<InterfaceMainMenuTools>().confirmationOn (playerPref, 0, button.GetComponent<Image>().sprite);
		} else {
			setVisualPref (playerPref);
		}
	}

	void confirmationPopUp (string carPlayerPref, int amount, int carIndex, Button carButton) {
		carConfirmation = true;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (PlayerPrefs.GetInt (carPlayerPref, 0) == 0) {
			Camera.main.GetComponent<InterfaceMainMenuTools>().turnOffAll ();
			globalCarPlayerPref = carPlayerPref;
			globalAmount = amount;
			globalCarIndex = carIndex;
			globalCarButton = carButton;
			Camera.main.GetComponent<InterfaceMainMenuTools>().confirmationOn (carPlayerPref, amount, carButton.GetComponent<Image>().sprite);
		} else {
			setCar (carPlayerPref, carIndex, carButton);
		}
	}

	void confirmYes () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) >= globalAmount) {
			if (carConfirmation) {
				Camera.main.GetComponent<SoundEffects> ().playBoughtItemSound ();
				GameObject newCar = (GameObject)Instantiate (carModels [globalCarIndex], car.transform.position, car.transform.rotation);
				Destroy (car);
				car = newCar;
				car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
				PlayerPrefs.SetInt (globalCarPlayerPref, 1);
				PlayerPrefs.SetInt (PlayerPrefManagement.exp, PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) - globalAmount);
				PlayerPrefs.SetInt (PlayerPrefManagement.carType, globalCarIndex);
				PlayerPrefs.Save ();
				globalCarButton.GetComponentInChildren<Text> ().text = "";
				expText.text = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0) + " EXP";
				GameObject.Find ("Highlight").transform.position = globalCarButton.transform.position;
				Camera.main.GetComponent<InterfaceMainMenuTools> ().setCarPosition (globalCarIndex);
				Camera.main.GetComponent<InterfaceMainMenuTools> ().storeOn ();
				Camera.main.GetComponent<OnlineServices> ().revealUnlockAchievements ();
				particles.Play ();
			} else {
				if (globalCarPlayerPref == PlayerPrefManagement.nightVisual) {
					Camera.main.GetComponent<InAppPurchases> ().BuyNonConsumableNight ();
				} else if (globalCarPlayerPref == PlayerPrefManagement.pixelVisual) {
					Camera.main.GetComponent<InAppPurchases> ().BuyNonConsumableClassic ();
				} else if (globalCarPlayerPref == "buy") {
					Camera.main.GetComponent<InAppPurchases> ().BuyConsumable ();
				}
			}
		} else {
			Camera.main.GetComponent<SoundEffects> ().playBadChoiceSound ();
		}
	}

	void confirmNo () {
		viewStore = true;
		viewConfirmation = false;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		Camera.main.GetComponent<InterfaceMainMenuTools>().storeOn ();
	}

	void setCar (string carPlayerPref, int carIndex, Button carButton) {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.carType, carIndex) != carIndex) {
			GameObject newCar = (GameObject)Instantiate (carModels [carIndex], car.transform.position, car.transform.rotation);
			Destroy (car);
			car = newCar;
			car.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
			PlayerPrefs.SetInt (PlayerPrefManagement.carType, carIndex);
			PlayerPrefs.Save ();
			GameObject.Find ("Highlight").transform.position = carButton.transform.position;
			Camera.main.GetComponent<InterfaceMainMenuTools>().setCarPosition (carIndex);
			particles.Play ();
		}
	}

	public void setVisualPref (string visualName) {
		if (visualName == PlayerPrefManagement.normalVisual) {
			PlayerPrefs.SetInt (PlayerPrefManagement.visual, 0);
			GameObject.Find ("Visual Highlight").transform.position = normalVisual.transform.position;
		} else if (visualName == PlayerPrefManagement.nightVisual) {
			PlayerPrefs.SetInt (PlayerPrefManagement.visual, 1);
			GameObject.Find ("Visual Highlight").transform.position = nightVisual.transform.position;
			nightVisual.GetComponentInChildren<Text> ().text = "";
		} else if (visualName == PlayerPrefManagement.outlineVisual) {
			PlayerPrefs.SetInt (PlayerPrefManagement.visual, 2);
			GameObject.Find ("Visual Highlight").transform.position = outlineVisual.transform.position;
			outlineVisual.GetComponentInChildren<Text> ().text = "";
		}  else if (visualName == PlayerPrefManagement.pixelVisual) {
			PlayerPrefs.SetInt (PlayerPrefManagement.visual, 3);
			GameObject.Find ("Visual Highlight").transform.position = pixelVisual.transform.position;
			pixelVisual.GetComponentInChildren<Text> ().text = "";
		} 
		setVisual ();
	}

	public void setVisual () {
		GameObject.Find ("Directional Light").GetComponent<Light> ().intensity = 1;
		Color sky = new Color (0.75f, 0.75f, 0.75f, 0.5f);
		RenderSettings.skybox.SetColor ("_Tint", sky);
		Camera.main.GetComponent<EdgeDetection> ().enabled = false;
		Camera.main.GetComponent<Chunky> ().enabled = false;
		GameObject.Find ("Point light").GetComponent<Light> ().enabled = false;
		if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 1) {
			GameObject.Find ("Directional Light").GetComponent<Light> ().intensity = 0;
			sky = new Color (0.5f, 0.5f, 0.5f, 0.5f);
			RenderSettings.skybox.SetColor ("_Tint", sky);
			GameObject.Find ("Point light").GetComponent<Light> ().enabled = true;
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 2) {
			Camera.main.GetComponent<EdgeDetection> ().enabled = true;
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 3) {;
			Camera.main.GetComponent<Chunky> ().enabled = true;
		}
	}
}
