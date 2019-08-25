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
	[AddComponentMenu("UI/UIBundle/Dialog")]
	[ExecuteInEditMode]
	public class Dialog : UIBehaviour 
	{
		[System.Serializable]
		public class EventButtonDetails
		{
			[SerializeField]
			private bool m_SetAsSelectedGameObject;
			public bool setAsSelectedGameObject { get { return m_SetAsSelectedGameObject; } set { m_SetAsSelectedGameObject = value; } }

			[SerializeField]
			private string m_Title;
			public string title { get { return m_Title; } set { m_Title = value; } }

			[SerializeField]
			private ColorBlock m_Colors;
			public ColorBlock colors { get { return m_Colors; } set { m_Colors = value; } }

			[SerializeField]
			private Button.ButtonClickedEvent m_Action;
			public Button.ButtonClickedEvent action { get { return m_Action; } set { m_Action = value; } }

			public EventButtonDetails () {}

			public EventButtonDetails (string title, Button.ButtonClickedEvent action)
			{
				this.title = title;
				this.action = action;
			}

			public EventButtonDetails (string title, Button.ButtonClickedEvent action, bool setAsSelectedGameObject) : this(title, action)
			{
				this.setAsSelectedGameObject = setAsSelectedGameObject;
			}
		}

		[System.Serializable]
		public class DialogDetails
		{
			[SerializeField]
			private string m_Title;
			public string title { get { return m_Title; } set { m_Title = value; } }

			[SerializeField]
			[TextArea(3, 10)]
			private string m_Question;
			public string question { get { return m_Question; } set { m_Question = value; } }

			[SerializeField]
			private Sprite m_Icon;
			public Sprite icon { get { return m_Icon; } set { m_Icon = value; } }

			[SerializeField]
			private Sprite m_Background;
			public Sprite background { get { return m_Background; } set { m_Background = value; } }

			[SerializeField]
			private List<EventButtonDetails> m_ButtonDetails;
			public List<EventButtonDetails> buttonDetails { get { return m_ButtonDetails; } set { m_ButtonDetails = value; } }

			public DialogDetails () { buttonDetails = new List<EventButtonDetails>(); }
			public DialogDetails (string question) : this() { this.question = question; }
			public DialogDetails (string title, string question) : this(question) { this.title = title; }
			public DialogDetails (string question, Sprite icon) : this(question) { this.icon = icon; }
			public DialogDetails (string title, string question, Sprite icon) : this(title, question) { this.icon = icon; }
			public DialogDetails (string title, string question, Sprite icon, Sprite background) : this(title, question, icon) { this.background = background; }
		}

		[SerializeField]
		private GameObject m_ButtonTemplate;
		public GameObject buttonTemplate { get { return m_ButtonTemplate; } set { m_ButtonTemplate = value; } }

		[Space]

		[SerializeField]
		private GameObject m_ModalPanel;
		public GameObject modalPanel { get { return m_ModalPanel; } set { m_ModalPanel = value; } }

		[SerializeField]
		private RectTransform m_ButtonPanel;
		public RectTransform buttonPanel { get { return m_ButtonPanel; } set { m_ButtonPanel = value; } }

		[Space]

		[SerializeField]
		private Graphic m_Question;
		public Graphic question { get { return m_Question; } set { m_Question = value; } }
		public Text textQuestion { get { return m_Question as Text; } set { m_Question = value; } }

		[SerializeField]
		private Graphic m_Title;
		public Graphic title { get { return m_Title; } set { m_Title = value; } }
		public Text textTitle { get { return m_Title as Text; } set { m_Title = value; } }

		[SerializeField]
		private Graphic m_Icon;
		public Graphic icon { get { return m_Icon; } set { m_Icon = value; } }
		public Image imageIcon { get { return m_Icon as Image; } set { m_Icon = value; } }

		[SerializeField]
		private Graphic m_Background;
		public Graphic background { get { return m_Background; } set { m_Background = value; } }
		public Image imageBackground { get { return m_Background as Image; } set { m_Background = value; } }

		[Space]

		[SerializeField]
		private Selectable m_SetNavigation;
		public Selectable setNavigation { get { return m_SetNavigation; } set { m_SetNavigation = value; } }

		[Space]

		[SerializeField]
		private DialogDetails m_Details = new DialogDetails();
		public DialogDetails details { get { return m_Details; } set { m_Details = value; Refresh(); } }

		[HideInInspector] public string openTrigger = "Open";
		[HideInInspector] public string closeTrigger = "Close";

		private Dictionary<string,float> m_AnimationClipsLength =  new Dictionary<string,float>();
		public Animator animator 
		{ 
			get 
			{ 
				Animator anim = modalPanel.GetComponent<Animator>();

				m_AnimationClipsLength.Clear();
				if (anim)
					foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
						m_AnimationClipsLength.Add(clip.name, clip.length);

				return anim;
			} 
		}

		// List of all the dialog objects currently active in the scene.
		private static Dictionary<string, Dialog> d_List = new Dictionary<string, Dialog>();
		public static Dictionary<string, Dialog> allDialogs { get { return d_List; } }

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		protected Dialog () {}

		protected override void Awake () 
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake();

			if (!d_List.ContainsKey(gameObject.name))
				d_List.Add(gameObject.name, this);

			m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			m_AlphaTweenRunner.Init(this);

			// By awaking Modal Panel is deactivates.
			if (m_ModalPanel != null)
				m_ModalPanel.SetActive(false);
		}

		protected override void OnDestroy ()
		{
			d_List.Remove(gameObject.name);
			base.OnDestroy();
		}

//		// Select on enable and add to the list.
//		protected override void OnEnable ()
//		{
//			base.OnEnable();
//			d_List.Add(gameObject.name, this);
//		}
//
//		// Remove from the list.
//		protected override void OnDisable ()
//		{
//			d_List.Remove(gameObject.name);
//			base.OnDisable();
//		}

		#if UNITY_EDITOR
		protected override void OnValidate ()
		{
			base.OnValidate();

			if (!base.isActiveAndEnabled)
				return;

			Refresh();
		}
		#endif

		public void Open () { Open(null); }
		public void Open (GameObject source) { Show(source); }

		private void Show (GameObject source)
		{
			if (!base.IsActive()) return;

			Preset();
			Refresh();
			SetupButtonTemplate(source);

			if (!animator)
			{
				// Fade in the popup.
				AlphaFadeList(0.15f, 0f, 1f);
			}
		}

		private void Preset ()
		{
			if (details == null) return;

			// Dialog is brings to front.
			transform.SetAsLastSibling();
			// By opening Modal Panel is activates.
			m_ModalPanel.SetActive(true);

			// It is deactivates the icon.
			m_Icon.gameObject.SetActive(false);
			// They are deactivate the buttons.
			foreach (Transform child in m_ButtonPanel as Transform)
				child.gameObject.SetActive(false);
		}

		public void Refresh ()
		{
			if (details == null) return;

			// Set the question.
			textQuestion.text = details.question;
			// Set the title.
			textTitle.text = details.title;
			// Set the icon whether there is.
			if (details.icon)
			{
				imageIcon.sprite = details.icon;
				m_Icon.gameObject.SetActive(true);
			}
			// Set the background.
			if (details.background)
			{
				imageBackground.sprite = details.background;
			}
		}

		private void SetupButtonTemplate (GameObject source)
		{
			if (details == null) return;

			int index = 0;
			foreach (EventButtonDetails eventButton in details.buttonDetails)
			{
				Button button = null;

				if (index < m_ButtonPanel.childCount)
				{
					button = m_ButtonPanel.GetChild(index).GetComponent<Button>();
					button.gameObject.name = "Button " + index + ": " + eventButton.title;
				}
				else if (m_ButtonTemplate != null)
				{
					GameObject goButton = Instantiate(m_ButtonTemplate) as GameObject;
					goButton.name = "Button " + index + ": " + eventButton.title;
					goButton.transform.SetParent(m_ButtonPanel, false);

					button = goButton.GetComponent<Button>();
				}
				else
				{
					Debug.LogWarning("Dialog needs a button template.");
					break;
				}

				if (button != null)
				{
					if (eventButton.colors != null) 
					{
						button.colors = eventButton.colors;
					}

					button.onClick.RemoveAllListeners();
					button.onClick = eventButton.action;
					button.onClick.AddListener(Close);
					if (source != null)
						button.onClick.AddListener(() => { EventSystem.current.SetSelectedGameObject(source); });
					
					button.transform.GetChild(0).GetComponent<Text>().text = eventButton.title;
					button.gameObject.SetActive(true);

					if (eventButton.setAsSelectedGameObject)
						EventSystem.current.SetSelectedGameObject(button.gameObject);
				}

				index++;
			}

			// Set button's navigation.
			if (m_SetNavigation != null) 
			{
				List<Selectable> list = new List<Selectable> ();
				gameObject.GetComponentsInChildren<Selectable> (false, list);
				Selectable first = null;
				foreach (Selectable sel in list) 
				{
					if (!sel.transform.IsChildOf (m_ButtonPanel.transform) && sel.interactable) 
					{
						first = sel;
						break;
					}
				}

				Button prev = null;
				foreach (Transform child in m_ButtonPanel as Transform) 
				{
					Button button = child.GetComponent<Button> ();

					if (prev != null) 
					{
						Navigation prevNav = prev.navigation;
						Navigation buttonNav = button.navigation;
						prevNav.mode = Navigation.Mode.Explicit;
						buttonNav.mode = Navigation.Mode.Explicit;

						prevNav.selectOnDown = button;
						prevNav.selectOnRight = button;
						buttonNav.selectOnLeft = prev;
						buttonNav.selectOnUp = prev;

						prev.navigation = prevNav;
						button.navigation = buttonNav;
					} 
					else if (first != null) 
					{
						Navigation buttonNav = button.navigation;
						buttonNav.mode = Navigation.Mode.Explicit;

						buttonNav.selectOnLeft = first;
						buttonNav.selectOnUp = first;

						button.navigation = buttonNav;
					}
					prev = button;
				}

				Navigation nav = m_SetNavigation.navigation;
				nav.mode = Navigation.Mode.Explicit;

				nav.selectOnDown = m_ButtonPanel.transform.GetChild (0).GetComponent<Selectable> ();

				m_SetNavigation.navigation = nav;
			}
		}

		private void AlphaFadeList (float duration, float alpha)
		{
			CanvasGroup group = m_ModalPanel.GetComponent<CanvasGroup>();
			AlphaFadeList(duration, group.alpha, alpha);
		}

		private void AlphaFadeList (float duration, float start, float end)
		{
			if (!m_ModalPanel)
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
			if (!m_ModalPanel)
				return;

			CanvasGroup group = m_ModalPanel.GetComponent<CanvasGroup>();
			group.alpha = alpha;
		}

		public void Close ()
		{
			if (animator)
			{
				animator.SetTrigger(closeTrigger);
				Invoke("Hide", m_AnimationClipsLength[closeTrigger]);
			}
			else
			{
				AlphaFadeList(0.15f, 0f);
				Invoke("Hide", 0.15f);
			}
		}

		private void Hide ()
		{
			m_ModalPanel.SetActive(false);
			transform.SetAsFirstSibling();
		}
	}
}
