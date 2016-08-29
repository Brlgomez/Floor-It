using UnityEngine;
using System.Collections;

public class AddBlock : MonoBehaviour {

	/* percentage of each special case blocks spawning, if rand num is not in the range of these, then it will spawn
	a random standard/common block */

	// chance an extra car block will spawn, out of a 100
	static int extraPercent = 1;
	// chance a super accelerate block will spawn, out of a 100
	static int superAccPercent = extraPercent + 1;
	// chance a decelerate accelerate block will spawn, out of a 100
	static int superDecPercent = superAccPercent + 1;
	// chance a bomb block will spawn, out of a 100
	static int bombPercent = superDecPercent + 2;
	// chance a common super block will spawn, out of a 100
	static int comSuperPercent = bombPercent + 1;
	// chance a evil car block will spawn out of a 100
	static int evilCarPercent = comSuperPercent + 1;

	// which level the player is on
	string level;
	// the car in the lead
	GameObject leadCar;
	// the speed of the lead car
	float leadCarSpeed;
	// max amount of cars
	static int maxAmountOfCars = 5;
	// player needs to be above this speed for super decelerate to spawn
	static float speedForSuperDec = 1.75f;
	// player needs to be below this speed for super accelerate to spawn
	static float speedUnderForSuperAcc = 7.0f;

	// length of the blocks
	static int lengthOfBlocks = 2;
	// number of standard blocks
	static int numOfStandard = 25;
	// number of hill blocks
	static int numOfHill = 7;
	// number of jagged blocks
	static int numOfJagged = 7;
	// number of all blocks
	static int numOfAllBlocks;

	/* which super block is activated */
	public bool superBlockActivated = false;
	public bool superSpeedBlockActivated = false;
	public bool superSlowBlockActivated = false;
	public bool superBouncyBlockActivated = false;
	public bool superBullseyeBlockActivated = false;
	public bool superPointBlockActivated = false;

	/* counters */
	// a counter that will increment when the player activated a super block
	public float superTimerCount = 0;
	// how long the super block is activated
	static int superTimerCap = 8;
	// a counter that will increment for bomb blocks
	float bombCounter = 0;
	// if the counter goes above the limit, a bomb can now possibly spawn
	static float bombLimit = 15;
	// a counter that will increment for super blocks
	float superCounter = 0;
	// if the counter goes above the limit, a super block can now possibly spawn
	static float superLimit = 20;
	// a counter that will increment for super blocks
	float evilCarCounter = 0;
	// if the counter goes above the limit, a super block can now possibly spawn
	static float evilCarLimit = 25;

	/* automatically add blocks */
	// a counter that will increment and will show when it can properly add a new block
	float countForNextBlock = 0;
	// where the next x position of the block will spawn
	int nextBlockX = -4;
	// where the next z position of the block will spawn
	int nextBlockZ = 14;
	// how far away from the from the car will blocks spawn
	static int nextBlockZLimit = 7;
	// how much the blocks in a row will be shifted
	int shiftAmount = 0;
	// how many blocks per row
	int blockPerRow = 5;
	// max amount of blocks in a row;
	static int maxBlocksPerRow = 5;
	// min amount of blocks in a row;
	static int minBlocksPerRow = 2;
	// the index of the xStart array
	int xStartIndex = 0;

	// array that will hold all the next x positions of the next blocks
	int[] xStart;
	static int resizePercent = 10;
	static int shiftPercent = resizePercent + 40;
	// row must have more at least 4 blocks in a row in order for a bomb to spawn
	static int blocksPerRowForBomb = 4;
	// player must be at least 4 units away in order for blocks to spawn
	static int distBlocksSpawn = 4;

	public GameObject hudBlock;
	// index that will randomly pick a block
	int randBlockIndex = 0;
	// array the will hold names of common blocks, these will be randomly selected to spawn
	public string[] blockNames;
	// will increment whenever a new block is spawned
	int numBlocksCount = 0;
	float deltaTime;

	void Start () {
		level = Camera.main.GetComponent<LevelManagement> ().level;

		// initial block
		GameObject temp = GameObject.Find (AllBlockNames.standardBlock);
		hudBlock = Instantiate (temp);
		hudBlock.tag = "On hud";
		hudBlock.name = temp.name + "_" + numBlocksCount;
		numBlocksCount++;

		// fills up starting xPositions
		xStart = new int[blockPerRow];
		for (int i = 0; i < xStart.Length; i++) {
			xStart [i] = nextBlockX + (i * lengthOfBlocks);
		}
		reshuffle (xStart);

		// names of each different type of block
		numOfAllBlocks = numOfStandard + numOfHill + numOfJagged + AllBlockNames.commonBlocks.Length;
		blockNames = new string[numOfAllBlocks];
		int j = 0;
		while (j < numOfStandard) {
			blockNames [j] = AllBlockNames.standardBlock;
			j++;
		}
		while (j < numOfStandard + numOfJagged) {
			blockNames [j] = AllBlockNames.jaggedBlock;
			j++;
		}
		while (j < numOfStandard + numOfJagged + numOfHill) {
			blockNames [j] = AllBlockNames.hillBlock;
			j++;
		}
		while (j < blockNames.Length) {
			blockNames [j] = AllBlockNames.commonBlocks [j % AllBlockNames.commonBlocks.Length];
			j++;
		}
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			deltaTime = Time.deltaTime;
			superCounter += deltaTime;
			bombCounter += deltaTime;
			evilCarCounter += deltaTime;
			checkForSuperBlocks ();
			if (level == LevelManagement.drive) {
				if (hudBlock != null && hudBlock.tag == "On hud") {
					hudBlock.tag = "Moveable";
				}
				if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
					leadCar = Camera.main.GetComponent<FollowCar> ().leadCar;
					if (leadCar.transform.position.z > nextBlockZ - nextBlockZLimit) {
						countForNextBlock += deltaTime; 
						float leadCarZ = leadCar.transform.position.z;
						if (countForNextBlock > (nextBlockZ - leadCarZ - distBlocksSpawn) / (blockPerRow)) {
							automaticallyAddBlocks ();
							countForNextBlock = 0;
						}
					}
				}
			}
			if (hudBlock == null) {
				ifHudBlockNull ();
			}
		}
	}

	void checkForSuperBlocks () {
		if (superBlockActivated || superSpeedBlockActivated || superSlowBlockActivated || superBullseyeBlockActivated ||
		    superBouncyBlockActivated || superPointBlockActivated) {
			superTimerCount += deltaTime;
			if (superTimerCount > superTimerCap) {
				superTimerCount = 0;
				superBlockActivated = false;
				superSpeedBlockActivated = false;
				superSlowBlockActivated = false;
				superBouncyBlockActivated = false;
				superBullseyeBlockActivated = false;
				superPointBlockActivated = false;
				Camera.main.GetComponent<SoundEffects> ().playGameplayMusic ();
				Camera.main.GetComponent<Interface> ().disableTextureOverlay ();
			}
		}
	}

	public void touchedPiece () {
		hudBlock.tag = "Selected";
	}

	public void addNewPiece () {
		blockAttributes (hudBlock.name);
		hudBlock.tag = "On road";
		spawnBlockByPowerUp ();
		hudBlock.tag = "On hud";
		Camera.main.GetComponent<Interface> ().changeHUDSprite (hudBlock.name.Split ('_') [0]);
	}

	void automaticallyAddBlocks () {
		int nextX = nextBlockXPosition ();
		xStartIndex++;
		if (hudBlock == null) {
			ifHudBlockNull ();
		}
		blockAttributes (hudBlock.name);
		spawnBlockByPowerUp ();
		Camera.main.GetComponent<SoundEffects> ().playeDropBlockSound (hudBlock.transform.position);
		hudBlock.tag = "On road";	
		Vector3 positionOfNewBlock = new Vector3 (nextX, -1, nextBlockZ);
		hudBlock.transform.position = positionOfNewBlock;
	}

	void ifHudBlockNull () {
		GameObject temp = GameObject.Find (AllBlockNames.standardBlock);
		hudBlock = Instantiate (temp);
		hudBlock.tag = "On hud";
	}

	void blockAttributes (string block) {
		string blockName = block.Split ('_') [0];
		if (blockName == AllBlockNames.bullseyeBlock || blockName == AllBlockNames.superBullseyeBlock) {
			spawnBall ();
		}
		if (blockName == AllBlockNames.bombBlock) {
			hudBlock.GetComponent<BombAttributes> ().placed = true;
		}
		if (blockName == AllBlockNames.evilCarBlock) {
			spawnEvilCar ();
		}
	}

	void spawnBlockByPowerUp () {
		GameObject temp;
		string block = "";
		float rand = Random.Range (0, 100);
		int numberOfCars = Camera.main.GetComponent<CarMangment> ().cars.Length;
		if (superBlockActivated) {
			if (rand > 0 && rand < extraPercent && numberOfCars <= maxAmountOfCars) {
				block = AllBlockNames.extraCarBlock;
			} else {
				randBlockIndex = (int)Random.Range (0, AllBlockNames.commonBlocks.Length);
				block = AllBlockNames.commonBlocks [randBlockIndex];
			}
		} else if (superSlowBlockActivated) {
			block = AllBlockNames.decelerateBlock;
		} else if (superSpeedBlockActivated) {
			block = AllBlockNames.accelerateBlock;
		} else if (superBouncyBlockActivated) {
			block = AllBlockNames.bouncyBlock;
		} else if (superBullseyeBlockActivated) {
			block = AllBlockNames.bullseyeBlock;
		} else if (superPointBlockActivated) {
			block = AllBlockNames.pointBlock;
		} else {
			if (leadCar != null) {
				leadCarSpeed = leadCar.GetComponent<CarMovement> ().speed;
			}
			randBlockIndex = (int)Random.Range (0, blockNames.Length);
			if (rand < extraPercent && numberOfCars <= maxAmountOfCars) {
				block = AllBlockNames.extraCarBlock;
			} else if (rand < superAccPercent && leadCarSpeed < speedUnderForSuperAcc && superCounter > superLimit) {
				superCounter = 0;
				block = AllBlockNames.superAccelerateBlock;
			} else if (rand < superDecPercent && leadCarSpeed > speedForSuperDec && superCounter > superLimit) {
				superCounter = 0;
				block = AllBlockNames.superDecelerateBlock;
			} else if (rand < bombPercent && blockPerRow >= blocksPerRowForBomb && bombCounter > bombLimit) {
				bombCounter = 0;
				block = AllBlockNames.bombBlock;
			} else if (rand < comSuperPercent && superCounter > superLimit) {
				superCounter = 0;
				randBlockIndex = (int)Random.Range (0, AllBlockNames.commonSuperBlocks.Length);
				block = AllBlockNames.commonSuperBlocks [randBlockIndex];
			} else if (rand < evilCarPercent && evilCarCounter > evilCarLimit) {
				evilCarCounter = 0;
				block = AllBlockNames.evilCarBlock;
			} else {
				randBlockIndex = (int)Random.Range (0, blockNames.Length);
				block = blockNames [randBlockIndex];
			}
		}
		temp = GameObject.Find (block);
		hudBlock = Instantiate (temp);
		hudBlock.name = block + "_" + numBlocksCount;
		numBlocksCount++;
	}

	int nextBlockXPosition () {
		if (xStartIndex > xStart.Length - 1) {
			xStartIndex = 0;
			float shiftRandom = Random.Range (0, 100);
			if (shiftRandom < resizePercent) {
				if (shiftRandom < (resizePercent / 2) && blockPerRow > minBlocksPerRow) {
					blockPerRow--;
					resizeArray (blockPerRow, ref xStart, nextBlockX + shiftAmount);
				} else if (shiftRandom < resizePercent && blockPerRow < maxBlocksPerRow) {
					blockPerRow++;
					resizeArray (blockPerRow, ref xStart, nextBlockX + shiftAmount);
				}
			} else if (shiftRandom < shiftPercent) {
				if (shiftRandom > (shiftPercent / 2)) {
					shiftAmount -= lengthOfBlocks;
				} else {
					shiftAmount += lengthOfBlocks;
				}
			}
			for (int i = 0; i < xStart.Length; i++) {
				xStart [i] = nextBlockX + (i * lengthOfBlocks) + shiftAmount;
			}
			nextBlockZ += lengthOfBlocks;
			reshuffle (xStart);
		}
		return xStart [xStartIndex];
	}

	void spawnBall () {
		Camera.main.GetComponent<Points> ().incrementPoints (1);
		int randomYSpawnPosition = Random.Range (10, 30);
		GameObject sphereTemp = GameObject.Find ("Sphere");
		GameObject sphere = Instantiate (sphereTemp);
		sphere.AddComponent<SphereActions> ();
		sphere.name = sphereTemp.name + "_Clone";
		sphere.transform.position = new Vector3 (
			hudBlock.transform.position.x, 
			randomYSpawnPosition, 
			hudBlock.transform.position.z
		);
	}

	void spawnEvilCar () {
		int randomYSpawnPosition = Random.Range (1, 3);
		float evilCarSpeed = leadCarSpeed * 1.1f;
		GameObject evilCarTemp = GameObject.Find ("Evil Car");
		GameObject evilCar = Instantiate (evilCarTemp);
		evilCar.AddComponent<CarMovement> ();
		evilCar.GetComponent<CarMovement> ().speed = evilCarSpeed;
		evilCar.AddComponent<EvilCarAttributes> ();
		evilCar.name = evilCarTemp.name + "_Clone";
		evilCar.transform.position = new Vector3 (
			hudBlock.transform.position.x, 
			randomYSpawnPosition,
			hudBlock.transform.position.z
		);
	}

	void resizeArray (int size, ref int[] array, int shift) {
		int[] temp = new int[size];
		for (int i = 0; i < size; i++) {
			temp [i] = (i * lengthOfBlocks) + shift;
		}
		array = temp;
	}

	void reshuffle (int[] intArray) {
		for (int i = 0; i < intArray.Length; i++) {
			int tmp = intArray [i];
			int rand = Random.Range (i, intArray.Length);
			intArray [i] = intArray [rand];
			intArray [rand] = tmp;
		}
	}
}
