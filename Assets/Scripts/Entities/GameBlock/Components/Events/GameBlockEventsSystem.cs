using UnityEngine;
using System;
using System.Collections.Generic;

[AddComponentMenu("GameBlock/Events")]
public class GameBlockEventsSystem : GameBlockSystem<GameBlockEventsConfig>
{
	private List<GameBlockType> NonPlayableBlocks = new List<GameBlockType>
	{
		GameBlockType.None,
		GameBlockType.BoxObstacle
	};

	public Action<GameBlockSkinInstance, GameBlockType> onSkinApplied = delegate { };
	public Action onSkinRemoved = delegate { };
	public bool isBlasted;
	public Action<int> onMatchingBlockCountChanged = delegate { };

	public override void Init(GameBlock gameBlock, GameBlockEventsConfig config)
	{
		base.Init(gameBlock, config);

		onSkinApplied -= SetBlockType;
		onSkinApplied += SetBlockType;
	}
	private void OnDestroy() => onSkinApplied -= SetBlockType;
	private void SetBlockType(GameBlockSkinInstance instance, GameBlockType type) => gameBlock.blockType = type;
	public bool IsPlayableBlock()
	{
		return !NonPlayableBlocks.Contains(gameBlock.blockType);
	}
}
