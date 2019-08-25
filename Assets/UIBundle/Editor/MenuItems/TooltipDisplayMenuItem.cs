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
	public class TooltipDisplayMenuItem 
	{
		private static int uiLayer = 5;
		private static string message = "A tooltip display shows the information in all the time from a UI currently selected.";

		[MenuItem("GameObject/UI/UIBundle/Tooltip Display")]
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
			GameObject go = new GameObject("Tooltip Display");
			go.AddComponent<RectTransform>();
			go.AddComponent<CanvasRenderer>();
			go.AddComponent<Image>();
			go.transform.SetParent(rootObj.transform, false);
			go.layer = uiLayer;

			RectTransform trans = go.GetComponent<RectTransform>();
			trans.localScale = Vector3.one;
			trans.pivot = new Vector2(.5f, .5f);
			trans.anchorMin = new Vector2(.5f, .5f);
			trans.anchorMax = new Vector2(.5f, .5f);
			trans.anchoredPosition = new Vector2(0f, 0f);
			trans.sizeDelta = new Vector2(350f, 100f);

			Image image = go.GetComponent<Image>();
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			image.type = Image.Type.Sliced;
			image.preserveAspect = true;

			// ### Label ###
			GameObject goLabel = new GameObject("Label");
			goLabel.AddComponent<RectTransform>();
			goLabel.AddComponent<CanvasRenderer>();
			goLabel.AddComponent<Text>();
			goLabel.transform.SetParent(go.transform, false);
			goLabel.layer = uiLayer;

			RectTransform transLabel = goLabel.GetComponent<RectTransform>();
			transLabel.localScale = Vector3.one;
			transLabel.pivot = new Vector2(.5f, .5f);
			transLabel.anchorMin = new Vector2(.5f, .5f);
			transLabel.anchorMax = new Vector2(.5f, .5f);
			transLabel.anchoredPosition = new Vector2(0f, 0f);
			transLabel.sizeDelta = new Vector2(330f, 80f);

			Text label = goLabel.GetComponent<Text>();
			label.text = message;
			label.fontStyle = FontStyle.Normal;
			label.fontSize = 14;
			label.color = new Color(.1961f, .1961f, .1961f, 1f);
			label.alignment = TextAnchor.UpperLeft;
			label.horizontalOverflow = HorizontalWrapMode.Wrap;
			label.verticalOverflow = VerticalWrapMode.Overflow;

			// ### TooltipDisplay ###
			go.AddComponent<TooltipDisplay>();
			TooltipDisplay tooltip = go.GetComponent<TooltipDisplay>();
			tooltip.targetGraphic = go.GetComponent<Graphic>();
			tooltip.label = goLabel.GetComponent<Graphic>();

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
