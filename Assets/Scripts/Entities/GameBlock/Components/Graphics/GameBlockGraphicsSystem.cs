using UnityEngine;
using Gruffdev.BCS;

[AddComponentMenu("GameBlock/Graphics")]
public class GameBlockGraphicsSystem : GameBlockSystem<GameBlockGraphicsConfig>
{
	public GameBlockSkinInstance skinInstance { get; private set; }
	public override void Init(GameBlock gameBlock, GameBlockGraphicsConfig config)
	{
		base.Init(gameBlock, config);
	}

	public override void LateSetup()
	{
		base.LateSetup();
		ApplySkin(config.skinData);
	}

	public void ApplySkin(GameBlockSkinData skinData)
	{
		ClearSkin();

		skinInstance = Instantiate(skinData.prefab, transform.position, transform.rotation, transform);
		skinInstance.Init(this);
		gameBlock.events?.onSkinApplied.Invoke(skinInstance);
	}

	public void ClearSkin()
	{
		if (skinInstance)
		{
			gameBlock.events?.onSkinRemoved.Invoke();
			Destroy(skinInstance.gameObject);
		}
		skinInstance = null;
	}
}
