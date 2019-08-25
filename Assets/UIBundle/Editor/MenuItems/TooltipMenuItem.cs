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
	public class TooltipMenuItem 
	{
		private static int uiLayer = 5;
		private static string title = "<b>Ottomans</b>";
		private static string message = "<color=#FFFF09><b>only units:</b> howitzer, Janissary, espahí, galley and large bombard.</color> \n<color=#5AF745><b>Bonus civilization:</b> urban centers generate settlers.</color> \n\n· They have more unique units than any other civilization.\n· The millet system increases the speed limit and the production of the settlers.\n· You can access artillery and infantry firearms in the second age, but lack of pikemen, archers and musketeers.";

		[MenuItem("GameObject/UI/UIBundle/Tooltip")]
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
			GameObject go = new GameObject("Tooltip Template");
			go.AddComponent<RectTransform>();
			go.AddComponent<CanvasRenderer>();
			go.AddComponent<Image>();
			go.AddComponent<HorizontalLayoutGroup>();
			go.transform.SetParent(rootObj.transform, false);
			go.layer = uiLayer;

			RectTransform trans = go.GetComponent<RectTransform>();
			RectTransform rootObjRectTransform = rootObj.transform as RectTransform;

			Vector2 rootObjSizeDelta = rootObjRectTransform.sizeDelta;
			Vector2 tooltipPos = trans.localPosition;

			tooltipPos.x = (rootObjSizeDelta.x * 0.5f);
			tooltipPos.y = (rootObjSizeDelta.y * 0.5f);

			trans.localScale = Vector3.one;
			trans.pivot = new Vector2(0f, 1f);
			trans.anchorMin = new Vector2(.5f, .5f);
			trans.anchorMax = new Vector2(.5f, .5f);
			trans.anchoredPosition = tooltipPos;
			trans.sizeDelta = new Vector2(290f, 185f);

			Image image = go.GetComponent<Image>();
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			image.color = new Color(.1961f, .1961f, .1961f, .5f);
			image.type = Image.Type.Sliced;

			HorizontalLayoutGroup hlg = go.GetComponent<HorizontalLayoutGroup>();
			hlg.padding = new RectOffset(2, 2, 2, 2);
			hlg.spacing = 3f;
			hlg.childAlignment = TextAnchor.UpperLeft;
			hlg.childForceExpandWidth = false;
			hlg.childForceExpandHeight = false;

			// ### Icon ###
			GameObject goIcon = new GameObject("Icon");
			goIcon.AddComponent<RectTransform>();
			goIcon.AddComponent<CanvasRenderer>();
			goIcon.AddComponent<Image>();
			goIcon.AddComponent<LayoutElement>();
			goIcon.transform.SetParent(go.transform, false);
			goIcon.layer = uiLayer;

			RectTransform transIcon = goIcon.GetComponent<RectTransform>();
			transIcon.localScale = Vector3.one;
			transIcon.pivot = new Vector2(.5f, .5f);
			transIcon.anchorMin = new Vector2(0f, 1f);
			transIcon.anchorMax = new Vector2(0f, 1f);
			transIcon.anchoredPosition = new Vector2(34f, -34f);
			transIcon.sizeDelta = new Vector2(64f, 64f);

			Image imageIcon = goIcon.GetComponent<Image>();
			imageIcon.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
			imageIcon.color = new Color(.1961f, .1961f, .1961f, 1f);

			LayoutElement layoutElementIcon = goIcon.GetComponent<LayoutElement>();
			layoutElementIcon.minWidth = 64f;
			layoutElementIcon.minHeight = 64f;
			layoutElementIcon.preferredWidth = 64f;
			layoutElementIcon.preferredHeight = 64f;

			// ### Message Panel ###
			GameObject goMessagePanel = new GameObject("Message Panel");
			goMessagePanel.AddComponent<RectTransform>();
			goMessagePanel.AddComponent<CanvasRenderer>();
			goMessagePanel.AddComponent<Image>();
			goMessagePanel.AddComponent<LayoutElement>();
			goMessagePanel.AddComponent<VerticalLayoutGroup>();
			goMessagePanel.transform.SetParent(go.transform, false);
			goMessagePanel.layer = uiLayer;

			RectTransform transMessagePanel = goMessagePanel.GetComponent<RectTransform>();
			transMessagePanel.localScale = Vector3.one;
			transMessagePanel.pivot = new Vector2(.5f, .5f);
			transMessagePanel.anchorMin = new Vector2(0f, 1f);
			transMessagePanel.anchorMax = new Vector2(0f, 1f);
			transMessagePanel.anchoredPosition = new Vector2(178.5f, -92.5f);
			transMessagePanel.sizeDelta = new Vector2(219f, 181f);

			Image imageMessagePanel = goMessagePanel.GetComponent<Image>();
			imageMessagePanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageMessagePanel.color = new Color(1f, 1f, 1f, 0f);
			imageMessagePanel.type = Image.Type.Sliced;

			LayoutElement layoutElementMessagePanel = goMessagePanel.GetComponent<LayoutElement>();
			layoutElementMessagePanel.minWidth = 0f;
			layoutElementMessagePanel.minHeight = 0f;
			layoutElementMessagePanel.flexibleWidth = 1f;
			layoutElementMessagePanel.flexibleHeight = 1f;

			VerticalLayoutGroup vlgMessagePanel = goMessagePanel.GetComponent<VerticalLayoutGroup>();
			vlgMessagePanel.padding = new RectOffset(3, 3, 0, 0);
			vlgMessagePanel.spacing = 5f;
			vlgMessagePanel.childAlignment = TextAnchor.UpperLeft;
			vlgMessagePanel.childForceExpandWidth = true;
			vlgMessagePanel.childForceExpandHeight = false;

			// ### Title ###
			GameObject goTitle = new GameObject("Title");
			goTitle.AddComponent<RectTransform>();
			goTitle.AddComponent<CanvasRenderer>();
			goTitle.AddComponent<Text>();
			goTitle.AddComponent<LayoutElement>();
			goTitle.transform.SetParent(goMessagePanel.transform, false);
			goTitle.layer = uiLayer;

			RectTransform transTitle = goTitle.GetComponent<RectTransform>();
			transTitle.localScale = Vector3.one;
			transTitle.pivot = new Vector2(.5f, .5f);
			transTitle.anchorMin = new Vector2(0f, 1f);
			transTitle.anchorMax = new Vector2(0f, 1f);
			transTitle.anchoredPosition = new Vector2(109.5f, -8.5f);
			transTitle.sizeDelta = new Vector2(213f, 17f);

			Text textTitle = goTitle.GetComponent<Text>();
			textTitle.text = title;
			textTitle.fontSize = 14;
			textTitle.color = new Color(.8471f, .8471f, .8471f, 1);
			textTitle.alignment = TextAnchor.MiddleCenter;
			textTitle.horizontalOverflow = HorizontalWrapMode.Wrap;
			textTitle.verticalOverflow = VerticalWrapMode.Truncate;

			LayoutElement layoutElementTitle = goTitle.GetComponent<LayoutElement>();
			layoutElementTitle.minHeight = 17f;
			layoutElementTitle.preferredHeight = 17f;

			// ### Message ###
			GameObject goMessage = new GameObject("Message");
			goMessage.AddComponent<RectTransform>();
			goMessage.AddComponent<CanvasRenderer>();
			goMessage.AddComponent<Text>();
			goMessage.AddComponent<LayoutElement>();
			goMessage.transform.SetParent(goMessagePanel.transform, false);
			goMessage.layer = uiLayer;

			RectTransform transMessage = goMessage.GetComponent<RectTransform>();
			transMessage.localScale = Vector3.one;
			transMessage.pivot = new Vector2(.5f, .5f);
			transMessage.anchorMin = new Vector2(0f, 1f);
			transMessage.anchorMax = new Vector2(0f, 1f);
			transMessage.anchoredPosition = new Vector2(109.5f, -101.5f);
			transMessage.sizeDelta = new Vector2(213f, 159f);

			Text textMessage = goMessage.GetComponent<Text>();
			textMessage.text = message;
			textMessage.fontSize = 14;
			textMessage.color = new Color(.8471f, .8471f, .8471f, 1);
			textMessage.alignment = TextAnchor.UpperLeft;
			textMessage.horizontalOverflow = HorizontalWrapMode.Wrap;
			textMessage.verticalOverflow = VerticalWrapMode.Truncate;
			textMessage.resizeTextForBestFit = true;
			textMessage.resizeTextMinSize = 10;
			textMessage.resizeTextMaxSize = 40;

			LayoutElement layoutElementMessage = goMessage.GetComponent<LayoutElement>();
			layoutElementMessage.minWidth = 0f;
			layoutElementMessage.minHeight = 0f;
			layoutElementMessage.flexibleWidth = 1f;
			layoutElementMessage.flexibleHeight = 1f;

			// ### Tooltip ###
			rootObj.AddComponent<Tooltip>();
			Tooltip tooltip = rootObj.GetComponent<Tooltip>();
			tooltip.template = trans;
			tooltip.title = textTitle as Graphic;
			tooltip.message = textMessage as Graphic;
			tooltip.icon = imageIcon as Graphic;
			tooltip.background = image as Graphic;

			// Fill with default values
			tooltip.details = new Tooltip.TooltipDetails()
			{
				title = title,
				message = message
			};

			trans.gameObject.SetActive(false);

			// Select game object in the hierarchy
			Selection.activeGameObject = rootObj;

			return go;
		}
	}
}
