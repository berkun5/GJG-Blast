using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public static GridManager I;
	public GridConfig config;
	public List<(int, int)> gridCoordinates = new List<(int, int)>();
	[SerializeField] private GameBlockSpawner blockSpawner;
	private RectTransform _canvasRect;
	private Vector2 _canvasSize;
	private float startX, startY;
	private float _blockSize, _widthOffset, _heightOffset;
	public int rows { get; private set; }
	public int columns { get; private set; }

	private void Awake() => I = this;
	private void Start() => Init(config);
	private void Init(GridConfig conf)
	{
		if (!conf)
			return;

		if (!_canvasRect)
			_canvasRect = GetComponentInChildren<RectTransform>();

		config = conf;
		_widthOffset = config.widthOffset;
		_heightOffset = config.heightOffset;
		rows = config.rows;
		columns = config.columns;
		GenerateGrid();
	}

	public GameBlock GetBlockAtCoordinates(int row, int col)
	{
		var _gridElements = GameBlockEntityManager.I.activeEntities;
		foreach (var block in _gridElements)
			if (block.coordinates.row == row && block.coordinates.column == col)
				return block;
		return null;
	}

	private void GenerateGrid()
	{
		gridCoordinates.Clear();
		RecalculateBlockSize();

		startX = -_canvasSize.x / 2f + _blockSize / 2f;
		startY = _canvasSize.y / 2f - _blockSize / 2f;
		float totalWidth = columns * _blockSize;
		float totalHeight = rows * _blockSize;

		// centering the grid to the canvas
		startX += (_canvasSize.x - totalWidth) / 2f;
		startY -= (_canvasSize.y - totalHeight) / 2f;

		int currentIndex = 0;
		for (int row = 0; row < rows; row++)
			for (int col = 0; col < columns; col++)
			{
				UpdateGridElement(row, col);
				gridCoordinates.Add((row, col));
				currentIndex++;
			}
		PooloutElements(currentIndex);
		SetNeighborsAndMatches();
	}
	public int GetGridIndex(int row, int col)
	{
		return row * columns + col;
	}
	public Vector2 GetGridPosition(int row, int col)
	{
		float xPos = startX + col * _blockSize;
		float yPos = startY - row * _blockSize;
		return new Vector2(xPos, yPos);
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
	private GameBlock UpdateGridElement(int _row, int _column)
	{
		var _currentIndex = GetGridIndex(_row, _column);
		var _pos = GetGridPosition(_row, _column);
		//Pool|Create element
		var _gridElements = GameBlockEntityManager.I.allEntities;
		GameBlock block;
		//if (_currentIndex < _gridElements.Count)
		//{
		//	block = _gridElements[_currentIndex];
		//	block.gameObject.SetActive(true);
		//	Debug.Log("enabling:" + block.gameObject.name);
		//}
		//else
		block = blockSpawner.SpawnBlock(_canvasRect.anchoredPosition, _canvasRect);

		//position and size skin
		block.coordinates.SetCoordinates((_row, _column));
		block.graphics.ApplySkin();

		var blockRect = block.gameBlockRect;
		blockRect.sizeDelta = new Vector2(_blockSize, _blockSize);
		blockRect.anchoredPosition = _pos;
		return block;
	}
	private void PooloutElements(int _currentIndex)
	{
		var _gridElements = GameBlockEntityManager.I.allEntities;
		//pool out element
		for (int i = _currentIndex; i < _gridElements.Count; i++)
			_gridElements[i].gameObject.SetActive(false);
	}
	public void SetNeighborsAndMatches()
	{
		var _gridElements = GameBlockEntityManager.I.activeEntities;
		for (int i = 0; i < _gridElements.Count; i++)
			_gridElements[i].coordinates.SetNeighbors();
		for (int i = 0; i < _gridElements.Count; i++)
			_gridElements[i].coordinates.SetMatches();
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var block = GetBlockAtCoordinates(0, 0);
			var vector2 = GetGridPosition(0, 4);
			block.gameBlockRect.anchoredPosition = new Vector2(vector2.x, vector2.y);
		}
		//Init(config);
		//EditGrid(_heightOffset, _widthOffset, _rows, _columns);
	}
}