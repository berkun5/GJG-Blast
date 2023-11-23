using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "c_GameBlocksCache", menuName = "SerializedData/Lists/GameBlocksCache", order = 3)]
public class GameBlockCache : ScriptableObject
{
	public List<GameBlockData> GameBlocksData = new List<GameBlockData>();
}

public static class AllGameBlockData
{
	static GameBlockCache SO = Resources.Load<GameBlockCache>("c_GameBlocksCache");
	public static List<GameBlockData> GameBlocks = new List<GameBlockData>(SO.GameBlocksData);
	public static GameBlockData GetBlockData(GameBlockType _type)
	{
		for (int i = 0; i < GameBlocks.Count; i++)
			if (_type == GameBlocks[i].blockType)
				return GameBlocks[i];

		Debug.LogWarning("Cache doesn't exist in this Scriptable Object.");
		return null;
	}

}
