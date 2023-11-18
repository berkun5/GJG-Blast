using Unity.VisualScripting;
using UnityEngine;

public class GameBlockSpawner : Spawner
{
	[SerializeField, InspectorLabel("Optional")] private GameBlockConfig _gameBlockConfig;
	public override void Spawn() => SpawnBlock();

	public GameBlock SpawnBlock()
	{
		if (_gameBlockConfig)
		{
			var block = GameBlockFactory.Create(_gameBlockConfig, transform.position, transform.rotation);
			return block;
		}
		return null;
	}

	public GameBlock SpawnBlock(GameBlockConfig gameBlockConfig, Vector3 position, Transform parent = null)
	{
		if (gameBlockConfig)
		{
			var block = GameBlockFactory.Create(gameBlockConfig, position, transform.rotation, parent);
			return block;
		}
		return null;
	}
}
