using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public GameBlockType GetRandomBlock()
	{
		var pool = config.configBlockPool;
		//GameBlockType[] blockTypes = (GameBlockType[])Enum.GetValues(typeof(GameBlockType));
		//int randIndex = UnityEngine.Random.Range(1, blockTypes.Length);
		int randIndex = UnityEngine.Random.Range(1, pool.Count);
		return pool[randIndex];
	}

	public bool IsDeadlock()
	{
		var activeEntities = GameBlockEntityManager.I.activeEntities;
		for (int i = 0; i < activeEntities.Count; i++)
			if (activeEntities[i].coordinates.matchingBlocks.Count > 0)
				return false;
		return true;
	}
}
