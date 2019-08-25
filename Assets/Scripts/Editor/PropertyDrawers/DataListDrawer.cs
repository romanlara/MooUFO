using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(DataBase.DataList), true)]
public class DataListDrawer : PropertyDrawer 
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
		m_ReorderableList.elementHeight += 55;
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
		SerializedProperty itemSprite = itemData.FindPropertyRelative("m_Sprite");
		SerializedProperty itemPrice = itemData.FindPropertyRelative("m_Price");
		SerializedProperty itemBought = itemData.FindPropertyRelative("m_Bought");

		RectOffset offset = new RectOffset(0, 0, -1, -3);
		rect = offset.Add(rect);
		rect.height = EditorGUIUtility.singleLineHeight;

		EditorGUI.PropertyField(rect, itemPrefab, new GUIContent("Prefab"));
		rect.y += EditorGUIUtility.singleLineHeight + 2;
		EditorGUI.PropertyField(rect, itemSprite, new GUIContent("Sprite"));
		rect.y += EditorGUIUtility.singleLineHeight + 2;
		EditorGUI.PropertyField(rect, itemPrice, new GUIContent("Price"));
		rect.y += EditorGUIUtility.singleLineHeight + 2;
		EditorGUI.PropertyField(rect, itemBought, new GUIContent("Bought"));
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		Init(property);

		return m_ReorderableList.GetHeight();
	}
}
