﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour {

	public AudioClip menuMusic;
	public AudioClip gameplayMusic;
	public AudioClip superMusic;

	public AudioClip dropBlockSound;
	public AudioClip explosionSound;
	public AudioClip ballBounceSound;
	public AudioClip carBounceSound;
	public AudioClip shuffleSound;
	public AudioClip invisibleSound;
	public AudioClip extraCarSound;
	public AudioClip carDeathSound;
	public AudioClip highScoreSound;
	public AudioClip buttonClickSound;
	public AudioClip badChoiceSound;
	public AudioClip boughtItemSound;
	public AudioClip bubbleSound;
	public AudioClip bubblePopSound;
	public AudioClip coinSound;
	public AudioClip resizeBigSound;
	public AudioClip resizeSmallSound;
	public AudioClip accelerationSound;
	public AudioClip decelerationSound;
	public AudioClip multiplierSound;
	public AudioClip multiplierRevertSound;
	public AudioClip expSound;
	public AudioClip evilCarSound;
	public AudioClip metalSound;
	public AudioClip chainOnSound;
	public AudioClip chainOffSound;
	public AudioClip pianoDropSound;
	public AudioClip missileSound;
	public AudioClip hydraulicSound;
	public AudioClip hydraulicDownSound;
	public AudioClip dropRoadBlockSound;
	public AudioClip pickUpRoadBlockSound;

	private AudioSource source;
	AudioSource blockSource;

	public int playSoundEffects;
	public int playMusic;

	void Awake () {
		source = GetComponent<AudioSource>();
		playSoundEffects = 0;
		playMusic = 0;
		playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		playMusic = PlayerPrefs.GetInt (PlayerPrefManagement.music, 0);

		blockSource = GameObject.Find("Block Audio").GetComponent<AudioSource>();
		blockSource.volume = 0.25f;
	}

	public void playMenuMusic() {
		playMusic = PlayerPrefs.GetInt (PlayerPrefManagement.music, 0);
		if (playMusic == 0) {
			source.clip = menuMusic;
			source.loop = true;
			source.Play ();
		}
	}

	public void playGameplayMusic() {
		playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		if (playMusic == 0) {
			source.clip = gameplayMusic;
			source.loop = true;
			source.Play ();
		}
	}

	public void playSuperMusic() {
		if (playMusic == 0) {
			source.clip = superMusic;
			source.loop = true;
			source.Play ();
		}
	}

	public void stopMusic() {
		if (playMusic == 0) {
			source.Stop ();
		}
	}

	public void pauseMusic() {
		if (playMusic == 0) {
			source.Pause ();
		}
	}

	public void unpauseMusic() {
		if (playMusic == 0) {
			source.UnPause ();
		}
	}

	public void playButtonClick() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.PlayOneShot (buttonClickSound);
		}
	}

	public void playPauseButtonClick(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (buttonClickSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.8f));
		}
	}

	public void playBoughtItemSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (boughtItemSound);
		}
	}

	public void playBadChoiceSound() {
		if (playSoundEffects == 0) {
			source.PlayOneShot (badChoiceSound);
		}
	}

	public void playHighScoreSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.clip = highScoreSound;
			source.loop = false;
			source.Play ();
		}
	}  

	public void playExpSound() {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.clip = expSound;
			source.loop = false;
			source.Play ();
		}
	}

	public void playeDropBlockSound() {
		if (playSoundEffects == 0) {
			blockSource.pitch = Random.Range (0.5f, 3f);
			blockSource.clip = dropBlockSound;
			blockSource.Play ();
		}
	}

	public void playExplosionSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (explosionSound, Vector3.Lerp (playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playBallBounceSound(Vector3 playAt, float vol) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (ballBounceSound, Vector3.Lerp(playAt, Camera.main.transform.position, vol));
		}
	}

	public void playMetalSound(Vector3 playAt, float vol) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (metalSound, Vector3.Lerp(playAt, Camera.main.transform.position, vol));
		}
	}

	public void playPianoDropSound(Vector3 playAt, float vol) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (pianoDropSound, Vector3.Lerp(playAt, Camera.main.transform.position, vol));
		}
	}

	public void playBounceSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (carBounceSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.8f));
		}
	}

	public void playShuffleSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (shuffleSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playInvisibleSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (invisibleSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playExtraCarSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (extraCarSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playCarDeathSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (carDeathSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playBubbleSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (bubbleSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playBubblePopSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (bubblePopSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playCoinSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (coinSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playResizeBigSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (resizeBigSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playResizeSmallSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (resizeSmallSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playAccelerationSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (accelerationSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playDecelerationSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (decelerationSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playMultiplierSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (multiplierSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.8f));
		}
	}

	public void playMultiplierRevertSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (multiplierRevertSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playEvilCarSound (AudioSource source) {
		if (playSoundEffects == 0) {
			source.pitch = 1;
			source.clip = evilCarSound;
			source.loop = true;
			source.Play ();
		}
	}

	public void playChainOnSound (AudioSource source, float pitch) {
		if (playSoundEffects == 0) {
			source.pitch = pitch;
			source.clip = chainOnSound;
			source.Play ();
		}
	}

	public void playChainOffSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (chainOffSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playMissileSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (missileSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playHydraulicUpSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (hydraulicSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playHydraulicDownSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (hydraulicDownSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playDropRoadBlockSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (dropRoadBlockSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}

	public void playPickUpRoadBlockSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (pickUpRoadBlockSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.9f));
		}
	}
}
