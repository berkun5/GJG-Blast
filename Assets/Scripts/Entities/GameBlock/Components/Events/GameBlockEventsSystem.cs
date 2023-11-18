using UnityEngine;
using Gruffdev.BCS;
using System;

[AddComponentMenu("GameBlock/Events")]
public class GameBlockEventsSystem : GameBlockSystem<GameBlockEventsConfig>
{

	public Action<GameBlockSkinInstance> onSkinApplied = delegate { };
	public Action onSkinRemoved = delegate { };

	public override void Init(GameBlock gameBlock, GameBlockEventsConfig config)
	{
		base.Init(gameBlock, config);
	}
}
