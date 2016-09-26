using UnityEngine;
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

	private AudioSource source;

	public int playSoundEffects;
	public int playMusic;

	void Awake () {
		source = GetComponent<AudioSource>();
		playSoundEffects = 0;
		playMusic = 0;
		playSoundEffects = PlayerPrefs.GetInt (PlayerPrefManagement.soundEffects, 0);
		playMusic = PlayerPrefs.GetInt (PlayerPrefManagement.music, 0);
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
			source.PlayOneShot (buttonClickSound);
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
			source.clip = highScoreSound;
			source.loop = false;
			source.Play ();
		}
	}

	public void playeDropBlockSound(Vector3 playAt) {
		if (playSoundEffects == 0) {
			AudioSource.PlayClipAtPoint (dropBlockSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.8f));
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
			AudioSource.PlayClipAtPoint (multiplierRevertSound, Vector3.Lerp(playAt, Camera.main.transform.position, 0.99f));
		}
	}
}
