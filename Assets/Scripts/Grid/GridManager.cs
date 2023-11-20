using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	[SerializeField] private GameBlockSpawner blockSpawner;
	public GridConfig config;
	private List<GameBlock> _gridElements = new List<GameBlock>();
	private RectTransform _canvasRect;
	private Vector2 _canvasSize;
	private float _blockSize, _widthOffset, _heightOffset;
	private int _rows, _columns;


	private void Start()
	{
		Init(config);
	}

	private void Init(GridConfig conf)
	{
		if (!conf)
			return;

		if (!_canvasRect)
			_canvasRect = GetComponentInChildren<RectTransform>();

		config = conf;
		_widthOffset = config.widthOffset;
		_heightOffset = config.heightOffset;
		_rows = config.rows;
		_columns = config.columns;

		GenerateGrid();
	}

	private void GenerateGrid()
	{
		RecalculateBlockSize();
		float startX = -_canvasSize.x / 2f + _blockSize / 2f;
		float startY = _canvasSize.y / 2f - _blockSize / 2f;
		float totalWidth = _columns * _blockSize;
		float totalHeight = _rows * _blockSize;

		// centering the grid to the canvas
		startX += (_canvasSize.x - totalWidth) / 2f;
		startY -= (_canvasSize.y - totalHeight) / 2f;

		int currentIndex = 0;
		for (int row = 0; row < _rows; row++)
			for (int col = 0; col < _columns; col++)
			{
				float xPos = startX + col * _blockSize;
				float yPos = startY - row * _blockSize;
				UpdateGridElements(currentIndex, row, col, new Vector2(xPos, yPos));
				currentIndex++;
			}


		for (int i = 0; i < _gridElements.Count; i++)
		{
			var blockSkin = _gridElements[i];
			var r = blockSkin.coordinates.row;
			var c = blockSkin.coordinates.column;
			SetBlockNeighbors(blockSkin, r, c);
			blockSkin.events.onBlockTypeChanged(GetRandomBlock());
		}
		for (int i = 0; i < _gridElements.Count; i++)
		{
			var blockSkin = _gridElements[i];
			blockSkin.coordinates.GetMatchingBlocks();
		}
		PooloutElements(currentIndex);
	}

	private RectTransform UpdateGridElements(int _currentIndex, int _row, int _column, Vector2 _pos)
	{
		//Pool|Create element
		GameBlock blockSkin;
		if (_currentIndex < _gridElements.Count)
		{
			blockSkin = _gridElements[_currentIndex];
			blockSkin.gameObject.SetActive(true);
		}
		else
		{
			blockSkin = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);
			_gridElements.Add(blockSkin);
		}

		//position skin
		blockSkin.coordinates.SetCoordinates(_row, _column);
		var skinRect = blockSkin.graphics.rect;
		skinRect.sizeDelta = new Vector2(_blockSize, _blockSize);
		skinRect.anchoredPosition = _pos;

		return skinRect;
	}


	private void PooloutElements(int _currentIndex)
	{
		//pool out element
		for (int i = _currentIndex; i < _gridElements.Count; i++)
			_gridElements[i].gameObject.SetActive(false);
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

		var colSize = _canvasSize.x / _columns;
		var rowSize = _canvasSize.y / _rows;
		_blockSize = Mathf.Min(colSize, rowSize);
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Init(config);
		//EditGrid(_heightOffset, _widthOffset, _rows, _columns);
	}


	private void SetBlockNeighbors(GameBlock block, int row, int col)
	{
		List<GameBlock> neighbors = new List<GameBlock>();

		// Up
		if (row - 1 >= 0)
			neighbors.Add(GetBlockAtCoordinates(row - 1, col));

		// Down
		if (row + 1 < _rows)
			neighbors.Add(GetBlockAtCoordinates(row + 1, col));

		// Left
		if (col - 1 >= 0)
			neighbors.Add(GetBlockAtCoordinates(row, col - 1));

		// Rigt
		if (col + 1 < _columns)
			neighbors.Add(GetBlockAtCoordinates(row, col + 1));

		block.coordinates.SetNeighbors(neighbors);
	}
	private GameBlock GetBlockAtCoordinates(int row, int col)
	{
		foreach (var block in _gridElements)
			if (block.coordinates.row == row && block.coordinates.column == col)
				return block;
		return null;
	}
}