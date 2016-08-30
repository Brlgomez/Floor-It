using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	private Rigidbody rb;
	GameObject invisibleFloor;
	//public Material leadCarMaterial;
	//public Material regularCarMaterial;

	public bool gameOver;
	public bool flying;
	public float speed;
	public float speedometer;
	Vector3 lastPos;

	public float slowestSpeed;
	public float fastestSpeed;
	public float acceleration;
	float jumpHeight;
	public float distToGround;

	float yPosFallingBarrier;
	float carFlippedLimit;

	float flyingTimer;
	float flyingTime;

	float forceTimer;
	float forceLimit = 0.5f;

	//string level;

	public bool resized = false;
	public float resizeCounter;
	float resizeLimit = 10;

	public bool carFlipped;

	void Start () {
		//level = Camera.main.GetComponent<LevelManagement>().level;

		rb = GetComponent<Rigidbody> ();
		invisibleFloor = GameObject.Find ("InvisibleFloor");
		Physics.IgnoreCollision (invisibleFloor.GetComponent<Collider> (), GetComponent<MeshCollider> ());
		Physics.IgnoreCollision (invisibleFloor.GetComponent<Collider> (), GetComponent<BoxCollider> ());

		gameOver = false;
		flying = false;
		if (speed == 0) {
			speed = 0.5f;
		}

		slowestSpeed = 0.5f;
		fastestSpeed = 10.0f;
		acceleration = 0.01f;
		jumpHeight = 5.0f;
		distToGround = transform.position.y + 0.025f;

		yPosFallingBarrier = invisibleFloor.transform.position.y;
		carFlippedLimit = 0f; //0 - 1

		flyingTimer = 0;
		flyingTime = 10; // in seconds

		carFlipped = false;

		//gameObject.GetComponent<Renderer> ().material = regularCarMaterial;
	}

	void FixedUpdate () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused && !gameOver) {
			float deltaTime = Time.deltaTime;
			if (!(Vector3.Dot (transform.up, Vector3.down) > carFlippedLimit) || flying) {
				rb.MovePosition (transform.position + transform.forward * deltaTime * speed);
				carFlipped = false;
			} else {
				carFlipped = true;
			}
			forceTimer += deltaTime;
			if (gameObject == Camera.main.GetComponent<CarMangment> ().cars [0] && tag != "Evil Car") {
				speedometer = (transform.position - lastPos).magnitude / Time.smoothDeltaTime;
				lastPos = transform.position;
			}
			if (speed < fastestSpeed) {
				speed += deltaTime * acceleration;
			}
			if (speed < slowestSpeed) {
				speed = slowestSpeed;
			}
			/*
			if (Camera.main.GetComponent<FollowCar> ().leadCar != null && level == LevelManagement.drive) {
				if (gameObject.name == Camera.main.GetComponent<FollowCar> ().leadCar.name &&
					gameObject.GetComponent<Renderer> ().material.name != (leadCarMaterial.name + " (Instance)")) {

					gameObject.GetComponent<Renderer> ().material = leadCarMaterial;
				} else if (gameObject.name != Camera.main.GetComponent<FollowCar> ().leadCar.name && 
					gameObject.GetComponent<Renderer> ().material.name == (leadCarMaterial.name + " (Instance)")){

					gameObject.GetComponent<Renderer> ().material = regularCarMaterial;
				}
			}
			*/

			checkGameOverConditions ();
			if (resized) {
				resizeCounter += deltaTime;
				if(resizeCounter > resizeLimit){
					if (rb.mass > 5) {
						Camera.main.GetComponent<SoundEffects> ().playResizeSmallSound (transform.position);
					} else {
						Camera.main.GetComponent<SoundEffects> ().playResizeBigSound (transform.position);
					}
					resizeCounter = 0;
					resized = false;
					distToGround = transform.position.y + 0.025f;
					transform.localScale = new Vector3 (1, 1, 1);
					rb.mass = 5;
				}
			}
			flyingConditions ();
		}
	}

	void checkGameOverConditions(){
		// car is too far away form lead car
		if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
			if (transform.position.z - Camera.main.GetComponent<FollowCar> ().leadCar.transform.position.z < -20) {
				setToGameOver ();
			}
		}
		//car is stopped
		if (rb.IsSleeping () && !flying) {
			setToGameOver ();
		}
		// car has fallen
		if (transform.position.y < yPosFallingBarrier) {
			setToGameOver ();
		}
	}
		
	void flyingConditions () {
		if (flying) {
			if (transform.position.y > 5) {
				rb.drag = 5;
			}
			flyingTimer += Time.deltaTime;
			if (flyingTimer > flyingTime) {
				flying = false;
				flyingTimer = 0;
				rb.useGravity = true;
				rb.angularDrag = 1;
				rb.drag = 0.5f;
				Behaviour halo = (Behaviour)gameObject.transform.GetChild(0).GetComponent("Halo");
				halo.enabled = false;
				Camera.main.GetComponent<SoundEffects> ().playBubblePopSound (transform.position);
			}

		}
	}

	public void jump () {
		if (IsGrounded () && !gameOver) {
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
			rb.velocity += Vector3.up * jumpHeight;
		}
	}

	void setToGameOver () {
		//gameObject.GetComponent<Renderer> ().material = regularCarMaterial;
		gameOver = true;
		gameObject.tag = "Dead Car";
		Camera.main.GetComponent<SoundEffects> ().playCarDeathSound (gameObject.transform.position);
	}

	bool IsGrounded () {
		return Physics.Raycast (transform.position, -Vector3.up, distToGround);
	}

	public void addForce (int multiplier) {
		if (forceTimer > forceLimit) {
			rb.AddForce (rb.transform.forward * multiplier);
			forceTimer = 0;
		}
	}

	void OnTriggerEnter(Collider hit) {
		if (!gameOver) {
			if ((hit.transform.tag == "Car" || hit.transform.tag == "Evil Car") && transform.tag == "Evil Car") {
				gameObject.GetComponent<EvilCarAttributes> ().explodeNow = true;
			}
			string blockName = hit.transform.name.Split ('_') [0];
			if (blockName == AllBlockNames.accelerateBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onSpeedBlock (GameObject.Find (hit.transform.name), gameObject);
			} else if (blockName == AllBlockNames.bouncyBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onJumpBlock (gameObject, rb);
			} else if (blockName == AllBlockNames.decelerateBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onSlowDownBlock (GameObject.Find (hit.transform.name), gameObject);
			} else if (blockName == AllBlockNames.extraCarBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onExtraLifeBlock (GameObject.Find (hit.transform.name));
			} else if (blockName == AllBlockNames.flyBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onFlyingBlock (gameObject, rb);
			} else if (blockName == AllBlockNames.shuffleBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onShuffleBlock (GameObject.Find (hit.transform.name));
			} else if (blockName == AllBlockNames.invisibleBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onInvisibleBlock (GameObject.Find (hit.transform.name));
			} else if (blockName == AllBlockNames.pointBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onPointBlock (GameObject.Find (hit.transform.name));
			} else if (blockName == AllBlockNames.sizeBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onSizeBlock (GameObject.Find (hit.transform.name), gameObject);
			} else if (blockName == AllBlockNames.superAccelerateBlock) {
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superSpeedBlockActivated = true;
				}
			} else if (blockName == AllBlockNames.superDecelerateBlock) {
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superSlowBlockActivated = true;
				}
			} else if (blockName == AllBlockNames.superBlock) {
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superBlockActivated = true;
				}
			} else if (blockName == AllBlockNames.superBouncyBlock) {
				Camera.main.GetComponent<BlockAttributes> ().onJumpBlock (gameObject, rb);
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superBouncyBlockActivated = true;
				}
			} else if (blockName == AllBlockNames.superBullseyeBlock) {
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superBullseyeBlockActivated = true;
				}
			} else if (blockName == AllBlockNames.superPointBlock) {
				if (!GameObject.Find (hit.transform.name).GetComponent<BlockActivated> ().hasActivated) {
					Camera.main.GetComponent<BlockAttributes> ().onSuperBlock (GameObject.Find (hit.transform.name), gameObject);
					Camera.main.GetComponent<AddBlock> ().superPointBlockActivated = true;
				}
			} 
		}
	}
}
