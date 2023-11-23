using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameBlockData", menuName = "SerializedData/GameBlockData", order = 3)]
public class GameBlockData : ScriptableObject
{
	public GameBlockType blockType;
	public Sprite defaultIcon;
	[Range(0, 10)] public int health = 0;
	public List<Sprite> conditionIcons = new List<Sprite>();
}
