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
using UIBundle.CoroutineTween;

namespace UIBundle
{
	[AddComponentMenu("UI/UIBundle/Tooltip Display")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class TooltipDisplay : UIBehaviour 
	{
		[SerializeField]
		private Graphic m_TargetGraphic;
		public Graphic targetGraphic { get { return m_TargetGraphic; } set { m_TargetGraphic = value; } }
		public RectTransform targetRect { get { return m_TargetGraphic.transform as RectTransform; } }

		[SerializeField]
		private Graphic m_Label;
		public Graphic label { get { return m_Label; } set { m_Label = value; } }
		public Text labelText { get { return m_Label as Text; } }

		[Space]

		[SerializeField]
		private bool m_AnimFade;
		public bool animFade { get { return m_AnimFade; } set { m_AnimFade = value; } }

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		protected TooltipDisplay () {}

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			m_AlphaTweenRunner.Init(this);
		}

		public void ShowDescription (string description)
		{
			if (!IsActive()) 
				return;
			labelText.text = description;

			if (m_AnimFade)
				AlphaFadeList(0.15f, 0f, 1f);
		}

		private static T GetOrAddComponent<T> (GameObject go) where T : Component
		{
			T comp = go.GetComponent<T>();
			if (!comp)
				comp = go.AddComponent<T>();
			return comp;
		}

		private void AlphaFadeList (float duration, float alpha)
		{
			Text label = labelText;
			AlphaFadeList(duration, label.color.a, alpha);
		}

		private void AlphaFadeList (float duration, float start, float end)
		{
			if (!m_Label)
				return;

			FloatTween tween = new FloatTween 
			{ 
				duration = duration,
				startValue = start,
				targetValue = end
			};
			tween.AddOnChangedCallback(SetAlpha);
			tween.ignoreTimeScale = true;
			m_AlphaTweenRunner.StartTween(tween);
		}

		private void SetAlpha (float alpha)
		{
			if (!m_Label)
				return;

			Text label = labelText;
			Color color = label.color;

			color.a = alpha;
			label.color = color;
		}
	}
}
