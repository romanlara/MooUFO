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
	[AddComponentMenu("UI/UIBundle/Tooltip")]
	[ExecuteInEditMode]
	public class Tooltip : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[System.Serializable]
		public class TooltipDetails
		{
			[SerializeField]
			private string m_Title;
			public string title { get { return m_Title; } set { m_Title = value; } }

			[SerializeField]
			[TextArea(3, 10)]
			private string m_Message;
			public string message { get { return m_Message; } set { m_Message = value; } }

			[SerializeField]
			private Sprite m_Icon;
			public Sprite icon { get { return m_Icon; } set { m_Icon = value; } }

			[SerializeField]
			private Sprite m_Background;
			public Sprite background { get { return m_Background; } set { m_Background = value; } }

			public TooltipDetails () {}
			public TooltipDetails (string message) { this.message = message; }
			public TooltipDetails (string title, string message) : this(message) { this.title = title; }
			public TooltipDetails (string message, Sprite icon) : this(message) { this.icon = icon; } 
			public TooltipDetails (string message, Sprite icon, Sprite background) : this(message, icon) { this.background = background; }
			public TooltipDetails (string title, string message, Sprite icon, Sprite background) : this(message, icon, background) { this.title = title; }
		}

		[SerializeField]
		private RectTransform m_Template;
		public RectTransform template { get { return m_Template; } set { m_Template = value; } }

		[Space]

		[SerializeField]
		private Graphic m_Title;
		public Graphic title { get { return m_Title; } set { m_Title = value; } }
		public Text textTitle { get { return m_Title as Text; } }

		[SerializeField]
		private Graphic m_Message;
		public Graphic message { get { return m_Message; } set { m_Message = value; } }
		public Text textMessage { get { return m_Message as Text; } }

		[SerializeField]
		private Graphic m_Icon;
		public Graphic icon { get { return m_Icon; } set { m_Icon = value; } }
		public Image imageIcon { get { return m_Icon as Image; } }

		[SerializeField]
		private Graphic m_Background;
		public Graphic background { get { return m_Background; } set { m_Background = value; } }
		public Image imageBackground { get { return m_Background as Image; } }

		[Space]

		[SerializeField]
		private TooltipDetails m_Details = new TooltipDetails();
		public TooltipDetails details { get { return m_Details; } set { m_Details = value; } }

		private GameObject m_Tooltip;
		private TweenRunner<FloatTween> m_AlphaTweenRunner;
		private bool validTemplate = false;

		protected Tooltip () {}

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			m_AlphaTweenRunner.Init(this);

			// By awaking Loading Panel is deactivates.
			if (m_Template != null)
				m_Template.gameObject.SetActive(false);
		}

		#if UNITY_EDITOR
		protected override void OnValidate ()
		{
			base.OnValidate();

			if (!base.isActiveAndEnabled)
				return;

			Refresh();
		}
		#endif

		private void Preset ()
		{
			if (details == null) return;

			// It is deactivates the graphics.
			m_Icon.gameObject.SetActive(false);
			m_Title.gameObject.SetActive(false);
			m_Message.gameObject.SetActive(false);
		}

		public void Refresh ()
		{
			if (details == null) return;

			// Set the background.
			if (details.background)
			{
				if (m_Background)
					imageBackground.sprite = details.background;
			}
			// Set the icon.
			if (details.icon)
			{
				if (m_Icon)
				{
					imageIcon.sprite = details.icon;
					m_Icon.gameObject.SetActive(true);
				}
			}
			// Set the title.
			if (!string.IsNullOrEmpty(details.title))
			{
				if (m_Title)
				{
					textTitle.text = details.title;
					m_Title.gameObject.SetActive(true);
				}
			}
			// Set the message.
			if (!string.IsNullOrEmpty(details.message))
			{
				if (m_Message)
				{
					textMessage.text = details.message;
					m_Message.gameObject.SetActive(true);
				}
			}
		}

		private void SetupTemplate ()
		{
			validTemplate = false;

			if (!m_Template)
			{
				Debug.LogError("The tooltip template is not assigned. The template needs to be assigned and must have a child GameObject with a Text Component serving as the message.");
				return;
			}

			GameObject templateGo = m_Template.gameObject;
			templateGo.SetActive(true);
			Text messageText = m_Template.GetComponentInChildren<Text>();

			validTemplate = true;
			if (!messageText || messageText.transform == template)
			{
				validTemplate = false;
				Debug.LogError("The tooltip template is not valid. The template must have a child GameObject with a Text component serving as the message.");
			}
			else if (!(messageText.transform.parent is RectTransform))
			{
				validTemplate = false;
				Debug.LogError("The tooltip template is not valid. The child GameObject with a Text component (the message) must have a RectTransform on its parent.");
			}

			if (!validTemplate)
			{
				templateGo.SetActive(false);
				return;
			}

			Canvas popupCanvas = GetOrAddComponent<Canvas>(templateGo);
			popupCanvas.overrideSorting = true;
			popupCanvas.sortingOrder = 30000;

			GetOrAddComponent<GraphicRaycaster>(templateGo);
			GetOrAddComponent<CanvasGroup>(templateGo);
			templateGo.SetActive(false);

			validTemplate = true;
		}

		private static T GetOrAddComponent<T> (GameObject go) where T : Component
		{
			T comp = go.GetComponent<T>();
			if (!comp)
				comp = go.AddComponent<T>();
			return comp;
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			Show();
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			Hide();
		}

		private void Show ()
		{
			if (!base.IsActive() || m_Tooltip != null || details == null) 
				return;

			if (!validTemplate)
			{
				SetupTemplate();
				if (!validTemplate)
					return;
			}

			Preset();
			Refresh();

			// Get root Canvas.
			var list = ListPool<Canvas>.Get();
			gameObject.GetComponentsInParent(false, list);
			if (list.Count == 0)
				return;
			Canvas rootCanvas = list[0];
			ListPool<Canvas>.Release(list);

			// Instatiate the tooltip template.
			m_Tooltip = CreateTooltip(m_Template.gameObject);
			m_Tooltip.name = "Tooltip";
			m_Tooltip.SetActive(true);

			// Make tooltip RectTransform have same values os original.
			RectTransform tooltipRectTransform = m_Tooltip.transform as RectTransform;
			tooltipRectTransform.SetParent(m_Template.transform.parent, false);

			// Invert anchoring and position if tooltip is partially or fully outside of canvas rect.
			Vector3[] corners = new Vector3[4];
			m_Template.GetWorldCorners(corners);
			bool[] outsides = new bool[4];
			RectTransform rootCanvasRectTransform = rootCanvas.transform as RectTransform;
			for (int i = 0; i < 4; i++)
			{
				outsides[i] = false;
				Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
				if (!rootCanvasRectTransform.rect.Contains(corner))
				{
					outsides[i] = true;
				}
			}
			if ((outsides[0] && outsides[1]) || (outsides[2] && outsides[3]))
			{
				RectTransformUtility.FlipLayoutOnAxis(tooltipRectTransform, 0, false, false);
			}
			else if ((outsides[1] && outsides[2]) || (outsides[0] && outsides[3]))
			{
				RectTransformUtility.FlipLayoutOnAxis(tooltipRectTransform, 1, false, false);
			}

			// Fade in the popup.
			AlphaFadeList(0.15f, 0f, 1f);

			// Make tooltip template inactive.
			m_Template.gameObject.SetActive(false);
		}

		protected virtual GameObject CreateTooltip (GameObject template)
		{
			return (GameObject) Instantiate(template);
		}

		protected virtual void DestroyTooltip (GameObject tooltip)
		{
			Destroy(tooltip);
		}

		private void AlphaFadeList (float duration, float alpha)
		{
			CanvasGroup group = m_Tooltip.GetComponent<CanvasGroup>();
			AlphaFadeList(duration, group.alpha, alpha);
		}

		private void AlphaFadeList (float duration, float start, float end)
		{
			if (!m_Tooltip)
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
			if (!m_Tooltip)
				return;

			CanvasGroup group = m_Tooltip.GetComponent<CanvasGroup>();
			group.alpha = alpha;
		}

		private void Hide ()
		{
			if (m_Tooltip != null)
			{
				AlphaFadeList(0.15f, 0f);
				StartCoroutine(DelayedDestroyTooltip(0.15f));
			}
		}

		private IEnumerator DelayedDestroyTooltip (float delay)
		{
			yield return new WaitForSeconds(delay);
			if (m_Tooltip != null)
				DestroyTooltip(m_Tooltip);
			m_Tooltip = null;
		}
	}
}
