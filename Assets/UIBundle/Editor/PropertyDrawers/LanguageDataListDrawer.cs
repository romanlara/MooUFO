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
//	[CustomPropertyDrawer(typeof(Localizer.LanguageDataList), true)]
	public class LanguageDataListDrawer : PropertyDrawer 
	{
		ReorderableList m_ReorderableList;

		private void Init (SerializedProperty property)
		{
			if (m_ReorderableList != null)
				return;

			SerializedProperty array = property.FindPropertyRelative("m_Languages");

			m_ReorderableList = new ReorderableList(property.serializedObject, array);
			m_ReorderableList.drawElementCallback = DrawOptionData;
			m_ReorderableList.drawHeaderCallback = DrawHeader;
			m_ReorderableList.elementHeight += 16;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Init(property);

			m_ReorderableList.DoList(position);
		}

		private void DrawHeader (Rect rect)
		{
			GUI.Label(rect, "Languages");
		}

		private void DrawOptionData (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty itemLanguage = itemData.FindPropertyRelative("m_Language");
			SerializedProperty itemGroup = itemData.FindPropertyRelative("m_Group");

			RectOffset offset = new RectOffset(0, 0, -1, -3);
			rect = offset.Add(rect);
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUIUtility.labelWidth = 66f;

			EditorGUI.PropertyField(rect, itemLanguage, new GUIContent("Language"));
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			EditorGUI.PropertyField(rect, itemGroup, new GUIContent("Group"));
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Init(property);

			return m_ReorderableList.GetHeight();
		}
	}
}
