using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "SerializedData/GridConfig")]
public class GridConfig : ScriptableObject
{
	public int gridLevel;
	[Range(2, 10)] public int rows = 2, columns = 2;
	[Range(0, 100)] public float widthOffset = 10f, heightOffset = 10f;
	[Space(10)]
	public List<GameBlockType> configBlockPool = new List<GameBlockType>();
}
