using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	private void GenerateGrid()
	{
		gridCoordinates.Clear();
		DestroyElements();
		RecalculateBlockSize();
		SetGridStartPosition();


		List<GameBlock> initialBlocks = new List<GameBlock>();
		for (int row = 0; row < rows; row++)
			for (int col = 0; col < columns; col++)
			{
				initialBlocks.Add(CreateGridBlock(row, col));
				gridCoordinates.Add((row, col));
			}


		SetNeighborsAndMatches();
		ReplaceDeadlocks(initialBlocks);
	}

	private GameBlock CreateGridBlock(int _row, int _column)
	{
		var _pos = GetGridPosition(_row, _column);
		var block = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);
		var blockRect = block.gameBlockRect;

		//position and size skin
		block.coordinates.SetCoordinates((_row, _column));
		block.graphics.ApplySkin(_row <= 0); //make sure first row don't have any obstacle

		blockRect.sizeDelta = new Vector2(_blockSize, _blockSize);
		blockRect.anchoredPosition = _pos;
		return block;
	}

	private void SetGridStartPosition()
	{
		var startX = -_canvasSize.x / 2f + _blockSize / 2f;
		var startY = _canvasSize.y / 2f - _blockSize / 2f;
		float totalWidth = columns * _blockSize;
		float totalHeight = rows * _blockSize;

		// centering the grid to the canvas
		startX += (_canvasSize.x - totalWidth) / 2f;
		startY -= (_canvasSize.y - totalHeight) / 2f;
		_gridStartPos = new Vector2(startX, startY);
	}

	private void DestroyElements()
	{
		var gridElements = new List<GameBlock>(GameBlockEntityManager.I.allEntities);
		GameBlockEntityManager.I.allEntities.Clear();
		GameBlockEntityManager.I.activeEntities.Clear();
		GameBlockEntityManager.I.inactiveEntities.Clear();
		//clear for reinitialization
		for (int i = 0; i < gridElements.Count; i++)
			DestroyImmediate(gridElements[i].gameObject);
	}
	private void RecalculateBlockSize()
	{
		float canvasWidth = _canvasRect.rect.width;
		float canvasHeight = _canvasRect.rect.height;
		float widthOffsetPercentage = Mathf.Clamp01(_widthOffset / 100f);
		float heightOffsetPercentage = Mathf.Clamp01(_heightOffset / 100f);
		float actualWidthOffset = canvasWidth * widthOffsetPercentage;
		float actualHeightOffset = canvasHeight * heightOffsetPercentage;

		var w = canvasWidth - actualWidthOffset;
		var h = canvasHeight - actualHeightOffset;
		_canvasSize = new Vector2(w, h);

		var colSize = _canvasSize.x / columns;
		var rowSize = _canvasSize.y / rows;
		_blockSize = Mathf.Min(colSize, rowSize);
	}
}