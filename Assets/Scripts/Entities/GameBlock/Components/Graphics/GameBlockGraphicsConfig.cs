using UnityEngine;
using Gruffdev.BCS;

[CreateAssetMenu(fileName = "Graphics", menuName = "Data/GameBlock/Graphics")]
public class GameBlockGraphicsConfig : GameBlockComponentConfig
{
	public GameBlockSkinData skinData;
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddGraphics(this);
	}
}
