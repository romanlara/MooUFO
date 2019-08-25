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
	public class OverlayTextMenuItem 
	{
		private static int uiLayer = 5;
		private static string label = "Overlay Text";

		[MenuItem("GameObject/UI/UIBundle/Overlay Text")]
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
			GameObject go = new GameObject("Overlay Text");
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

			// ### Text Template ###
			GameObject goText = new GameObject("Text Template");
			goText.AddComponent<RectTransform>();
			goText.AddComponent<CanvasRenderer>();
			goText.AddComponent<Text>();
			goText.transform.SetParent(go.transform, false);
			goText.layer = uiLayer;

			RectTransform transText = goText.GetComponent<RectTransform>();
			transText.localScale = Vector3.one;
			transText.pivot = new Vector2(.5f, .5f);
			transText.anchorMin = new Vector2(.5f, .5f);
			transText.anchorMax = new Vector2(.5f, .5f);
			transText.anchoredPosition = new Vector2(0f, 0f);
			transText.sizeDelta = new Vector2(0f, 0f);

			Text text = goText.GetComponent<Text>();
			text.text = label;
			text.fontStyle = FontStyle.Normal;
			text.fontSize = 16;
			text.color = new Color(.8078f, .7882f, .0823f, 1f);
			text.alignment = TextAnchor.MiddleCenter;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.verticalOverflow = VerticalWrapMode.Overflow;

			goText.SetActive(false);

			// ### OverlayText ###
			go.AddComponent<OverlayText>();
			OverlayText overlayText = go.GetComponent<OverlayText>();
			overlayText.textTemplate = transText;
			overlayText.duration = 1f;

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
