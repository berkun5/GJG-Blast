using UnityEngine;
using Gruffdev.BCS;

[CreateAssetMenu(fileName = "Events", menuName = "Data/GameBlock/Events")]
public class GameBlockEventsConfig : GameBlockComponentConfig
{
	public override void ConstructSystemComponent(GameBlock entityObject)
	{
		entityObject.AddEvents(this);
	}
}
