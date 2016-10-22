using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Analytics;
using Assets.Pixelation.Scripts;

public class Interface : MonoBehaviour {

	public Button restartButton, mainMenuButton ,pauseButton, leftButton, rightButton, jumpButton;

	public Text highScoreText ,loadingText, scoreText, blockPointText ,carPointText, speedText, instructionsText;
	public Text multiplierText, expText;

	public Sprite accelerate, decelerate, bullseye, bouncy, fly, car, point, resizeBig, multiThree, multiTwo;
	public Sprite hill, jagged, shuffle, invisible, standard, super, bombT, bombX, resizeSmall, evilCar;
	public Image nextBlockSprite, nextBlockBackground, jumpProgressBar;
	public Sprite pause, resume;

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

	bool blockPointOn = false;
	float blockPointAlpha = 0;
	bool carPointOn = false;
	float carPointAlpha = 0;
	float instructionsAlpha = 1;
	bool multiplierBig = false;

	float carSpeed;
	float scoreAndSpeedUpdateCount;
	static float scoreAndSpeedUpdateLimit = 0.15f;

	float exp;
	float score;
	float timePassed;
	float playSoundTime;
	static float playSoundsLimit = 0.075f;
	public float lastJumpTime = 5;
	static int jumpTimeLimit = 5;
	float deltaTime;

	void Start () {
		buttonOn = new Vector4 (1, 1, 1, 1);
		buttonOff = new Vector4 (0.5f, 0.5f, 0.5f, 0);
		jumpButtonOff = new Vector4 (0.1f, 0.1f, 0.1f, 0.1f);
		textOn = new Vector4 (1, 1, 1, 1);
		textOff = new Vector4 (1, 1, 1, 0);
		restartButton.onClick.AddListener(delegate { restartButtonClick(); });
		mainMenuButton.onClick.AddListener(delegate { menuButtonClick(); });
		turnOnOrOffButton (restartButton, false);
		turnOnOrOffButton (mainMenuButton, false);
		level = Camera.main.GetComponent<LevelManagement>().level;
		exp = PlayerPrefs.GetInt (PlayerPrefManagement.exp, 0);
		setVisual ();
		Time.timeScale = 1;
	}

	void Update(){
		deltaTime = Time.deltaTime;
		if (!Camera.main.GetComponent<Interface> ().paused && !Camera.main.GetComponent<CarMangment> ().trueGameOver) {
			updateGameplayInterface ();
		} 
		if (Camera.main.GetComponent<CarMangment>().trueGameOver) {
			updateGameOverInterface ();
		}
	}

	/*
	 * gameplay interface 
	 */

	void updateGameplayInterface (){
		updateScoreAndSpeed ();
		textEffects ();
		if (level == LevelManagement.drive) {
			updateJumpInterface ();
		}
	}

	void updateScoreAndSpeed () {
		scoreAndSpeedUpdateCount += deltaTime;
		if (scoreAndSpeedUpdateCount > scoreAndSpeedUpdateLimit) {
			scoreAndSpeedUpdateCount = 0;
			if (Camera.main.GetComponent<CarMangment> ().cars.Length > 0) {
				carSpeed = Camera.main.GetComponent<CarMangment> ().cars [0].GetComponent<CarMovement> ().speedometer;
			}
			float normalizedSpeed = Mathf.Round (carSpeed * 2 * 60) / 10;
			score = Mathf.FloorToInt (Camera.main.GetComponent<Points> ().total);
			scoreText.text = score.ToString () + "\n";
			speedText.text = string.Format("{0:F1}\nu/m", normalizedSpeed);
		}
	}

	void textEffects () {
		if (blockPointOn) {
			blockPointAlpha -= deltaTime / 2;
			blockPointText.GetComponent<Text> ().color = new Color (1, 1, 1, blockPointAlpha);
			if (blockPointAlpha < 0) {
				blockPointOn = false;
			}
		}
		if (carPointOn) {
			carPointAlpha -= deltaTime / 2;
			carPointText.GetComponent<Text> ().color = new Color (1, 1, 1, carPointAlpha);
			if (carPointAlpha < 0) {
				carPointOn = false;
			}
		}
		if (instructionsAlpha > 0) {
			instructionsAlpha -= deltaTime / 4;
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
	}

	void updateJumpInterface () {
		if (lastJumpTime < jumpTimeLimit) {
			lastJumpTime += deltaTime;
			float progressBarScale = jumpProgressBar.transform.localScale.x + deltaTime/jumpTimeLimit;
			jumpProgressBar.transform.localScale = new Vector3 (progressBarScale, 1, 1);
			if (jumpButton.enabled) {
				jumpProgressBar.color = textOn;
				jumpButton.GetComponent<Button> ().enabled = false;
				jumpButton.GetComponent<Image> ().color = jumpButtonOff;
			}
		} else {
			if (!jumpButton.enabled) {
				jumpProgressBar.color = buttonOff;
				jumpButton.GetComponent<Button> ().enabled = true;
				jumpButton.GetComponent<Image> ().color = buttonOn;
			}
		}
		if (Input.GetButtonDown ("Jump") && lastJumpTime >= jumpTimeLimit) {
			Camera.main.GetComponent<CarAttributes>().jump();
		}
	}

	/*
	 * gameover interface 
	 */

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
		blockPointText.GetComponent<Text> ().color = textOff;
		multiplierText.GetComponent<Text> ().color = textOff;
		speedText.GetComponent<Text> ().color = textOff;
		carPointText.GetComponent<Text> ().color = textOff;
		if (level == LevelManagement.floorIt) {
			nextBlockSprite.GetComponent<Image> ().color = buttonOff;
			nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score\n" + Camera.main.GetComponent<Points> ().highscoreInfinite;
				sendToAnalytics (Camera.main.GetComponent<Points> ().highscoreInfinite);
			} else {
				highScoreText.text = "High Score\n" + Camera.main.GetComponent<Points> ().highscoreInfinite;
			}
		} else if (level == LevelManagement.bowl) {
			nextBlockSprite.GetComponent<Image> ().color = buttonOff;
			nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score\n" + Camera.main.GetComponent<Points> ().highscoreBowling;
				sendToAnalytics (Camera.main.GetComponent<Points> ().highscoreBowling);
			} else {
				highScoreText.text = "High Score\n" + Camera.main.GetComponent<Points> ().highscoreBowling;
			}
		} else if (level == LevelManagement.drive) {			
			turnOffDriveButtons();
			jumpProgressBar.GetComponent<Image> ().color = buttonOff;
			if (Camera.main.GetComponent<Points> ().newHighScore) {
				highScoreText.text = "New High Score\n" + Camera.main.GetComponent<Points> ().highscoreDriving;
				sendToAnalytics (Camera.main.GetComponent<Points> ().highscoreDriving);
			} else {
				highScoreText.text = "High Score\n" + Camera.main.GetComponent<Points> ().highscoreDriving;
			}
		}
	}

	void sendToAnalytics (int highscore) {
		Analytics.CustomEvent("gameOver", new Dictionary<string, object> {
			{ "Highscore in " + Camera.main.GetComponent<LevelManagement> ().level, highscore },
			{ "car", Camera.main.GetComponent<CarMangment>().carNum }
		});
	}

	/*
	 * Button functions
	 */

	public void pauseButtonClick() {
		paused = !paused;
		Camera.main.GetComponent<SoundEffects> ().playButtonClick ();
		if (paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			Time.timeScale = 0;
			loadingText.GetComponentInChildren<Text>().text = "Paused";
			pauseButton.GetComponentInChildren<Image> ().sprite = resume;
			turnOnMainButtons ();
			Camera.main.GetComponent<SoundEffects> ().pauseMusic ();
			if (level == LevelManagement.drive) {
				turnOffDriveButtons ();
				jumpProgressBar.GetComponent<Image> ().color = buttonOff;
			} else {
				nextBlockSprite.GetComponent<Image> ().color = buttonOff;
				nextBlockBackground.GetComponent<Image> ().color = buttonOff;
			}
		}
		if (!paused && !Camera.main.GetComponent<CarMangment>().trueGameOver) {
			Time.timeScale = 1;
			loadingText.GetComponentInChildren<Text>().text = "";
			pauseButton.GetComponentInChildren<Image> ().sprite = pause;
			turnOffMainButtons ();
			Camera.main.GetComponent<SoundEffects> ().unpauseMusic ();
			if (level == LevelManagement.drive) {
				turnOnDriveButtons ();
			} else {
				nextBlockSprite.GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.9f);
				nextBlockBackground.GetComponent<Image> ().color = new Vector4 (0.5f, 0.5f, 0.5f, 0.5f);
			}
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

	/*
	 * Drive buttons for car movement 
	 */

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

	/*
	 * public tools 
	 */
		
	public void changePointsText(float pointAmount, GameObject obj){
		blockPointText.text = "+" + pointAmount;
		blockPointOn = true;
		blockPointAlpha = 1.0f;
		blockPointText.GetComponent<Text> ().color = textOn;
		Vector3 pointPosition = new Vector3 (
			obj.transform.position.x,
			obj.transform.position.y,
			obj.transform.position.z + 1
		);
		blockPointText.GetComponent<Text> ().rectTransform.position = Camera.main.WorldToScreenPoint(pointPosition);
	}

	public void changeCarPointsText(float pointAmount) {
		carPointText.text = "+" + pointAmount;
		carPointOn = true;
		carPointAlpha = 1.0f;
		carPointText.GetComponent<Text> ().color = textOn;
	}

	public void multiplierOn(){
		multiplierText.GetComponent<Text> ().color = textOn;
		multiplierBig = true;
	}

	public void multiplierOff(){
		multiplierText.GetComponent<Text> ().color = textOff;
		multiplierText.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
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

	/*
	 * class tools 
	 */

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

	void turnOnDriveButtons() {
		if (!leftButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (leftButton, true);
			turnOnOrOffButton (rightButton, true);
			turnOnOrOffButton (jumpButton, true);
		}
	}

	void turnOffDriveButtons() {
		if (leftButton.GetComponent<Button> ().enabled) {
			turnOnOrOffButton (leftButton, false);
			turnOnOrOffButton (rightButton, false);
			turnOnOrOffButton (jumpButton, false);
		}
	}

	void turnOnOrOffButton(Button button, bool setting){
		button.GetComponent<Button> ().enabled = setting;
		if (setting) {
			button.GetComponent<Image> ().raycastTarget = true;
			button.interactable = true;
			button.GetComponent<Image> ().color = buttonOn;
		} else {
			button.GetComponent<Image> ().raycastTarget = false;
			button.interactable = false;
			button.GetComponent<Image> ().color = buttonOff;
		}
	}

	void setVisual () {
		GameObject.Find ("Directional Light").GetComponent<Light> ().intensity = 1;
		Color sky = new Color (0.75f, 0.75f, 0.75f, 0.5f);
		RenderSettings.skybox.SetColor ("_Tint", sky);
		Camera.main.GetComponent<EdgeDetection> ().enabled = false;
		Camera.main.GetComponent<Chunky> ().enabled = false;
		if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 1) {
			GameObject.Find ("Directional Light").GetComponent<Light> ().intensity = 0;
			sky = new Color (0.5f, 0.5f, 0.5f, 0.5f);
			RenderSettings.skybox.SetColor ("_Tint", sky);
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 2) {
			Camera.main.GetComponent<EdgeDetection> ().enabled = true;
		} else if (PlayerPrefs.GetInt (PlayerPrefManagement.visual) == 3) {;
			Camera.main.GetComponent<Chunky> ().enabled = true;
		}
	}
}
