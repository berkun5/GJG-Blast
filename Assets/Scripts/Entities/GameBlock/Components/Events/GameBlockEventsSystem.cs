using UnityEngine;
using Gruffdev.BCS;
using System;

[AddComponentMenu("GameBlock/Events")]
public class GameBlockEventsSystem : GameBlockSystem<GameBlockEventsConfig>
{

	public Action<GameBlockSkinInstance> onSkinApplied = delegate { };
	public Action onSkinRemoved = delegate { };
	public Action<GameBlockType> onBlockTypeChanged = delegate { };
	public Action<int> onMatchingBlockCountChanged = delegate { };
	public override void Init(GameBlock gameBlock, GameBlockEventsConfig config)
	{
		base.Init(gameBlock, config);

		onBlockTypeChanged -= SetBlockType;
		onBlockTypeChanged += SetBlockType;
	}
	private void OnDestroy() => onBlockTypeChanged -= SetBlockType;
	private void SetBlockType(GameBlockType _type) => gameBlock.blockType = _type;
}
