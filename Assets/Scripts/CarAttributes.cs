﻿using UnityEngine;
using System.Collections;

public class CarAttributes : MonoBehaviour {

	public Material leadCarMaterial;
	public Material regularCarMaterial;
	string level;

	void Start () {
		level = Camera.main.GetComponent<LevelManagement>().level;
	}

	public void changeMaterialOfCars() {
		if(level == LevelManagement.drive){
			for (int i = 0; i < GameObject.FindGameObjectsWithTag (TagManagement.car).Length; i++) {
				Renderer rend = GameObject.FindGameObjectsWithTag (TagManagement.car)[i].GetComponent<Renderer> ();
				Material[] mats = new Material[rend.materials.Length];
				for (int j = 0; j < rend.materials.Length; j++) {
					mats [j] = rend.materials [j];
				}
				for (int j = 0; j < mats.Length; j++){
					if(mats[j].name == "Bumper (Instance)" && i == 0){
						mats [j] = leadCarMaterial;
					} else if (mats[j].name == "Bumper (Instance)" && i != 0){
						mats[j] = regularCarMaterial;
					}
				}
				rend.materials = mats;
			}
		}
	}

	public void resizedTimer(GameObject car){
		car.GetComponent<CarMovement>().resizeCounter += Time.deltaTime;
		if(car.GetComponent<CarMovement>().resizeCounter > CarMovement.resizeLimit){
			if (car.GetComponent<Rigidbody>().mass > Camera.main.GetComponent<CarMangment>().carMass) {
				Camera.main.GetComponent<SoundEffects> ().playResizeSmallSound (car.transform.position);
			} else {
				Camera.main.GetComponent<SoundEffects> ().playResizeBigSound (car.transform.position);
			}
			car.GetComponent<CarMovement>().resizeCounter = 0;
			car.GetComponent<CarMovement>().resized = false;
			car.GetComponent<CarMovement> ().distToGround = car.transform.position.y;
			car.transform.localScale = new Vector3 (1, 1, 1);
			car.GetComponent<Rigidbody>().mass = Camera.main.GetComponent<CarMangment>().carMass;
		}
	}

	public void flyingTimer (GameObject car) {
		car.GetComponent<CarMovement>().flyingTimer += Time.deltaTime;
		Quaternion toRotation = new Quaternion (
			0,
			car.transform.rotation.y,
			0,
			car.transform.rotation.w
		);
		car.transform.rotation = Quaternion.Lerp(car.transform.rotation, toRotation, Time.deltaTime);
		if (car.transform.position.y > 5) {
			car.GetComponent<Rigidbody>().drag = 5;
		}
		if (car.GetComponent<CarMovement>().flyingTimer > CarMovement.flyingTime) {
			car.GetComponent<CarMovement>().flying = false;
			car.GetComponent<CarMovement>().flyingTimer = 0;
			car.GetComponent<Rigidbody>().useGravity = true;
			car.GetComponent<Rigidbody>().angularDrag = 1;
			car.GetComponent<Rigidbody>().drag = 0.5f;
			Behaviour halo = (Behaviour)car.transform.GetChild(0).GetComponent("Halo");
			halo.enabled = false;
			Camera.main.GetComponent<SoundEffects> ().playBubblePopSound (car.transform.position);
		}
	}

	public bool isGrounded (GameObject car) {
		return Physics.Raycast (transform.position, -Vector3.up, car.GetComponent<CarMovement>().distToGround);
	}

	public void jump (GameObject car) {
		if (isGrounded (car) && !car.GetComponent<CarMovement>().gameOver) {
			Quaternion newRoation = new Quaternion (
				transform.rotation.x + (Random.Range (-0.02f, 0.02f)), 
				transform.rotation.y, 
				transform.rotation.z + (Random.Range (-0.02f, 0.02f)), 
				transform.rotation.w
			);
			transform.rotation = Quaternion.Slerp (
				transform.rotation, 
				newRoation, 
				Time.deltaTime * 100
			);
			car.GetComponent<Rigidbody>().velocity += Vector3.up * CarMovement.jumpHeight;
		}
	}

	public void addForce (int multiplier, GameObject car) {
		if (car.GetComponent<CarMovement>().forceTimer > CarMovement.forceLimit) {
			car.GetComponent<Rigidbody>().AddForce (car.GetComponent<Rigidbody>().transform.forward * multiplier);
			car.GetComponent<CarMovement>().forceTimer = 0;
		}
	}

	public void onBlock(Collision hit, GameObject car, Rigidbody rb){
		if (car.tag == TagManagement.evilCar && (hit.transform.tag == TagManagement.car || hit.transform.tag == TagManagement.evilCar)) {
			car.GetComponent<EvilCarAttributes> ().explodeNow = true;
		}
		string blockName = hit.transform.name.Split ('_') [0];
		if (blockName == AllBlockNames.accelerateBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onSpeedBlock (GameObject.Find (hit.transform.name), car);
		} else if (blockName == AllBlockNames.bouncyBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onJumpBlock (car, rb);
		} else if (blockName == AllBlockNames.decelerateBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onSlowDownBlock (GameObject.Find (hit.transform.name), car);
		} else if (blockName == AllBlockNames.extraCarBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onExtraLifeBlock (GameObject.Find (hit.transform.name));
		} else if (blockName == AllBlockNames.flyBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onFlyingBlock (car, rb);
		} else if (blockName == AllBlockNames.shuffleBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onShuffleBlock (GameObject.Find (hit.transform.name));
		} else if (blockName == AllBlockNames.invisibleBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onInvisibleBlock (GameObject.Find (hit.transform.name));
		} else if (blockName == AllBlockNames.pointBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onPointBlock (GameObject.Find (hit.transform.name));
		} else if (blockName == AllBlockNames.sizeBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onSizeBlock (GameObject.Find (hit.transform.name), car);
		} else if (blockName == AllBlockNames.superAccelerateBlock) {
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superSpeedBlockActivated = true;
			}
		} else if (blockName == AllBlockNames.superDecelerateBlock) {
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superSlowBlockActivated = true;
			}
		} else if (blockName == AllBlockNames.superBlock) {
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superBlockActivated = true;
			}
		} else if (blockName == AllBlockNames.superBouncyBlock) {
			Camera.main.GetComponent<BlockAttributes> ().onJumpBlock (car, rb);
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superBouncyBlockActivated = true;
			}
		} else if (blockName == AllBlockNames.superBullseyeBlock) {
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superBullseyeBlockActivated = true;
			}
		} else if (blockName == AllBlockNames.superPointBlock) {
			if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
				Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), car);
				Camera.main.GetComponent<AddBlock> ().superPointBlockActivated = true;
			}
		} 
	}
}
