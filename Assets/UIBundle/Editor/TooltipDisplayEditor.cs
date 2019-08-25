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
	[CustomEditor(typeof(TooltipDisplay), true)]
	[CanEditMultipleObjects]
	public class TooltipDisplayEditor : Editor 
	{
		SerializedProperty m_TargetGraphic;
		SerializedProperty m_Label;
		SerializedProperty m_AnimFade;

		public void OnEnable ()
		{
			m_TargetGraphic = serializedObject.FindProperty("m_TargetGraphic");
			m_Label = serializedObject.FindProperty("m_Label");
			m_AnimFade = serializedObject.FindProperty("m_AnimFade");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_TargetGraphic);
			EditorGUILayout.PropertyField(m_Label);
			EditorGUILayout.PropertyField(m_AnimFade);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
