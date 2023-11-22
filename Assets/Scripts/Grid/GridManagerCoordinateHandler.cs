using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public partial class GridManager : MonoBehaviour
{

	public void RecycleAndShift(List<(int, int)> blastedCoordinates)
	{
		// Recycle blasted blocks
		foreach (var (blastedRow, blastedCol) in blastedCoordinates)
		{
			var block = GetBlockAtCoordinates(blastedRow, blastedCol);
			if (block)
				block.gameObject.SetActive(false);
		}
		var recycledBlocks = ChangeRows(blastedCoordinates);
		SetNeighborsAndMatches();
		CheckDeadlocks(recycledBlocks);
	}


	private void CheckDeadlocks(List<GameBlock> recycledBlocks)
	{
		var randomIndex = UnityEngine.Random.Range(0, recycledBlocks.Count);
		if (IsDeadlock())
		{
			for (int i = 0; i < recycledBlocks.Count; i++)
			{
				// Randomly decide whether to skip this iteration
				bool skipIteration = UnityEngine.Random.Range(0f, 1f) < 0.5f;
				if (i == randomIndex || !skipIteration)
				{
					var block = recycledBlocks[i];
					var neighborCount = block.coordinates.neighbors.Count;
					var randomNeighbor = block.coordinates.neighbors[UnityEngine.Random.Range(0, neighborCount)];
					GameBlockType forceMatchingType = randomNeighbor.blockType;
					recycledBlocks[i].events.onBlockedTypeChanged(forceMatchingType);

					for (int nc = 0; nc < neighborCount; nc++)
					{
						var n = block.coordinates.neighbors[nc];
						n.coordinates.ResetNeighbors();
					}
				}
			}
		}
	}
	private List<GameBlock> ChangeRows(List<(int, int)> blastedCoordinates)
	{
		var recycledBlocks = new List<GameBlock>();
		List<int> blastedRows = new List<int>();
		List<int> allBlastedColumns = new List<int>();
		foreach (var (blastedRow, blastedCol) in blastedCoordinates)
		{
			if (!blastedRows.Contains(blastedRow))
				blastedRows.Add(blastedRow);
			if (!allBlastedColumns.Contains(blastedCol))
				allBlastedColumns.Add(blastedCol);
		}

		Dictionary<int, int> columnRequiresNewBlocks = new Dictionary<int, int>();

		for (int i = 0; i < allBlastedColumns.Count; i++)
		{
			var blastedColumn = allBlastedColumns[i];
			for (int k = 0; k < blastedCoordinates.Count; k++)
			{
				if (blastedCoordinates[k].Item2 == blastedColumn)
				{
					if (!columnRequiresNewBlocks.ContainsKey(blastedColumn))
						columnRequiresNewBlocks.Add(blastedColumn, 1);
					else
						columnRequiresNewBlocks[blastedColumn]++;
				}
			}
		}


		//new created ones at i, col coordinates
		foreach (var pair in columnRequiresNewBlocks)
		{
			var col = pair.Key;
			var rowCount = pair.Value; //how many rows needed

			int iterationIncreased = 0;
			for (int i = rows - 1; i >= 0; i--)
			{
				var blockOrigin = GetBlockAtCoordinates(i, col);
				if (!blockOrigin) //if blasted on the bottom
				{
					iterationIncreased++;
					continue;
				}

				var movedRow = i + iterationIncreased;
				blockOrigin.coordinates.SetCoordinates((movedRow, col));

				float duration = 0.3f + (movedRow * 0.05f);
				var endPos = GetGridPosition(movedRow, col);
				blockOrigin.animations.SlideAnimation(endPos, duration);

				//blockOrigin.gameBlockRect.DOAnchorPos(GetGridPosition(movedRow, col), duration)
				//						 .SetEase(Ease.OutBack);
			}


			for (int i = 0; i < rowCount; i++)
			{
				var block = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);
				recycledBlocks.Add(block);
				block.coordinates.SetCoordinates((i, col));
				block.graphics.ApplySkin();
				block.gameBlockRect.sizeDelta = new Vector2(_blockSize, _blockSize);
				block.gameBlockRect.anchoredPosition = GetGridPosition(i, col);
			}
		}

		return recycledBlocks;
	}
}
