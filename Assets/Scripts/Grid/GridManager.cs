using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public List<RectTransform> _gridElements = new List<RectTransform>();
	private RectTransform _canvasRect;
	public int rows = 4;
	public int columns = 4;
	public GameObject blockPrefab;
	private Vector2 canvasSize;
	[Range(0, 100)] public float heightOffset = 10f;
	[Range(0, 100)] public float widthOffset = 10f;


	public GameBlockSpawner blockSpawner;
	public GameBlockConfig defaultBlockConfig;
	private float blockSize;

	private void Awake()
	{
		_canvasRect = GetComponent<RectTransform>();
	}
	void Start()
	{
		GenerateGrid();
	}

	void GenerateGrid()
	{
		RecalculateBlockSize();
		float startX = -canvasSize.x / 2f + blockSize / 2f;
		float startY = canvasSize.y / 2f - blockSize / 2f;
		float totalWidth = columns * blockSize;
		float totalHeight = rows * blockSize;

		// centering the grid to the canvas
		startX += (canvasSize.x - totalWidth) / 2f;
		startY -= (canvasSize.y - totalHeight) / 2f;

		int currentIndex = 0;
		for (int row = 0; row < rows; row++)
			for (int col = 0; col < columns; col++)
			{
				var element = GetGridElement(currentIndex);

				float xPos = startX + col * blockSize;
				float yPos = startY - row * blockSize;

				var rect = element;
				rect.sizeDelta = new Vector2(blockSize, blockSize);
				rect.anchoredPosition = new Vector2(xPos, yPos);
				currentIndex++;
			}

		PooloutElements(currentIndex);
	}

	void RecalculateBlockSize()
	{
		float canvasWidth = _canvasRect.rect.width;
		float canvasHeight = _canvasRect.rect.height;
		float widthOffsetPercentage = Mathf.Clamp01(widthOffset / 100f);
		float heightOffsetPercentage = Mathf.Clamp01(heightOffset / 100f);
		float actualWidthOffset = canvasWidth * widthOffsetPercentage;
		float actualHeightOffset = canvasHeight * heightOffsetPercentage;

		var w = canvasWidth - actualWidthOffset;
		var h = canvasHeight - actualHeightOffset;
		canvasSize = new Vector2(w, h);

		var colSize = canvasSize.x / columns;
		var rowSize = canvasSize.y / rows;
		blockSize = Mathf.Min(colSize, rowSize);
	}

	private RectTransform GetGridElement(int _currentIndex)
	{
		//pool in element
		RectTransform gridElement;
		if (_currentIndex < _gridElements.Count)
		{
			gridElement = _gridElements[_currentIndex];
			gridElement.parent.gameObject.SetActive(true);
			return gridElement;
		}

		var blockSkin = blockSpawner.SpawnBlock(defaultBlockConfig, _canvasRect.anchoredPosition, transform)
								.graphics.skinInstance;

		gridElement = blockSkin.rectTransform;
		_gridElements.Add(gridElement);
		return gridElement;
	}

	private void PooloutElements(int _currentIndex)
	{
		//pool out element
		for (int i = _currentIndex; i < _gridElements.Count; i++)
			_gridElements[i].parent.gameObject.SetActive(false);
	}

	public void EditGrid(float _heightOffset = -1, float _widthOffset = -1, int _rows = -1, int _column = -1)
	{
		heightOffset = _heightOffset == -1 ? heightOffset : _heightOffset;
		widthOffset = _widthOffset == -1 ? widthOffset : _widthOffset;
		rows = _rows == -1 ? rows : _rows;
		columns = _column == -1 ? columns : _column;

		GenerateGrid();
	}

	private void Update()
	{
		EditGrid(heightOffset, widthOffset, rows, columns);
	}
}








