// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UIBundle.MenuItems
{
	public class GameMessageMenuItem 
	{
		private static int uiLayer = 5;
		private static string messageText = "Overlay Message";

		[MenuItem("GameObject/UI/UIBundle/Overlay Message")]
		public static GameObject Create ()
		{
			// Get root object.
			GameObject rootObj = null;

			if (Selection.activeGameObject != null 
				&& Selection.activeGameObject.transform is RectTransform)
			{
				rootObj = Selection.activeGameObject;
			}
			else if (GameObject.FindObjectOfType<Canvas>() is Canvas)
			{
				rootObj = GameObject.FindObjectOfType<Canvas>().gameObject;
			}
			else if (!(GameObject.FindObjectOfType<Canvas>() is Canvas))
			{
				GameObject goCanvas = new GameObject("Canvas");
				goCanvas.AddComponent<RectTransform>();
				goCanvas.AddComponent<Canvas>();
				goCanvas.AddComponent<CanvasScaler>();
				goCanvas.AddComponent<GraphicRaycaster>();
				rootObj = goCanvas;

				Canvas canvas = goCanvas.GetComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;

				if (!(GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() as UnityEngine.EventSystems.EventSystem))
				{
					GameObject eventSystem = new GameObject("EventSystem");
					eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
					eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
				}
			}
			else
			{
				Debug.LogError("There was a problem to create the UI.");
				return null;
			}

			// ### Game Object ###
			GameObject go = new GameObject("Overlay Message");
			go.AddComponent<RectTransform>();
			go.transform.SetParent(rootObj.transform, false);
			go.layer = uiLayer;

			RectTransform trans = go.GetComponent<RectTransform>();
			trans.localScale = Vector3.one;
			trans.pivot = new Vector2(.5f, .5f);
			trans.anchorMin = new Vector2(0f, 0f);
			trans.anchorMax = new Vector2(1f, 1f);
			trans.anchoredPosition = new Vector2(0f, 0f);
			trans.offsetMin = new Vector2(0f, 0f);
			trans.offsetMax = new Vector2(0f, 0f);

			// ### Message Template ###
			GameObject goMessage = new GameObject("Message Template");
			goMessage.AddComponent<RectTransform>();
			goMessage.AddComponent<CanvasRenderer>();
			goMessage.AddComponent<Text>();
			goMessage.transform.SetParent(go.transform, false);
			goMessage.layer = uiLayer;

			RectTransform transMessage = goMessage.GetComponent<RectTransform>();
			transMessage.localScale = Vector3.one;
			transMessage.pivot = new Vector2(.5f, .5f);
			transMessage.anchorMin = new Vector2(.2f, 0f);
			transMessage.anchorMax = new Vector2(.8f, 0f);
			transMessage.anchoredPosition = new Vector2(0f, 120f);
			transMessage.sizeDelta = new Vector2(0f, 30f);

			Text textMessage = goMessage.GetComponent<Text>();
			textMessage.text = messageText;
			textMessage.fontStyle = FontStyle.Bold;
			textMessage.fontSize = 18;
			textMessage.color = new Color(.8078f, .7882f, .0823f, 1f);
			textMessage.alignment = TextAnchor.MiddleCenter;
			textMessage.horizontalOverflow = HorizontalWrapMode.Wrap;
			textMessage.verticalOverflow = VerticalWrapMode.Overflow;

			goMessage.SetActive(false);

			// ### OverlayMessage ###
			go.AddComponent<OverlayMessage>();
			OverlayMessage overlayMessage = go.GetComponent<OverlayMessage>();
			overlayMessage.messageTemplate = transMessage;

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
