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
	[CustomEditor(typeof(OverlayHitPointBar), true)]
	[CanEditMultipleObjects]
	public class OverlayHitPointBarEditor : Editor 
	{
		SerializedProperty m_HPBarTemplate;
		SerializedProperty m_Options;

//		private bool m_Foldout = false;

		public void OnEnable ()
		{
			m_HPBarTemplate = serializedObject.FindProperty("m_HPBarTemplate");
			m_Options = serializedObject.FindProperty("m_Options");
		}
		
		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var template = m_HPBarTemplate.objectReferenceValue as RectTransform;

			EditorGUILayout.PropertyField(m_HPBarTemplate);
			if (template != null)
			{
//				EditorGUILayout.Space();

//				m_Foldout = EditorGUILayout.Foldout(m_Foldout, "Hit Point Bars (" + (target as OverlayHitPointBar).options.Count + ")");
//				if (m_Foldout)
				GUI.enabled = false;
					EditorGUILayout.PropertyField(m_Options);
				GUI.enabled = true;
			}
			else
			{
				EditorGUILayout.HelpBox("You must have a RectTransform template serving show HP bars.", MessageType.Warning);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
