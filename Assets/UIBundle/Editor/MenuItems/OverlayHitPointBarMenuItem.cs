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
	public class OverlayHitPointBarMenuItem  
	{
		private static int uiLayer = 5;

		[MenuItem("GameObject/UI/UIBundle/Overlay HP Bar")]
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
			GameObject go = new GameObject("Overlay HP Bar");
			go.AddComponent<RectTransform>();
			go.transform.SetParent(rootObj.transform, false);
			go.layer = uiLayer;

			RectTransform trans = go.GetComponent<RectTransform>();
			trans.localScale = Vector3.one;
			trans.pivot = new Vector2(.5f, .5f);
			trans.anchorMin = new Vector2(0f, 0f);
			trans.anchorMax = new Vector2(0f, 0f);
			trans.anchoredPosition = new Vector2(0f, 0f);
			trans.offsetMin = new Vector2(0f, 0f);
			trans.offsetMax = new Vector2(0f, 0f);

			// ### HPBar ###
//			GameObject goHPBar = new GameObject("HPBar");
//			goHPBar.AddComponent<RectTransform>();
//			goHPBar.AddComponent<CanvasRenderer>();
//			goHPBar.AddComponent<Image>();
//			goHPBar.transform.SetParent(go.transform, false);
//			goHPBar.layer = uiLayer;
//
//			RectTransform transHPBar = goHPBar.GetComponent<RectTransform>();
//			transHPBar.localScale = Vector3.one;
//			transHPBar.pivot = new Vector2(.5f, .5f);
//			transHPBar.anchorMin = new Vector2(.5f, .5f);
//			transHPBar.anchorMax = new Vector2(.5f, .5f);
//			transHPBar.anchoredPosition = new Vector2(0f, 0f);
//			transHPBar.sizeDelta = new Vector2(20f, 4f);
//
//			Image imageHPBar = goHPBar.GetComponent<Image>();
//			imageHPBar.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
//			imageHPBar.type = Image.Type.Simple;
//			imageHPBar.preserveAspect = false;
			GameObject goHPBar = ProgressbarMenuItem.Create();
			goHPBar.name = "HPBar Template";
			goHPBar.transform.SetParent(go.transform, false);

			Progressbar progressbar = goHPBar.GetComponent<Progressbar>();
			GameObject.DestroyImmediate(progressbar.indicatorRect.gameObject);
			progressbar.indicatorRect = null;
			progressbar.label = null;

			RectTransform transHPBar = goHPBar.GetComponent<RectTransform>();
			transHPBar.sizeDelta = new Vector2(50f, 7f);

			Image imageHPBar = goHPBar.GetComponent<Image>();
			imageHPBar.sprite = null;
			imageHPBar.color = new Color(.1411f, .2274f, 0f, 1f);

			Image imageFilling = progressbar.fillingRect.GetComponent<Image>();
			imageFilling.sprite = null;
			imageFilling.color = new Color(.2156f, .8156f, .0235f, 1f);

			goHPBar.SetActive(false);

			// ### Overlay Hit Point Bar ###
			go.AddComponent<OverlayHitPointBar>();
			OverlayHitPointBar overlayHPBar = go.GetComponent<OverlayHitPointBar>();
			overlayHPBar.hpBarTemplate = transHPBar;

			// Select game object in the hierarchy
			Selection.activeGameObject = go;

			return go;
		}
	}
}
