using UnityEngine;
using DG.Tweening;
using System;

[AddComponentMenu("GameBlock/Animations")]
public class GameBlockAnimationsSystem : GameBlockSystem<GameBlockAnimationsConfig>
{
	public override void Init(GameBlock gameBlock, GameBlockAnimationsConfig config)
	{
		base.Init(gameBlock, config);
		AddEvents();
	}

	public override void LateSetup()
	{
		base.LateSetup();
	}

	private void AddEvents()
	{
		gameBlock.events.onSkinApplied -= FirstSpawnAnimation;
		gameBlock.events.onSkinApplied += FirstSpawnAnimation;
		//gameBlock.events.onBlockTypeChanged += SpawnAnimation;
	}


	private void FirstSpawnAnimation(GameBlockSkinInstance instance, GameBlockType type)
	{
		if (gameBlock.blockType == GameBlockType.None)
			return;

		RectTransform skinRect = gameBlock.graphics.rect;
		Vector2 startPosition = new Vector2(skinRect.anchoredPosition.x, Screen.height + skinRect.rect.height / 2f);
		float duration = 0.3f + (gameBlock.coordinates.row * 0.05f);
		float delay = gameBlock.coordinates.column * 0.05f;
		skinRect.DOAnchorPos(startPosition, duration)
				.SetDelay(delay)
				.From()
				.SetEase(Ease.OutBack);

		gameBlock.events.onSkinApplied -= FirstSpawnAnimation;
	}

	public void DoMergeAnimation(Vector2 endValue, Action onCompleteCallback)
	{
		RectTransform skinRect = gameBlock.gameBlockRect;
		DOTween.Complete(skinRect);
		skinRect.DOAnchorPos(endValue, config.mergeDuration)
				.SetEase(Ease.InBack, 3)
				.OnComplete(() => onCompleteCallback());
	}

	public void DoFailedMergeAnimation()
	{
		RectTransform skinRect = gameBlock.graphics.rect;
		if (!DOTween.IsTweening(skinRect))
			skinRect.DOPunchScale(punch: skinRect.localScale * .25f,
								  duration: .2f,
								  vibrato: 2,
								  elasticity: .1f)
					.SetEase(Ease.InBack);
	}

	public void SlideAnimation(Vector2 startPos)
	{
		RectTransform skinRect = gameBlock.graphics.rect;
		float duration = 0.3f + (gameBlock.coordinates.row * 0.05f);

		skinRect.DOAnchorPos(startPos, duration)
				.From()
				.SetEase(Ease.OutBack);
	}

	public void SlideAnimation()
	{
		RectTransform skinRect = gameBlock.graphics.rect;
		//skinRect.anchoredPosition = gameBlock.coordinates.gridPosition;

		var startPos = new Vector2(skinRect.anchoredPosition.x, Screen.height + skinRect.rect.height / 2f);
		float duration = 0.3f + (gameBlock.coordinates.row * 0.05f);

		skinRect.DOAnchorPos(startPos, duration)
				.From()
				.SetEase(Ease.OutBack);
	}
}
