using UnityEditor;
using Gruffdev.BCSEditor;

[CustomEditor(typeof(GameBlockAnimationsConfig))]
public class GameBlockAnimationsConfigEditor : EntityComponentEditorBase<GameBlockAnimationsConfig>
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
