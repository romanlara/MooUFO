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
	[AddComponentMenu("UI/UIBundle/Loading Screen")]
	[ExecuteInEditMode]
	public class LoadingScreen : UIBehaviour 
	{
		[System.Serializable]
		public class LoadingScreenDetails
		{
			[SerializeField]
			private string m_SceneName;
			public string sceneName { get { return m_SceneName; } set { m_SceneName = value; } }

			[SerializeField]
			private string m_Title;
			public string title { get { return m_Title; } set { m_Title = value; } }

			[SerializeField]
			[TextArea(3, 10)]
			private string m_Message;
			public string message { get { return m_Message; } set { m_Message = value; } }

			[SerializeField]
			private Sprite m_Background;
			public Sprite background { get { return m_Background; } set { m_Background = value; } }

			public LoadingScreenDetails () {}
			public LoadingScreenDetails (string sceneName) { this.sceneName = sceneName; }
			public LoadingScreenDetails (string sceneName, string message) : this(sceneName) { this.message = message; }
			public LoadingScreenDetails (string sceneName, string title, string message) : this(sceneName, message) { this.title = title; }
			public LoadingScreenDetails (string sceneName, string title, string message, Sprite background) : this(sceneName, title, message) { this.background = background; }
		}

		[SerializeField]
		private GameObject m_LoadingPanel;
		public GameObject loadingPanel { get { return m_LoadingPanel; } set { m_LoadingPanel = value; } }

		[Space]

		[SerializeField]
		private Graphic m_Title;
		public Graphic title { get { return m_Title; } set { m_Title = value; } }
		public Text textTitle { get { return m_Title as Text; } set { m_Title = value; } }

		[SerializeField]
		private Graphic m_Message;
		public Graphic message { get { return m_Message; } set { m_Message = value; } }
		public Text textMessage { get { return m_Message as Text; } set { m_Message = value; } }

		[SerializeField]
		private Graphic m_Background;
		public Graphic background { get { return m_Background; } set { m_Background = value; } }
		public Image imageBackground { get { return m_Background as Image; } set { m_Background = value; } }

		[SerializeField]
		private Progressbar m_Progressbar;
		public Progressbar progressbar { get { return m_Progressbar; } set { m_Progressbar = value; } }

		[Space]

		[SerializeField]
		private LoadingScreenDetails m_Details = new LoadingScreenDetails();
		public LoadingScreenDetails details { get { return m_Details; } set { m_Details = value; } }

		[HideInInspector] public string openTrigger = "Open";
		[HideInInspector] public string closeTrigger = "Close";

		private Dictionary<string,float> m_AnimationClipsLength =  new Dictionary<string,float>();
		public Animator animator 
		{ 
			get 
			{ 
				Animator anim = m_LoadingPanel.GetComponent<Animator>();

				m_AnimationClipsLength.Clear();
				if (anim)
					foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
						m_AnimationClipsLength.Add(clip.name, clip.length);

				return anim;
			} 
		}

		// List of all the loading screen objects currently active in the scene.
		private static Dictionary<string, LoadingScreen> ls_List = new Dictionary<string, LoadingScreen>();
		public static Dictionary<string, LoadingScreen> allLoadingScreens { get { return ls_List; } }

		private float m_Progress;
		private bool m_bLoading;

		private TweenRunner<FloatTween> m_AlphaTweenRunner;

		protected LoadingScreen () {}

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			if (!ls_List.ContainsKey(gameObject.name))
				ls_List.Add(gameObject.name, this);

			m_AlphaTweenRunner = new TweenRunner<FloatTween>();
			m_AlphaTweenRunner.Init(this);

			// By awaking Loading Panel is deactivates.
			if (m_LoadingPanel != null)
				m_LoadingPanel.SetActive(false);
		}

		protected override void OnDestroy ()
		{
			ls_List.Remove(gameObject.name);
			base.OnDestroy();
		}

//		// Select on enable and add to the list.
//		protected override void OnEnable ()
//		{
//			base.OnEnable();
//			ls_List.Add(gameObject.name, this);
//		}
//
//		// Remove from the list.
//		protected override void OnDisable ()
//		{
//			ls_List.Remove(gameObject.name);
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

		public void Open () { Show(); }

		private void Show ()
		{
			if (!base.IsActive()) return;
			if (details == null) return;

			if (!string.IsNullOrEmpty(details.sceneName))
			{
				Preset();
				Refresh();

				if (!animator)
				{
					// Fade in the popup.
					AlphaFadeList(0.15f, 0f, 1f);
				}

				StartCoroutine(AsyncLoading(details));
			}
		}

		private void Preset ()
		{
			if (details == null) return;

			// Dialog is brings to front.
			transform.SetAsLastSibling();
			// By opening Modal Panel is activates.
			m_LoadingPanel.SetActive(true);

			// It is deactivates the graphics.
			m_Title.gameObject.SetActive(false);
			m_Message.gameObject.SetActive(false);
			// Set the progressbar to zero.
			m_Progressbar.value = m_Progress = 0f;
			m_Progressbar.gameObject.SetActive(true);
		}

		public void Refresh ()
		{
			if (details == null) return;

			// Set the background.
			if (details.background)
			{
				imageBackground.sprite = details.background;
			}
			// Set the title.
			if (!string.IsNullOrEmpty(details.title))
			{
				textTitle.text = details.title;
				m_Title.gameObject.SetActive(true);
			}
			// Set the message.
			if (!string.IsNullOrEmpty(details.message))
			{
				textMessage.text = details.message;
				m_Message.gameObject.SetActive(true);
			}
		}

		public IEnumerator AsyncLoading (LoadingScreenDetails details)
		{
			if (animator)
				yield return new WaitForSeconds(m_AnimationClipsLength[openTrigger]);
			else
				yield return new WaitForSeconds(1f);

			if (!string.IsNullOrEmpty(details.sceneName))
			{
				m_bLoading = true;
				StartCoroutine(UpdateProgressbar());

				AsyncOperation async;

				#if UNITY_5_3 || UNITY_5_3_2
				async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(details.sceneName);
				#else
				async = Application.LoadLevelAsync(details.sceneName);
				#endif

				while (!async.isDone)
				{
					m_Progress = async.progress;
					yield return null;
				}

				m_Progress = 1f;
				yield return new WaitForSeconds(1f);
				m_Progressbar.gameObject.SetActive(false);
				Close();
			}
			else
			{
				Debug.LogWarning("Assign a Scene Name to load the scene.");
			}
		}

		public IEnumerator UpdateProgressbar ()
		{
			while (m_bLoading)
			{
				m_Progressbar.value = Mathf.Lerp(progressbar.value, m_Progress, 3f * Time.deltaTime);
				yield return null;//new WaitForEndOfFrame();
			}
		}

		private void AlphaFadeList (float duration, float alpha)
		{
			CanvasGroup group = m_LoadingPanel.GetComponent<CanvasGroup>();
			AlphaFadeList(duration, group.alpha, alpha);
		}

		private void AlphaFadeList (float duration, float start, float end)
		{
			if (!m_LoadingPanel)
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
			if (!m_LoadingPanel)
				return;

			CanvasGroup group = m_LoadingPanel.GetComponent<CanvasGroup>();
			group.alpha = alpha;
		}

		private void Close ()
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
			m_bLoading = false;
			m_LoadingPanel.SetActive(false);
			transform.SetAsFirstSibling();
		}
	}
}
