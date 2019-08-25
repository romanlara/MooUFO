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
	public class LoadingScreenMenuItem 
	{
		private static int uiLayer = 5;
		private static string sceneName = "Level01";
		private static string title = "Loading...";
		private static string message = "Only dreamers fight to the death. There are only a handful of such fools in the world. All others surrender and hope they will not be beaten too badly by their new masters.\n- Vinus Solamnus, Founder of the knights and kingdom of Solamnia";

		[MenuItem("GameObject/UI/UIBundle/Loading Screen")]
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
			GameObject go = new GameObject("Loading Screen");
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

			// ### Loading Panel ###
			GameObject goLoadingPanel = new GameObject("Loading Panel");
			goLoadingPanel.AddComponent<RectTransform>();
			goLoadingPanel.AddComponent<CanvasRenderer>();
			goLoadingPanel.AddComponent<Image>();
			goLoadingPanel.AddComponent<CanvasGroup>();
			goLoadingPanel.transform.SetParent(go.transform, false);
			goLoadingPanel.layer = uiLayer;

			RectTransform transModalPanel = goLoadingPanel.GetComponent<RectTransform>();
			transModalPanel.localScale = Vector3.one;
			transModalPanel.pivot = new Vector2(.5f, .5f);
			transModalPanel.anchorMin = new Vector2(0f, 0f);
			transModalPanel.anchorMax = new Vector2(1f, 1f);
			transModalPanel.offsetMin = new Vector2(0f, 0f);
			transModalPanel.offsetMax = new Vector2(0f, 0f);

			Image imageLoadingPanel = goLoadingPanel.GetComponent<Image>();
			imageLoadingPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageLoadingPanel.color = Color.white;
			imageLoadingPanel.type = Image.Type.Sliced;
			imageLoadingPanel.preserveAspect = true;

			CanvasGroup canvasGroupModalPanel = goLoadingPanel.GetComponent<CanvasGroup>();
			canvasGroupModalPanel.alpha = 1f;
			canvasGroupModalPanel.interactable = false;
			canvasGroupModalPanel.blocksRaycasts = true;
			canvasGroupModalPanel.ignoreParentGroups = false;

			// ### Progress Panel ###
			GameObject goProgressPanel = new GameObject("Progress Panel");
			goProgressPanel.AddComponent<RectTransform>();
			goProgressPanel.AddComponent<CanvasRenderer>();
			goProgressPanel.AddComponent<Image>();
			goProgressPanel.AddComponent<VerticalLayoutGroup>();
			goProgressPanel.transform.SetParent(goLoadingPanel.transform, false);
			goProgressPanel.layer = uiLayer;

			RectTransform transProgressPanel = goProgressPanel.GetComponent<RectTransform>();
			transProgressPanel.localScale = Vector3.one;
			transProgressPanel.pivot = new Vector2(.5f, 0f);
			transProgressPanel.anchorMin = new Vector2(0f, 0f);
			transProgressPanel.anchorMax = new Vector2(1f, 0f);
			transProgressPanel.offsetMin = new Vector2(0f, 0f);
			transProgressPanel.offsetMax = new Vector2(0f, 0f);
			transProgressPanel.anchoredPosition = new Vector2(0f, 0f);
			transProgressPanel.sizeDelta = new Vector2(0f, 256f);

			Image imageProgressPanel = goProgressPanel.GetComponent<Image>();
			imageProgressPanel.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			imageProgressPanel.color = new Color(.1961f, .1961f, .1961f, .5f);
			imageProgressPanel.type = Image.Type.Sliced;
			imageProgressPanel.preserveAspect = true;

			VerticalLayoutGroup verticalLayoutGroupProgressPanel = goProgressPanel.GetComponent<VerticalLayoutGroup>();
			verticalLayoutGroupProgressPanel.padding = new RectOffset(25, 25, 0, 15);
			verticalLayoutGroupProgressPanel.spacing = 15f;
			verticalLayoutGroupProgressPanel.childAlignment = TextAnchor.MiddleCenter;
			verticalLayoutGroupProgressPanel.childForceExpandWidth = false;
			verticalLayoutGroupProgressPanel.childForceExpandHeight = false;

			// ### Loading Text ###
			GameObject goLoadingText = new GameObject("Loading Text");
			goLoadingText.AddComponent<RectTransform>();
			goLoadingText.AddComponent<CanvasRenderer>();
			goLoadingText.AddComponent<Text>();
			goLoadingText.transform.SetParent(goProgressPanel.transform, false);
			goLoadingText.layer = uiLayer;

			RectTransform transLoadingText = goLoadingText.GetComponent<RectTransform>();
			transLoadingText.localScale = Vector3.one;
			transLoadingText.pivot = new Vector2(.5f, .5f);

			Text textLoadingText = goLoadingText.GetComponent<Text>();
			textLoadingText.text = title;
			textLoadingText.fontSize = 25;
			textLoadingText.color = new Color(.1961f, .1961f, .1961f, 1);
			textLoadingText.alignment = TextAnchor.MiddleCenter;
			textLoadingText.horizontalOverflow = HorizontalWrapMode.Overflow;
			textLoadingText.verticalOverflow = VerticalWrapMode.Truncate;

			// ### Progressbar ###
			GameObject goProgressbar = ProgressbarMenuItem.Create();
			goProgressbar.AddComponent<LayoutElement>();
			goProgressbar.transform.SetParent(goProgressPanel.transform, false);

			LayoutElement layoutElementProgressbar = goProgressbar.GetComponent<LayoutElement>();
			layoutElementProgressbar.preferredWidth = 440f;
			layoutElementProgressbar.preferredHeight = 20f;

			Progressbar progressbar = goProgressbar.GetComponent<Progressbar>();
			GameObject.DestroyImmediate(progressbar.indicatorRect.gameObject);
			progressbar.indicatorRect = null;
			progressbar.label = null;

			// ### Message ###
			GameObject goMessage = new GameObject("Message");
			goMessage.AddComponent<RectTransform>();
			goMessage.AddComponent<CanvasRenderer>();
			goMessage.AddComponent<LayoutElement>();
			goMessage.AddComponent<Text>();
			goMessage.transform.SetParent(goProgressPanel.transform, false);
			goMessage.layer = uiLayer;

			RectTransform transMessage = goMessage.GetComponent<RectTransform>();
			transMessage.localScale = Vector3.one;
			transMessage.pivot = new Vector2(.5f, .5f);

			LayoutElement layoutElementMessage = goMessage.GetComponent<LayoutElement>();
			layoutElementMessage.minWidth = 0f;
			layoutElementMessage.minHeight = 0f;
			layoutElementMessage.preferredWidth = 1290f;

			Text textMessage = goMessage.GetComponent<Text>();
			textMessage.text = message;
			textMessage.fontSize = 18;
			textMessage.color = new Color(.1961f, .1961f, .1961f, 1);
			textMessage.alignment = TextAnchor.MiddleCenter;
			textMessage.horizontalOverflow = HorizontalWrapMode.Wrap;
			textMessage.verticalOverflow = VerticalWrapMode.Truncate;

			// ### Loading Screen ###
			go.AddComponent<LoadingScreen>();
			LoadingScreen loadingScreen = go.GetComponent<LoadingScreen>();
			loadingScreen.loadingPanel = goLoadingPanel;
			loadingScreen.title = textLoadingText as Graphic;
			loadingScreen.message = textMessage as Graphic;
			loadingScreen.background = imageLoadingPanel as Graphic;
			loadingScreen.progressbar = progressbar;

			// Fill with default values
			loadingScreen.details = new LoadingScreen.LoadingScreenDetails()
			{
				sceneName = sceneName,
				title = title,
				message = message
			};

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
