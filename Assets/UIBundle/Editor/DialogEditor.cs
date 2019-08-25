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
	[CustomEditor(typeof(Dialog), true)]
	[CanEditMultipleObjects]
	public class DialogEditor : Editor 
	{
		SerializedProperty m_ButtonTemplate;
		SerializedProperty m_ModalPanel;
		SerializedProperty m_ButtonPanel;
		SerializedProperty m_Question;
		SerializedProperty m_Title;
		SerializedProperty m_Icon;
		SerializedProperty m_Background;
		SerializedProperty m_SetNavigation;
		SerializedProperty m_Details;

		public void OnEnable ()
		{
			m_ButtonTemplate = serializedObject.FindProperty("m_ButtonTemplate");
			m_ModalPanel = serializedObject.FindProperty("m_ModalPanel");
			m_ButtonPanel = serializedObject.FindProperty("m_ButtonPanel");
			m_Question = serializedObject.FindProperty("m_Question");
			m_Title = serializedObject.FindProperty("m_Title");
			m_Icon = serializedObject.FindProperty("m_Icon");
			m_Background = serializedObject.FindProperty("m_Background");
			m_SetNavigation = serializedObject.FindProperty("m_SetNavigation");
			m_Details = serializedObject.FindProperty("m_Details");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var animator = ((target as Dialog).modalPanel != null) ? (target as Dialog).modalPanel.GetComponent<Animator>() : null;
			var buttonTemplate = m_ButtonTemplate.objectReferenceValue as GameObject;
			var modalPanel = m_ModalPanel.objectReferenceValue as GameObject;
			var buttonPanel = m_ButtonPanel.objectReferenceValue as RectTransform;

			EditorGUILayout.PropertyField(m_ButtonTemplate);
			if (buttonTemplate == null)
				EditorGUILayout.HelpBox("You must have a Game Object button template in order to use a button style and send events.", MessageType.Warning);

			EditorGUILayout.PropertyField(m_ModalPanel);
			if (modalPanel == null)
				EditorGUILayout.HelpBox("You must have a Game Object modal panel in order to hide and show the dialog with or without an animation.", MessageType.Warning);

			EditorGUILayout.PropertyField(m_ButtonPanel);
			if (buttonPanel == null)
				EditorGUILayout.HelpBox("Specify a RectTransform for to locate buttons. It must have a parent RectTrasform that the buttons can be within.", MessageType.Warning);

			EditorGUILayout.PropertyField(m_Question);
			EditorGUILayout.PropertyField(m_Title);
			EditorGUILayout.PropertyField(m_Icon);
			EditorGUILayout.PropertyField(m_Background);
			EditorGUILayout.PropertyField(m_SetNavigation);

			if (animator == null || animator.runtimeAnimatorController == null)
			{
				if (GUI.Button(EditorGUILayout.GetControlRect(), "Auto Generate Animation", EditorStyles.miniButton))
				{
					var controller = GenerateDialogAnimatorController(target as Dialog);
					if (controller != null)
					{
						if (animator == null)
							animator = (target as Dialog).modalPanel.AddComponent<Animator>();

						AnimatorController.SetAnimatorController(animator, controller);
					}
				}
			}

			EditorGUILayout.PropertyField(m_Details);

			serializedObject.ApplyModifiedProperties();
		}

		private static string GetSaveControllerPath (Dialog target)
		{
			var defaultName = target.gameObject.name;
			var message = string.Format("Create a new animator for the game object '{0}':", defaultName + " > Modal Panel");
			return EditorUtility.SaveFilePanelInProject("New Animation Controller", defaultName, "controller", message);
		}

		private static AnimatorController GenerateDialogAnimatorController (Dialog target)
		{
			if (target == null)
				return null;

			var path = GetSaveControllerPath(target);
			if (string.IsNullOrEmpty(path))
				return null;

			var openName = string.IsNullOrEmpty(target.openTrigger) ? "Open" : target.openTrigger;
			var closeName = string.IsNullOrEmpty(target.closeTrigger) ? "Close" : target.closeTrigger;
			
			var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
			var openState = GenerateTriggableTransition(openName, controller, null);
			GenerateTriggableTransition(closeName, controller, openState);

			AssetDatabase.ImportAsset(path);

			return controller;
		}

		private static AnimatorState GenerateTriggableTransition (string name, AnimatorController controller, AnimatorState otherState)
		{
			// Create the clip
			var clip = AnimatorController.AllocateAnimatorClip(name);
			AssetDatabase.AddObjectToAsset(clip, controller);

			// Create a state in animator controller for this clip
			var state = controller.AddMotion(clip);

			if (otherState != null)
			{
				// Add a transition property
				controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

				// Add an state transition
				var transition = otherState.AddTransition(state);
				transition.AddCondition(AnimatorConditionMode.If, 0, name);
			}

			return state;
		}
	}
}
