// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace UIBundle
{
	[CustomEditor(typeof(Spinner), true)]
	[CanEditMultipleObjects]
	public class SpinnerEditor : SelectableEditor 
	{
		SerializedProperty m_ArrowBackwardSpriteState;
		SerializedProperty m_ArrowForwardSpriteState;
		SerializedProperty m_ArrowBackwardAnimationTriggers;
		SerializedProperty m_ArrowForwardAnimationTriggers;
		SerializedProperty m_ArrowBackward;
		SerializedProperty m_ArrowForward;
		SerializedProperty m_Label;
		SerializedProperty m_Image;
		SerializedProperty m_Orientation;
		SerializedProperty m_Loop;
		SerializedProperty m_Value;
		SerializedProperty m_Type;
		SerializedProperty m_Range;
		SerializedProperty m_Options;
		SerializedProperty m_Scrollbar;
		SerializedProperty m_ScrollRect;
		SerializedProperty m_OnValueChanged;

		SerializedProperty m_Transition;

		private AnimBool m_ShowArrowSpriteState = new AnimBool();
		private AnimBool m_ShowArrowAnimationTriggers = new AnimBool();
		
		protected override void OnEnable ()
		{
			base.OnEnable();

			m_ArrowBackwardSpriteState = serializedObject.FindProperty("m_ArrowBackwardSpriteState");
			m_ArrowForwardSpriteState = serializedObject.FindProperty("m_ArrowForwardSpriteState");
			m_ArrowBackwardAnimationTriggers = serializedObject.FindProperty("m_ArrowBackwardAnimationTriggers");
			m_ArrowForwardAnimationTriggers = serializedObject.FindProperty("m_ArrowForwardAnimationTriggers");
			m_ArrowBackward = serializedObject.FindProperty("m_ArrowBackward");
			m_ArrowForward = serializedObject.FindProperty("m_ArrowForward");
			m_Label = serializedObject.FindProperty("m_Label");
			m_Image = serializedObject.FindProperty("m_Image");
			m_Orientation = serializedObject.FindProperty("m_Orientation");
			m_Loop = serializedObject.FindProperty("m_Loop");
			m_Value = serializedObject.FindProperty("m_Value");
			m_Type = serializedObject.FindProperty("m_Type");
			m_Range = serializedObject.FindProperty("m_Range");
			m_Options = serializedObject.FindProperty("m_Options");
			m_Scrollbar = serializedObject.FindProperty("m_Scrollbar");
			m_ScrollRect = serializedObject.FindProperty("m_ScrollRect");
			m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");

			m_Transition = serializedObject.FindProperty("m_Transition");

			var trans = GetTransition(m_Transition);
			m_ShowArrowSpriteState.value = (trans == Selectable.Transition.SpriteSwap);
			m_ShowArrowAnimationTriggers.value = (trans == Selectable.Transition.Animation);

			m_ShowArrowSpriteState.valueChanged.AddListener(Repaint);
			m_ShowArrowAnimationTriggers.valueChanged.AddListener(Repaint);
		}
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space();

			serializedObject.Update();

			var trans = GetTransition(m_Transition);
			var animator = (target as Selectable).GetComponent<Animator>();
			var graphicArrowBackward = m_ArrowBackward.objectReferenceValue as Graphic;
			var graphicArrowForward = m_ArrowForward.objectReferenceValue as Graphic;

			m_ShowArrowSpriteState.target = (trans == Selectable.Transition.SpriteSwap);
			m_ShowArrowAnimationTriggers.target = (trans == Selectable.Transition.Animation);

			EditorGUILayout.PropertyField(m_ArrowBackward);
			if (EditorGUILayout.BeginFadeGroup(m_ShowArrowSpriteState.faded))
			{
				++EditorGUI.indentLevel;
				{
					if (graphicArrowBackward == null)
						EditorGUILayout.HelpBox("You must have a Graphic arrow backward in order to use a color transition.", MessageType.Warning);
					EditorGUILayout.PropertyField(m_ArrowBackwardSpriteState.FindPropertyRelative("m_HighlightedSprite"));
					EditorGUILayout.PropertyField(m_ArrowBackwardSpriteState.FindPropertyRelative("m_PressedSprite"));
					EditorGUILayout.PropertyField(m_ArrowBackwardSpriteState.FindPropertyRelative("m_DisabledSprite"));
				}
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFadeGroup();
			if (EditorGUILayout.BeginFadeGroup(m_ShowArrowAnimationTriggers.faded))
			{
				++EditorGUI.indentLevel;
				{
					if (graphicArrowBackward as Image == null)
						EditorGUILayout.HelpBox("You must have a Image arrow backward in order to use a sprite swap transition.", MessageType.Warning);
					EditorGUILayout.PropertyField(m_ArrowBackwardAnimationTriggers.FindPropertyRelative("m_NormalTrigger"));
					EditorGUILayout.PropertyField(m_ArrowBackwardAnimationTriggers.FindPropertyRelative("m_HighlightedTrigger"));
					EditorGUILayout.PropertyField(m_ArrowBackwardAnimationTriggers.FindPropertyRelative("m_PressedTrigger"));
					EditorGUILayout.PropertyField(m_ArrowBackwardAnimationTriggers.FindPropertyRelative("m_DisabledTrigger"));
				}
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.PropertyField(m_ArrowForward);
			if (EditorGUILayout.BeginFadeGroup(m_ShowArrowSpriteState.faded))
			{
				++EditorGUI.indentLevel;
				{
					if (graphicArrowForward == null)
						EditorGUILayout.HelpBox("You must have a Graphic arrow forward in order to use a color transition.", MessageType.Warning);
					EditorGUILayout.PropertyField(m_ArrowForwardSpriteState.FindPropertyRelative("m_HighlightedSprite"));
					EditorGUILayout.PropertyField(m_ArrowForwardSpriteState.FindPropertyRelative("m_PressedSprite"));
					EditorGUILayout.PropertyField(m_ArrowForwardSpriteState.FindPropertyRelative("m_DisabledSprite"));
				}
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFadeGroup();
			if (EditorGUILayout.BeginFadeGroup(m_ShowArrowAnimationTriggers.faded))
			{
				++EditorGUI.indentLevel;
				{
					if (graphicArrowForward as Image == null)
						EditorGUILayout.HelpBox("You must have a Image arrow forward in order to use a sprite swap transition.", MessageType.Warning);
					EditorGUILayout.PropertyField(m_ArrowForwardAnimationTriggers.FindPropertyRelative("m_NormalTrigger"));
					EditorGUILayout.PropertyField(m_ArrowForwardAnimationTriggers.FindPropertyRelative("m_HighlightedTrigger"));
					EditorGUILayout.PropertyField(m_ArrowForwardAnimationTriggers.FindPropertyRelative("m_PressedTrigger"));
					EditorGUILayout.PropertyField(m_ArrowForwardAnimationTriggers.FindPropertyRelative("m_DisabledTrigger"));

					var controller = GetController(target as Selectable);
					if (animator != null && controller != null && controller.layers.Length == 1)
					{
						Rect buttonRect = EditorGUILayout.GetControlRect();
						buttonRect.xMin += EditorGUIUtility.labelWidth;
						if (GUI.Button(buttonRect, "Update Animations", EditorStyles.miniButton))
						{
							controller = UpdateSelectableAnimatorController(
								(target as Spinner).arrowBackwardAnimationTriggers, 
								(target as Spinner).arrowForwardAnimationTriggers,
								target as Selectable
							);

							if (controller != null)
							{
								if (animator == null)
									animator = (target as Selectable).gameObject.AddComponent<Animator>();
								
								AnimatorController.SetAnimatorController(animator, controller);
							}
						}
					}
				}
				--EditorGUI.indentLevel;
			}
			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.PropertyField(m_Label);
			EditorGUILayout.PropertyField(m_Image);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_Orientation);
			if (EditorGUI.EndChangeCheck())
			{
				Spinner.Orientation orientation = (Spinner.Orientation)m_Orientation.enumValueIndex;
				foreach (var obj in serializedObject.targetObjects)
				{
					Spinner spinner = obj as Spinner;
					spinner.SetOrientation(orientation, true);
				}
			}

			EditorGUILayout.PropertyField(m_Loop);
			EditorGUILayout.PropertyField(m_Value);

			var type = GetType(m_Type);
			EditorGUILayout.PropertyField(m_Type);
			switch (type)
			{
			case Spinner.SpinnerType.Range:
				++EditorGUI.indentLevel;
				{
					EditorGUILayout.PropertyField(m_Range);
				}
				--EditorGUI.indentLevel;
				break;
			case Spinner.SpinnerType.List:
				EditorGUILayout.PropertyField(m_Options);
				break;
			case Spinner.SpinnerType.Scrollbar:
				++EditorGUI.indentLevel;
				{
					EditorGUILayout.PropertyField(m_Scrollbar);
				}
				--EditorGUI.indentLevel;
				break;
			case Spinner.SpinnerType.ScrollRect:
				++EditorGUI.indentLevel;
				{
					EditorGUILayout.PropertyField(m_ScrollRect);
				}
				--EditorGUI.indentLevel;
				break;
			}

			EditorGUILayout.PropertyField(m_OnValueChanged);
			serializedObject.ApplyModifiedProperties();
		}

		static Spinner.SpinnerType GetType (SerializedProperty spinnerType)
		{
			return (Spinner.SpinnerType) spinnerType.enumValueIndex;
		}

		static Selectable.Transition GetTransition (SerializedProperty transition)
		{
			return (Selectable.Transition) transition.enumValueIndex;
		}

		private static AnimatorController UpdateSelectableAnimatorController (Spinner.ArrowAnimationTriggers arrowBackwardAnimationTriggers, Spinner.ArrowAnimationTriggers arrowForwardAnimationTriggers, Selectable target)
		{
			if (target == null) 
				return null;

			// Default names
			var normalNameBackwardDefault = Spinner.ArrowAnimationTriggers.kDefaultNormalAnimName + Spinner.ArrowAnimationTriggers.kArrowBackward;
			var normalNameForwardDefault = Spinner.ArrowAnimationTriggers.kDefaultNormalAnimName + Spinner.ArrowAnimationTriggers.kArrowForward;

			var highlightedNameBackwardDefault = Spinner.ArrowAnimationTriggers.kDefaultSelectedAnimName + Spinner.ArrowAnimationTriggers.kArrowBackward;
			var highlightedNameForwardDefault = Spinner.ArrowAnimationTriggers.kDefaultSelectedAnimName + Spinner.ArrowAnimationTriggers.kArrowForward;

			var pressedNameBackwardDefault = Spinner.ArrowAnimationTriggers.kDefaultPressedAnimName + Spinner.ArrowAnimationTriggers.kArrowBackward;
			var pressedNameForwardDefault = Spinner.ArrowAnimationTriggers.kDefaultPressedAnimName + Spinner.ArrowAnimationTriggers.kArrowForward;

			var disabledNameBackwardDefault = Spinner.ArrowAnimationTriggers.kDefaultDisabledAnimName + Spinner.ArrowAnimationTriggers.kArrowBackward;
			var disabledNameForwardDefault = Spinner.ArrowAnimationTriggers.kDefaultDisabledAnimName + Spinner.ArrowAnimationTriggers.kArrowForward;

			// Transition names
			var normalNameBackward = string.IsNullOrEmpty(arrowBackwardAnimationTriggers.normalTrigger) ? normalNameBackwardDefault : arrowBackwardAnimationTriggers.normalTrigger;
			var highlightedNameBackward = string.IsNullOrEmpty(arrowBackwardAnimationTriggers.highlightedTrigger) ? highlightedNameBackwardDefault : arrowBackwardAnimationTriggers.highlightedTrigger;
			var pressedNameBackward = string.IsNullOrEmpty(arrowBackwardAnimationTriggers.pressedTrigger) ? pressedNameBackwardDefault : arrowBackwardAnimationTriggers.pressedTrigger;
			var disabledNameBackward = string.IsNullOrEmpty(arrowBackwardAnimationTriggers.disabledTrigger) ? disabledNameBackwardDefault : arrowBackwardAnimationTriggers.disabledTrigger;

			var normalNameForward = string.IsNullOrEmpty(arrowForwardAnimationTriggers.normalTrigger) ? normalNameForwardDefault : arrowForwardAnimationTriggers.normalTrigger;
			var highlightedNameForward = string.IsNullOrEmpty(arrowForwardAnimationTriggers.highlightedTrigger) ? highlightedNameForwardDefault : arrowForwardAnimationTriggers.highlightedTrigger;
			var pressedNameForward = string.IsNullOrEmpty(arrowForwardAnimationTriggers.pressedTrigger) ? pressedNameForwardDefault : arrowForwardAnimationTriggers.pressedTrigger;
			var disabledNameForward = string.IsNullOrEmpty(arrowForwardAnimationTriggers.disabledTrigger) ? disabledNameForwardDefault : arrowForwardAnimationTriggers.disabledTrigger;

			// Get controller
			var controller = GetController(target); //AssetDatabase.LoadAssetAtPath<AnimatorController>(GetControllerPath(target));
			if (controller == null)
				return null;
			
			int layer = 1;
			string layerName = "Arrow Backward";

			// Create new layer
			controller.AddLayer(new AnimatorControllerLayer() {
				name = layerName,
				defaultWeight = 1f,
				stateMachine = new AnimatorStateMachine() { 
					name = layerName,
					hideFlags = HideFlags.HideInHierarchy
				}
			});
			AssetDatabase.AddObjectToAsset(controller.layers[layer].stateMachine, controller);

			// Generate transitions
			GenerateTriggarebleTransition(normalNameBackward, controller, layer);
			GenerateTriggarebleTransition(highlightedNameBackward, controller, layer);
			GenerateTriggarebleTransition(pressedNameBackward, controller, layer);
			GenerateTriggarebleTransition(disabledNameBackward, controller, layer);

			layer = 2;
			layerName = "Arrow Forward";

			// Create new layer
			controller.AddLayer(new AnimatorControllerLayer() {
				name = layerName,
				defaultWeight = 1f,
				stateMachine = new AnimatorStateMachine() { 
					name = layerName,
					hideFlags = HideFlags.HideInHierarchy
				}
			});
			AssetDatabase.AddObjectToAsset(controller.layers[layer].stateMachine, controller);

			// Generate transitions
			GenerateTriggarebleTransition(normalNameForward, controller, layer);
			GenerateTriggarebleTransition(highlightedNameForward, controller, layer);
			GenerateTriggarebleTransition(pressedNameForward, controller, layer);
			GenerateTriggarebleTransition(disabledNameForward, controller, layer);

			AssetDatabase.ImportAsset(GetControllerPath(target));

			return controller;
		}

		private static AnimatorController GetController (Selectable target)
		{
			return AssetDatabase.LoadAssetAtPath<AnimatorController>(GetControllerPath(target));
		}

		private static string GetControllerPath (Selectable target)
		{
			return AssetDatabase.GetAssetPath((target.animator != null ? target.animator.runtimeAnimatorController : null));//EditorUtility.OpenFilePanel("Update Generated Animations", "", "controller");
		}

		private static AnimationClip GenerateTriggarebleTransition (string name, AnimatorController controller, int layer)
		{
			// Create the clip
			var clip = AnimatorController.AllocateAnimatorClip(name);
			AssetDatabase.AddObjectToAsset(clip, controller);

			// Create a state in animator controller for this clip
			var state = controller.AddMotion(clip, layer);

			// Add a transition property
			controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

			// Add an any state transition
			var stateMachine = controller.layers[layer].stateMachine;
			var transition = stateMachine.AddAnyStateTransition(state);
			transition.AddCondition(AnimatorConditionMode.If, 0, name);

			return clip;
		}
	}
}
