using UnityEngine;
using System.Collections;

public class AllBlockNames : MonoBehaviour {

	// names of all blocks
	public static string standardBlock = "StandardBlock";
	public static string jaggedBlock = "JaggedBlock";
	public static string hillBlock = "HillBlock";
	public static string accelerateBlock = "AccelerateBlock";
	public static string bullseyeBlock = "BullseyeBlock";
	public static string bouncyBlock = "BouncyBlock";
	public static string shuffleBlock = "ShuffleBlock";
	public static string invisibleBlock = "InvisibleBlock";
	public static string decelerateBlock = "DecelerateBlock";
	public static string flyBlock = "FlyBlock";
	public static string pointBlock = "PointBlock";
	public static string sizeBlock = "SizeBlock";
	public static string multiplierBlock = "MultiplierBlock";

	public static string chainBlock = "ChainBlock";
	public static string extraCarBlock = "ExtraCarBlock";
	public static string bombBlock = "BombBlock";
	public static string evilCarBlock = "EvilCarBlock";
	public static string objectBlock = "ObjectBlock";

	public static string superAccelerateBlock = "SuperAccelerateBlock";
	public static string superDecelerateBlock = "SuperDecelerateBlock";

	public static string superBlock = "SuperBlock";
	public static string superBullseyeBlock = "SuperBullseyeBlock";
	public static string superBouncyBlock = "SuperBouncyBlock";
	public static string superPointBlock = "SuperPointBlock";

	// common special blocks and can spawn at any time
	public static string[] commonBlocks = {
		accelerateBlock, bullseyeBlock, bouncyBlock, decelerateBlock, objectBlock
	};

	public static string[] blocksToBeAdded = {
		pointBlock, flyBlock, invisibleBlock, sizeBlock, shuffleBlock, multiplierBlock
	};

	public static string[] superBlockActivated = {
		accelerateBlock, bullseyeBlock, bouncyBlock, decelerateBlock,
		pointBlock, flyBlock, invisibleBlock, sizeBlock, shuffleBlock, multiplierBlock
	};

	// common super blocks that can spawn if timer has passed the limit
	public static string[] commonSuperBlocks = {
		superBlock, superBullseyeBlock, superBouncyBlock, superPointBlock
	};

	// blocks that can be deactivates
	public static string[] blocksThatCanBeDeactivated = {
		extraCarBlock, shuffleBlock, superDecelerateBlock, superAccelerateBlock, superBlock, superBullseyeBlock,
		superBouncyBlock, pointBlock, superPointBlock, sizeBlock, invisibleBlock, accelerateBlock, superAccelerateBlock,
		decelerateBlock, superDecelerateBlock, multiplierBlock, chainBlock
	};

	// blocks that have limits
	public static string[] blocksLimitOverTime = {
		accelerateBlock, superAccelerateBlock, decelerateBlock, superDecelerateBlock
	};
}
