using UnityEngine;
using Gruffdev.BCS;

[CreateAssetMenu(fileName = "Coordinates", menuName = "Data/GameBlock/Coordinates")]
public class GameBlockCoordinatesConfig : GameBlockComponentConfig
{
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddCoordinates(this);
	}
}
