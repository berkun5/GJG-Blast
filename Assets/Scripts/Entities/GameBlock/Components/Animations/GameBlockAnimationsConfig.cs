using UnityEngine;
using Gruffdev.BCS;

[CreateAssetMenu(fileName = "Animations", menuName = "Data/GameBlock/Animations")]
public class GameBlockAnimationsConfig : GameBlockComponentConfig
{
	public float mergeDuration = 0.3f;
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddAnimations(this);
	}
}
