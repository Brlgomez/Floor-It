﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllBlockAttributes : MonoBehaviour {

	static float onSpeedBlockAcc = 3;
	static int speedUpForce = 130;
	static int speedDownForce = -120;
	static float onJumpBlockHeight = 4;
	static float sizeBig = 1.5f;
	static float sizeSmall = 0.5f;

	static int pointBlockPoints = 10;
	static int accelerateBlockPoints = 2;
	static int decelerateBlockPoints = 2;
	static int jumpBlockPoints = 2;
	static int flyBlockPoints = 3;
	static int shuffleBlockPoints = 3;
	static int invisibleBlockPoints = 3;
	static int sizeBlockPoints = 3;
	public int blockActivated = 0;

	float numberOfCars;
	public float chainCount = 0;

	public void onSpeedBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playAccelerationSound (block.transform.position);
			float multiplier = car.transform.localScale.x * car.GetComponent<Rigidbody> ().mass;
			float force = speedUpForce * multiplier;
			Camera.main.GetComponent<CarAttributes>().addForce ((int)force, car);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (accelerateBlockPoints, block);
				if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
					float currentSpeed = Camera.main.GetComponent<FollowCar> ().leadCar.GetComponent<CarMovement> ().speed;
					float fasterSpeed = CarMovement.fastestSpeed;
					if (currentSpeed < fasterSpeed) {
						changeSpeedOfAllCars (onSpeedBlockAcc);
					}
				}
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			car.GetComponentsInChildren<ParticleSystem> () [0].Play ();
		}
	}

	public void onSlowDownBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playDecelerationSound (block.transform.position);
			float multiplier = car.transform.localScale.x * car.GetComponent<Rigidbody> ().mass;
			float force = speedDownForce * multiplier;
			Camera.main.GetComponent<CarAttributes>().addForce ((int)force, car);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (decelerateBlockPoints, block);
				if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
					float currentSpeed = Camera.main.GetComponent<FollowCar> ().leadCar.GetComponent<CarMovement> ().speed;
					float slowestSpeed = car.GetComponent<CarMovement> ().slowestSpeed;
					if (currentSpeed > slowestSpeed) {
						changeSpeedOfAllCars (-currentSpeed * 2.5f);
					}
				}
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
		}
	}

	public void changeSpeedOfAllCars (float multiplier) {
		GameObject[] aliveCars = Camera.main.GetComponent<CarMangment> ().cars;
		for (int i = 0; i < aliveCars.Length; i++) {
			if (aliveCars [i] != null && !aliveCars [i].GetComponent<CarMovement> ().gameOver) {
				aliveCars [i].GetComponent<CarMovement> ().speed += Time.deltaTime * (multiplier);
			}
		}
	}

	public void onJumpBlock (GameObject car, Rigidbody rb) {
		if (rb.velocity.y < onJumpBlockHeight && !car.GetComponent<CarMovement> ().flying) {
			Camera.main.GetComponent<SoundEffects> ().playBounceSound (car.transform.position);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (jumpBlockPoints, car);
			}
			rb.velocity += Vector3.up * onJumpBlockHeight;
		}
	}

	public void onFlyingBlock (GameObject car, Rigidbody rb) {
		if (!car.GetComponent<CarMovement> ().flying) {
			car.GetComponent<CarMovement> ().flying = true;
			Camera.main.GetComponent<SoundEffects> ().playBubbleSound (transform.position);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (flyBlockPoints, car);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			Behaviour halo = (Behaviour)car.transform.GetChild (0).GetComponent ("Halo");
			halo.enabled = true;
			rb.useGravity = false;
			rb.angularDrag = 100;
			rb.velocity += Vector3.up / 2;
		}
	}

	public void onExtraLifeBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playExtraCarSound (block.transform.position);
			if (car.tag == TagManagement.car) {
				List<float> aliveCarsZPos = new List<float> ();
				for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
					if (!Camera.main.GetComponent<CarMangment> ().cars [i].GetComponent<CarMovement> ().gameOver) {
						aliveCarsZPos.Add (Camera.main.GetComponent<CarMangment> ().cars [i].transform.position.z);
					}
				}
				aliveCarsZPos.Sort ();
				aliveCarsZPos.Reverse ();
				float previousZ = 0;
				float currentZ = 0;
				float spawnZ = 0;
				GameObject lastCar = Camera.main.GetComponent<FollowCar> ().lastCar;
				for (int i = 0; i < aliveCarsZPos.Count; i++) {
					currentZ = aliveCarsZPos [i];
					float zPos = Camera.main.GetComponent<CarMangment> ().newCarSpawnDist;
					if (lastCar.transform.localScale.x > 1) {
						zPos *= lastCar.transform.localScale.x;
					}
					if (previousZ != 0 && previousZ - currentZ > -zPos + 1) {
						spawnZ = previousZ + zPos;
						break;
					}
					previousZ = currentZ;
				}
				GameObject temp = Camera.main.GetComponent<CarMangment> ().cars [0];
				GameObject nextCar = Instantiate (temp);
				nextCar.name = "Car_" + numberOfCars;
				nextCar.GetComponent<CarMovement> ().speed = temp.GetComponent<CarMovement> ().speed;
				nextCar.GetComponent<CarMovement> ().resized = false;
				nextCar.transform.localScale = new Vector3 (1, 1, 1);
				numberOfCars++;
				if (spawnZ == 0) {
					float zPos = Camera.main.GetComponent<CarMangment> ().newCarSpawnDist;
					if (lastCar.transform.localScale.x > 1) {
						zPos *= lastCar.transform.localScale.x;
					}
					nextCar.transform.position = new Vector3 (
						lastCar.transform.position.x + Random.Range (-0.5f, 0.5f), 
						lastCar.GetComponent<CarMovement> ().distToGround, 
						lastCar.transform.position.z + zPos 
					);
				} else {
					nextCar.transform.position = new Vector3 (
						temp.transform.position.x + Random.Range (-0.5f, 0.5f), 
						temp.GetComponent<CarMovement> ().distToGround, 
						spawnZ
					);
				}
				if (Camera.main.GetComponent<LevelManagement> ().level == LevelManagement.bowl) {
					nextCar.transform.rotation = new Quaternion (0, 0, 0, temp.transform.rotation.w);
				} else {
					nextCar.transform.rotation = new Quaternion (0, temp.transform.rotation.y, 0, temp.transform.rotation.w);
				}
				nextCar.GetComponent<Rigidbody> ().mass = Camera.main.GetComponent<CarMangment> ().carMass;
				nextCar.GetComponentsInChildren<ParticleSystem> () [1].Play ();
				Camera.main.GetComponent<CarAttributes> ().changeMaterialOfCars ();
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
				if (GameObject.FindGameObjectsWithTag (TagManagement.car).Length >= 5) {
					Camera.main.GetComponent<OnlineServices> ().revealFiveFriendlyCarsAchievement ();
				}
			} else {
				spawnEvilCar (block, GameObject.FindGameObjectsWithTag (TagManagement.car) [0].GetComponent<CarMovement> ().speed);
			}
		}
	}

	public void onShuffleBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playShuffleSound (block.transform.position);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (shuffleBlockPoints, block);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			GameObject[] allBlocks = GameObject.FindGameObjectsWithTag (TagManagement.blockOnRoad);
			for (int i = 0; i < allBlocks.Length; i++) {
				Vector3 tmp = allBlocks [i].transform.position;
				int rand = Random.Range (i, allBlocks.Length);
				allBlocks [i].transform.position = allBlocks [rand].transform.position;
				allBlocks [rand].transform.position = tmp;
			}
		}
	}

	public void onInvisibleBlock (GameObject block, GameObject car) {
		Material clearMaterial = GameObject.Find ("ClearBlock").GetComponent<Renderer> ().material; 
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playInvisibleSound (block.transform.position);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (invisibleBlockPoints, block);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			GameObject[] allBlocks = GameObject.FindGameObjectsWithTag (TagManagement.blockOnRoad);
			for (int i = 0; i < allBlocks.Length; i++) {
				if (allBlocks [i].name.Split ('_') [0] == AllBlockNames.bombBlock) {
					allBlocks [i].GetComponent<BombAttributes> ().isTransparent = true;
					continue;
				}
				for (int j = 0; j < AllBlockNames.blocksThatCanBeDeactivated.Length; j++) {
					if (allBlocks [i].name.Split ('_') [0] == AllBlockNames.blocksThatCanBeDeactivated [j]) {
						allBlocks [i].GetComponent<BlockActivated> ().isTransparent = true;
						break;
					}
				}
				allBlocks [i].GetComponent<Renderer> ().material = clearMaterial;
			}
		}
	}

	public void onPointBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<SoundEffects> ().playCoinSound (block.transform.position);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (pointBlockPoints, block);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
		}
	}

	public void onSizeBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().incrementPoints (sizeBlockPoints, block);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			if (block.GetComponent<SizeBlockAttributes> ().big) {
				car.transform.localScale = new Vector3 (sizeBig, sizeBig, sizeBig);
				car.GetComponent<CarMovement> ().distToGround *= sizeBig;
				car.GetComponent<Rigidbody> ().mass = Camera.main.GetComponent<CarMangment>().carMass;
				car.GetComponent<Rigidbody> ().mass *= sizeBig;
				Camera.main.GetComponent<SoundEffects> ().playResizeBigSound (car.transform.position);
			} else {
				car.transform.localScale = new Vector3 (sizeSmall, sizeSmall, sizeSmall);
				car.GetComponent<CarMovement> ().distToGround *= sizeSmall;
				car.GetComponent<Rigidbody> ().mass = Camera.main.GetComponent<CarMangment>().carMass;
				car.GetComponent<Rigidbody> ().mass *= sizeSmall;
				Camera.main.GetComponent<SoundEffects> ().playResizeSmallSound (car.transform.position);
			}
			car.GetComponent<CarMovement> ().resized = true;
			car.GetComponent<CarMovement> ().resizeCounter = 0;
		}
	}

	public void onSuperBlock (GameObject block, GameObject car) {
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
				Camera.main.GetComponent<OnlineServices> ().revealOnSpecialBlockAchievement ();
			} else if (car.tag == TagManagement.evilCar) {
				Camera.main.GetComponent<OnlineServices> ().revealBombOnSpecialBlockAchievement ();
			}
			string blockName = block.name.Split ('_') [0];
			Camera.main.GetComponent<Interface> ().setTextureOverlay (blockName);
			Camera.main.GetComponent<SoundEffects> ().playSuperMusic ();
			Camera.main.GetComponent<AddBlock> ().superTimerCount = 0;
			Camera.main.GetComponent<AddBlock> ().superBullseyeBlockActivated = false;
			Camera.main.GetComponent<AddBlock> ().superBouncyBlockActivated = false;
			Camera.main.GetComponent<AddBlock> ().superSlowBlockActivated = false;
			Camera.main.GetComponent<AddBlock> ().superSpeedBlockActivated = false;
			Camera.main.GetComponent<AddBlock> ().superBlockActivated = false;
			Camera.main.GetComponent<AddBlock> ().superPointBlockActivated = false;
			block.GetComponent<LensFlare> ().enabled = false;
			if (blockName == AllBlockNames.superDecelerateBlock) {
				onSlowDownBlock (block, car);
				for (int i = 0; i < Camera.main.GetComponent<CarMangment> ().cars.Length; i++) {
					Camera.main.GetComponent<CarMangment> ().cars [i].GetComponent<CarMovement> ().acceleration *= 1.25f;
				}
				block.GetComponent<BlockActivated> ().activated (true);
			} else if (blockName == AllBlockNames.superAccelerateBlock) {
				onSpeedBlock (block, car);
				block.GetComponent<BlockActivated> ().activated (true);
			} else if (blockName == AllBlockNames.superPointBlock) {
				Camera.main.GetComponent<Points> ().incrementPoints (pointBlockPoints, block);
				Camera.main.GetComponent<SoundEffects> ().playCoinSound (block.transform.position);
				block.GetComponent<BlockActivated> ().activated (true);
			} else {
				block.GetComponent<BlockActivated> ().activated (false);
			}
		}
	}

	public void spawnBall (GameObject block) {
		int randomYSpawnPosition = Random.Range (10, 30);
		if (Random.Range (0, 100) > 10) {
			GameObject sphereTemp = GameObject.Find ("Sphere");
			GameObject sphere = Instantiate (sphereTemp);
			sphere.AddComponent<RigidbodySounds> ();
			sphere.name = sphereTemp.name + "_Clone";
			sphere.transform.position = new Vector3 (
				block.transform.position.x, 
				randomYSpawnPosition, 
				block.transform.position.z
			);
			sphere.transform.Rotate(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
			sphere.GetComponent<Rigidbody> ().velocity += Vector3.forward * Random.Range(-0.25f, 0.25f);
			sphere.GetComponent<Rigidbody> ().velocity += Vector3.right * Random.Range(-0.25f, 0.25f);
			sphere.GetComponent<Rigidbody> ().useGravity = true;
			sphere.GetComponent<Rigidbody> ().isKinematic = false;
		} else {
			GameObject temp = GameObject.Find ("Anvil");
			GameObject anvil = Instantiate (temp);
			anvil.AddComponent<RigidbodySounds> ();
			anvil.name = temp.name + "_Clone";
			anvil.transform.Rotate (Random.Range (-15f, 15f), Random.Range (0.0f, 360.0f), Random.Range (-15f, 15f));
			anvil.transform.position = new Vector3 (
				block.transform.position.x + Random.Range (-0.5f, 0.5f), 
				randomYSpawnPosition, 
				block.transform.position.z + Random.Range (-0.5f, 0.5f)
			);
			anvil.GetComponent<Rigidbody> ().useGravity = true;
			anvil.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

	public void spawnEvilCar (GameObject block, float leadCarSpeed) {
		float ySpawnPosition = 0.5f;
		float evilCarSpeed = leadCarSpeed * 1.1f;
		GameObject evilCarTemp = GameObject.Find (TagManagement.evilCar);
		GameObject evilCar = Instantiate (evilCarTemp);
		evilCar.AddComponent<CarMovement> ();
		evilCar.GetComponent<CarMovement> ().speed = evilCarSpeed;
		evilCar.AddComponent<EvilCarAttributes> ();
		evilCar.name = evilCarTemp.name + "_Clone";
		evilCar.GetComponent<Rigidbody> ().useGravity = true;
		evilCar.GetComponent<Rigidbody> ().isKinematic = false;
		evilCar.transform.position = new Vector3 (
			block.transform.position.x, 
			ySpawnPosition,
			block.transform.position.z
		);
		evilCar.GetComponentsInChildren<ParticleSystem> () [1].Play ();
	}

	public void spawnObject (GameObject block) {
		float rand = Random.Range (0, 10);
		string obj;
		GameObject temp;
		GameObject nextObject;
		if (rand < 2) {
			obj = "BrickWall";
			temp = GameObject.Find (obj);
			nextObject = Instantiate (temp);
			nextObject.transform.position = new Vector3 (block.transform.position.x, 0.5f, block.transform.position.z);
			nextObject.transform.Rotate (transform.rotation.x, Random.Range (0.0f, 360.0f), transform.rotation.z);
			Rigidbody[] bricks = nextObject.GetComponentsInChildren<Rigidbody> ();
			foreach (Rigidbody brick in bricks) {
				brick.GetComponent<Rigidbody> ().useGravity = true;
				brick.GetComponent<Rigidbody> ().isKinematic = false;
			}
			nextObject.transform.DetachChildren ();
			Destroy (nextObject);
		} else if (rand < 7) {
			obj = "Cone";
			temp = GameObject.Find (obj);
			nextObject = Instantiate (temp);
			nextObject.transform.position = new Vector3 (
				block.transform.position.x + Random.Range (-0.75f, 0.75f), 
				0.5f, 
				block.transform.position.z + Random.Range (-0.75f, 0.75f)
			);
			nextObject.transform.Rotate (Random.Range (-15f, 15f), Random.Range (0.0f, 360.0f), Random.Range (-15f, 15f));
			nextObject.GetComponent<Rigidbody> ().useGravity = true;
			nextObject.GetComponent<Rigidbody> ().isKinematic = false;
		} else {
			obj = "Ramp";
			temp = GameObject.Find (obj);
			nextObject = Instantiate (temp);
			nextObject.transform.position = new Vector3 (block.transform.position.x, 0.5f, block.transform.position.z);
			nextObject.transform.Rotate (Random.Range (-15f, 15f), Random.Range (-30f, 30f), Random.Range (-15f, 15f));
			nextObject.GetComponent<Rigidbody> ().useGravity = true;
			nextObject.GetComponent<Rigidbody> ().isKinematic = false;
		} 
		nextObject.name = temp.name + "_Clone";
	}

	public void onMultiplayerBlock (GameObject block, GameObject car){
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			if (car.tag == TagManagement.car) {
				Camera.main.GetComponent<Points> ().multiplier = block.GetComponent<MultiplierBlockAttributes> ().multiplier;
				Camera.main.GetComponent<Points> ().multiplierCount = 0;
				Camera.main.GetComponent<Interface> ().multiplierText.text = "x" + block.GetComponent<MultiplierBlockAttributes> ().multiplier + "\n";
				Camera.main.GetComponent<Interface> ().multiplierOn ();
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				blockActivated++;
			}
			Camera.main.GetComponent<SoundEffects> ().playMultiplierSound (block.transform.position);
		}
	}

	public void onChainBlock (GameObject block, GameObject car){
		if (!block.GetComponent<BlockActivated> ().hasActivated) {
			block.GetComponent<BlockActivated> ().activated (true);
			Camera.main.GetComponent<AddBlock> ().canSpawnChain = true;
			if (car.tag == TagManagement.car) {
				chainCount++;
				Camera.main.GetComponent<Points> ().incrementPoints (chainCount, block);
				Camera.main.GetComponent<PlayerPrefManagement> ().increaseBlocksActivated ();
				Camera.main.GetComponent<SoundEffects> ().playChainOnSound (Camera.main.GetComponent<AudioSource>(), 0.95f + (chainCount * 0.05f));
				blockActivated++;
			} else {
				Camera.main.GetComponent<SoundEffects> ().playChainOffSound (block.transform.position);
				chainCount = 0;
			}
		}
	}
}
