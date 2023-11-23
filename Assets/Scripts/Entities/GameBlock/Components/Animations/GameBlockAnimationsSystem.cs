using UnityEngine;
using DG.Tweening;

[AddComponentMenu("GameBlock/Animations")]
public class GameBlockAnimationsSystem : GameBlockSystem<GameBlockAnimationsConfig>
{
	public override void Init(GameBlock gameBlock, GameBlockAnimationsConfig config)
	{
		base.Init(gameBlock, config);
		AddEvents();
	}
	public override void ReusedSetup()
	{
		base.ReusedSetup();
		AddEvents();
	}
	private void AddEvents()
	{
		gameBlock.events.onSkinApplied -= FirstSpawnAnimation;
		gameBlock.events.onSkinApplied += FirstSpawnAnimation;

		gameBlock.events.onSkinRemoved -= SkinRemoved;
		gameBlock.events.onSkinRemoved += SkinRemoved;

		gameBlock.events.onTakeDamage -= TakeDamageAnimation;
		gameBlock.events.onTakeDamage += TakeDamageAnimation;
	}

	private void SkinRemoved()
	{
		DOTween.Kill(gameBlock.graphics.rect, true);
		DOTween.Kill(gameBlock.gameBlockRect, true);
	}
	private void FirstSpawnAnimation(GameBlockSkinInstance instance, GameBlockType type)
	{
		//if (gameBlock.blockType == GameBlockType.None)
		//return;

		RectTransform rect = gameBlock.graphics.rect;
		DOTween.Kill(rect);

		Vector2 startPosition = new Vector2(rect.anchoredPosition.x, Screen.height + rect.rect.height / 2f);
		float duration = 0.3f + (gameBlock.coordinates.row * 0.05f);
		float delay = gameBlock.coordinates.column * 0.05f;

		rect.DOAnchorPos(startPosition, duration)
			.SetDelay(delay)
			.From()
			.SetEase(Ease.OutSine);

		gameBlock.events.onSkinApplied -= FirstSpawnAnimation;
	}
	public void DoMergeAnimation(Vector2 endValue)
	{
		RectTransform rect = gameBlock.gameBlockRect;
		DOTween.Complete(rect);
		rect.DOAnchorPos(endValue, config.MERGE_ANIMATION_DURATION)
			.SetEase(Ease.InBack);
	}
	public void DoFailedMergeAnimation()
	{
		RectTransform rect = gameBlock.graphics.rect;
		if (!DOTween.IsTweening(rect))
			rect.DOPunchScale(punch: rect.localScale * .25f,
							duration: .2f,
							vibrato: 2,
							elasticity: .1f)
				.SetEase(Ease.InBack);
	}
	public void SlideAnimation(Vector2 endPos, float duration)
	{
		RectTransform rect = gameBlock.gameBlockRect;
		DOTween.Complete(rect);
		rect.DOAnchorPos(endPos, duration)
			.SetEase(Ease.OutBack);
	}

	private void TakeDamageAnimation(int remainingHealth)
	{
		if (remainingHealth <= 0)
			return;

		var duration = config.MERGE_ANIMATION_DURATION / 1.1f; //shouldnt be longer than merge time, it might cancel merge anim

		RectTransform rect = gameBlock.graphics.rect;
		if (!DOTween.IsTweening(rect))
			rect.DOPunchScale(punch: rect.localScale * .25f,
							duration,
							vibrato: 2,
							elasticity: .1f)
				.SetEase(Ease.OutBack);
	}
}
