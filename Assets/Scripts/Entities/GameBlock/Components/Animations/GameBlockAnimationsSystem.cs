using UnityEngine;
using DG.Tweening;

[AddComponentMenu("GameBlock/Animations")]
public class GameBlockAnimationsSystem : GameBlockSystem<GameBlockAnimationsConfig>
{
	public override void Init(GameBlock gameBlock, GameBlockAnimationsConfig config)
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
		gameBlock.events.onBlockTypeChanged -= SpawnAnimation;
		gameBlock.events.onBlockTypeChanged += SpawnAnimation;
	}

	private void SpawnAnimation(GameBlockType type)
	{
		if (type == GameBlockType.None)
			return;

		RectTransform skinRect = gameBlock.graphics.rect;
		Vector2 startPosition = new Vector2(skinRect.anchoredPosition.x, Screen.height + skinRect.rect.height / 2f);
		float duration = 0.3f + (gameBlock.coordinates.row * 0.05f);
		float delay = gameBlock.coordinates.column * 0.05f;

		skinRect.DOAnchorPos(startPosition, duration)
				.SetDelay(delay)
				.From()
				.SetEase(Ease.OutBack);
	}
}
