using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[AddComponentMenu("GameBlock/Events")]
public class GameBlockEventsSystem : GameBlockSystem<GameBlockEventsConfig>
{
	public Action<GameBlockSkinInstance, GameBlockType> onSkinApplied = delegate { };
	public Action onSkinRemoved = delegate { };
	public Action<GameBlockType> onBlockedTypeChanged = delegate { };
	public Action<int> onMatchingBlockCountChanged = delegate { };
	public Action onBlasted = delegate { };
	public Action onNeighborBlasted = delegate { };
	public Action<int> onTakeDamage = delegate { }; //passes current health
	public override void Init(GameBlock gameBlock, GameBlockEventsConfig config)
	{
		base.Init(gameBlock, config);

		onBlockedTypeChanged -= BlockChanged;
		onBlockedTypeChanged += BlockChanged;

		onSkinApplied -= SetBlockType;
		onSkinApplied += SetBlockType;
	}
	private void OnDestroy() => onSkinApplied -= SetBlockType;
	private void BlockChanged(GameBlockType type) => gameBlock.blockType = type;
	private void SetBlockType(GameBlockSkinInstance instance, GameBlockType type) => gameBlock.blockType = type;
	public bool IsPlayableBlock()
	{
		return !GridManager.I.nonPlayableBlocks.Contains(gameBlock.blockType);
	}
}
