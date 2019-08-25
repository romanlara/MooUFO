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
	[CustomEditor(typeof(Language), true)]
	[CanEditMultipleObjects]
	public class LanguageEditor : Editor 
	{
		SerializedProperty m_Label;
		SerializedProperty m_Translations;

		public void OnEnable ()
		{
			m_Label = serializedObject.FindProperty("m_Label");
			m_Translations = serializedObject.FindProperty("m_Translations");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_Label);
			EditorGUILayout.PropertyField(m_Translations);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
