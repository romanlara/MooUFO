// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace UIBundle
{
	[AddComponentMenu("UI/UIBundle/Spinner")]
	[RequireComponent(typeof(RectTransform))]
	public class Spinner : Selectable, ICanvasElement
	{
		[System.Serializable]
		public class OptionData
		{
			[SerializeField]
			private string m_Text;
			public string text { get { return m_Text; } set { m_Text = value; } }

			[SerializeField]
			private Sprite m_Image;
			public Sprite image { get { return m_Image; } set { m_Image = value; } }
			
			public OptionData () {}
			
			public OptionData (string text)
			{
				this.text = text;
			}

			public OptionData (Sprite image)
			{
				this.image = image;
			}

			public OptionData (string text, Sprite image)
			{
				this.text = text;
				this.image = image;
			}
		}
		
		[System.Serializable]
		public class OptionDataList
		{
			[SerializeField]
			private List<OptionData> m_Options;
			public List<OptionData> options { get { return m_Options; } set { m_Options = value; } }
			
			public OptionDataList ()
			{
				options = new List<OptionData>();
			}
		}

		[System.Serializable]
		public class RangeData
		{
			[SerializeField]
			private float m_Min;
			public float min { get { return m_Min; } set { m_Min = value; } }

			[SerializeField]
			private float m_Max;
			public float max { get { return m_Max; } set { m_Max = value; } }

			[SerializeField]
			private float m_Step = 1f;
			public float step { get { return m_Step; } set { m_Step = value; } }

			public RangeData () {}

			public RangeData (float step)
			{
				this.step = step;
			}

			public RangeData (float min, float max)
			{
				this.min = min;
				this.max = max;
			}

			public RangeData (float min, float max, float step) : this(min, max)
			{
				this.step = step;
			}
		}

		[System.Serializable]
		public class ArrowSpriteState
		{
			[SerializeField]
			private Sprite m_HighlightedSprite;
			public Sprite highlightedSprite { get { return m_HighlightedSprite; } set { m_HighlightedSprite = value; } }

			[SerializeField]
			private Sprite m_PressedSprite;
			public Sprite pressedSprite { get { return m_PressedSprite; } set { m_PressedSprite = value; } }

			[SerializeField]
			private Sprite m_DisabledSprite;
			public Sprite disabledSprite { get { return m_DisabledSprite; } set { m_DisabledSprite = value; } }
		}

		[System.Serializable]
		public class ArrowAnimationTriggers
		{
			public const string kArrowBackward = "Backward";
			public const string kArrowForward = "Forward";
			public const string kDefaultNormalAnimName = "Normal ";
			public const string kDefaultSelectedAnimName = "Highlighted ";
			public const string kDefaultPressedAnimName = "Pressed ";
			public const string kDefaultDisabledAnimName = "Disabled ";

			public enum WhatArrowType { Backward, Forward }

			[SerializeField]
			private string m_NormalTrigger = kDefaultNormalAnimName;
			public string normalTrigger { get { return m_NormalTrigger; } set { m_NormalTrigger = value; } }

			[SerializeField]
			private string m_HighlightedTrigger = kDefaultSelectedAnimName;
			public string highlightedTrigger { get { return m_HighlightedTrigger; } set { m_HighlightedTrigger = value; } }

			[SerializeField]
			private string m_PressedTrigger = kDefaultPressedAnimName;
			public string pressedTrigger { get { return m_PressedTrigger; } set { m_PressedTrigger = value; } }

			[SerializeField]
			private string m_DisabledTrigger = kDefaultDisabledAnimName;
			public string disabledTrigger { get { return m_DisabledTrigger; } set { m_DisabledTrigger = value; } }

			public ArrowAnimationTriggers (WhatArrowType arrowType)
			{
				switch (arrowType)
				{
				case WhatArrowType.Backward:
					normalTrigger += kArrowBackward;
					highlightedTrigger += kArrowBackward;
					pressedTrigger += kArrowBackward;
					disabledTrigger += kArrowBackward;
					break;
				case WhatArrowType.Forward:
					normalTrigger += kArrowForward;
					highlightedTrigger += kArrowForward;
					pressedTrigger += kArrowForward;
					disabledTrigger += kArrowForward;
					break;
				}
			}
		}
		
		[System.Serializable]
		public class SpinnerListEvent : UnityEvent<float> {}

		public delegate string FixedFormat (float value);
		public FixedFormat OnFixedFormat;

		public delegate OptionData FlexibleFormat (int value, List<OptionData> options);
		public FlexibleFormat OnFlexibleFormat;

		public delegate void BackwardEvent ();
		public BackwardEvent OnBackward;

		public delegate void ForwardEvent ();
		public ForwardEvent OnForward;

		public enum Orientation
		{
			Horizontal = 0,
			Vertical = 1
		}

		public enum SpinnerType
		{
			Range,
			List,
			Scrollbar,
			ScrollRect
		}

		[SerializeField]
		private Graphic m_ArrowBackward;
		public Graphic arrowBackward { get { return m_ArrowBackward; } set { m_ArrowBackward = value; } }

		[SerializeField]
		private ArrowSpriteState m_ArrowBackwardSpriteState;
		public ArrowSpriteState arrowBackwardSpriteState { get { return m_ArrowBackwardSpriteState; } set { m_ArrowBackwardSpriteState = value; } }

		[SerializeField]
		private ArrowAnimationTriggers m_ArrowBackwardAnimationTriggers = new ArrowAnimationTriggers(ArrowAnimationTriggers.WhatArrowType.Backward);
		public ArrowAnimationTriggers arrowBackwardAnimationTriggers { get { return m_ArrowBackwardAnimationTriggers; } set { m_ArrowBackwardAnimationTriggers = value; } }
		
		[SerializeField]
		private Graphic m_ArrowForward;
		public Graphic arrowForward { get { return m_ArrowForward; } set { m_ArrowForward = value; } }

		[SerializeField]
		private ArrowSpriteState m_ArrowForwardSpriteState;
		public ArrowSpriteState arrowForwardSpriteState { get { return m_ArrowForwardSpriteState; } set { m_ArrowForwardSpriteState = value; } }

		[SerializeField]
		private ArrowAnimationTriggers m_ArrowForwardAnimationTriggers = new ArrowAnimationTriggers(ArrowAnimationTriggers.WhatArrowType.Forward);
		public ArrowAnimationTriggers arrowForwardAnimationTriggers { get { return m_ArrowForwardAnimationTriggers; } set { m_ArrowForwardAnimationTriggers = value; } }

		[Space]

		[SerializeField]
		private Graphic m_Label;
		public Graphic label { get { return m_Label; } set { m_Label = value; Refresh(); } }

		[SerializeField]
		private Graphic m_Image;
		public Graphic imageSpinner { get { return m_Image; } set { m_Image = value; Refresh(); } }
		
		[Space]

		[SerializeField]
		private Orientation m_Orientation = Orientation.Horizontal;
		public Orientation orientation { get { return m_Orientation; } set { m_Orientation = value; } }
		
		[SerializeField]
		private bool m_Loop;
		public bool loop { get { return m_Loop; } set { m_Loop = value; } }

		[Space]

		[SerializeField]
		private float m_Value;
		public float value
		{
			get { return m_Value; }
			set
			{
				if (Application.isPlaying && ((type == SpinnerType.List) ? options.Count == 0 : false))
					return;

				switch (type)
				{
				case SpinnerType.Range: m_Value = AdjustValue(value); break;
				case SpinnerType.List: m_Value = Mathf.Clamp((int)value, 0, options.Count - 1); break;
				case SpinnerType.Scrollbar: m_Value = Mathf.Clamp01(value); break;
				case SpinnerType.ScrollRect: 
					if (m_ScrollRect.movementType == ScrollRect.MovementType.Clamped) 
						m_Value = Mathf.Clamp01(value); 
					else
						m_Value = value; 
					break;
				}

				Refresh();
				m_OnValueChanged.Invoke(m_Value);
			}
		}
		
		[Space]

		[SerializeField]
		private SpinnerType m_Type = SpinnerType.Range;
		public SpinnerType type { get { return m_Type; } set { m_Type = value; Refresh(); } }

		[SerializeField]
		private RangeData m_Range = new RangeData();
		public RangeData range { get { return m_Range; } set { m_Range = value; Refresh(); } }

		[SerializeField]
		private OptionDataList m_Options = new OptionDataList();
		public List<OptionData> options { get { return m_Options.options; } set { m_Options.options = value; Refresh(); } }

		[SerializeField]
		private Scrollbar m_Scrollbar;
		public Scrollbar scrollbar 
		{ 
			get { return m_Scrollbar; } 
			set 
			{ 
				if (m_Scrollbar)
					m_Scrollbar.onValueChanged.RemoveListener(SynchronizeScrollbarValue);
				m_Scrollbar = value;
				if (m_Scrollbar)
					m_Scrollbar.onValueChanged.AddListener(SynchronizeScrollbarValue);
				Refresh();
			} 
		}

		[SerializeField]
		private ScrollRect m_ScrollRect;
		public ScrollRect scrollRect 
		{ 
			get { return m_ScrollRect; } 
			set 
			{ 
				if (m_ScrollRect)
					m_ScrollRect.onValueChanged.RemoveListener(SynchronizeScrollRectValue);
				m_ScrollRect = value; 
				if (m_ScrollRect)
					m_ScrollRect.onValueChanged.AddListener(SynchronizeScrollRectValue);
				Refresh(); 
			} 
		}
		
		[Space]
		
		// Event delegates triggered on value changed.
		[SerializeField]
		private SpinnerListEvent m_OnValueChanged = new SpinnerListEvent();
		public SpinnerListEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }
		
		[HideInInspector] public Image imageArrowBackward { get { return m_ArrowBackward as Image; } set { m_ArrowBackward = value; } }
		[HideInInspector] public Image imageArrowForward { get { return m_ArrowForward as Image; } set { m_ArrowForward = value; } }
		
		private RectTransform m_ArrowBackwardRect;
		private RectTransform m_ArrowForwardRect; 

		private SelectionState m_CurrentArrowStateBackward;
		protected SelectionState currentArrowStateBackward { get { return m_CurrentArrowStateBackward; } }
		private SelectionState m_CurrentArrowStateForward;
		protected SelectionState currentArrowStateForward { get { return m_CurrentArrowStateForward; } }

		private DrivenRectTransformTracker m_Tracker;
		
		protected Spinner () {}
		
		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif
			base.Awake ();

			m_ArrowBackwardRect = m_ArrowBackward.GetComponent<RectTransform>();
			m_ArrowForwardRect = m_ArrowForward.GetComponent<RectTransform>();
		}
		
		protected override void Start () 
		{
			base.Start();
			
			this.value = m_Value;
		}
		
		protected override void OnEnable ()
		{
			base.OnEnable();

			if (m_Type == SpinnerType.Scrollbar && m_Scrollbar)
				m_Scrollbar.onValueChanged.AddListener(SynchronizeScrollbarValue);
			if (m_Type == SpinnerType.ScrollRect && m_ScrollRect)
				m_ScrollRect.onValueChanged.AddListener(SynchronizeScrollRectValue);

			SelectionState transitionState = base.currentSelectionState;
			
			if (base.IsActive() && !base.IsInteractable()) 
				transitionState = SelectionState.Disabled;
			
			DoStateTransition(transitionState, true);
//			UpdateVisuals();
		}
		
		protected override void OnDisable ()
		{
			if (m_Type == SpinnerType.Scrollbar && m_Scrollbar)
				m_Scrollbar.onValueChanged.RemoveListener(SynchronizeScrollbarValue);
			if (m_Type == SpinnerType.ScrollRect && m_ScrollRect)
				m_ScrollRect.onValueChanged.RemoveListener(SynchronizeScrollRectValue);

			this.InstantClearState();
			m_Tracker.Clear();
			base.OnDisable();
		}
		
		protected override void InstantClearState ()
		{
			switch (base.transition)
			{
			case Transition.ColorTint:
				StartColorTween(m_ArrowBackward, Color.white, true);
				StartColorTween(m_ArrowForward, Color.white, true);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(imageArrowBackward, null);
				DoSpriteSwap(imageArrowForward, null);
				break;
			case Transition.Animation:
				TriggerAnimation(m_ArrowBackwardAnimationTriggers.normalTrigger, 1);
				TriggerAnimation(m_ArrowForwardAnimationTriggers.normalTrigger, 2);
				break;
			}
			
			base.InstantClearState();
		}
		
		#if UNITY_EDITOR
		protected override void OnValidate ()
		{
			if (base.IsActive() || !(animator == null || !animator.isInitialized))
				base.OnValidate();

			if (base.IsActive())
			{
//				UpdateVisuals();
				Refresh();
			}

			var prefabType = UnityEditor.PrefabUtility.GetPrefabType(this);
			if (prefabType != UnityEditor.PrefabType.Prefab && !Application.isPlaying)
				CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
		}
		#endif

		public virtual void Rebuild (CanvasUpdate executing)
		{
			#if UNITY_EDITOR
			if (executing == CanvasUpdate.Prelayout)
				onValueChanged.Invoke(value);
			#endif
		}

		public virtual void LayoutComplete () {}
		public virtual void GraphicUpdateComplete () {}

		private void SynchronizeScrollbarValue (float value) { this.value = value; }
		private void SynchronizeScrollRectValue (Vector2 value) { /*this.value = value[(int)m_Orientation];*/ }

		private string _OnFixedFormat (float value) { return value.ToString(); }
		private OptionData _OnFlexibleFormat (int value, List<OptionData> options) { return options[Mathf.Clamp(value, 0 , options.Count - 1)]; }

		void Refresh ()
		{
			if ((type == SpinnerType.List) ? options.Count == 0 : false)
				return;
			
			switch (type)
			{
			case SpinnerType.Range:
				if (m_Label)
					m_Label.GetComponent<Text>().text = (OnFixedFormat != null) ? OnFixedFormat(m_Value) : _OnFixedFormat(m_Value);
				if (m_Image as Image)
					(m_Image as Image).enabled = false;
				break;
			case SpinnerType.List:
				OptionData data = (OnFlexibleFormat != null) 
					? OnFlexibleFormat((int)m_Value, options) 
					: _OnFlexibleFormat((int)m_Value, options);

				if (m_Label && data != null)
					m_Label.GetComponent<Text>().text = data.text;
				
				if (m_Image as Image && data != null)
				{
					(m_Image as Image).sprite = data.image;
					(m_Image as Image).enabled = (data.image != null);
				}
				break;
			case SpinnerType.Scrollbar: 
				if (m_Label)
					m_Label.GetComponent<Text>().text = (OnFixedFormat != null) ? OnFixedFormat(m_Value) : _OnFixedFormat(m_Value);
				if (m_Image as Image)
					(m_Image as Image).enabled = false;
				if (m_Scrollbar && m_Scrollbar.value != m_Value)
					m_Scrollbar.value = m_Value;
				break;
			case SpinnerType.ScrollRect: 
				if (m_Label)
					m_Label.GetComponent<Text>().text = (OnFixedFormat != null) ? OnFixedFormat(m_Value) : _OnFixedFormat(m_Value);
				if (m_Image as Image)
					(m_Image as Image).enabled = false;
				if (m_ScrollRect)
				{
					Vector2 position = m_ScrollRect.normalizedPosition;
					position[(int)m_Orientation] = m_Value;
					m_ScrollRect.normalizedPosition = position;
				}
				break;
			}
		}

//		public void UpdateVisuals ()
//		{
//			
//		}

		public void SetOrientation (Orientation orientation, bool includeRectLayouts)
		{
			Orientation oldOrientation = m_Orientation;
			this.orientation = orientation;

			if (!includeRectLayouts)
				return;

			if (this.orientation != oldOrientation)
				RectTransformUtility.FlipLayoutAxes(transform as RectTransform, true, true);
		}

		public bool isArrowBackwardOnBound 
		{
			get 
			{
				switch (type)
				{
				default: case SpinnerType.Range: return (range.min != range.max && m_Value == range.min);
				case SpinnerType.List: return (m_Value == 0);
				case SpinnerType.Scrollbar: return (m_Value == 0);
				case SpinnerType.ScrollRect: return (m_Value == 0);
				}
			}
		}

		public bool isArrowForwardOnBound
		{
			get
			{
				switch (type)
				{
				default: case SpinnerType.Range: return (range.min != range.max && m_Value == range.max);
				case SpinnerType.List: return (m_Value == options.Count - 1);
				case SpinnerType.Scrollbar: return (m_Value == 1);
				case SpinnerType.ScrollRect: return (m_Value == 1);
				}
			}
		}

		public bool isArrowBackwardWithinBound
		{
			get
			{
				switch (type)
				{
				default: case SpinnerType.Range: return (range.min != range.max && m_Value > range.min);
				case SpinnerType.List: return (m_Value > 0);
				case SpinnerType.Scrollbar: return (m_Value > 0);
				case SpinnerType.ScrollRect: return (m_Value > 0);
				}
			}
		}

		public bool isArrowForwardWithinBound
		{
			get
			{
				switch (type)
				{
				default: case SpinnerType.Range: return (range.min != range.max && m_Value < range.max);
				case SpinnerType.List: return (m_Value < options.Count - 1);
				case SpinnerType.Scrollbar: return (m_Value < 1);
				case SpinnerType.ScrollRect: return (m_Value < 1);
				}
			}
		}

		protected override void DoStateTransition (SelectionState state, bool instant)
		{
			if (gameObject.activeInHierarchy)
			{
				if (!m_Loop 
					&& isArrowBackwardOnBound /*((type == SpinnerType.Range) 
						? range.min != range.max && m_Value == range.min 
						: m_Value == 0) */
					&& arrowBackward)
					DoStateTransitionArrow(m_ArrowBackward, SelectionState.Disabled, false);
				else
					DoStateTransitionArrow(m_ArrowBackward, state, false);

				if (!m_Loop 
					&& isArrowForwardOnBound /*((type == SpinnerType.Range) 
						? range.min != range.max && m_Value == range.max 
						: m_Value == options.Count - 1)*/ 
					&& arrowForward)
					DoStateTransitionArrow(m_ArrowForward, SelectionState.Disabled, false);
				else
					DoStateTransitionArrow(m_ArrowForward, state, false);
			}
			
			base.DoStateTransition(state, instant);
		}
		
		protected void DoStateTransitionArrow (Graphic target, SelectionState state, bool instant)
		{
			if (target == m_ArrowBackward)
				m_CurrentArrowStateBackward = state;
			else if (target == m_ArrowForward)
				m_CurrentArrowStateForward = state;
			
			Color tintColor;
			Sprite transitionSprite;
			string triggername;
			
			switch (state)
			{
			case SelectionState.Normal:
				tintColor = base.colors.normalColor;
				transitionSprite = null;
				triggername = (target == m_ArrowBackward) ? m_ArrowBackwardAnimationTriggers.normalTrigger : m_ArrowForwardAnimationTriggers.normalTrigger;
				break;
			case SelectionState.Highlighted:
				tintColor = base.colors.highlightedColor;
				transitionSprite = (target == m_ArrowBackward) ? m_ArrowBackwardSpriteState.highlightedSprite : m_ArrowForwardSpriteState.highlightedSprite;
				triggername = (target == m_ArrowBackward) ? m_ArrowBackwardAnimationTriggers.highlightedTrigger : m_ArrowForwardAnimationTriggers.highlightedTrigger;
				break;
			case SelectionState.Pressed:
				tintColor = base.colors.pressedColor;
				transitionSprite = (target == m_ArrowBackward) ? m_ArrowBackwardSpriteState.pressedSprite : m_ArrowForwardSpriteState.pressedSprite;
				triggername = (target == m_ArrowBackward) ? m_ArrowBackwardAnimationTriggers.pressedTrigger : m_ArrowForwardAnimationTriggers.pressedTrigger;
				break;
			case SelectionState.Disabled:
				tintColor = base.colors.disabledColor;
				transitionSprite = (target == m_ArrowBackward) ? m_ArrowBackwardSpriteState.disabledSprite : m_ArrowForwardSpriteState.disabledSprite;
				triggername = (target == m_ArrowBackward) ? m_ArrowBackwardAnimationTriggers.disabledTrigger : m_ArrowForwardAnimationTriggers.disabledTrigger;
				break;
			default:
				tintColor = Color.black;
				transitionSprite = null;
				triggername = string.Empty;
				break;
			}
			
			if (gameObject.activeInHierarchy)
			{
				switch (base.transition)
				{
				case Transition.ColorTint:
					StartColorTween(target, tintColor * base.colors.colorMultiplier, instant);
					break;
				case Transition.SpriteSwap:
					DoSpriteSwap((target == m_ArrowBackward) ? imageArrowBackward : imageArrowForward, transitionSprite);
					break;
				case Transition.Animation:
					TriggerAnimation(triggername, (target == m_ArrowBackward) ? 1 : 2);
					break;
				}
			}
		}

		private IEnumerator OnFinishPressed (Graphic target, Graphic other)
		{
			float fadeTime = 0.1f;
			float elapsedTime = 0f;
			
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}

			SelectionState state = base.currentSelectionState;
			
			if (!m_Loop 
				&&  (isArrowBackwardOnBound /*((type == SpinnerType.Range) 
					? range.min != range.max && m_Value == range.min 
					: m_Value == 0)*/ 
				|| isArrowForwardOnBound /*((type == SpinnerType.Range) 
						? range.min != range.max && m_Value == range.max 
						: m_Value == options.Count - 1)*/) 
				&& target)
			{
				state = SelectionState.Disabled;
			}
			
			if (!m_Loop 
				&& (isArrowBackwardWithinBound /*((type == SpinnerType.Range) 
					? range.min != range.max && m_Value > range.min 
					: m_Value > 0)*/ 
				|| isArrowForwardWithinBound /*((type == SpinnerType.Range) 
						? range.min != range.max && m_Value < range.max 
						: m_Value < options.Count - 1)*/) 
				&& other)
			{
				DoStateTransitionArrow(other, base.currentSelectionState, false);
			}
			
			DoStateTransitionArrow(target, state, false);
		}
		
		public void StartColorTween (Graphic target, Color targetColor, bool instant)
		{
			if (target == null)
				return;
			
			target.CrossFadeColor(targetColor, instant ? 0f : 0.1f, true, true);
		}
		
		public void DoSpriteSwap (Image target, Sprite newSprite)
		{
			if (target == null)
				return;
			
			target.overrideSprite = newSprite;
		}
		
		public void TriggerAnimation (string triggername, int layer)
		{
			if (animator == null 
				|| !animator.isInitialized 
				|| !animator.isActiveAndEnabled 
				|| animator.runtimeAnimatorController == null 
				|| string.IsNullOrEmpty(triggername))
				return;
			
			if (layer == 1)
			{
				animator.ResetTrigger(arrowBackwardAnimationTriggers.normalTrigger);
				animator.ResetTrigger(arrowBackwardAnimationTriggers.highlightedTrigger);
				animator.ResetTrigger(arrowBackwardAnimationTriggers.pressedTrigger);
				animator.ResetTrigger(arrowBackwardAnimationTriggers.disabledTrigger);
			}
			else if (layer == 2)
			{
				animator.ResetTrigger(arrowForwardAnimationTriggers.normalTrigger);
				animator.ResetTrigger(arrowForwardAnimationTriggers.highlightedTrigger);
				animator.ResetTrigger(arrowForwardAnimationTriggers.pressedTrigger);
				animator.ResetTrigger(arrowForwardAnimationTriggers.disabledTrigger);
			}
			animator.SetTrigger(triggername);
		}
		
		public override void OnSelect (BaseEventData eventData)
		{
			if (!base.IsActive() || !base.IsInteractable())
				return;
			
			base.OnSelect(eventData);
		}
		
		public override void OnDeselect (BaseEventData eventData)
		{
			if (!base.IsActive() || !base.IsInteractable())
				return;
			
			base.OnDeselect(eventData);
		}

		private float stepSizeScrollbar { get { return (m_Scrollbar.numberOfSteps > 1) ? 1f / (m_Scrollbar.numberOfSteps - 1) : 0.1f; } }

		private float AdjustValue (float value)
		{
			float baseValue = range.min;
			float aboveMin = value - baseValue;

			aboveMin = Mathf.Round(aboveMin / range.step) * range.step;
			value = baseValue + aboveMin;

			value = (float)System.Convert.ToDouble(value.ToString("N" + Precision(range.step).ToString())); 

			return (range.min == range.max) ? value : Mathf.Clamp(value, range.min, range.max);
		}

		private float Precision (float num)
		{
			string str = num.ToString();
			int decimalPoint = str.IndexOf(".");

			return decimalPoint == -1 ? 0 : str.Length - decimalPoint - 1;
		}

		private void _Backward ()
		{
			switch (type)
			{
			case SpinnerType.Range:
				if (value <= range.min && m_Loop)
					value = range.max;
				else
					value += -1 * range.step;
				break;
			case SpinnerType.List:
				if ((int)--this.value <= -1 && m_Loop)
					this.value = options.Count - 1;
				break;
			case SpinnerType.Scrollbar: 
				if (value <= 0 && m_Loop)
					value = 1;
				else
					value = m_Scrollbar.value - stepSizeScrollbar;
				break;
			case SpinnerType.ScrollRect: 
//				if (value <= 0 && m_Loop)
//					StartCoroutine(Smooth(1f));//target = 1;
//				else
//					StartCoroutine(Smooth(m_Value - 0.1f));//target = m_Value - 0.1f;
				break;
			}
		}
		
		public void Backward ()
		{
			if (!base.IsActive() || !base.IsInteractable())
				return;

			if (OnBackward != null)
				OnBackward();
			else
				_Backward();

			if (m_CurrentArrowStateBackward != SelectionState.Disabled)
			{
				DoStateTransitionArrow(m_ArrowBackward, SelectionState.Pressed, false);
				StartCoroutine(OnFinishPressed(m_ArrowBackward, m_ArrowForward));
			}
		}

		private void _Forward ()
		{
			switch (type)
			{
			case SpinnerType.Range:
				if (value >= range.max && m_Loop)
					value = range.min;
				else
					value += 1 * range.step;
				break;
			case SpinnerType.List:
				if ((int)++this.value >= options.Count && m_Loop)
					this.value = 0;
				break;
			case SpinnerType.Scrollbar: 
				if (value >= 1 && m_Loop)
					value = 0;
				else
					value = m_Scrollbar.value + stepSizeScrollbar;
				break;
			case SpinnerType.ScrollRect: 
//				if (value >= 1 && m_Loop)
//					StartCoroutine(Smooth(0f));//target = 0;
//				else
//					StartCoroutine(Smooth(m_Value + 0.1f));//target = m_Value + 0.1f;
				break;
			}
		}

		public void Forward ()
		{
			if (!base.IsActive() || !base.IsInteractable())
				return;
			
			if (OnForward != null)
				OnForward();
			else
				_Forward();

			if (m_CurrentArrowStateForward != SelectionState.Disabled)
			{
				DoStateTransitionArrow(m_ArrowForward, SelectionState.Pressed, false);
				StartCoroutine(OnFinishPressed(m_ArrowForward, m_ArrowBackward));
			}
		}

//		private float target;

		private IEnumerator Smooth (float target)
		{
			if (m_Type == SpinnerType.ScrollRect && m_ScrollRect)
			{
				while (Mathf.Round(value) != target)
				{
					value = Mathf.Lerp(m_Value, target, Time.deltaTime * 5);
					yield return new WaitForEndOfFrame();
				}
			}
		}
		
		public override void OnPointerDown (PointerEventData eventData)
		{
			if (!(base.IsActive() && base.IsInteractable() && eventData.button == PointerEventData.InputButton.Left))
				return;

			if (IsInteractable() && navigation.mode != Navigation.Mode.None)
				EventSystem.current.SetSelectedGameObject(gameObject, eventData);

			if (m_ArrowBackwardRect != null && 
			    RectTransformUtility.RectangleContainsScreenPoint(m_ArrowBackwardRect, eventData.position, eventData.enterEventCamera))
			{
				Backward();
			}
			
			if (m_ArrowForwardRect != null && 
			    RectTransformUtility.RectangleContainsScreenPoint(m_ArrowForwardRect, eventData.position, eventData.enterEventCamera))
			{
				Forward();
			}
		}
		
		public override void OnMove (AxisEventData eventData)
		{
			if (!base.IsActive() || !base.IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			
			switch (eventData.moveDir)
			{
			case MoveDirection.Right: 
				switch (orientation)
				{
				case Orientation.Horizontal: Forward(); break;
				case Orientation.Vertical: base.OnMove(eventData); break;
				}
				break;
			case MoveDirection.Up: 
				switch (orientation)
				{
				case Orientation.Horizontal: base.OnMove(eventData); break;
				case Orientation.Vertical: Forward(); break;
				}
				break;
			case MoveDirection.Left: 
				switch (orientation)
				{
				case Orientation.Horizontal: Backward(); break;
				case Orientation.Vertical: base.OnMove(eventData); break;
				}
				break;
			case MoveDirection.Down: 
				switch (orientation)
				{
				case Orientation.Horizontal: base.OnMove(eventData); break;
				case Orientation.Vertical: Backward(); break;
				}
				break;
			}
		}
		
		public override Selectable FindSelectableOnLeft ()
		{
			if (orientation == Orientation.Horizontal && navigation.mode == Navigation.Mode.Automatic)
				return null;
			return base.FindSelectableOnLeft ();
		}
		
		public override Selectable FindSelectableOnRight ()
		{
			if (orientation == Orientation.Horizontal && navigation.mode == Navigation.Mode.Automatic)
				return null;
			return base.FindSelectableOnRight ();
		}
		
		public override Selectable FindSelectableOnUp ()
		{
			if (orientation == Orientation.Vertical && navigation.mode == Navigation.Mode.Automatic)
				return null;
			return base.FindSelectableOnUp ();
		}
		
		public override Selectable FindSelectableOnDown ()
		{
			if (orientation == Orientation.Vertical && navigation.mode == Navigation.Mode.Automatic)
				return null;
			return base.FindSelectableOnDown ();
		}
	}
}
