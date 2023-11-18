using UnityEditor;
using Gruffdev.BCSEditor;

[CustomEditor(typeof(GameBlockSkinData))]
public class GameBlockSkinDataEditor : Editor
{
	private GameBlockSkinData data;

	private void OnEnable()
	{
		if (target != null)
			data = (GameBlockSkinData)target;
	}

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			data.prefab = EditorExt.ObjectFieldWithPreview(data.prefab, 64);

			if (check.changed)
			{
				EditorUtility.SetDirty(data);
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
