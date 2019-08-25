
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections;

namespace UIBundle
{
	[CustomEditor(typeof(OverlayMessage), true)]
	[CanEditMultipleObjects]
	public class OverlayMessageEditor : Editor 
	{
		SerializedProperty m_MessageTemplate;
		SerializedProperty m_Zoom;
		SerializedProperty m_FadeOut;
		SerializedProperty m_ScaleDown;
		SerializedProperty m_MessageList;

		public void OnEnable ()
		{
			m_MessageTemplate = serializedObject.FindProperty("m_MessageTemplate");
			m_Zoom = serializedObject.FindProperty("m_Zoom");
			m_FadeOut = serializedObject.FindProperty("m_FadeOut");
			m_ScaleDown = serializedObject.FindProperty("m_ScaleDown");
			m_MessageList = serializedObject.FindProperty("m_MessageList");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var template = m_MessageTemplate.objectReferenceValue as RectTransform;

			EditorGUILayout.PropertyField(m_MessageTemplate);
			if (template != null)
			{
				EditorGUILayout.PropertyField(m_Zoom);
				EditorGUILayout.PropertyField(m_FadeOut);
				EditorGUILayout.PropertyField(m_ScaleDown);

				GUI.enabled = false;
				EditorGUILayout.PropertyField(m_MessageList);
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
