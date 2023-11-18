using UnityEditor;
using Gruffdev.BCSEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(GameBlockGraphicsConfig))]
public class GameBlockGraphicsConfigEditor : EntityComponentEditorBase<GameBlockGraphicsConfig>
{
	[System.NonSerialized]
	public AnimBool skinEditorFoldout = new AnimBool
	{
		value = true,
		target = true,
		speed = 10f,
	};

	private Editor _skinEditor;

	protected override void OnEnable()
	{
		base.OnEnable();
		RefreshSkinEditor();
	}

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope())
		{
			config.skinData = EditorExt.FoldoutObject<GameBlockSkinData>("Skin", ref skinEditorFoldout, config.skinData, _skinEditor);

			if (check.changed)
			{
				EditorUtility.SetDirty(config);
				serializedObject.ApplyModifiedProperties();
				RefreshSkinEditor();
			}
		}
	}

	private void RefreshSkinEditor()
	{
		_skinEditor = config.skinData
			? Editor.CreateEditor(config.skinData)
			: null;
	}
}
