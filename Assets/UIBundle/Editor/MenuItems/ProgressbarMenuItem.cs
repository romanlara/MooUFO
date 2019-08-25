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
	public class ProgressbarMenuItem 
	{
		private static int uiLayer = 5;

		[MenuItem("GameObject/UI/UIBundle/Progressbar")]
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
			GameObject go = new GameObject("Progressbar");
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
			trans.sizeDelta = new Vector2(160f, 20f);

			Image image = go.GetComponent<Image>();
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");//Resources.Load<Sprite>("unity_builtin_extra/UISprite");
			image.type = Image.Type.Sliced;
			image.preserveAspect = true;

			// ### Filling ###
			GameObject goFilling = new GameObject("Filling");
			goFilling.AddComponent<RectTransform>();
			goFilling.AddComponent<CanvasRenderer>();
			goFilling.AddComponent<Image>();
			goFilling.transform.SetParent(go.transform, false);
			goFilling.layer = uiLayer;

			RectTransform transFilling = goFilling.GetComponent<RectTransform>();
			transFilling.localScale = Vector3.one;
			transFilling.pivot = new Vector2(.5f, .5f);
			transFilling.anchorMin = new Vector2(0f, 0f);
			transFilling.anchorMax = new Vector2(.5f, 1f);
			transFilling.offsetMin = new Vector2(0f, 0f);
			transFilling.offsetMax = new Vector2(0f, 0f);

			Image imageFilling = goFilling.GetComponent<Image>();
			imageFilling.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
			imageFilling.type = Image.Type.Sliced;
			imageFilling.preserveAspect = true;

			// ### Indicator ###
			GameObject goIndicator = new GameObject("Indicator");
			goIndicator.AddComponent<RectTransform>();
			goIndicator.AddComponent<CanvasRenderer>();
			goIndicator.AddComponent<Image>();
			goIndicator.transform.SetParent(go.transform, false);
			goIndicator.layer = uiLayer;

			RectTransform transIndicator = goIndicator.GetComponent<RectTransform>();
			transIndicator.localScale = Vector3.one;
			transIndicator.pivot = new Vector2(.5f, .5f);
			transIndicator.anchorMin = new Vector2(.5f, .5f);
			transIndicator.anchorMax = new Vector2(.5f, .5f);
			transIndicator.anchoredPosition = new Vector2(1.18f, 25f);
			transIndicator.sizeDelta = new Vector2(30f, 30f);

			Image imageIndicator = goIndicator.GetComponent<Image>();
			imageIndicator.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
			imageIndicator.type = Image.Type.Simple;
			imageIndicator.preserveAspect = false;

			// ### Label ###
			GameObject goLabel = new GameObject("Label");
			goLabel.AddComponent<RectTransform>();
			goLabel.AddComponent<CanvasRenderer>();
			goLabel.AddComponent<Text>();
			goLabel.transform.SetParent(goIndicator.transform, false);
			goLabel.layer = uiLayer;

			RectTransform transLabel = goLabel.GetComponent<RectTransform>();
			transLabel.localScale = Vector3.one;
			transLabel.pivot = new Vector2(.5f, .5f);
			transLabel.anchorMin = new Vector2(0f, 0f);
			transLabel.anchorMax = new Vector2(1f, 1f);
			transLabel.offsetMin = new Vector2(2.5f, 2.5f);
			transLabel.offsetMax = new Vector2(-2.5f, -2.5f);

			Text textLabel = goLabel.GetComponent<Text>();
			textLabel.text = "100%";
			textLabel.fontSize = 9;
			textLabel.color = new Color(.1961f, .1961f, .1961f, 1);
			textLabel.alignment = TextAnchor.MiddleCenter;
			textLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
			textLabel.verticalOverflow = VerticalWrapMode.Truncate;
			textLabel.resizeTextForBestFit = false;

			// ### Progressbar ###
			go.AddComponent<Progressbar>();
			Progressbar progressbar = go.GetComponent<Progressbar>();
			progressbar.targetGraphic = go.GetComponent<Graphic>();
			progressbar.fillingRect = transFilling;
			progressbar.indicatorRect = transIndicator;
			progressbar.label = goLabel.GetComponent<Graphic>();

			// Fill with default values
			progressbar.value = .2f;

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
