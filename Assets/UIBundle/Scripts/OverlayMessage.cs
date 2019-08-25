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
	[AddComponentMenu("UI/UIBundle/Overlay Message")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class OverlayMessage : UIBehaviour 
	{
		[System.Serializable]
		public class MessageBehaviour : MonoBehaviour
		{
			public string text 
			{
				set 
				{
					GetComponent<Text>().text = value;
				}
			}

			public RectTransform rect { get { return transform as RectTransform; } }

			private TweenRunner<FloatTween> m_AlphaTweenRunner;
			private const float k_Duration = 0.15f;

			void Awake ()
			{
				m_AlphaTweenRunner = new TweenRunner<FloatTween>();
				m_AlphaTweenRunner.Init(this);
			}

			public void FadeIn (float duration)
			{
				AlphaFadeList(duration, 0f, 1f);
			}

			public void FadeOut (float duration)
			{
				AlphaFadeList(duration, 0f);
			}

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

			public void TweenPosition (float duration, Vector3 target)
			{
				StartCoroutine(_TweenPosition(1f / duration, target));
			}

			private IEnumerator _TweenPosition (float timeMultiplied, Vector3 target)
			{ 
				Vector3 start = rect.localPosition;

				float duration = 0;
				while (duration < 1)
				{
					rect.localPosition = Vector3.Lerp(start, target, duration);
					duration += Time.unscaledDeltaTime * timeMultiplied;
					yield return null;
				}
				rect.localPosition = target;
			}

			public void TweenScale (float duration, Vector3 target)
			{
				StartCoroutine(_TweenScale(1f / duration, target));
			}

			private IEnumerator _TweenScale (float timeMultiplied, Vector3 target)
			{
				Vector3 start = rect.localScale;

				float duration = 0;
				while (duration < 1)
				{
					rect.localScale = Vector3.Lerp(start, target, duration);
					duration += Time.unscaledDeltaTime * timeMultiplied;
					yield return null;
				}
				rect.localScale = target;
			}
		}

		[System.Serializable]
		public class MessageData
		{
			[SerializeField]
			private MessageBehaviour m_MessageBehaviour;
			public MessageBehaviour messageBehaviour { get { return m_MessageBehaviour; } set { m_MessageBehaviour = value; } }

			public MessageData () {}
			public MessageData (MessageBehaviour messageBehaviour) : this() { this.messageBehaviour = messageBehaviour; }
		}

		[System.Serializable]
		public class MessageDataList
		{
			[SerializeField]
			private List<MessageData> m_List;
			public List<MessageData> list { get { return m_List; } set { m_List = value; } }

			public MessageDataList () { m_List = new List<MessageData>(); }
		}

		[SerializeField]
		private RectTransform m_MessageTemplate;
		public RectTransform messageTemplate { get { return m_MessageTemplate; } set { m_MessageTemplate = value; } }

		[Space]

		[SerializeField]
		private bool m_Zoom;
		public bool zoom { get { return m_Zoom; } set { m_Zoom = value; } }

		[SerializeField]
		private bool m_FadeOut;
		public bool fadeOut { get { return m_FadeOut; } set { m_FadeOut = value; } }

		[SerializeField]
		private bool m_ScaleDown;
		public bool scaleDown { get { return m_ScaleDown; } set { m_ScaleDown = value; } }

		[Space]

		[SerializeField]
		private MessageDataList m_MessageList;
		public List<MessageData> messageList { get { return m_MessageList.list; } set { m_MessageList.list = value; } }

		// List of all the Ovelay Messages objects currently active in the scene.
		private static Dictionary<string, OverlayMessage> om_List = new Dictionary<string, OverlayMessage>();
		public static Dictionary<string, OverlayMessage> allOverlayMessages { get { return om_List; } }

		private bool m_ValidTemplate = false;
		private int m_MsgCounter = 0;
		private Color m_DefaultColor;
//		private List<Transform> msgT_List = new List<Transform>();

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			if (!om_List.ContainsKey(gameObject.name))
				om_List.Add(gameObject.name, this);
		}

		protected override void OnDestroy ()
		{
			om_List.Remove(gameObject.name);
			base.OnDestroy();
		}
		
		private void SetupTemplate ()
		{
			m_ValidTemplate = false;

			if (!m_MessageTemplate)
			{
				Debug.LogError("The Message template is not assigned. The template needs to be assigned and must have a Text Component serving as text message.");
				return;
			}

			GameObject templateGO = m_MessageTemplate.gameObject;
			templateGO.SetActive(true);
			Text text = m_MessageTemplate.GetComponent<Text>();

			m_ValidTemplate = true;
			if (!text)
			{
				m_ValidTemplate = false;
				Debug.LogError("The Message template is not valid. The template must have a Text Component serving as text message.");
			}

			if (!m_ValidTemplate)
			{
				templateGO.SetActive(false);
				return;
			}

			CanvasGroup canvasGroup = GetOrAddComponent<CanvasGroup>(templateGO);
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			m_DefaultColor = text.color;
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

		public void Show (string message, Color color = default(Color))
		{
			if (!base.IsActive()) 
				return;
			
			if (!m_ValidTemplate)
			{
				SetupTemplate();
				if (!m_ValidTemplate)
					return;
			}

			if (color == default(Color))
				color = m_DefaultColor;

			int counter = messageList.Count + 1;
			foreach (MessageData msgData in messageList)
			{
				Vector3 position = m_MessageTemplate.localPosition + new Vector3(0, (counter -= 1) * 20, 0);
				msgData.messageBehaviour.TweenPosition(0.15f, position);
//				TweenPosition(msdData.messageBehaviour.trans, 0.15f, position);
			}

			GameObject messageGO = CreateMessage(m_MessageTemplate.gameObject);
			messageGO.name = "Message: " + (++m_MsgCounter);
			messageGO.SetActive(true);

			RectTransform msgRectTransform = messageGO.transform as RectTransform;
			msgRectTransform.SetParent(m_MessageTemplate.parent, false);

			MessageBehaviour msgBehaviour = GetOrAddComponent<MessageBehaviour>(messageGO);
			msgBehaviour.text = message;
			msgBehaviour.FadeIn(0.15f);

			messageList.Add(new MessageData(msgBehaviour));
			DestroyMessage(msgBehaviour);
		}

		protected virtual GameObject CreateMessage (GameObject template)
		{
			return (GameObject) Instantiate(template);
		}

		protected virtual void DestroyMessage (MessageBehaviour message)
		{
			StartCoroutine(_DestroyMessage(message));
		}

		IEnumerator _DestroyMessage (MessageBehaviour message)
		{
			if (zoom)
			{
//				TweenScale(message.transform, 0.1f, new Vector3(1.1f, 1.1f, 1.1f));
				message.TweenScale(0.1f, new Vector3(1.1f, 1.1f, 1.1f));
				yield return new WaitForSeconds(0.2f);
				message.TweenScale(0.2f, new Vector3(1f, 1f, 1f));
//				TweenScale(message.transform, 0.2f, new Vector3(1.0f, 1.0f, 1.0f));
			}

			float duration = 0;
			while (duration < 1.25f)
			{
				duration += Time.unscaledDeltaTime;
				yield return null;
			}

			if (fadeOut)
				message.FadeOut(0.5f);
//				FadeOut(message.GetComponent<CanvasGroup>(), 0.5f);

			if (scaleDown)
				message.TweenScale(0.5f, new Vector3(0.5f, 0.5f, 0.5f));
//				TweenScale(message.transform, 0.5f, new Vector3(0.5f, 0.5f, 0.5f));

			duration = 0;
			while (duration < 0.75f)
			{
				duration += Time.unscaledDeltaTime;
				yield return null;
			}


			int index = GetIndexOfMessageList(message);
			if (index != -1)
				messageList.RemoveAt(index);
			
			Destroy(message.gameObject);
		}

		private int GetIndexOfMessageList (MessageBehaviour message)
		{
			int index = -1;
			for (int i = 0; i < messageList.Count; i++)
			{
				if (messageList[i].messageBehaviour.gameObject == message.gameObject)
				{
					index = i;
					break;
				}
			}
			return index;
		}

//		private void TweenPosition (Transform trans, float duration, Vector3 target)
//		{
//			
//		}
//
//		private void TweenScale (Transform trans, float duration, Vector3 target)
//		{
//			
//		}

//		private void FadeOut (CanvasGroup canvasGroup, float duration = 0.25f, GameObject go = null)
//		{ 
//			StartCoroutine(FadeOut_Coroutine(canvasGroup, 1f / duration, go));
//		}
//
//		IEnumerator FadeOut_Coroutine (CanvasGroup canvasGroup, float timeMultiplied, GameObject go)
//		{
//			float duration = 0;
//			while (duration < 1)
//			{
//				canvasGroup.alpha = Mathf.Lerp(1f, 0f, duration);
//				duration += Time.unscaledDeltaTime * timeMultiplied;
//				yield return null;
//			}
//			canvasGroup.alpha = 0f;
//
//			if (go != null) go.SetActive(false);
//		}
//
//		private void FadeIn (CanvasGroup canvasGroup, float duration = 0.25f, GameObject go = null)
//		{ 
//			StartCoroutine(FadeIn_Coroutine(canvasGroup, 1f / duration, go)); 
//		}
//
//		IEnumerator FadeIn_Coroutine(CanvasGroup canvasGroup, float timeMultiplied, GameObject go)
//		{
//			if ( go != null) go.SetActive(true);
//
//			float duration = 0;
//			while (duration < 1) 
//			{
//				canvasGroup.alpha = Mathf.Lerp(0f, 1f, duration);
//				duration += Time.unscaledDeltaTime * timeMultiplied;
//				yield return null;
//			}
//			canvasGroup.alpha = 1f;
//		}
	}
}
