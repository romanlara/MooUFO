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
	[AddComponentMenu("UI/UIBundle/Progressbar")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class Progressbar : UIBehaviour, ICanvasElement
	{
		[System.Serializable]
		public class ProgressbarListEvent : UnityEvent<float> {}

		public delegate string ValueFormat (float value);
		public ValueFormat OnValueFormat;

		public enum Direction
		{
			LeftToRight,
			RightToLeft,
			BottomToTop,
			TopToBottom
		}

		[SerializeField]
		private Graphic m_TargetGraphic;
		public Graphic targetGraphic { get { return m_TargetGraphic; } set { m_TargetGraphic = value; } }

		[Space]

		[SerializeField]
		private RectTransform m_FillingRect;
		public RectTransform fillingRect { get { return m_FillingRect; } set { m_FillingRect = value; UpdateVisuals(); } }

		[SerializeField]
		private Direction m_Direction = Direction.LeftToRight;
		public Direction direction { get { return m_Direction; } set { m_Direction = value; UpdateVisuals(); } }

		[Space]

		[SerializeField]
		private RectTransform m_IndicatorRect;
		public RectTransform indicatorRect { get { return m_IndicatorRect; } set { m_IndicatorRect = value; UpdateVisuals(); } }

		[SerializeField]
		private Graphic m_Label;
		public Graphic label { get { return m_Label; } set { m_Label = value; Refresh(); } }

		[Space]

		[Range(0f, 1f)]
		[SerializeField]
		private float m_Value = 1f;
		public float value 
		{ 
			get { return m_Value; } 
			set 
			{ 
				m_Value = Mathf.Clamp01(value); 
				Refresh();
				UpdateVisuals(); 
				m_OnValueChanged.Invoke(m_Value); 
			} 
		}

		[Space]

		// Event delegates triggered on value changed.
		[SerializeField]
		private ProgressbarListEvent m_OnValueChanged = new ProgressbarListEvent();
		public ProgressbarListEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

		public RectTransform targetRect { get { return m_TargetGraphic.gameObject.GetComponent<RectTransform>(); } }
		public Text labelText { get { return m_Label as Text; } }

		private DrivenRectTransformTracker m_Tracker;

		protected Progressbar () {}

		protected override void OnEnable ()
		{
			base.OnEnable();
			UpdateVisuals();
		}

		protected override void OnDisable ()
		{
			m_Tracker.Clear();
			base.OnDisable();
		}

		#if UNITY_EDITOR
		protected override void OnValidate ()
		{
			base.OnValidate();

			if (!base.isActiveAndEnabled)
				return;

			value = value;

			Refresh();
			UpdateVisuals();

			var prefabType = UnityEditor.PrefabUtility.GetPrefabType(this);
			if (prefabType != UnityEditor.PrefabType.Prefab && !Application.isPlaying)
				CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
		}

		protected override void Reset ()
		{
			m_TargetGraphic = GetComponent<Graphic>();
			base.Reset();
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

		protected override void OnRectTransformDimensionsChange ()
		{
			base.OnRectTransformDimensionsChange();
			UpdateVisuals();
		}

		private string _OnValueFormat (float value) { return (value * 100) + "%"; }
		void Refresh() { if (m_Label) labelText.text = (OnValueFormat != null) ? OnValueFormat(m_Value) : _OnValueFormat(m_Value); }

		public enum Axis
		{
			Horizontal = 0,
			Vertical = 1
		}

		Axis axis { get { return (m_Direction == Direction.LeftToRight || m_Direction == Direction.RightToLeft) ? Axis.Horizontal : Axis.Vertical; } }
		bool reverseValue { get { return (m_Direction == Direction.RightToLeft || m_Direction == Direction.TopToBottom); } }

		public void UpdateVisuals ()
		{
			m_Tracker.Clear();

			if (m_FillingRect != null)
			{
				m_Tracker.Add(this, m_FillingRect, DrivenTransformProperties.Anchors);
				Vector2 anchorMin = Vector2.zero;
				Vector2 anchorMax = Vector2.one;

				if (reverseValue)
				{
					anchorMin[(int)axis] = 1 - value;
					anchorMax[(int)axis] = 1;
				}
				else
				{
					anchorMin[(int)axis] = 0;
					anchorMax[(int)axis] = value;
				}

				m_FillingRect.anchorMin = anchorMin;
				m_FillingRect.anchorMax = anchorMax;

				if (m_IndicatorRect != null)
				{
					m_Tracker.Add(this, m_IndicatorRect, DrivenTransformProperties.Anchors);
					anchorMin = new Vector2(.5f, .5f);
					anchorMax = new Vector2(.5f, .5f);

					if (reverseValue)
					{
						anchorMin[(int)axis] = 1 - value;
						anchorMax[(int)axis] = 1 - value;
					}
					else
					{
						anchorMin[(int)axis] = value;
						anchorMax[(int)axis] = value;
					}

					m_IndicatorRect.anchorMin = anchorMin;
					m_IndicatorRect.anchorMax = anchorMax;
				}
			}
		}

		public void SetDirection (Direction direction, bool includeRectLayouts)
		{
			Axis oldAxis = axis;
			bool oldReverse = reverseValue;
			this.direction = direction;

			if (!includeRectLayouts)
				return;

			if (axis != oldAxis)
				RectTransformUtility.FlipLayoutAxes(transform as RectTransform, true, true);

			if (reverseValue != oldReverse)
				RectTransformUtility.FlipLayoutOnAxis(transform as RectTransform, (int)axis, true, true);
		}
	}
}
