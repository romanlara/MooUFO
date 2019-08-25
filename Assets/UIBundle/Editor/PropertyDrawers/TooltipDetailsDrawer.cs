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
	[CustomPropertyDrawer(typeof(Tooltip.TooltipDetails))]
	public class TooltipDetailsDrawer : PropertyDrawer 
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
				contentPosition.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Message"));//60f;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Message"), new GUIContent("Message"));
				contentPosition.y += contentPosition.height + 3;
				contentPosition.height = 16f;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Icon"), new GUIContent("Icon"));
				contentPosition.y += contentPosition.height + 3;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Background"), new GUIContent("Background"));
				contentPosition.y += contentPosition.height + 3;
			} EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 5.7f + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_Message"));
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
	}
}
