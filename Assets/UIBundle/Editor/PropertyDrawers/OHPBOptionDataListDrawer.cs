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
	[CustomPropertyDrawer(typeof(OverlayHitPointBar.OptionDataList), true)]
	public class OHPBOptionDataListDrawer : PropertyDrawer 
	{
		ReorderableList m_ReorderableList;

		private void Init (SerializedProperty property)
		{
			if (m_ReorderableList != null)
				return;

			SerializedProperty array = property.FindPropertyRelative("m_Options");

			m_ReorderableList = new ReorderableList(property.serializedObject, array);
			m_ReorderableList.drawElementCallback = DrawOptionData;
			m_ReorderableList.drawHeaderCallback = DrawHeader;
			m_ReorderableList.elementHeight += 6;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Init(property);

			m_ReorderableList.DoList(position);
		}

		private void DrawHeader (Rect rect)
		{
			GUI.Label(rect, "HP Bars");
		}

		private void DrawOptionData (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty itemHitPointBar = itemData.FindPropertyRelative("m_HitPointBar");

			RectOffset offset = new RectOffset(0, 0, -1, -3);
			rect = offset.Add(rect);
			rect.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(rect, itemHitPointBar, GUIContent.none);
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Init(property);

			return m_ReorderableList.GetHeight();
		}
	}
}
