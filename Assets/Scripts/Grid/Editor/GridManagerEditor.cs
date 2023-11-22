using Gruffdev.BCSEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{

	[System.NonSerialized]
	public AnimBool skinEditorFoldout = new AnimBool
	{
		value = true,
		target = true,
		speed = 10f,
	};
	private Editor _skinEditor;
	private GridManager gridManager;


	protected virtual void OnEnable()
	{
		if (target != null)
			gridManager = target as GridManager;

		RefreshSkinEditor(gridManager.config);
	}
	public override void OnInspectorGUI()
	{
		DrawProperties();
		DrawConfig();
	}

	private void DrawConfig()
	{
		using var check = new EditorGUI.ChangeCheckScope();

		var confExt = EditorExt.FoldoutObject<GridConfig>("Config",
												  ref skinEditorFoldout,
												  gridManager.config,
												  _skinEditor,
												  1,
												  false);

		if (check.changed)
		{
			if (confExt)
				EditorUtility.SetDirty(confExt);

			serializedObject.ApplyModifiedProperties();
			RefreshSkinEditor(confExt);
		}
	}
	private void DrawProperties()
	{
		DrawDefaultInspector();
		serializedObject.ApplyModifiedProperties();
		EditorGUILayout.Space(25);
	}

	private void RefreshSkinEditor(GridConfig conf)
	{
		_skinEditor = conf
			? Editor.CreateEditor(conf)
			: null;
	}
}
