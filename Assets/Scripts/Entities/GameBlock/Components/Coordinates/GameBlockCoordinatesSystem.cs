using UnityEngine;
using System.Collections.Generic;
using System;

[AddComponentMenu("GameBlock/Coordinates")]
public class GameBlockCoordinatesSystem : GameBlockSystem<GameBlockCoordinatesConfig>
{
	public int row { get; private set; }
	public int column { get; private set; }
	public List<GameBlock> neighbors /*{ get; private set; } */= new List<GameBlock>();
	public List<GameBlock> matchingBlocks/* { get; private set; } */= new List<GameBlock>();

	public override void Init(GameBlock gameBlock, GameBlockCoordinatesConfig config)
	{
		base.Init(gameBlock, config);
		AddEvents();
	}

	public override void ReusedSetup()
	{
		base.ReusedSetup();
		AddEvents();
	}
	private void AddEvents()
	{
		gameBlock.events.onSkinRemoved -= SkinRemoved;
		gameBlock.events.onSkinRemoved += SkinRemoved;

		gameBlock.events.onBlockedTypeChanged -= BlockTypeChanged;
		gameBlock.events.onBlockedTypeChanged += BlockTypeChanged;
	}
	private void SkinRemoved()
	{
		neighbors.Clear();
		matchingBlocks.Clear();
	}
	private void BlockTypeChanged(GameBlockType type)
	{
		ResetNeighbors();
	}
	public void ResetNeighbors()
	{
		SetNeighbors();
		SetMatches();
	}
	public void SetCoordinates((int, int) _coords)
	{
		row = _coords.Item1;
		column = _coords.Item2;
	}
	public void SetNeighbors()
	{
		var newNeighbors = new List<GameBlock>();
		// Up
		if (row - 1 >= 0)
		{
			var n = GridManager.I.GetBlockAtCoordinates(row - 1, column);
			if (n)
				newNeighbors.Add(n);
		}
		// Down
		if (row + 1 < GridManager.I.rows)
		{
			var n = GridManager.I.GetBlockAtCoordinates(row + 1, column);
			if (n)
				newNeighbors.Add(n);
		}
		// Left
		if (column - 1 >= 0)
		{
			var n = GridManager.I.GetBlockAtCoordinates(row, column - 1);
			if (n)
				newNeighbors.Add(n);
		}
		// Rigt
		if (column + 1 < GridManager.I.columns)
		{
			var n = GridManager.I.GetBlockAtCoordinates(row, column + 1);
			if (n)
				newNeighbors.Add(n);
		}

		neighbors.Clear();
		neighbors = new List<GameBlock>(newNeighbors);
	}

	public void SetMatches()
	{
		matchingBlocks.Clear();
		foreach (var neighbor in neighbors)
			CheckAndAddMatchingBlocks(neighbor);
		gameBlock.events.onMatchingBlockCountChanged(matchingBlocks.Count);
	}
	private void CheckAndAddMatchingBlocks(GameBlock currentBlock)
	{
		if (TypesAreMatching(currentBlock.coordinates) && !matchingBlocks.Contains(currentBlock))
		{
			matchingBlocks.Add(currentBlock);
			foreach (var nestedNeighbor in currentBlock.coordinates.neighbors)
				CheckAndAddMatchingBlocks(nestedNeighbor);
		}
	}
	private bool TypesAreMatching(GameBlockCoordinatesSystem neighbor)
	{
		// Add your logic here to compare types
		var contains = matchingBlocks.Contains(neighbor.gameBlock);
		var matchingBlock = gameBlock.blockType == neighbor.gameBlock.blockType;
		return !contains && matchingBlock;
	}
}
