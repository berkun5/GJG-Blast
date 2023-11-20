using UnityEditor;
using Gruffdev.BCSEditor;

[CustomEditor(typeof(GameBlockCoordinatesConfig))]
public class GameBlockCoordinatesConfigEditor : EntityComponentEditorBase<GameBlockCoordinatesConfig>
{

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			base.OnInspectorGUI();
			
			if (check.changed)
			{
				EditorUtility.SetDirty(config);
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
