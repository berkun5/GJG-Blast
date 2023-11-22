using UnityEngine;
using Gruffdev.BCS;

public partial class GameBlock : MonoBehaviour, IEntity
{
	public bool hasPhysics { private set; get; }
	public GameBlockPhysicsSystem physics { private set; get; }
	public GameBlockPhysicsConfig physicsConfig { private set; get; }
	
	public GameBlockPhysicsSystem AddPhysics(GameBlockPhysicsConfig config)
	{
		if (hasPhysics)
			Destroy(physics);
		
		physics = gameObject.AddComponent<GameBlockPhysicsSystem>();
		physicsConfig = config;
		physics.Init(this, config);
		hasPhysics = true;
		return physics;
	}
	
	public void RemovePhysics()
	{
		if (!hasPhysics)
			return;
	
		physics.Remove();
		Destroy(physics);
	
		hasPhysics = false;
		physics = null;
		physicsConfig = null;
	}
}
