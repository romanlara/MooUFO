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
	[CustomEditor(typeof(Localizer), true)]
	[CanEditMultipleObjects]
	public class LocalizerEditor : Editor 
	{
		SerializedProperty m_Current;
		SerializedProperty m_SelectedIndex;
//		SerializedProperty m_Languages;

		public void OnEnable ()
		{
			m_Current = serializedObject.FindProperty("m_Current");
			m_SelectedIndex = serializedObject.FindProperty("m_SelectedIndex");
//			m_Languages = serializedObject.FindProperty("m_Languages");

			Localizer localizer = target as Localizer;
			var allNames = localizer.languageNames;
			if (allNames.Length > 0)
				localizer.LoadLanguage(allNames[m_SelectedIndex.intValue]);
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			Localizer localizer = target as Localizer;
			var allNames = localizer.languageNames;

			if (GUILayout.Button("Update"))
			{
				allNames = localizer.languageNames;
				localizer.LoadLanguage(allNames[m_SelectedIndex.intValue]);
			}

			if (allNames.Length > 0)
			{
				m_SelectedIndex.intValue = EditorGUILayout.Popup("Selected", m_SelectedIndex.intValue, allNames);
				m_Current.stringValue = allNames[m_SelectedIndex.intValue];
			}
			else
				EditorGUILayout.HelpBox("The Localizer has not languages assigned or their Label property is empty.", MessageType.Error);

			EditorGUILayout.Space();

//			EditorGUILayout.PropertyField(m_Languages);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
