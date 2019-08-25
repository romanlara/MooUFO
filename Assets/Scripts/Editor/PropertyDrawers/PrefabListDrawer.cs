using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(PoolSystem.PrefabList), true)]
public class PrefabListDrawer : PropertyDrawer
{
	ReorderableList m_ReorderableList;
	string m_PropertyName = string.Empty;

	private void Init (SerializedProperty property)
	{
		if (m_ReorderableList != null)
			return;

		SerializedProperty array = property.FindPropertyRelative("m_List");

		m_ReorderableList = new ReorderableList(property.serializedObject, array);
		m_ReorderableList.drawElementCallback = DrawOptionData;
		m_ReorderableList.drawHeaderCallback = DrawHeader;
		m_ReorderableList.elementHeight += 5;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		Init(property);

		m_PropertyName = label.text;
		m_ReorderableList.DoList(position);
	}

	private void DrawHeader (Rect rect)
	{
		GUI.Label(rect, m_PropertyName);
	}

	private void DrawOptionData (Rect rect, int index, bool isActive, bool isFocused)
	{
		SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
		SerializedProperty itemPrefab = itemData.FindPropertyRelative("m_Prefab");
		SerializedProperty itemAmount = itemData.FindPropertyRelative("m_Amount");
		SerializedProperty itemType = itemData.FindPropertyRelative("m_Type");

		RectOffset offset = new RectOffset(0, 0, -1, -3);
		rect = offset.Add(rect);
		rect.height = EditorGUIUtility.singleLineHeight;
		rect.width = 140f;

		EditorGUI.PropertyField(rect, itemPrefab, GUIContent.none);
		rect.x += rect.width + 10;//EditorGUIUtility.singleLineHeight + 2;
		rect.width = 50f;
		EditorGUI.PropertyField(rect, itemAmount, GUIContent.none);
		rect.x += rect.width + 10;
		rect.width = 100f;
		EditorGUI.PropertyField(rect, itemType, GUIContent.none);
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		Init(property);

		return m_ReorderableList.GetHeight();
	}
}

