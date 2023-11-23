using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public void RecycleAndShift(List<(int, int)> blastedCoordinates)
	{
		// Recycle blasted blocks
		foreach (var (blastedRow, blastedCol) in blastedCoordinates)
		{
			var block = GetBlockAtCoordinates(blastedRow, blastedCol);
			if (block)
			{ block.gameObject.SetActive(false); }
		}
		var recycledBlocks = RearrangeRows(blastedCoordinates);
		SetNeighborsAndMatches();
		ReplaceDeadlocks(recycledBlocks);
	}

	private List<GameBlock> RearrangeRows(List<(int, int)> blastedCoordinates)
	{
		var blastedRows = new List<int>();
		var allBlastedColumns = new List<int>();

		//remove duplicate rows and columns
		foreach (var (blastedRow, blastedCol) in blastedCoordinates)
		{
			if (!blastedRows.Contains(blastedRow))
				blastedRows.Add(blastedRow);
			if (!allBlastedColumns.Contains(blastedCol))
				allBlastedColumns.Add(blastedCol);
		}

		//store which column needs how many new blocks
		Dictionary<int, int> columnRequiresNewBlocks = new Dictionary<int, int>();
		for (int i = 0; i < allBlastedColumns.Count; i++)
		{
			var blastedColumn = allBlastedColumns[i];
			for (int k = 0; k < blastedCoordinates.Count; k++)
			{
				if (blastedCoordinates[k].Item2 != blastedColumn)
					continue;

				if (!columnRequiresNewBlocks.ContainsKey(blastedColumn))
					columnRequiresNewBlocks.Add(blastedColumn, 1);
				else
					columnRequiresNewBlocks[blastedColumn]++;
			}
		}
		return SlideAndGetNewBlocks(columnRequiresNewBlocks);
	}

	private List<GameBlock> SlideAndGetNewBlocks(Dictionary<int, int> columnRequiresNewBlocks)
	{
		var recycledBlocks = new List<GameBlock>();

		foreach (var pair in columnRequiresNewBlocks)
		{
			var targetColumn = pair.Key;
			var requiredBlockCount = pair.Value;

			SlideBlocks(targetColumn);
			var newBlocks = GetNewBlocks(requiredBlockCount, targetColumn);
			recycledBlocks.AddRange(newBlocks);
		}
		return recycledBlocks;
	}
	private void SlideBlocks(int targetColumn)
	{
		int iterationIncreased = 0;

		// loop from bottom to top
		for (int i = rows - 1; i >= 0; i--)
		{
			var targetBlock = GetBlockAtCoordinates(i, targetColumn);
			//if target is empty, check how many more target is empty below that
			if (!targetBlock)
			{
				iterationIncreased++;
				continue;
			}
			//if target is obstacle, reset previously registered empty blocks, they can't pass obstacle anyway
			if (nonPlayableBlocks.Contains(targetBlock.blockType))
			{
				iterationIncreased = 0;
				continue;
			}

			//new coordinates
			var movedRow = i + iterationIncreased;
			targetBlock.coordinates.SetCoordinates((movedRow, targetColumn));

			//animation
			var endPos = GetGridPosition(movedRow, targetColumn);
			var duration = 0.3f + (movedRow * 0.05f);
			targetBlock.animations.SlideAnimation(endPos, duration);
		}
	}
	private List<GameBlock> GetNewBlocks(int requiredBlockCount, int targetColumn)
	{
		var newBlocks = new List<GameBlock>();
		for (int i = 0; i < requiredBlockCount; i++)
		{
			var targetBlock = GetBlockAtCoordinates(i, targetColumn);
			//already occupied, skipping
			if (targetBlock)
			{
				//if target is obstacle no point to check further more, can't place below an obstacle
				if (nonPlayableBlocks.Contains(targetBlock.blockType))
					break;
				continue;
			}

			var block = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);
			newBlocks.Add(block);
			block.coordinates.SetCoordinates((i, targetColumn));
			block.graphics.ApplySkin(true);
			block.gameBlockRect.sizeDelta = new Vector2(_blockSize, _blockSize);
			block.gameBlockRect.anchoredPosition = GetGridPosition(i, targetColumn);
		}
		return newBlocks;
	}
}
