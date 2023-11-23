using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public List<GameBlockType> nonPlayableBlocks { get; private set; } = new List<GameBlockType>
	{
		GameBlockType.Obstacle
	};

	public GameBlockType GetRandomBlock(bool onlyPlayable)
	{
		//GameBlockType[] blockTypes = (GameBlockType[])Enum.GetValues(typeof(GameBlockType));
		//int randIndex = UnityEngine.Random.Range(1, blockTypes.Length);

		List<GameBlockType> pool = config.configBlockPool;
		List<GameBlockType> filteredPool = new List<GameBlockType>();

		if (onlyPlayable)
		{
			foreach (var blockType in config.configBlockPool)
				if (!nonPlayableBlocks.Contains(blockType))
					filteredPool.Add(blockType);
		}

		int count = onlyPlayable ? filteredPool.Count : pool.Count;
		int randIndex = UnityEngine.Random.Range(0, count);
		return onlyPlayable ? filteredPool[randIndex] : pool[randIndex];
	}

	public bool IsDeadlock()
	{
		//TODO: to perfect this completely;
		//...need to ignore the matches that are under an obstacle,
		//...if one match group's all elements are under an obstacle it should be ignored
		var activeEntities = GameBlockEntityManager.I.activeEntities;
		for (int i = 0; i < activeEntities.Count; i++)
			if (activeEntities[i].coordinates.matchingBlocks.Count > 0)
				return false;
		return true;
	}

	private void ReplaceDeadlocks(List<GameBlock> recycledBlocks)
	{
		if (!IsDeadlock())
			return;

		var randomIndex = UnityEngine.Random.Range(0, recycledBlocks.Count);
		for (int i = 0; i < recycledBlocks.Count; i++)
		{
			// Randomly decide whether to skip this iteration, make sure at least one passes 
			bool rand = UnityEngine.Random.Range(0f, 1f) < 0.5f;
			bool valid = i == randomIndex || !rand;

			if (!valid)
				continue;

			GameBlockType forceMatchingType = GameBlockType.Obstacle;
			var block = recycledBlocks[i];
			var neighborCount = block.coordinates.neighbors.Count;

			while (nonPlayableBlocks.Contains(forceMatchingType))
			{
				//change block type to same as any other playable random neighbor
				var randomNeighbor = block.coordinates.neighbors[UnityEngine.Random.Range(0, neighborCount)];
				forceMatchingType = randomNeighbor.blockType;
			}
			recycledBlocks[i].events.onBlockedTypeChanged(forceMatchingType);


			//refresh neighbor' neigbour
			for (int nc = 0; nc < neighborCount; nc++)
				block.coordinates.neighbors[nc].coordinates.ResetNeighbors();
		}
	}
}
