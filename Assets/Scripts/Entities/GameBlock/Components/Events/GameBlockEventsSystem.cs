using UnityEngine;
using Gruffdev.BCS;

[AddComponentMenu("GameBlock/Events")]
public class GameBlockEventsSystem : GameBlockSystem<GameBlockEventsConfig>
{
	public override void Init(GameBlock gameBlock, GameBlockEventsConfig config)
	{
		base.Init(gameBlock, config);
	}
}
