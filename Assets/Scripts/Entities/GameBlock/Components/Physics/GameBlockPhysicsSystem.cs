using UnityEngine;
using Gruffdev.BCS;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;
using System.Collections;

[AddComponentMenu("GameBlock/Physics")]
public class GameBlockPhysicsSystem : GameBlockSystem<GameBlockPhysicsConfig>
{
	private Button _blockButton;
	public override void Init(GameBlock gameBlock, GameBlockPhysicsConfig config)
	{
		base.Init(gameBlock, config);
		gameBlock.events.onSkinApplied -= InitBlockButton;
		gameBlock.events.onSkinApplied += InitBlockButton;
	}

	public override void LateSetup()
	{
		base.LateSetup();
	}

	private void InitBlockButton(GameBlockSkinInstance instance, GameBlockType type)
	{
		if (!gameBlock.events.IsPlayableBlock())
			return;

		if (!_blockButton)
		{
			_blockButton = gameBlock.AddComponent<Button>();
			_blockButton.onClick.AddListener(TryMerge);
		}
		_blockButton.targetGraphic = gameBlock.graphics.blockIcon;
	}

	private void TryMerge()
	{
		var matchingBlocks = new List<GameBlock>(gameBlock.coordinates.matchingBlocks);
		List<(int, int)> blastedCoordinates = new List<(int, int)>();

		//Merge Failed
		if (matchingBlocks.Count <= 0)
		{
			gameBlock.animations.DoFailedMergeAnimation();
			return;
		}

		//Succeess, trigger animation
		var targetPos = gameBlock.gameBlockRect.anchoredPosition;
		for (int i = 0; i < matchingBlocks.Count; i++)
		{
			gameBlock.coordinates.matchingBlocks.Remove(matchingBlocks[i]);
			var coords = (matchingBlocks[i].coordinates.row, matchingBlocks[i].coordinates.column);
			blastedCoordinates.Add(coords);
			//matchingBlocks[i].events.onBlasted(coords);
			matchingBlocks[i].events.isBlasted = true;
			var otherBlockAnim = matchingBlocks[i].animations;

			int index = i;
			//otherBlockAnim.DoMergeAnimation(targetPos, delegate
			//				{
			//				});
		}
		//if (matchingBlocks[index] == gameBlock)
		GridManager.I.RecycleAndShift(blastedCoordinates);
	}

}
