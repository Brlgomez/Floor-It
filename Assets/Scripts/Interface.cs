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
	public Button mainMenuButton;
	public Button pauseButton;
	public Button leftButton;
	public Button rightButton;
	public Button jumpButton;

	public Text highScoreText;
	public Text loadingText;
	public Text scoreText;
	public Text pointText;
	public Text carPointText;
	public Text speedText;
	public Text instructionsText;
	public Text multiplierText;
	public Text expText;

	public Sprite accelerate, decelerate, bullseye, bouncy, fly, car, point, resizeBig, multiThree, multiTwo;
	public Sprite hill, jagged, shuffle, invisible, standard, super, bombT, bombX, resizeSmall, evilCar;
	public Image nextBlockSprite;
	public Image nextBlockBackground;
	public Image jumpProgressBar;

	public Texture2D superAccelerateOverlay, superDecelerateOverlay, superBullseyeOverlay;
	public Texture2D superBouncyOverlay, superOverlay, superPointOverlay;

	Vector4 buttonOn;
	Vector4 buttonOff;
	Vector4 jumpButtonOff;
	Vector4 textOn;
	Vector4 textOff;

	string level;

	public bool paused = false;
	bool loading = false;

	float carSpeed;
	float updateCount;
	static float updateLimit = 0.25f;

	bool gotPoints = false;
	bool carPoints = false;
	float pointAlpha = 0;
	float carPointAlpha = 0;
	float instructionsAlpha = 1;
	bool multiplierBig = false;

	float deltaTime;
	float exp;
	float score;
	float timePassed;
	static float playSoundsLimit = 0.075f;
	float playSoundTime;

	public float lastJumpTime = 5;
	static int jumpTimeLimit = 5;

	void Start () {
		buttonOn = new Vector4 (0.5f, 0.5f, 0.5f, 1);
		buttonOff = new Vector4 (0.5f, 0.5f, 0.5f, 0);
		jumpButtonOff = new Vector4 (0.5f, 0.5f, 0.5f, 0.1f);
		textOn = new Vector4 (1, 1, 1, 1);
		textOff = new Vector4 (1, 1, 1, 0);

		restartButton.onClick.AddListener(delegate { restartButtonClick(); });
		mainMenuButton.onClick.AddListener(delegate { menuButtonClick(); });

		turnOnOrOffButton (restartButton, false);
		turnOnOrOffButton (mainMenuButton, false);

		highScoreText.GetComponent<Text> ().color = textOff;

		level = Camera.main.GetComponent<LevelManagement>().level;
		Time.timeScale = 1;
		loadingText.text = "";

		nextBlockSprite = GameObject.Find ("NextBlock").GetComponent<Image> ();

		exp = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
	}

	void Update(){
		deltaTime = Time.deltaTime;
		if (!Camera.main.GetComponent<Interface> ().paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			updateGameplayInterface ();
		}
		if (Camera.main.GetComponent<CarMangment>().trueGameOver) {
			updateGameOverInterface ();
		}
	}

	void updateGameplayInterface (){
		updateCount += deltaTime;
		if (updateCount > updateLimit) {
			updateCount = 0;
			if (Camera.main.GetComponent<CarMangment> ().cars.Length != 0) {
				if (Camera.main.GetComponent<CarMangment> ().cars [0] != null) {
					carSpeed = Camera.main.GetComponent<CarMangment> ().cars [0].GetComponent<CarMovement> ().speedometer;
				} 
			}
			if (carSpeed != 0) {
				float normalizedSpeed = Mathf.Round (carSpeed * 100) / 10;
				score = Mathf.FloorToInt (Camera.main.GetComponent<Points> ().total);
				scoreText.text = score.ToString () + "\n";
				speedText.text = string.Format("{0:F1}\nm/s", normalizedSpeed);
			}
		}
		if (gotPoints) {
			pointAlpha -= deltaTime * 0.75f;
			pointText.GetComponent<Text> ().color = new Color (1, 1, 1, pointAlpha);
			if (pointAlpha < 0) {
				gotPoints = false;
			}
		}
		if (carPoints) {
			carPointAlpha -= deltaTime * 0.75f;
			carPointText.GetComponent<Text> ().color = new Color (1, 1, 1, carPointAlpha);
			if (carPointAlpha < 0) {
				carPoints = false;
			}
		}
		if (instructionsAlpha > 0) {
			instructionsAlpha -= deltaTime * 0.25f;
			instructionsText.GetComponent<Text> ().color = new Color (1, 1, 1, instructionsAlpha);
		} 
		if (multiplierBig) {
			if (multiplierText.transform.localScale.x <= 1) {
				float multiScale = multiplierText.transform.localScale.x + deltaTime * 3;
				multiplierText.transform.localScale = new Vector3 (multiScale, multiScale, multiScale);
			} else {
				multiplierBig = false;
			}
		}
		if (level == LevelManagement.drive) {
			if (lastJumpTime < jumpTimeLimit) {
				lastJumpTime += deltaTime;
				float progressBarScale = jumpProgressBar.transform.localScale.x + deltaTime/jumpTimeLimit;
				jumpProgressBar.transform.localScale = new Vector3 (progressBarScale, 1, 1);
				if (jumpButton.enabled) {
					jumpProgressBar.color = textOn;
					jumpButton.GetComponent<Button> ().enabled = false;
					jumpButton.GetComponent<Image> ().color = jumpButtonOff;
					jumpButton.GetComponentInChildren<Text> ().color = jumpButtonOff;
				}
			} else {
				if (!jumpButton.enabled) {
					jumpProgressBar.color = buttonOff;
					jumpButton.GetComponent<Button> ().enabled = true;
					jumpButton.GetComponent<Image> ().color = buttonOn;
					jumpButton.GetComponentInChildren<Text> ().color = textOn;
				}
			}
			if (Input.GetButtonDown ("Jump") && lastJumpTime >= jumpTimeLimit) {
				Camera.main.GetComponent<CarAttributes>().jump();
			}
		}
	}

	void updateGameOverInterface (){
		if (score >= 0) {
			scoreText.transform.position = Vector3.Lerp (
				scoreText.transform.position, 
				GameObject.Find ("Instructions").transform.position, 
				deltaTime * 3
			);
			if (Vector2.Distance (scoreText.transform.position, GameObject.Find ("Instructions").transform.position) < 1) {
				expText.text = exp + " EXP";
				timePassed += deltaTime;
				playSoundTime += deltaTime;
				if (playSoundTime > playSoundsLimit) {
					playSoundTime = 0;
					Camera.main.GetComponent<SoundEffects> ().playExpSound ();
				}
				if (timePassed > (0.1f / score) || score == 0) {
					int decrementAmount = Mathf.CeilToInt(score / 100);
					if (decrementAmount == 0) {
						decrementAmount = 1;
					}
					score -= decrementAmount;
					exp += decrementAmount;
					timePassed = 0;
				}
				if (!mainMenuButton.enabled) {
					turnOnMainButtons ();
				}
			}
		}
	}

	public void gameOverInterface(){
		float total = Camera.main.GetComponent<Points> ().total;
		float tempMulti = Camera.main.GetComponent<Points> ().highestMulti;
		float multi = tempMulti;
		if (multi == 0) {
			multi = 1;
		}
		score = total * multi;
		if (tempMulti == 0) {
			scoreText.text = "Score\n" + total;
		} else if (tempMulti == 1) {
			scoreText.text = "Score\n" + total + " x " + multi + " pin = " + total * multi;
		} else {
			scoreText.text = "Score\n" + total + " x " + multi + " pins = " + total * multi;
		}
		turnOnOrOffButton (pauseButton, false);
		highScoreText.GetComponent<Text> ().color = textOn;
		pointText.GetComponent<Text> ().color = textOff;
		multiplierText.GetComponent<Text> ().color = textOff;
		speedText.GetComponent<Text> ().color = textOff;
		carPointText.GetComponent<Text> ().color = textOff;
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
			jumpProgressBar.GetComponent<Image> ().color = buttonOff;
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
			pauseButton.GetComponentInChildren<Text>().text = "I>";
			turnOnMainButtons ();
			Camera.main.GetComponent<SoundEffects> ().pauseMusic ();
			if (level == LevelManagement.drive) {
				turnOffLeftandRightButtons ();
				jumpProgressBar.GetComponent<Image> ().color = buttonOff;
			} else {
				nextBlockSprite.GetComponent<Image> ().color = buttonOff;
				nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			}
		}
		if (!paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			Time.timeScale = 1;
			pauseButton.GetComponentInChildren<Text>().text = "II";
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
		
	public void multiplierOn(){
		multiplierText.GetComponent<Text> ().color = textOn;
		multiplierBig = true;
	}

	public void multiplierOff(){
		multiplierText.GetComponent<Text> ().color = textOff;
		multiplierText.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	void turnOnMainButtons() {
		if (!restartButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (restartButton, true);
			turnOnOrOffButton (mainMenuButton, true);
		}
	}

	void turnOffMainButtons() {
		if (restartButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (restartButton, false);
			turnOnOrOffButton (mainMenuButton, false);
		}
	}

	void turnOnLeftandRightButtons() {
		if (!leftButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (leftButton, true);
			turnOnOrOffButton (rightButton, true);
			turnOnOrOffButton (jumpButton, true);
		}
	}

	void turnOffLeftandRightButtons() {
		if (leftButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (leftButton, false);
			turnOnOrOffButton (rightButton, false);
			turnOnOrOffButton (jumpButton, false);
		}
	}

	void turnOnOrOffButton(Button button, bool setting){
		button.GetComponent<Button> ().enabled = setting;
		if (setting) {
			button.GetComponent<Image> ().color = buttonOn;
			button.GetComponentInChildren<Text> ().color = textOn;
		} else {
			button.GetComponent<Image> ().color = buttonOff;
			button.GetComponentInChildren<Text> ().color = textOff;
		}
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

	public void onPointerDownJumpButton() {
		if (lastJumpTime >= jumpTimeLimit) {
			Camera.main.GetComponent<CarAttributes>().jump();
		}
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
		Camera.main.GetComponent<ScreenOverlay> ().texture = overlay;
		Camera.main.GetComponent<ScreenOverlay> ().enabled = true;
	}

	public void disableTextureOverlay(){ 	
		Camera.main.GetComponent<ScreenOverlay> ().enabled = false;
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
