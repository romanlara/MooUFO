// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UIBundle
{
//	[CustomPropertyDrawer(typeof(Language.TranslationDataList), true)]
	public class TranslationDataListDrawer : PropertyDrawer 
	{
		ReorderableList m_ReorderableList;

		private void Init (SerializedProperty property)
		{
			if (m_ReorderableList != null)
				return;

			SerializedProperty array = property.FindPropertyRelative("m_Translations");

			m_ReorderableList = new ReorderableList(property.serializedObject, array);
			m_ReorderableList.drawElementBackgroundCallback = DrawElementBackground;
			m_ReorderableList.drawElementCallback = DrawOptionData;
			m_ReorderableList.drawHeaderCallback = DrawHeader;
			m_ReorderableList.elementHeightCallback = ElementHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Init(property);

			m_ReorderableList.DoList(position);
		}

		private void DrawHeader (Rect rect)
		{
			GUI.Label(rect, "Translations");
		}

		private void DrawOptionData (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty itemKey = itemData.FindPropertyRelative("m_Key");
			SerializedProperty itemValue = itemData.FindPropertyRelative("m_Value");

			RectOffset offset = new RectOffset(0, 0, -1, -3);
			rect = offset.Add(rect);
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUIUtility.labelWidth = 26f;
				
			EditorGUI.PropertyField(rect, itemKey, new GUIContent("Key"));
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			rect.height = EditorGUI.GetPropertyHeight(itemValue);
			EditorGUI.PropertyField(rect, itemValue, new GUIContent("Value"));
		}

		private void DrawElementBackground (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty itemValue = null;

			if (m_ReorderableList.count > 0)
				itemValue = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Value");

			rect.height = 26 + (itemValue != null ? EditorGUI.GetPropertyHeight(itemValue) : 0);
			rect.width = rect.width - 3;
			rect.x = rect.x + 1;

			Texture2D tex = new Texture2D(1, 1);
			if (EditorGUIUtility.isProSkin) 
			{
				if (isFocused)
					tex.SetPixel(0, 0, new Color(0.2392f, 0.3764f, 0.5686f, 1f));
				else
					tex.SetPixel(0, 0, new Color(0.3647f, 0.3647f, 0.3647f, 1f));
			}
			else 
			{
				if (isFocused)
					tex.SetPixel(0, 0, new Color(0.3491f, 0.5732f, 0.8117f, 1f));
				else
					tex.SetPixel(0, 0, new Color(0.6117f, 0.6117f, 0.6117f, 1f));
			}
			tex.Apply();

			if (isActive) GUI.DrawTexture(rect, tex as Texture);
		}

		private float ElementHeight (int index)
		{
			SerializedProperty itemValue = null;

			if (m_ReorderableList.count > 0)
				itemValue = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Value");

			return (26 + (itemValue != null ? EditorGUI.GetPropertyHeight(itemValue) : 0));
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Init(property);

			return m_ReorderableList.GetHeight() + EditorGUI.GetPropertyHeight(property);
		}
	}
}
