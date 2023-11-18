using UnityEngine;
using Gruffdev.BCS;

public partial class GameBlock : MonoBehaviour, IEntity
{
	public bool hasGraphics { private set; get; }
	public GameBlockGraphicsSystem graphics { private set; get; }
	public GameBlockGraphicsConfig graphicsConfig { private set; get; }
	
	public GameBlockGraphicsSystem AddGraphics(GameBlockGraphicsConfig config)
	{
		if (hasGraphics)
			Destroy(graphics);
		
		graphics = gameObject.AddComponent<GameBlockGraphicsSystem>();
		graphicsConfig = config;
		graphics.Init(this, config);
		hasGraphics = true;
		return graphics;
	}
	
	public void RemoveGraphics()
	{
		if (!hasGraphics)
			return;
	
		graphics.Remove();
		Destroy(graphics);
	
		hasGraphics = false;
		graphics = null;
		graphicsConfig = null;
	}
}
