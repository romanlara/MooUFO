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
	public class DialogMenuItem 
	{
		private static int uiLayer = 5;
		private static string title = "Dialog Title";
		private static string question = "Question Text\n(If you see this, everything is right)";

		[MenuItem("GameObject/UI/UIBundle/Dialog")]
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
			GameObject go = new GameObject("Dialog");
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

			// ### Modal Panel ###
			GameObject goModalPanel = new GameObject("Modal Panel");
			goModalPanel.AddComponent<RectTransform>();
			goModalPanel.AddComponent<CanvasRenderer>();
			goModalPanel.AddComponent<Image>();
			goModalPanel.AddComponent<CanvasGroup>();
			goModalPanel.transform.SetParent(go.transform, false);
			goModalPanel.layer = uiLayer;

			RectTransform transModalPanel = goModalPanel.GetComponent<RectTransform>();
			transModalPanel.localScale = Vector3.one;
			transModalPanel.pivot = new Vector2(.5f, .5f);
			transModalPanel.anchorMin = new Vector2(0f, 0f);
			transModalPanel.anchorMax = new Vector2(1f, 1f);
			transModalPanel.offsetMin = new Vector2(0f, 0f);
			transModalPanel.offsetMax = new Vector2(0f, 0f);

			Image imageModalPanel = goModalPanel.GetComponent<Image>();
			imageModalPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageModalPanel.color = new Color(.1961f, .1961f, .1961f, .5f);
			imageModalPanel.type = Image.Type.Sliced;
			imageModalPanel.preserveAspect = true;

			CanvasGroup canvasGroupModalPanel = goModalPanel.GetComponent<CanvasGroup>();
			canvasGroupModalPanel.alpha = 1f;
			canvasGroupModalPanel.interactable = true;
			canvasGroupModalPanel.blocksRaycasts = true;
			canvasGroupModalPanel.ignoreParentGroups = false;

			// ### Dialog Panel ###
			GameObject goDialogPanel = new GameObject("Dialog Panel");
			goDialogPanel.AddComponent<RectTransform>();
			goDialogPanel.AddComponent<CanvasRenderer>();
			goDialogPanel.AddComponent<Image>();
			goDialogPanel.transform.SetParent(goModalPanel.transform, false);
			goDialogPanel.layer = uiLayer;

			RectTransform transDialogPanel = goDialogPanel.GetComponent<RectTransform>();
			transDialogPanel.localScale = Vector3.one;
			transDialogPanel.pivot = new Vector2(.5f, .5f);
			transDialogPanel.anchorMin = new Vector2(.5f, .5f);
			transDialogPanel.anchorMax = new Vector2(.5f, .5f);
			transDialogPanel.anchoredPosition = new Vector2(0f, 0f);
			transDialogPanel.sizeDelta = new Vector2(512f, 300f);

			Image imageDialogPanel = goDialogPanel.GetComponent<Image>();
			imageDialogPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageDialogPanel.color = new Color(1f, 1f, 1f, .7882f);
			imageDialogPanel.type = Image.Type.Sliced;
			imageDialogPanel.preserveAspect = true;

			// ### Title ###
			GameObject goTitle = new GameObject("Text");
			goTitle.AddComponent<RectTransform>();
			goTitle.AddComponent<CanvasRenderer>();
			goTitle.AddComponent<Text>();
			goTitle.transform.SetParent(goDialogPanel.transform, false);
			goTitle.layer = uiLayer;

			RectTransform transTitle = goTitle.GetComponent<RectTransform>();
			transTitle.localScale = Vector3.one;
			transTitle.pivot = new Vector2(.5f, .5f);
			transTitle.anchorMin = new Vector2(.5f, 1f);
			transTitle.anchorMax = new Vector2(.5f, 1f);
			transTitle.anchoredPosition = new Vector2(0f, -20f);
			transTitle.sizeDelta = new Vector2(160f, 30f);

			Text textTitle = goTitle.GetComponent<Text>();
			textTitle.text = title;
			textTitle.fontSize = 14;
			textTitle.color = new Color(.1961f, .1961f, .1961f, 1);
			textTitle.alignment = TextAnchor.MiddleCenter;
			textTitle.horizontalOverflow = HorizontalWrapMode.Wrap;
			textTitle.verticalOverflow = VerticalWrapMode.Truncate;
			textTitle.resizeTextForBestFit = true;
			textTitle.resizeTextMinSize = 14;
			textTitle.resizeTextMaxSize = 20;

			// ### Question Panel ###
			GameObject goQuestionPanel = new GameObject("Question Panel");
			goQuestionPanel.AddComponent<RectTransform>();
			goQuestionPanel.AddComponent<CanvasRenderer>();
			goQuestionPanel.AddComponent<Image>();
			goQuestionPanel.AddComponent<HorizontalLayoutGroup>();
			goQuestionPanel.transform.SetParent(goDialogPanel.transform, false);
			goQuestionPanel.layer = uiLayer;

			RectTransform transQuestionPanel = goQuestionPanel.GetComponent<RectTransform>();
			transQuestionPanel.localScale = Vector3.one;
			transQuestionPanel.pivot = new Vector2(.5f, .5f);
			transQuestionPanel.anchorMin = new Vector2(.5f, 1f);
			transQuestionPanel.anchorMax = new Vector2(.5f, 1f);
			transQuestionPanel.anchoredPosition = new Vector2(0f, -120f);
			transQuestionPanel.sizeDelta = new Vector2(459f, 165f);

			Image imageQuestionPanel = goQuestionPanel.GetComponent<Image>();
			imageQuestionPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageQuestionPanel.color = new Color(1f, 1f, 1f, .7882f);
			imageQuestionPanel.type = Image.Type.Sliced;
			imageQuestionPanel.preserveAspect = true;

			HorizontalLayoutGroup horizontalLayoutGroupQuestionPanel = goQuestionPanel.GetComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroupQuestionPanel.spacing = 7f;
			horizontalLayoutGroupQuestionPanel.childAlignment = TextAnchor.UpperLeft;
			horizontalLayoutGroupQuestionPanel.childForceExpandWidth = false;
			horizontalLayoutGroupQuestionPanel.childForceExpandHeight = false;

			// ### Question Image ###
			GameObject goQuestionImage = new GameObject("Image");
			goQuestionImage.AddComponent<RectTransform>();
			goQuestionImage.AddComponent<CanvasRenderer>();
			goQuestionImage.AddComponent<Image>();
			goQuestionImage.AddComponent<LayoutElement>();
			goQuestionImage.transform.SetParent(goQuestionPanel.transform, false);
			goQuestionImage.layer = uiLayer;

			RectTransform transQuestionImage = goQuestionImage.GetComponent<RectTransform>();
			transQuestionImage.localScale = Vector3.one;
			transQuestionImage.pivot = new Vector2(.5f, .5f);
			transQuestionImage.anchorMin = new Vector2(0f, 1f);
			transQuestionImage.anchorMax = new Vector2(0f, 1f);
			transQuestionImage.anchoredPosition = new Vector2(64f, -64f);
			transQuestionImage.sizeDelta = new Vector2(128f, 128f);

			Image imageQuestionImage = goQuestionImage.GetComponent<Image>();
			imageQuestionImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			imageQuestionImage.color = new Color(1f, 1f, 1f, 1f);

			LayoutElement layoutElementQuestionImage = goQuestionImage.GetComponent<LayoutElement>();
			layoutElementQuestionImage.minWidth = 128f;
			layoutElementQuestionImage.minHeight = 128f;
			layoutElementQuestionImage.preferredWidth = 128f;
			layoutElementQuestionImage.preferredHeight = 128f;

			// ### Question Text ###
			GameObject goQuestionText = new GameObject("Text");
			goQuestionText.AddComponent<RectTransform>();
			goQuestionText.AddComponent<CanvasRenderer>();
			goQuestionText.AddComponent<Text>();
			goQuestionText.AddComponent<LayoutElement>();
			goQuestionText.transform.SetParent(goQuestionPanel.transform, false);
			goQuestionText.layer = uiLayer;

			RectTransform transQuestionText = goQuestionText.GetComponent<RectTransform>();
			transQuestionText.localScale = Vector3.one;
			transQuestionText.pivot = new Vector2(.5f, .5f);
			transQuestionText.anchorMin = new Vector2(0f, 1f);
			transQuestionText.anchorMax = new Vector2(0f, 1f);
			transQuestionText.anchoredPosition = new Vector2(283.5f, -82.5f);
			transQuestionText.sizeDelta = new Vector2(297f, 165f);

			Text textQuestionText = goQuestionText.GetComponent<Text>();
			textQuestionText.text = question;
			textQuestionText.fontSize = 14;
			textQuestionText.color = new Color(.1961f, .1961f, .1961f, 1);
			textQuestionText.alignment = TextAnchor.UpperLeft;
			textQuestionText.horizontalOverflow = HorizontalWrapMode.Wrap;
			textQuestionText.verticalOverflow = VerticalWrapMode.Truncate;
			textQuestionText.resizeTextForBestFit = true;
			textQuestionText.resizeTextMinSize = 14;
			textQuestionText.resizeTextMaxSize = 23;

			LayoutElement layoutElementQuestionText = goQuestionText.GetComponent<LayoutElement>();
			layoutElementQuestionText.minWidth = 0f;
			layoutElementQuestionText.minHeight = 0f;
			layoutElementQuestionText.flexibleWidth = 1f;
			layoutElementQuestionText.flexibleHeight = 1f;

			// ### Button Panel ###
			GameObject goButtonPanel = new GameObject("Button Panel");
			goButtonPanel.AddComponent<RectTransform>();
			goButtonPanel.AddComponent<CanvasRenderer>();
			goButtonPanel.AddComponent<Image>();
			goButtonPanel.AddComponent<HorizontalLayoutGroup>();
			goButtonPanel.transform.SetParent(goDialogPanel.transform, false);
			goButtonPanel.layer = uiLayer;

			RectTransform transButtonPanel = goButtonPanel.GetComponent<RectTransform>();
			transButtonPanel.localScale = Vector3.one;
			transButtonPanel.pivot = new Vector2(.5f, 0f);
			transButtonPanel.anchorMin = new Vector2(.5f, 0f);
			transButtonPanel.anchorMax = new Vector2(.5f, 0f);
			transButtonPanel.anchoredPosition = new Vector2(0f, 20f);
			transButtonPanel.sizeDelta = new Vector2(459f, 61f);

			Image imageButtonPanel = goButtonPanel.GetComponent<Image>();
			imageButtonPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageButtonPanel.color = new Color(1f, 1f, 1f, .7882f);
			imageButtonPanel.type = Image.Type.Sliced;
			imageButtonPanel.preserveAspect = true;

			HorizontalLayoutGroup horizontalLayoutGroupButtonPanel = goButtonPanel.GetComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroupButtonPanel.spacing = 7f;
			horizontalLayoutGroupButtonPanel.childAlignment = TextAnchor.MiddleCenter;
			horizontalLayoutGroupButtonPanel.childForceExpandWidth = true;
			horizontalLayoutGroupButtonPanel.childForceExpandHeight = true;

			// ### Button Template ###
			GameObject goButtonTemplate = new GameObject("Button Template");
			goButtonTemplate.AddComponent<RectTransform>();
			goButtonTemplate.AddComponent<CanvasRenderer>();
			goButtonTemplate.AddComponent<Image>();
			goButtonTemplate.AddComponent<Button>();
			goButtonTemplate.transform.SetParent(go.transform, false);
			goButtonTemplate.layer = uiLayer;

			RectTransform transButtonTemplate = goButtonTemplate.GetComponent<RectTransform>();
			transButtonTemplate.localScale = Vector3.one;
			transButtonTemplate.pivot = new Vector2(.5f, .5f);
			transButtonTemplate.anchorMin = new Vector2(0f, 1f);
			transButtonTemplate.anchorMax = new Vector2(0f, 1f);
			transButtonTemplate.anchoredPosition = new Vector2(641f, -460f);
			transButtonTemplate.sizeDelta = new Vector2(459f, 61f);

			Image imageButtonTemplate = goButtonTemplate.GetComponent<Image>();
			imageButtonTemplate.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			imageButtonTemplate.color = new Color(1f, 1f, 1f, 1f);
			imageButtonTemplate.type = Image.Type.Sliced;
			imageButtonTemplate.preserveAspect = true;

			// ### Text Template ###
			GameObject goTextTemplate = new GameObject("Text");
			goTextTemplate.AddComponent<CanvasRenderer>();
			goTextTemplate.AddComponent<Text>();
			goTextTemplate.transform.SetParent(goButtonTemplate.transform, false);
			goTextTemplate.layer = uiLayer;

			RectTransform transTextTemplate = goTextTemplate.GetComponent<RectTransform>();
			transTextTemplate.localScale = Vector3.one;
			transTextTemplate.pivot = new Vector2(.5f, .5f);
			transTextTemplate.anchorMin = new Vector2(0f, 0f);
			transTextTemplate.anchorMax = new Vector2(1f, 1f);
			transTextTemplate.offsetMin = new Vector2(0f, 0f);
			transTextTemplate.offsetMax = new Vector2(0f, 0f);

			Text textTextTemplate = goTextTemplate.GetComponent<Text>();
			textTextTemplate.text = "Button";
			textTextTemplate.fontSize = 14;
			textTextTemplate.color = new Color(.1961f, .1961f, .1961f, 1);
			textTextTemplate.alignment = TextAnchor.MiddleCenter;
			textTextTemplate.horizontalOverflow = HorizontalWrapMode.Wrap;
			textTextTemplate.verticalOverflow = VerticalWrapMode.Truncate;

			goButtonTemplate.SetActive(false);

			// ### Dialog ###
			go.AddComponent<Dialog>();
			Dialog dialog = go.GetComponent<Dialog>();
			dialog.buttonTemplate = goButtonTemplate;
			dialog.buttonPanel = transButtonPanel;
			dialog.modalPanel = goModalPanel;
			dialog.question = textQuestionText as Graphic;
			dialog.title = textTitle as Graphic;
			dialog.icon = imageQuestionImage as Graphic;
			dialog.background = imageModalPanel as Graphic;

			// Fill with default values
			dialog.details = new Dialog.DialogDetails() {
				title = title,
				question = question
			};
			dialog.details.buttonDetails.Add(new Dialog.EventButtonDetails() { 
				title = "Accept",
				setAsSelectedGameObject = true
			});

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
