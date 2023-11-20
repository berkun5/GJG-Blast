using UnityEngine;
using Gruffdev.BCS;

public partial class GameBlock : MonoBehaviour, IEntity
{
	public bool hasCoordinates { private set; get; }
	public GameBlockCoordinatesSystem coordinates { private set; get; }
	public GameBlockCoordinatesConfig coordinatesConfig { private set; get; }
	
	public GameBlockCoordinatesSystem AddCoordinates(GameBlockCoordinatesConfig config)
	{
		if (hasCoordinates)
			Destroy(coordinates);
		
		coordinates = gameObject.AddComponent<GameBlockCoordinatesSystem>();
		coordinatesConfig = config;
		coordinates.Init(this, config);
		hasCoordinates = true;
		return coordinates;
	}
	
	public void RemoveCoordinates()
	{
		if (!hasCoordinates)
			return;
	
		coordinates.Remove();
		Destroy(coordinates);
	
		hasCoordinates = false;
		coordinates = null;
		coordinatesConfig = null;
	}
}
