// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UIBundle.CoroutineTween;

namespace UIBundle
{
	[AddComponentMenu("UI/UIBundle/Overlay HP Bar")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class OverlayHitPointBar : UIBehaviour 
	{
		[System.Serializable]
		public class HitPointBar : MonoBehaviour
		{
			[SerializeField]
			private GameObject m_Source;
			public GameObject source { get { return m_Source; } set { m_Source = value; } }

			[SerializeField]
			private Progressbar m_HPBar;
			public Progressbar hpBar { get { return m_HPBar; } set { m_HPBar = value; } }

			public float value 
			{
				get { return m_HPBar.value; }
				set
				{
					if (!m_HPBar) 
						return;
					Show();
					m_HPBar.value = value;
				}
			}

			[Space]

			[SerializeField]
			private Vector2 m_Offset;
			public Vector2 offset { get { return m_Offset; } set { m_Offset = value; } }

			public RectTransform rect { get { return transform as RectTransform; } }
			public Transform transSource { get { return source.transform; } }

			private TweenRunner<FloatTween> m_AlphaTweenRunner;
			private const float k_Duration = 0.15f;

			void Awake ()
			{
				m_AlphaTweenRunner = new TweenRunner<FloatTween>();
				m_AlphaTweenRunner.Init(this);

				hpBar = GetComponent<Progressbar>();
			}

			void LateUpdate () 
			{
				if (source == null)
				{
					if (gameObject.activeInHierarchy)
						gameObject.SetActive(false);
					return;
				}

				// Always than it has an any value will switchs visualy.
//				if (hpBar.value != 0)
//				{
//					// Get root Canvas.
//					var list = ListPool<Canvas>.Get();
//					gameObject.GetComponentsInParent(false, list);
//					if (list.Count == 0)
//						return;
//					Canvas rootCanvas = list[0];
//					ListPool<Canvas>.Release(list);
//
//					// Find anchoring and position if HP Bar is partially or fully outside of canvas rect
//					// to hide it, and in case contrary show it.
//					Vector3[] corners = new Vector3[4];
//					rect.GetWorldCorners(corners);
//					bool outside = false;
//					RectTransform rootCanvasRectTransform = rootCanvas.transform as RectTransform;
//					for (int i = 0; i < 4; i++)
//					{
//						Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
//						if (!rootCanvasRectTransform.rect.Contains(corner))
//						{
//							outside = true;
//						}
//					}
//					if (outside)
//						Hide();
//					else
//						Show();
//				}
//				// If its value is zero then it is hided and desactivated. 
//				else if (hpBar.value == 0)
//				{
//					HideAndDesactive();
//				}

				// When is desactivated no continues.
				if (!gameObject.activeInHierarchy)
					return;

				Vector3 offset3 = new Vector3(offset.x, offset.y, 0);
				Vector3 screenPos = Camera.main.WorldToScreenPoint(transSource.position + offset3);
				rect.localPosition = screenPos;
			}

			public void Show () 
			{
				if (gameObject.activeInHierarchy)
					AlphaFadeList(k_Duration, 1f); 
				else
					ShowAndActive();
			}

			public void Hide () 
			{ 
				if (gameObject.activeInHierarchy)
					AlphaFadeList(k_Duration, 0f); 
			}

			public void ShowAndActive () 
			{ 
				gameObject.SetActive(true);
				Show();
			}

			public void HideAndDesactive ()
			{
				if (gameObject.activeInHierarchy)
				{
					Hide();
					Invoke("Desactive", k_Duration);
				}
			}
			private void Desactive () { gameObject.SetActive(false); }

			private void AlphaFadeList (float duration, float alpha)
			{
				CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
				AlphaFadeList(duration, group.alpha, alpha);
			}

			private void AlphaFadeList (float duration, float start, float end)
			{
				if (!gameObject.activeInHierarchy)
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
				if (!gameObject.activeInHierarchy)
					return;

				CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
				group.alpha = alpha;
			}
		}

		[System.Serializable]
		public class OptionData
		{
			[SerializeField]
			private HitPointBar m_HitPointBar;
			public HitPointBar hitPointBar { get { return m_HitPointBar; } set { m_HitPointBar = value; } }

			public OptionData () {}
			public OptionData (HitPointBar hitPointBar) : this() { this.hitPointBar = hitPointBar; }
		}

		[System.Serializable]
		public class OptionDataList
		{
			[SerializeField]
			private List<OptionData> m_Options;
			public List<OptionData> options { get { return m_Options; } set { m_Options = value; } }

			public OptionDataList () { options = new List<OptionData>(); }
		}

		[SerializeField]
		private RectTransform m_HPBarTemplate;
		public RectTransform hpBarTemplate { get { return m_HPBarTemplate; } set { m_HPBarTemplate = value; } }

		[Space]

		[SerializeField]
		private OptionDataList m_Options = new OptionDataList();
		public List<OptionData> options { get { return m_Options.options; } set { m_Options.options = value; } }

		// List of all the dialog objects currently active in the scene.
		private static Dictionary<string, OverlayHitPointBar> ohpb_List = new Dictionary<string, OverlayHitPointBar>();
		public static Dictionary<string, OverlayHitPointBar> allOverlayHitPointBars { get { return ohpb_List; } }

		private bool m_ValidTemplate = false;

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			if (!ohpb_List.ContainsKey(gameObject.name))
				ohpb_List.Add(gameObject.name, this);

			options.Clear();
		}

		protected override void OnDestroy ()
		{
			ohpb_List.Remove(gameObject.name);
			base.OnDestroy();
		}

		private void SetupTemplate ()
		{
			m_ValidTemplate = false;

			if (!m_HPBarTemplate)
			{
				Debug.LogError("The HP Bar template is not assigned. The template needs to be assigned and must have a Progressbar Component serving as the health bar.");
				return;
			}

			GameObject templateGO = m_HPBarTemplate.gameObject;
			templateGO.SetActive(true);
			Progressbar healthbar = m_HPBarTemplate.GetComponent<Progressbar>();

			m_ValidTemplate = true;
			if (!healthbar)
			{
				m_ValidTemplate = false;
				Debug.LogError("The HP Bar template is not valid. The template must have a Progressbar Component serving as the health bar.");
			}

			if (!m_ValidTemplate)
			{
				templateGO.SetActive(false);
				return;
			}

			GetOrAddComponent<CanvasGroup>(templateGO);
			templateGO.SetActive(false);

			m_ValidTemplate = true;
		}

		private static T GetOrAddComponent<T> (GameObject go) where T : Component
		{
			T comp = go.GetComponent<T>();
			if (!comp)
				comp = go.AddComponent<T>();
			return comp;
		}

		public void AddHPBar (GameObject source, Vector2 offset = default(Vector2))
		{
			if (!base.IsActive()) 
				return;
			
			if (!m_ValidTemplate)
			{
				SetupTemplate();
				if (!m_ValidTemplate)
					return;
			}

			if (source == null) return;

			foreach (OptionData data in options)
			{
				if (data.hitPointBar != null 
					&& data.hitPointBar.source != null)
					continue;

				if (data.hitPointBar == null)
				{
					data.hitPointBar = CreateHPBar(source, offset);
					return;
				}
				else if (data.hitPointBar.source == null)
				{
					data.hitPointBar.source = source;
					data.hitPointBar.offset = offset;
					return;
				}
			}

			options.Add(new OptionData(CreateHPBar(source, offset)));
		}

		private HitPointBar CreateHPBar (GameObject source, Vector2 offset)
		{
			GameObject newObj = Instantiate(m_HPBarTemplate.gameObject) as GameObject;
			newObj.name = "HPBar: " + source.name;
			newObj.AddComponent<HitPointBar>();
			newObj.SetActive(true);

			RectTransform hpBarRectTransform = newObj.transform as RectTransform;
			hpBarRectTransform.SetParent(m_HPBarTemplate.parent, false);

			HitPointBar newHPBar = newObj.GetComponent<HitPointBar>();
			newHPBar.source = source;
			newHPBar.offset = offset;

			return newHPBar;
		}

		public HitPointBar GetHPBar (GameObject source)
		{
			HitPointBar hitPointBar = null;
			foreach (OptionData data in options)
			{
				if (data.hitPointBar != null 
					&& data.hitPointBar.source == source)
				{
					hitPointBar = data.hitPointBar;
					break;
				}
			}
			return hitPointBar;
		}
	}
}
