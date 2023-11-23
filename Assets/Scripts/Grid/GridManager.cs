using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GridManager : MonoBehaviour
{
	public static GridManager I;
	public GridConfig config;
	public List<(int, int)> gridCoordinates { get; private set; } = new List<(int, int)>();
	public int rows { get; private set; }
	public int columns { get; private set; }
	[SerializeField] private GameBlockSpawner blockSpawner;
	[SerializeField] private bool enableAutoMatching;
	private RectTransform _canvasRect;
	private Vector2 _gridStartPos, _canvasSize;
	private float _blockSize, _widthOffset, _heightOffset;

	private void Awake() => I = this;
	private void Start() => Init(config);
	public void Init(GridConfig gridConfig)
	{
		if (!gridConfig)
			return;

		if (!_canvasRect)
			_canvasRect = GetComponentInChildren<RectTransform>();

		config = gridConfig;
		_widthOffset = config.widthOffset;
		_heightOffset = config.heightOffset;
		rows = config.rows;
		columns = config.columns;

		StopAllCoroutines();
		GenerateGrid();
		StartCoroutine(TestRandom());
	}
	public IEnumerator TestRandom()
	{
		if (!enableAutoMatching)
			yield break;

		yield return new WaitForSeconds(3f);
		while (true)
		{
			var activeEntities = GameBlockEntityManager.I.activeEntities;
			activeEntities[Random.Range(0, activeEntities.Count)].physics.TryMerge();
			yield return new WaitForSeconds(0.7f);
		}
	}

	public GameBlock GetBlockAtCoordinates(int row, int col)
	{
		var gridElements = GameBlockEntityManager.I.activeEntities;
		foreach (var block in gridElements)
			if (block.coordinates.row == row && block.coordinates.column == col)
				return block;
		return null;
	}
	public Vector2 GetGridPosition(int row, int col)
	{
		var startX = _gridStartPos.x;
		var startY = _gridStartPos.y;
		float xPos = startX + col * _blockSize;
		float yPos = startY - row * _blockSize;
		return new Vector2(xPos, yPos);
	}

	public List<(int, int)> GetAllEmptyBelow(int row, int column)
	{
		//get all the empty blocks below from the given row and column until met an obstacle
		var emptyBlocks = new List<(int, int)>();
		var rowCount = rows;
		for (int k = row; k < rowCount; k++)
		{
			var belowBlock = GetBlockAtCoordinates(k, column);
			if (!belowBlock)
				emptyBlocks.Add((k, column));
			else if (!belowBlock.events.IsPlayableBlock())
				break;
		}
		return emptyBlocks;
	}

	public void SetNeighborsAndMatches()
	{
		//cant match without settings all neighbors first
		var gridElements = GameBlockEntityManager.I.activeEntities;
		for (int i = 0; i < gridElements.Count; i++)
			gridElements[i].coordinates.SetNeighbors();
		for (int i = 0; i < gridElements.Count; i++)
			gridElements[i].coordinates.SetMatches();
	}
}