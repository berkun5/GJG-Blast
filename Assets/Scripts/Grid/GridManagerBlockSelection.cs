using System;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	private GameBlockType GetRandomBlock()
	{
		GameBlockType[] blockTypes = (GameBlockType[])Enum.GetValues(typeof(GameBlockType));
		int randIndex = UnityEngine.Random.Range(0, blockTypes.Length);

		return blockTypes[randIndex];
	}
}
