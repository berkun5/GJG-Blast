using System;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public GameBlockType GetRandomBlock()
	{
		GameBlockType[] blockTypes = (GameBlockType[])Enum.GetValues(typeof(GameBlockType));
		int randIndex = UnityEngine.Random.Range(1, blockTypes.Length);

		return blockTypes[randIndex];
	}
}
