using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "SerializedData/GridConfig")]
public class GridConfig : ScriptableObject
{
	public int gridLevel;
	[Range(1, 10)] public int rows = 1, columns = 1;
	[Range(0, 100)] public float widthOffset = 10f, heightOffset = 10f;
}
