using System.Collections.Generic;
using Gruffdev.BCSEditor;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(GameBlockCache))]
public class GameBlockCacheEditor : Editor
{
	public List<AnimBool> skinEditorFoldout = new();
	private SerializedProperty gameblocksDataProperty;

	private void OnEnable()
	{
		gameblocksDataProperty = serializedObject.FindProperty("GameBlocksData");
		AdjustSkinEditorFoldoutSize();
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(gameblocksDataProperty, true);
		AdjustSkinEditorFoldoutSize();
		for (int i = 0; i < gameblocksDataProperty.arraySize; i++)
		{
			SerializedProperty GameBlockDataProperty = gameblocksDataProperty.GetArrayElementAtIndex(i);
			GameBlockData GameBlockData = GameBlockDataProperty.objectReferenceValue as GameBlockData;

			if (GameBlockData == null)
			{
				EditorGUILayout.HelpBox("Null GameBlockData element at index " + i, MessageType.Error);
				continue;
			}

			if (GameBlockData != null)
			{
				SerializedObject GameBlockDataSerializedObject = new SerializedObject(GameBlockData);
				DrawBlockData(GameBlockData, i);
				GameBlockDataSerializedObject.ApplyModifiedProperties();
			}
		}
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawBlockData(GameBlockData GameBlockData, int index)
	{
		AdjustSkinEditorFoldoutSize();
		var skinFoldout = skinEditorFoldout[index];
		using var check = new EditorGUI.ChangeCheckScope();
		GameBlockData = EditorExt.FoldoutObject<GameBlockData>(GetFoldoutLabel(GameBlockData, index),
																ref skinFoldout,
																GameBlockData,
																RefreshSkinEditor(GameBlockData));

		if (check.changed)
		{
			if (GameBlockData)
				EditorUtility.SetDirty(GameBlockData);
			RefreshSkinEditor(GameBlockData);
		}
	}
	private Editor RefreshSkinEditor(GameBlockData GameBlockData)
	{
		return GameBlockData
			? CreateEditor(GameBlockData)
			: null;
	}

	private string GetFoldoutLabel(GameBlockData GameBlockData, int index)
	{
		string folderPath = AssetDatabase.GetAssetPath(GameBlockData);
		string folderName = System.IO.Path.GetFileNameWithoutExtension(folderPath);
		return $"{index} - {folderName}";
	}

	private void AdjustSkinEditorFoldoutSize()
	{
		// Ensure the size of skinEditorFoldout matches the array size
		if (skinEditorFoldout.Count < gameblocksDataProperty.arraySize)
		{
			skinEditorFoldout.Add(new AnimBool
			{
				value = true,
				target = true,
				speed = 10f,
			});
		}
		if (skinEditorFoldout.Count > gameblocksDataProperty.arraySize)
		{
			skinEditorFoldout.RemoveAt(skinEditorFoldout.Count - 1);
		}
	}
}