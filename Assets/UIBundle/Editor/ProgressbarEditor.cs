// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace UIBundle
{
	[CustomEditor(typeof(Progressbar), true)]
	[CanEditMultipleObjects]
	public class ProgressbarEditor : Editor 
	{
		SerializedProperty m_TargetGraphic;
		SerializedProperty m_FillingRect;
		SerializedProperty m_Direction;
		SerializedProperty m_IndicatorRect;
		SerializedProperty m_Label;
		SerializedProperty m_Value;
		SerializedProperty m_OnValueChanged;

		public void OnEnable ()
		{
			m_TargetGraphic = serializedObject.FindProperty("m_TargetGraphic");
			m_FillingRect = serializedObject.FindProperty("m_FillingRect");
			m_Direction = serializedObject.FindProperty("m_Direction");
			m_IndicatorRect = serializedObject.FindProperty("m_IndicatorRect");
			m_Label = serializedObject.FindProperty("m_Label");
			m_Value = serializedObject.FindProperty("m_Value");
			m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var fillingRect = (target as Progressbar).fillingRect;

			EditorGUILayout.PropertyField(m_TargetGraphic);
			EditorGUILayout.PropertyField(m_FillingRect);

			if (fillingRect != null)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_Direction);
				if (EditorGUI.EndChangeCheck())
				{
					Progressbar.Direction direction = (Progressbar.Direction)m_Direction.enumValueIndex;
					foreach (var obj in serializedObject.targetObjects)
					{
						Progressbar progressbar = obj as Progressbar;
						progressbar.SetDirection(direction, true);
					}
				}

				EditorGUILayout.PropertyField(m_IndicatorRect);
				EditorGUILayout.PropertyField(m_Label);
				EditorGUILayout.PropertyField(m_Value);
				EditorGUILayout.PropertyField(m_OnValueChanged);
			}
			else
			{
				EditorGUILayout.HelpBox("Specify a RectTransform for the progressbar filling.", MessageType.Info);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
