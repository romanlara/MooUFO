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
	[CustomEditor(typeof(LoadingScreen), true)]
	[CanEditMultipleObjects]
	public class LoadingScreenEditor : Editor 
	{
		SerializedProperty m_LoadingPanel;
		SerializedProperty m_Title;
		SerializedProperty m_Message;
		SerializedProperty m_Background;
		SerializedProperty m_Progressbar;
		SerializedProperty m_Details;

		public void OnEnable ()
		{
			m_LoadingPanel = serializedObject.FindProperty("m_LoadingPanel");
			m_Title = serializedObject.FindProperty("m_Title");
			m_Message = serializedObject.FindProperty("m_Message");
			m_Background = serializedObject.FindProperty("m_Background");
			m_Progressbar = serializedObject.FindProperty("m_Progressbar");
			m_Details = serializedObject.FindProperty("m_Details");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			var animator = ((target as LoadingScreen).loadingPanel != null) ? (target as LoadingScreen).loadingPanel.GetComponent<Animator>() : null;
			var loadingPanel = m_LoadingPanel.objectReferenceValue as GameObject;

			EditorGUILayout.PropertyField(m_LoadingPanel);
			if (loadingPanel == null)
				EditorGUILayout.HelpBox("You must have a Game Object loading panel in order to hide and show the loading screen with or without an animation.", MessageType.Warning);

			EditorGUILayout.PropertyField(m_Title);
			EditorGUILayout.PropertyField(m_Message);
			EditorGUILayout.PropertyField(m_Background);
			EditorGUILayout.PropertyField(m_Progressbar);

			if (animator == null || animator.runtimeAnimatorController == null)
			{
				if (GUI.Button(EditorGUILayout.GetControlRect(), "Auto Generate Animation", EditorStyles.miniButton))
				{
					var controller = GenerateDialogAnimatorController(target as LoadingScreen);
					if (controller != null)
					{
						if (animator == null)
							animator = (target as LoadingScreen).loadingPanel.AddComponent<Animator>();

						AnimatorController.SetAnimatorController(animator, controller);
					}
				}
			}

			EditorGUILayout.PropertyField(m_Details);

			serializedObject.ApplyModifiedProperties();
		}

		private static string GetSaveControllerPath (LoadingScreen target)
		{
			var defaultName = target.gameObject.name;
			var message = string.Format("Create a new animator for the game object '{0}':", defaultName + " > Loading Panel");
			return EditorUtility.SaveFilePanelInProject("New Animation Controller", defaultName, "controller", message);
		}

		private static AnimatorController GenerateDialogAnimatorController (LoadingScreen target)
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
