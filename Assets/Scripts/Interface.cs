using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class Interface : MonoBehaviour {

	public Button restartButton;
	public Text restartText;
	public Button mainMenuButton;
	public Text mainMenuText;
	public Text highScoreText;
	public Button pauseButton;
	public Text pauseText;

	public Text loadingText;
	public Text score;
	public Text pointText;
	public Text carPointText;
	public Text speed;
	public Text instructions;
	public Text multiplier;

	public Button leftButton;
	public Text leftText;
	public Button rightButton;
	public Text rightText;

	public Sprite accelerate, decelerate, bullseye, bouncy, fly, car, point, resizeBig, multiThree, multiTwo;
	public Sprite hill, jagged, shuffle, invisible, standard, super, bombT, bombX, resizeSmall, evilCar;
	public Image nextBlockSprite;
	public Image nextBlockBackground;

	public Texture2D superAccelerateOverlay, superDecelerateOverlay, superBullseyeOverlay;
	public Texture2D superBouncyOverlay, superOverlay, superPointOverlay;

	public bool paused;
	string level;

	Vector4 buttonOn;
	Vector4 buttonOff;
	Vector4 textOn;
	Vector4 textOff;

	bool loading;

	public float carSpeed;
	float updateCount;
	float updateLimit;

	bool gotPoints;
	bool carPoints;
	float pointAlpha;
	float carPointAlpha;
	float instructionsAlpha;

	void Start () {
		buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
		buttonOff = new Vector4 (0.5f, 0.5f, 0.5f, 0);
		textOn = new Vector4 (1, 1, 1, 1);
		textOff = new Vector4 (1, 1, 1, 0);

		restartButton.GetComponent<Button>().enabled = false;
		restartButton.GetComponent<Image> ().color = buttonOff;
		restartText.GetComponent<Text> ().color = textOff;
		restartButton.onClick.AddListener(delegate { restartButtonClick(); });
		mainMenuButton.GetComponent<Button>().enabled = false;
		mainMenuButton.GetComponent<Image> ().color = buttonOff;
		mainMenuText.GetComponent<Text> ().color = textOff;
		mainMenuButton.onClick.AddListener(delegate { menuButtonClick(); });
		highScoreText.GetComponent<Text> ().color = textOff;

		level = Camera.main.GetComponent<LevelManagement>().level;
		paused = false;
		Time.timeScale = 1;
		loadingText.text = "";
		loading = false;

		nextBlockSprite = GameObject.Find ("NextBlock").GetComponent<Image> ();

		updateLimit = 0.25f;

		gotPoints = false;
		carPoints = false;
		pointAlpha = 0;
		carPointAlpha = 0;
		instructionsAlpha = 1;
	}

	void Update(){
		if (!Camera.main.GetComponent<Interface> ().paused) {
			updateGUI ();
		}
		if (Camera.main.GetComponent<CarMangment>().trueGameOver) {
			trueGameOver ();
		}
	}

	public void updateGUI(){
		updateCount += Time.deltaTime;
		if (updateCount > updateLimit) {
			updateCount = 0;
			if (Camera.main.GetComponent<CarMangment> ().cars.Length != 0) {
				if (Camera.main.GetComponent<CarMangment> ().cars [0] != null) {
					carSpeed = Camera.main.GetComponent<CarMangment> ().cars [0].GetComponent<CarMovement> ().speedometer;
				} 
			}
			if (carSpeed != 0) {
				float normalizedSpeed = Mathf.Round (carSpeed * 100) / 10;
				score.text = Mathf.FloorToInt (Camera.main.GetComponent<Points>().total * Camera.main.GetComponent<Points>().highestMulti).ToString ();
				speed.text = string.Format("{0:F1}\nm/s", normalizedSpeed);
			}
		}
		if (gotPoints) {
			pointAlpha -= Time.deltaTime * 0.75f;
			pointText.GetComponent<Text> ().color = new Color (1, 1, 1, pointAlpha);
			if (pointAlpha < 0) {
				gotPoints = false;
			}
		}
		if (carPoints) {
			carPointAlpha -= Time.deltaTime * 0.75f;
			carPointText.GetComponent<Text> ().color = new Color (1, 1, 1, carPointAlpha);
			if (carPointAlpha < 0) {
				carPoints = false;
			}
		}
		if (instructionsAlpha > 0) {
			instructionsAlpha -= Time.deltaTime * 0.25f;
			instructions.GetComponent<Text> ().color = new Color (1, 1, 1, instructionsAlpha);
		} 
	}

	public void multiplierOn(){
		multiplier.GetComponent<Text> ().color = textOn;
	}

	public void multiplierOff(){
		multiplier.GetComponent<Text> ().color = textOff;
	}

	public void trueGameOver(){
		if (!restartButton.GetComponent<Button> ().enabled) {
			turnOnMainButtons ();
			pauseButton.GetComponent<Button> ().enabled = false;
			pauseButton.GetComponent<Image> ().color = buttonOff;
			pauseText.GetComponent<Text> ().color = textOff;
			highScoreText.GetComponent<Text> ().color = textOn;
			pointText.GetComponent<Text> ().color = textOff;
			multiplier.GetComponent<Text> ().color = textOff;
		}
		if (level == LevelManagement.floorIt) {
			nextBlockSprite.GetComponent<Image> ().color = buttonOff;
			nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score " + Camera.main.GetComponent<Points> ().highscoreInfinite;
			} else {
				highScoreText.text = "High Score " + Camera.main.GetComponent<Points> ().highscoreInfinite;
			}
		} else if (level == LevelManagement.bowl) {
			nextBlockSprite.GetComponent<Image> ().color = buttonOff;
			nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score " + Camera.main.GetComponent<Points> ().highscoreBowling;
			} else {
				highScoreText.text = "High Score " + Camera.main.GetComponent<Points> ().highscoreBowling;
			}
		} else if (level == LevelManagement.drive) {			
			turnOffLeftandRightButtons ();
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score " + Camera.main.GetComponent<Points> ().highscoreDriving;
			} else {
				highScoreText.text = "High Score " + Camera.main.GetComponent<Points> ().highscoreDriving;
			}
		}
	}

	public void pauseButtonClick() {
		paused = !paused;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			Time.timeScale = 0;
			pauseText.text = "I>";
			turnOnMainButtons ();
			Camera.main.GetComponent<SoundEffects> ().pauseMusic ();
			if (level == LevelManagement.drive) {
				turnOffLeftandRightButtons ();
			} else {
				nextBlockSprite.GetComponent<Image> ().color = buttonOff;
				nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			}
		}
		if (!paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			Time.timeScale = 1;
			pauseText.text = "II";
			turnOffMainButtons ();
			Camera.main.GetComponent<SoundEffects> ().unpauseMusic ();
			if (level == LevelManagement.drive) {
				turnOnLeftandRightButtons ();
			} else {
				nextBlockSprite.GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.9f);
				nextBlockBackground.GetComponent<Image> ().color = new Vector4 (0.5f, 0.5f, 0.5f, 0.5f);
			}
		}
	}

	void turnOnMainButtons() {
		restartButton.GetComponent<Button> ().enabled = true;
		restartButton.GetComponent<Image> ().color = buttonOn;
		restartText.GetComponent<Text> ().color = textOn;
		mainMenuButton.GetComponent<Button> ().enabled = true;
		mainMenuButton.GetComponent<Image> ().color = buttonOn;
		mainMenuText.GetComponent<Text> ().color = textOn;
	}

	void turnOffMainButtons() {
		restartButton.GetComponent<Button>().enabled = false;
		restartButton.GetComponent<Image> ().color = buttonOff;
		restartText.GetComponent<Text> ().color = textOff;
		mainMenuButton.GetComponent<Button>().enabled = false;
		mainMenuButton.GetComponent<Image> ().color = buttonOff;
		mainMenuText.GetComponent<Text> ().color = textOff;
	}

	void turnOnLeftandRightButtons() {
		leftButton.GetComponent<Button> ().enabled = true;
		leftButton.GetComponent<Image> ().color = buttonOn;
		leftText.GetComponent<Text> ().color = textOn;
		rightButton.GetComponent<Button> ().enabled = true;
		rightButton.GetComponent<Image> ().color = buttonOn;
		rightText.GetComponent<Text> ().color = textOn;
	}

	void turnOffLeftandRightButtons() {
		leftButton.GetComponent<Button> ().enabled = false;
		leftButton.GetComponent<Image> ().color = buttonOff;
		leftText.GetComponent<Text> ().color = textOff;
		rightButton.GetComponent<Button> ().enabled = false;
		rightButton.GetComponent<Image> ().color = buttonOff;
		rightText.GetComponent<Text> ().color = textOff;
	}

	public void restartButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		loadingText.text = "Loading...";
		if (loading == false) {
			StartCoroutine (loadNewScene (SceneManager.GetActiveScene ().name));
		}
		loading = true;
	}

	public void menuButtonClick() {
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		Time.timeScale = 1;
		loadingText.text = "Loading...";
		if (loading == false) {
			StartCoroutine (loadNewScene (LevelManagement.mainMenu));
		}
		loading = true;
	}

	IEnumerator loadNewScene(string level) {
		yield return null;
		SceneManager.LoadScene (level);
	}

	public void onPointerDownLeftButton() {
		Camera.main.GetComponent<MakeCarsTurn>().leftButtonPressed = true;
	}

	public void onPointerUpLeftButton() {
		Camera.main.GetComponent<MakeCarsTurn>().leftButtonPressed = false;
	}

	public void onPointerDownRightButton() {
		Camera.main.GetComponent<MakeCarsTurn>().rightButtonPressed = true;
	}

	public void onPointerUpRightButton() {
		Camera.main.GetComponent<MakeCarsTurn>().rightButtonPressed = false;
	}

	public void changeHUDSprite (string blockName, string fullBlockName) {
		if (blockName == AllBlockNames.hillBlock) {
			nextBlockSprite.sprite = hill;
		} else if (blockName == AllBlockNames.jaggedBlock) {
			nextBlockSprite.sprite = jagged;
		} else if (blockName == AllBlockNames.decelerateBlock || blockName == AllBlockNames.superDecelerateBlock) {
			nextBlockSprite.sprite = decelerate;
		} else if (blockName == AllBlockNames.accelerateBlock || blockName == AllBlockNames.superAccelerateBlock) {
			nextBlockSprite.sprite = accelerate;
		} else if (blockName == AllBlockNames.bouncyBlock || blockName == AllBlockNames.superBouncyBlock) {
			nextBlockSprite.sprite = bouncy;
		} else if (blockName == AllBlockNames.bullseyeBlock || blockName == AllBlockNames.superBullseyeBlock) {
			nextBlockSprite.sprite = bullseye;
		} else if (blockName == AllBlockNames.pointBlock || blockName == AllBlockNames.superPointBlock) {
			nextBlockSprite.sprite = point;
		} else if (blockName == AllBlockNames.flyBlock) {
			nextBlockSprite.sprite = fly;
		} else if (blockName == AllBlockNames.extraCarBlock) {
			nextBlockSprite.sprite = car;
		} else if (blockName == AllBlockNames.shuffleBlock) {
			nextBlockSprite.sprite = shuffle;
		} else if (blockName == AllBlockNames.invisibleBlock) {
			nextBlockSprite.sprite = invisible;
		} else if (blockName == AllBlockNames.evilCarBlock) {
			nextBlockSprite.sprite = evilCar;
		} else if (blockName == AllBlockNames.bombBlock) {
			if (GameObject.Find (fullBlockName).GetComponent<BombAttributes> ().isBombX) {
				nextBlockSprite.sprite = bombX;
			} else {
				nextBlockSprite.sprite = bombT;
			}
		} else if (blockName == AllBlockNames.superBlock) {
			nextBlockSprite.sprite = super;
		} else if (blockName == AllBlockNames.sizeBlock) {
			if (GameObject.Find (fullBlockName).GetComponent<SizeBlockAttributes> ().big) {
				nextBlockSprite.sprite = resizeBig;
			} else {
				nextBlockSprite.sprite = resizeSmall;
			}
		} else if (blockName == AllBlockNames.multiplierBlock) {
			if (GameObject.Find (fullBlockName).GetComponent<MultiplierBlockAttributes> ().timesTwo) {
				nextBlockSprite.sprite = multiTwo;
			} else {
				nextBlockSprite.sprite = multiThree;
			}
		} else {
			nextBlockSprite.sprite = standard;
		}
	}

	public void setTextureOverlay(string blockName){
		Texture2D overlay = superOverlay;
		if (blockName == AllBlockNames.superDecelerateBlock) {
			overlay = superDecelerateOverlay;
		} else if (blockName == AllBlockNames.superAccelerateBlock) {
			overlay = superAccelerateOverlay;
		} else if (blockName == AllBlockNames.superBouncyBlock) {
			overlay = superBouncyOverlay;
		} else if (blockName == AllBlockNames.superBullseyeBlock) {
			overlay = superBullseyeOverlay;
		} else if (blockName == AllBlockNames.superPointBlock) {
			overlay = superPointOverlay;
		} else if (blockName == AllBlockNames.superBlock) {
			overlay = superOverlay;
		} 			
		Camera.main.GetComponent<ScreenOverlay> ().intensity = 1;
		Camera.main.GetComponent<ScreenOverlay> ().texture = overlay;
	}

	public void disableTextureOverlay(){ 			
		Camera.main.GetComponent<ScreenOverlay> ().intensity = 0;
	}

	public void changePointsText(float pointAmount, GameObject obj){
		pointText.text = "+" + pointAmount;
		gotPoints = true;
		pointAlpha = 1.0f;
		pointText.GetComponent<Text> ().color = textOn;
		Vector3 pointPosition = new Vector3 (
			obj.transform.position.x,
			obj.transform.position.y,
			obj.transform.position.z + 1
		);
		pointText.GetComponent<Text> ().rectTransform.position = Camera.main.WorldToScreenPoint(pointPosition);
	}

	public void changeCarPointsText(float pointAmount) {
		carPointText.text = "+" + pointAmount;
		carPoints = true;
		carPointAlpha = 1.0f;
		carPointText.GetComponent<Text> ().color = textOn;
	}
}
