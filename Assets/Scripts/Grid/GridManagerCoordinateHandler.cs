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
		ChangeRows(blastedCoordinates);
		SetNeighborsAndMatches();
	}

	private void ChangeRows(List<(int, int)> blastedCoordinates)
	{
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

				blockOrigin.coordinates.SetCoordinates((i + iterationIncreased, col));
				blockOrigin.gameBlockRect.anchoredPosition = GetGridPosition(i + iterationIncreased, col);
			}

			for (int i = 0; i < rowCount; i++)
			{
				var block = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);
				block.coordinates.SetCoordinates((i, col));
				block.graphics.ApplySkin();
				block.gameBlockRect.sizeDelta = new Vector2(_blockSize, _blockSize);
				block.gameBlockRect.anchoredPosition = GetGridPosition(i, col);
			}
		}
	}

}
