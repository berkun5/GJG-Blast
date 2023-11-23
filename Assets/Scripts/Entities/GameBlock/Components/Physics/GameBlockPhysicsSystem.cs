using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[AddComponentMenu("GameBlock/Physics")]
public class GameBlockPhysicsSystem : GameBlockSystem<GameBlockPhysicsConfig>
{
	public int health { get; private set; }
	private Button _blockButton;
	private bool _hasActiveButton;

	public override void Init(GameBlock gameBlock, GameBlockPhysicsConfig config)
	{
		base.Init(gameBlock, config);
		_blockButton = gameBlock.gameObject.AddComponent<Button>();
		AddEvents();
	}
	public override void ReusedSetup()
	{
		base.ReusedSetup();
		AddEvents();
	}
	private void AddEvents()
	{
		gameBlock.events.onSkinApplied -= InitBlockButton;
		gameBlock.events.onSkinApplied += InitBlockButton;
	}
	private void InitBlockButton(GameBlockSkinInstance instance, GameBlockType type)
	{
		health = AllGameBlockData.GetBlockData(gameBlock.blockType).health;

		if (!gameBlock.events.IsPlayableBlock())
		{
			if (_hasActiveButton)
				_blockButton.enabled = _hasActiveButton = false;
			return;
		}

		if (!_hasActiveButton)
		{
			_blockButton.enabled = _hasActiveButton = true;
			_blockButton.onClick.AddListener(TryMerge);
		}

		_blockButton.targetGraphic = gameBlock.graphics.blockIcon;
	}

	public int DamageNeighbor()
	{
		if (health > 0)
			health--;

		gameBlock.events.onTakeDamage(health);
		return health;
	}
	public void TryMerge()
	{
		if (!gameBlock.events.IsPlayableBlock())
			return;

		var matchingBlocks = new List<GameBlock>(gameBlock.coordinates.matchingBlocks);
		if (matchingBlocks.Count <= 0) //Merge Failed
			gameBlock.animations.DoFailedMergeAnimation();
		else
			TriggerMerge(matchingBlocks);
	}
	private void TriggerMerge(List<GameBlock> matchingBlocks)
	{
		//Succeess, trigger animation
		var blastedCoordinates = new List<(int, int)>();
		var targetPos = gameBlock.gameBlockRect.anchoredPosition;
		for (int i = 0; i < matchingBlocks.Count; i++)
		{
			var matchBlock = matchingBlocks[i];
			gameBlock.coordinates.matchingBlocks.Remove(matchBlock);
			var coords = (matchBlock.coordinates.row, matchBlock.coordinates.column);
			blastedCoordinates.Add(coords);
			matchBlock.animations.DoMergeAnimation(targetPos);
			blastedCoordinates.AddRange(GetAllBlastedNeighborObstacle(matchBlock));
		}
		MergeAtCoordinates(blastedCoordinates);
	}
	private List<(int, int)> GetAllBlastedNeighborObstacle(GameBlock matchingBlock)
	{
		//get all possible obstacles that are connected to this or, its neighbors
		var neighboringObstacles = new List<(int, int)>();
		var matchingBlockNeighbors = matchingBlock.coordinates.neighbors;

		for (int i = 0; i < matchingBlockNeighbors.Count; i++)
		{
			var neighbor = matchingBlockNeighbors[i];
			bool isObstacle = !neighbor.events.IsPlayableBlock();
			if (isObstacle)
			{
				//obstacle still has health
				if (neighbor.physics.DamageNeighbor() > 0)
					continue;

				//blasting the obstacle
				neighbor.blockType = GameBlockType.None;
				var coords = (neighbor.coordinates.row, neighbor.coordinates.column);
				neighboringObstacles.Add(coords);
				var emptyCoordsBelowObstacle = GridManager.I.GetAllEmptyBelow(0,
																			neighbor.coordinates.column);
				neighboringObstacles.AddRange(emptyCoordsBelowObstacle);
			}
		}
		return neighboringObstacles;
	}
	private void MergeAtCoordinates(List<(int, int)> blastedCoordinates) => StartCoroutine(PostMergeAnimation(blastedCoordinates));
	private IEnumerator PostMergeAnimation(List<(int, int)> blastedCoordinates)
	{
		var holdDuration = gameBlock.hasAnimations ? gameBlock.animations.Config.MERGE_ANIMATION_DURATION : Time.deltaTime;
		yield return new WaitForSeconds(holdDuration);
		GridManager.I.RecycleAndShift(blastedCoordinates);
	}
}
