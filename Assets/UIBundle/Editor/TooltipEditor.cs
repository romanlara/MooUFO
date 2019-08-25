// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections;

namespace UIBundle
{
	[CustomEditor(typeof(Tooltip), true)]
	[CanEditMultipleObjects]
	public class TooltipEditor : Editor 
	{
		SerializedProperty m_Template;
		SerializedProperty m_Title;
		SerializedProperty m_Message;
		SerializedProperty m_Icon;
		SerializedProperty m_Background;
		SerializedProperty m_Details;

		public void OnEnable ()
		{
			m_Template = serializedObject.FindProperty("m_Template");
			m_Title = serializedObject.FindProperty("m_Title");
			m_Message = serializedObject.FindProperty("m_Message");
			m_Icon = serializedObject.FindProperty("m_Icon");
			m_Background = serializedObject.FindProperty("m_Background");
			m_Details = serializedObject.FindProperty("m_Details");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var template = m_Template.objectReferenceValue as RectTransform;

			EditorGUILayout.PropertyField(m_Template);
			if (template != null)
			{
				EditorGUILayout.PropertyField(m_Title);
				EditorGUILayout.PropertyField(m_Message);
				EditorGUILayout.PropertyField(m_Icon);
				EditorGUILayout.PropertyField(m_Background);
				EditorGUILayout.PropertyField(m_Details);
			}
			else
			{
				EditorGUILayout.HelpBox("You must have a RectTransform template in order to hide and show the tooltip.", MessageType.Warning);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
