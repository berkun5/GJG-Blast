using UnityEngine;
using Gruffdev.BCS;

public partial class GameBlock : MonoBehaviour, IEntity
{
	public bool hasAnimations { private set; get; }
	public GameBlockAnimationsSystem animations { private set; get; }
	public GameBlockAnimationsConfig animationsConfig { private set; get; }
	
	public GameBlockAnimationsSystem AddAnimations(GameBlockAnimationsConfig config)
	{
		if (hasAnimations)
			Destroy(animations);
		
		animations = gameObject.AddComponent<GameBlockAnimationsSystem>();
		animationsConfig = config;
		animations.Init(this, config);
		hasAnimations = true;
		return animations;
	}
	
	public void RemoveAnimations()
	{
		if (!hasAnimations)
			return;
	
		animations.Remove();
		Destroy(animations);
	
		hasAnimations = false;
		animations = null;
		animationsConfig = null;
	}
}
