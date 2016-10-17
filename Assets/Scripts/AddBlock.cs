using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	/* How far a car must be in order for blocks to spawn */
	float extraCarDistSpawn = 35;
	float bombBlockDistSpawn = 70;
	float evilCarDistSpawn = 105;
	float superBlockDistSpawn = 130;

	/* counters */
	// a counter that will increment when the player activated a super block
	public float superTimerCount = 0;
	// how long the super block is activated
	static int superTimerCap = 8;
	// a counter that will increment for bomb blocks
	float bombCounter = 0;
	// if the counter goes above the limit, a bomb can now possibly spawn
	static float bombLimit = 20;
	// a counter that will increment for super blocks
	float superCounter = 0;
	// if the counter goes above the limit, a super block can now possibly spawn
	static float superLimit = 25;
	// a counter that will increment for evil cars
	float evilCarCounter = 0;
	// if the counter goes above the limit, an evil car can now possibly spawn
	static float evilCarLimit = 25;
	// a counter that will increment for extra blocks
	float extraCarCounter = 0;
	// if the counter goes above the limit, an extra block can now possibly spawn
	static float extraCarLimit = 15;

	/* which super block is activated */
	public bool superBlockActivated = false;
	public bool superSpeedBlockActivated = false;
	public bool superSlowBlockActivated = false;
	public bool superBouncyBlockActivated = false;
	public bool superBullseyeBlockActivated = false;
	public bool superPointBlockActivated = false;

	/* automatically add blocks */
	// a counter that will increment and will show when it can properly add a new block
	float countForNextBlock = 0;
	// where the next x position of the block will spawn
	int nextBlockX = -4;
	// where the next z position of the block will spawn
	int nextBlockZ = 14;
	// how far away from the from the car will blocks spawn
	static int nextBlockZLimit = 6;
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
	// row must have more at least 3 blocks in a row in order for an evil car to spawn
	static int blocksPerRowForEvilCar = 3;
	// player must be at least 4 units away in order for blocks to spawn
	static int distBlocksSpawn = 4;

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

	public GameObject hudBlock;
	// index that will randomly pick a block
	int randBlockIndex = 0;
	// array the will hold names of common blocks, these will be randomly selected to spawn
	List<string> blockNames = new List<string>();
	//public string[] blockNames;
	// will increment whenever a new block is spawned
	int numBlocksCount = 0;
	// Time.deltaTime
	float deltaTime;

	int addBlockIndex = 0;
	int distDifference = 20;

	void Start () {
		level = Camera.main.GetComponent<LevelManagement> ().level;

		if (level == LevelManagement.bowl) {
			extraCarDistSpawn = 10;
			bombBlockDistSpawn = 15;
			evilCarDistSpawn = 20;
			superBlockDistSpawn = 25;
			distDifference = 5;
		}

		// initial block
		GameObject temp = GameObject.Find (AllBlockNames.standardBlock);
		hudBlock = Instantiate (temp);
		hudBlock.tag = TagManagement.blockOnHud;
		hudBlock.name = temp.name + "_" + numBlocksCount;
		numBlocksCount++;

		// fills up starting xPositions
		xStart = new int[blockPerRow];
		for (int i = 0; i < xStart.Length; i++) {
			xStart [i] = nextBlockX + (i * lengthOfBlocks);
		}
		reshuffle (xStart);

		// fills up blocks array
		for (int j = 0; j < numOfStandard; j++) {
			blockNames.Add (AllBlockNames.standardBlock);
		}
		for (int j = 0; j < numOfJagged; j++) {
			blockNames.Add (AllBlockNames.jaggedBlock);
		}
		for (int j = 0; j < numOfHill; j++) {
			blockNames.Add (AllBlockNames.hillBlock);
		}
		for (int j = 0; j < AllBlockNames.commonBlocks.Length; j++) {
			blockNames.Add (AllBlockNames.commonBlocks [j]);
		}
	}

	void Update () {
		if (!Camera.main.GetComponent<CarMangment> ().trueGameOver && !Camera.main.GetComponent<Interface> ().paused) {
			deltaTime = Time.deltaTime;
			superCounter += deltaTime;
			bombCounter += deltaTime;
			evilCarCounter += deltaTime;
			extraCarCounter += deltaTime;
			checkBlocksToBeAdded ();
			checkForSuperBlocks ();
			if (level == LevelManagement.drive) {
				checkWhenToAutomaticallyAddBlocks ();
			}
			if (hudBlock == null) {
				ifHudBlockNull ();
			}
		}
	}

	void checkBlocksToBeAdded () {
		if (transform.position.z > (addBlockIndex + 1) * distDifference && addBlockIndex < AllBlockNames.blocksToBeAdded.Length) {
			blockNames.Add (AllBlockNames.blocksToBeAdded[addBlockIndex]);
			addBlockIndex++;
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

	public void addNewPiece () {
		blockAttributes (hudBlock.name);
		hudBlock.tag = TagManagement.blockOnRoad;
		spawnNextBlock ();
		hudBlock.tag = TagManagement.blockOnHud;
		Camera.main.GetComponent<Interface> ().changeHUDSprite (hudBlock.name.Split ('_') [0], hudBlock.name);
	}

	void checkWhenToAutomaticallyAddBlocks () {
		if (hudBlock != null && hudBlock.tag == TagManagement.blockOnHud) {
			hudBlock.tag = TagManagement.moveableObject;
		}
		if (Camera.main.GetComponent<FollowCar> ().leadCar != null) {
			leadCar = Camera.main.GetComponent<FollowCar> ().leadCar;
			if (leadCar.transform.position.z + leadCarSpeed > nextBlockZ - nextBlockZLimit) {
				countForNextBlock += deltaTime; 
				float leadCarZ = leadCar.transform.position.z;
				if (countForNextBlock > (nextBlockZ - leadCarZ - (distBlocksSpawn + leadCarSpeed/5)) / (blockPerRow)) {
					automaticallyAddBlocks ();
					countForNextBlock = 0;
				}
			}
		}
	}

	void automaticallyAddBlocks () {
		int nextX = nextBlockXPosition ();
		xStartIndex++;
		if (hudBlock == null) {
			ifHudBlockNull ();
		}
		blockAttributes (hudBlock.name);
		spawnNextBlock ();
		Camera.main.GetComponent<SoundEffects> ().playeDropBlockSound (hudBlock.transform.position);
		Vector3 positionOfNewBlock = new Vector3 (nextX, -1, nextBlockZ);
		hudBlock.transform.position = positionOfNewBlock;
		hudBlock.tag = TagManagement.blockOnRoad;	
	}

	void spawnNextBlock () {
		GameObject temp;
		string nextBlock = "";
		float rand = Random.Range (0, 100);
		int numberOfCars = Camera.main.GetComponent<CarMangment> ().cars.Length;
		if (superSlowBlockActivated) {
			nextBlock = AllBlockNames.decelerateBlock;
		} else if (superSpeedBlockActivated) {
			nextBlock = AllBlockNames.accelerateBlock;
		} else if (superBouncyBlockActivated) {
			nextBlock = AllBlockNames.bouncyBlock;
		} else if (superBullseyeBlockActivated) {
			nextBlock = AllBlockNames.bullseyeBlock;
		} else if (superPointBlockActivated) {
			nextBlock = AllBlockNames.pointBlock;
		} else if (superBlockActivated) {
			if (rand > 0 && rand < extraPercent && numberOfCars < maxAmountOfCars) {
				nextBlock = AllBlockNames.extraCarBlock;
			} else {
				randBlockIndex = (int)Random.Range (0, AllBlockNames.superBlockActivated.Length);
				nextBlock = AllBlockNames.superBlockActivated [randBlockIndex];
			}
		} else {
			randBlockIndex = (int)Random.Range (0, blockNames.Count);
			float cameraZPos = Camera.main.transform.position.z;
			if (leadCar != null) {
				leadCarSpeed = leadCar.GetComponent<CarMovement> ().speed;
			}
			if (rand < extraPercent && numberOfCars < maxAmountOfCars && cameraZPos > extraCarDistSpawn && extraCarCounter > extraCarLimit) {
				extraCarCounter = 0;
				nextBlock = AllBlockNames.extraCarBlock;
			} else if (rand < superAccPercent && leadCarSpeed < speedUnderForSuperAcc && superCounter > superLimit && cameraZPos > superBlockDistSpawn) {
				superCounter = 0;
				nextBlock = AllBlockNames.superAccelerateBlock;
			} else if (rand < superDecPercent && leadCarSpeed > speedForSuperDec && superCounter > superLimit && cameraZPos > superBlockDistSpawn) {
				superCounter = 0;
				nextBlock = AllBlockNames.superDecelerateBlock;
			} else if (rand < bombPercent && blockPerRow >= blocksPerRowForBomb && bombCounter > bombLimit && cameraZPos > bombBlockDistSpawn) {
				bombCounter = 0;
				nextBlock = AllBlockNames.bombBlock;
			} else if (rand < comSuperPercent && superCounter > superLimit && cameraZPos > superBlockDistSpawn) {
				superCounter = 0;
				randBlockIndex = (int)Random.Range (0, AllBlockNames.commonSuperBlocks.Length);
				nextBlock = AllBlockNames.commonSuperBlocks [randBlockIndex];
			} else if (rand < evilCarPercent && blockPerRow >= blocksPerRowForEvilCar && evilCarCounter > evilCarLimit && cameraZPos > evilCarDistSpawn) {
				evilCarCounter = 0;
				nextBlock = AllBlockNames.evilCarBlock;
			} else {
				randBlockIndex = (int)Random.Range (0, blockNames.Count);
				nextBlock = blockNames [randBlockIndex];
			}
		}
		temp = GameObject.Find (nextBlock);
		hudBlock = Instantiate (temp);
		hudBlock.name = nextBlock + "_" + numBlocksCount;
		numBlocksCount++;
		if (nextBlock == AllBlockNames.bombBlock) {
			hudBlock.AddComponent<BombAttributes> ();
		}
	}

	void ifHudBlockNull () {
		GameObject temp = GameObject.Find (AllBlockNames.standardBlock);
		hudBlock = Instantiate (temp);
		hudBlock.tag = TagManagement.blockOnHud;
	}

	void blockAttributes (string block) {
		string blockName = block.Split ('_') [0];
		if (blockName == AllBlockNames.bullseyeBlock || blockName == AllBlockNames.superBullseyeBlock) {
			Camera.main.GetComponent<AllBlockAttributes> ().spawnBall (hudBlock);
		} else if (blockName == AllBlockNames.bombBlock) {
			hudBlock.GetComponent<BombAttributes> ().placed = true;
		} else if (blockName == AllBlockNames.evilCarBlock) {
			Camera.main.GetComponent<AllBlockAttributes> ().spawnEvilCar (hudBlock, leadCarSpeed);
		} else if (blockName == AllBlockNames.objectBlock) {
			Camera.main.GetComponent<AllBlockAttributes> ().spawnObject (hudBlock);
		}
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
