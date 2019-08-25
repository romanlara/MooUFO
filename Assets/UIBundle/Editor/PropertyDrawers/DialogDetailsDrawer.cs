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
	[CustomPropertyDrawer(typeof(Dialog.DialogDetails))]
	public class DialogDetailsDrawer : PropertyDrawer 
	{
		public class Styles
		{
			public readonly GUIStyle headerBackground = "RL Header";
			public readonly GUIStyle boxBackground = "RL Background";

			public void DrawHeaderBackground (Rect headerRect)
			{
				if (Event.current.type == EventType.Repaint)
					this.headerBackground.Draw(headerRect, false, false, false, false);
			}

			public void DrawBoxBackground (Rect boxRect)
			{
				if (Event.current.type == EventType.Repaint)
					this.boxBackground.Draw(boxRect, false, false, false, false);
			}
		}

		private static Styles s_Styles;

		ReorderableList m_ReorderableList;

		private void Init (SerializedProperty property)
		{
			if (m_ReorderableList != null)
				return;
			
			SerializedProperty array = property.FindPropertyRelative("m_ButtonDetails");

			m_ReorderableList = new ReorderableList(array.serializedObject, array);
			m_ReorderableList.drawElementBackgroundCallback = DrawElementBackground;
			m_ReorderableList.drawElementCallback = DrawOptionData;
			m_ReorderableList.drawHeaderCallback = DrawHeader;
			m_ReorderableList.elementHeightCallback = ElementHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (s_Styles == null)
				s_Styles = new Styles();

			label = EditorGUI.BeginProperty(position, label, property);
			{
				if (Event.current.type == EventType.Repaint)
					DoBoxBackground(position);

				if (Event.current.type == EventType.Repaint)
					s_Styles.DrawHeaderBackground(position);

				EditorGUI.LabelField(GetHeaderRect(position), label);
				Rect contentPosition = position;

				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect(position);
				contentPosition.y += 18f;

				contentPosition.width -= 6f;
				contentPosition.height = 16f;
				EditorGUI.indentLevel = 0;
				EditorGUIUtility.labelWidth = 106f;

				contentPosition.y += 3;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Title"), new GUIContent("Title"));
				contentPosition.y += contentPosition.height + 3;
				contentPosition.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Question"));//60f;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Question"), new GUIContent("Question"));
				contentPosition.y += contentPosition.height + 3;
				contentPosition.height = 16f;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Icon"), new GUIContent("Icon"));
				contentPosition.y += contentPosition.height + 3;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Background"), new GUIContent("Background"));
				contentPosition.y += contentPosition.height + 3;
				Init(property);
				m_ReorderableList.DoList(contentPosition);

			} EditorGUI.EndProperty();
		}

		private void DrawHeader (Rect rect)
		{
			GUI.Label(rect, "Buttons");
		}

		private void DrawOptionData (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty itemTitle = itemData.FindPropertyRelative("m_Title");
			SerializedProperty itemSetAsSelectedGameObject = itemData.FindPropertyRelative("m_SetAsSelectedGameObject");
			SerializedProperty itemAction = itemData.FindPropertyRelative("m_Action");

			RectOffset offset = new RectOffset(0, 0, -1, -3);
			rect = offset.Add(rect);
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUIUtility.labelWidth = 36f;

			EditorGUI.PropertyField(rect, itemSetAsSelectedGameObject, new GUIContent("Select"));
			rect.y += EditorGUIUtility.singleLineHeight + 3f;
			EditorGUI.PropertyField(rect, itemTitle, new GUIContent("Title"));
			rect.y += EditorGUIUtility.singleLineHeight + 3f;
			EditorGUI.PropertyField(rect, itemAction, GUIContent.none);
		}

		private float ElementHeight (int index)
		{
			SerializedProperty action = null;

			if (m_ReorderableList.count > 0)
				action = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Action");

			return (43 + (action != null ? EditorGUI.GetPropertyHeight(action) : 0));
		}

		private void DrawElementBackground (Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty action = null;

			if (m_ReorderableList.count > 0)
				action = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_Action");

			rect.height = 43 + (action != null ? EditorGUI.GetPropertyHeight(action) : 0);
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

		private Rect GetHeaderRect (Rect headerRect)
		{
			headerRect.x += 6f;
			return headerRect;
		}

		private void DoBoxBackground (Rect boxRect)
		{
			boxRect.y += 2f;
			boxRect.height -= 3f;
			s_Styles.DrawBoxBackground(boxRect);
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Init(property);
			float heightQuestion = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Question"));
			return base.GetPropertyHeight(property, label) * 5.7f + heightQuestion + m_ReorderableList.GetHeight();
		}
	}
}
