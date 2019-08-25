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
	[CustomEditor(typeof(Translator), true)]
	[CanEditMultipleObjects]
	public class TranslatorEditor : Editor 
	{
		SerializedProperty m_Text;
		SerializedProperty m_UseKey;
		SerializedProperty m_Group;
		SerializedProperty m_Key;
		SerializedProperty m_GroupIndex;
		SerializedProperty m_KeyIndex;

		private Translator instance;
//		private int m_KeyIndex;
//		private int m_GroupIndex;

		public void OnEnable ()
		{
			instance = target as Translator;

			m_Text = serializedObject.FindProperty("m_Text");
			m_UseKey = serializedObject.FindProperty("m_UseKey");
			m_Group = serializedObject.FindProperty("m_Group");
			m_Key = serializedObject.FindProperty("m_Key");
			m_GroupIndex = serializedObject.FindProperty("m_GroupIndex");
			m_KeyIndex = serializedObject.FindProperty("m_KeyIndex");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();
			
			var localizer = GameObject.FindObjectOfType<Localizer>();

			EditorGUILayout.PropertyField(m_Text);

			if (localizer)
			{
				EditorGUILayout.Space();

				if (localizer.language != null)
				{
					EditorGUILayout.PropertyField(m_UseKey);

					var allGroups = localizer.language.AllGroups();

					if (allGroups.Length > 0)
					{
						m_GroupIndex.intValue = EditorGUILayout.Popup("Group", m_GroupIndex.intValue, allGroups);
						m_Group.stringValue = allGroups[m_GroupIndex.intValue];
					}
					else
						EditorGUILayout.HelpBox("The Localizer has not groups assigned. Please you must have for at least one group.", MessageType.Warning);

					if (instance.useKey)
					{
						var language = localizer.language;//.GetCurrent(instance.group);
						if (language != null)
						{
							var allKeys = language.AllKeys(m_Group.stringValue);

							if (allKeys.Length > 0/* && (m_KeyIndex.intValue > 0 && m_KeyIndex.intValue < allKeys.Length)*/)
							{
								m_KeyIndex.intValue= EditorGUILayout.Popup("Key", m_KeyIndex.intValue, allKeys);
								m_Key.stringValue = allKeys[Mathf.Clamp(m_KeyIndex.intValue, 0, allKeys.Length - 1)];
							}
							else
								EditorGUILayout.HelpBox("The Language asset is empty, has no keys.", MessageType.Warning);
						}
						else
							EditorGUILayout.HelpBox("The Language asset has not assigned a label.", MessageType.Warning);
					}
				}
				else
					EditorGUILayout.HelpBox("You must have for at least one language in the Localizer component.", MessageType.Warning);
			}
			else
				EditorGUILayout.HelpBox("You must have a Localizer component in your scene. The Translator component just works with the Localizer.", MessageType.Error);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
