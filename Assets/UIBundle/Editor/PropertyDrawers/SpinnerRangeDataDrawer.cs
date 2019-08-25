// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace UIBundle
{
	[CustomPropertyDrawer(typeof(Spinner.RangeData))]
	public class SpinnerRangeDataDrawer : PropertyDrawer 
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			{
				Rect contentPosition = EditorGUI.PrefixLabel(position, label);

				if (position.height > 16f)
				{
					position.height = 16f;
					EditorGUI.indentLevel += 1;
					contentPosition = EditorGUI.IndentedRect(position);
					contentPosition.y += 18f;
				}

				contentPosition.width *= 0.33f;
				EditorGUI.indentLevel = 0;
				EditorGUIUtility.labelWidth = 26f;

				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Min"), new GUIContent("Min"));
				contentPosition.x += contentPosition.width;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Max"), new GUIContent("Max"));
				contentPosition.x += contentPosition.width;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("m_Step"), new GUIContent("Step"));
			} EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return Screen.width < 333 ? (16f + 18f) : 16f;
		}
	}
}
