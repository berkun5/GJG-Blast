using UnityEditor;
using Gruffdev.BCSEditor;

[CustomEditor(typeof(GameBlockPhysicsConfig))]
public class GameBlockPhysicsConfigEditor : EntityComponentEditorBase<GameBlockPhysicsConfig>
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
