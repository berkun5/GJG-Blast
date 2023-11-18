using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "GameBlockData", menuName = "SerializedData/GameBlockData", order = 3)]
public class GameBlockData : ScriptableObject
{
	public GameBlockType blockType;
	public Sprite defaultIcon;
	public List<Sprite> conditionIcons = new List<Sprite>();
}
