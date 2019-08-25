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
	public class SpinnerMenuItem 
	{
		private static int uiLayer = 5;

		[MenuItem("GameObject/UI/UIBundle/Spinner")]
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
			GameObject go = new GameObject("Spinner");
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
			trans.sizeDelta = new Vector2(160f, 30f);

			Image image = go.GetComponent<Image>();
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");//Resources.Load<Sprite>("unity_builtin_extra/UISprite");
			image.type = Image.Type.Sliced;
			image.preserveAspect = true;

			// ### Arrow Backward ###
			GameObject goArrowBackward = new GameObject("Backward");
			goArrowBackward.AddComponent<RectTransform>();
			goArrowBackward.AddComponent<CanvasRenderer>();
			goArrowBackward.AddComponent<Image>();
			goArrowBackward.transform.SetParent(go.transform, false);
			goArrowBackward.layer = uiLayer;

			RectTransform transArrowBackward = goArrowBackward.GetComponent<RectTransform>();
			transArrowBackward.localScale = Vector3.one;
			transArrowBackward.pivot = new Vector2(.5f, .5f);
			transArrowBackward.anchorMin = new Vector2(0f, .5f);
			transArrowBackward.anchorMax = new Vector2(0f, .5f);
			transArrowBackward.anchoredPosition = new Vector2(16f, 0f);
			transArrowBackward.sizeDelta = new Vector2(22f, 22f);

			Image imageBackward = goArrowBackward.GetComponent<Image>();
			imageBackward.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");

			// ### Arrow Forward ###
			GameObject goArrowForward = new GameObject("Forward");
			goArrowForward.AddComponent<RectTransform>();
			goArrowForward.AddComponent<CanvasRenderer>();
			goArrowForward.AddComponent<Image>();
			goArrowForward.transform.SetParent(go.transform, false);
			goArrowForward.layer = uiLayer;

			RectTransform transArrowForward = goArrowForward.GetComponent<RectTransform>();
			transArrowForward.localScale = Vector3.one;
			transArrowForward.pivot = new Vector2(.5f, .5f);
			transArrowForward.anchorMin = new Vector2(1f, .5f);
			transArrowForward.anchorMax = new Vector2(1f, .5f);
			transArrowForward.anchoredPosition = new Vector2(-16f, 0f);
			transArrowForward.sizeDelta = new Vector2(22f, 22f);

			Image imageForward = goArrowForward.GetComponent<Image>();
			imageForward.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");

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
			transLabel.anchorMin = new Vector2(0f, 0f);
			transLabel.anchorMax = new Vector2(1f, 1f);
			transLabel.offsetMin = new Vector2(30f, 5f);
			transLabel.offsetMax = new Vector2(-30f, -5f);

			Text textLabel = goLabel.GetComponent<Text>();
			textLabel.text = "Spinner";
			textLabel.fontSize = 18;
			textLabel.color = new Color(.1961f, .1961f, .1961f, 1);
			textLabel.alignment = TextAnchor.MiddleCenter;
			textLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
			textLabel.resizeTextForBestFit = true;

			// ### Spinner ###
			go.AddComponent<Spinner>();
			Spinner spinner = go.GetComponent<Spinner>();
			spinner.arrowBackward = goArrowBackward.GetComponent<Graphic>();
			spinner.arrowForward = goArrowForward.GetComponent<Graphic>();
			spinner.label = goLabel.GetComponent<Graphic>();

			Selectable selectable = go.GetComponent<Selectable>();
			selectable.targetGraphic = go.GetComponent<Graphic>();

			// Fill with default values
			spinner.range = new Spinner.RangeData(0f, 10f);
			spinner.options = new System.Collections.Generic.List<Spinner.OptionData>() {
				new Spinner.OptionData("Option A"),
				new Spinner.OptionData("Option B"),
				new Spinner.OptionData("Option C")
			};

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
