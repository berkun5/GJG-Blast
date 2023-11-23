using UnityEngine;

[CreateAssetMenu(fileName = "Animations", menuName = "Data/GameBlock/Animations")]
public class GameBlockAnimationsConfig : GameBlockComponentConfig
{
	public float MERGE_ANIMATION_DURATION = 0.2f;
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddAnimations(this);
	}
}
