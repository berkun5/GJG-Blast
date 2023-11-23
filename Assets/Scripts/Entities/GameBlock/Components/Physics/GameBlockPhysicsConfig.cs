using UnityEngine;

[CreateAssetMenu(fileName = "Physics", menuName = "Data/GameBlock/Physics")]
public class GameBlockPhysicsConfig : GameBlockComponentConfig
{
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddPhysics(this);
	}
}
