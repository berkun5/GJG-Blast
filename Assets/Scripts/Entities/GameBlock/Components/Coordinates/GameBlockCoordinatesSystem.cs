using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("GameBlock/Coordinates")]
public class GameBlockCoordinatesSystem : GameBlockSystem<GameBlockCoordinatesConfig>
{
	public int row { get; private set; }
	public int column { get; private set; }
	public List<GameBlock> neighbors { get; private set; } = new List<GameBlock>();
	public List<GameBlock> matchingBlocks { get; private set; } = new List<GameBlock>();

	public override void Init(GameBlock gameBlock, GameBlockCoordinatesConfig config)
	{
		base.Init(gameBlock, config);
	}
	public void SetCoordinates(int _row, int _column)
	{
		row = _row;
		column = _column;
	}
	public void SetNeighbors(List<GameBlock> newN)
	{
		neighbors = new List<GameBlock>(newN);
	}

	public List<GameBlock> GetMatchingBlocks()
	{
		matchingBlocks.Clear();
		foreach (var neighbor in neighbors)
			if (TypesAreMatching(neighbor.coordinates))
			{
				matchingBlocks.Add(neighbor);
				foreach (var nestedNeighbor in neighbor.coordinates.neighbors)
					if (TypesAreMatching(nestedNeighbor.coordinates))
						matchingBlocks.Add(nestedNeighbor);
			}

		gameBlock.events.onMatchingBlockCountChanged(matchingBlocks.Count);
		return matchingBlocks;
	}

	private bool TypesAreMatching(GameBlockCoordinatesSystem neighbor)
	{
		// Add your logic here to compare types
		var contains = matchingBlocks.Contains(neighbor.gameBlock);
		var matchingBlock = gameBlock.blockType == neighbor.gameBlock.blockType;
		return !contains && matchingBlock;
	}


}
