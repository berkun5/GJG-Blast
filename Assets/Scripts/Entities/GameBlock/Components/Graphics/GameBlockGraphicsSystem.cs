using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("GameBlock/Graphics")]
public class GameBlockGraphicsSystem : GameBlockSystem<GameBlockGraphicsConfig>
{
	public GameBlockSkinInstance skinInstance { get; private set; }
	public RectTransform rect { get; private set; }
	public Image blockIcon { get; private set; }
	public override void Init(GameBlock gameBlock, GameBlockGraphicsConfig config)
	{
		base.Init(gameBlock, config);
	}

	public override void LateSetup()
	{
		base.LateSetup();
		AddEvents();
	}
	private void AddEvents()
	{
		gameBlock.events.onMatchingBlockCountChanged -= MatchingBlockCountChanged;
		gameBlock.events.onMatchingBlockCountChanged += MatchingBlockCountChanged;
	}
	public void ApplySkin()
	{
		if (gameBlock.blockType == GameBlockType.None)
			gameBlock.blockType = GridManager.I.GetRandomBlock();

		ClearSkin();
		var skinData = config.skinData;
		skinInstance = Instantiate(skinData.prefab, transform.position, transform.rotation, transform);
		skinInstance.Init(this);
		rect = skinInstance.GetComponent<RectTransform>();
		blockIcon = skinInstance.GetComponentInChildren<Image>(true);
		gameBlock.events?.onSkinApplied(skinInstance, gameBlock.blockType);
		BlockTypeChanged(gameBlock.blockType);
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
	private void BlockTypeChanged(GameBlockType _type)
	{
		var blockData = AllGameBlockData.GetBlockData(_type);
		blockIcon.sprite = blockData.defaultIcon;
	}

	private void MatchingBlockCountChanged(int matchCount)
	{
		var blockData = AllGameBlockData.GetBlockData(gameBlock.blockType);
		var img = blockData.defaultIcon;
		var conditionSpritesCount = blockData.conditionIcons.Count;
		switch (matchCount)
		{
			case 2:
				if (conditionSpritesCount > 0)
					img = blockData.conditionIcons[0];
				break;
			case 3:
				if (conditionSpritesCount > 1)
					img = blockData.conditionIcons[1];
				break;
			default:
				if (matchCount > 3)
					if (conditionSpritesCount > 2)
						img = blockData.conditionIcons[2];
				break;
		}

		blockIcon.sprite = img;
	}
}
