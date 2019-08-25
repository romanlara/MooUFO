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
	[AddComponentMenu("UI/UIBundle/Overlay Text")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class OverlayText : UIBehaviour 
	{
		[SerializeField]
		private RectTransform m_TextTemplate;
		public RectTransform textTemplate { get { return m_TextTemplate; } set { m_TextTemplate = value; } }

		[Space]

		[SerializeField]
		private float m_Duration;
		public float duration { get { return m_Duration; } set { m_Duration = value; } }

		// List of all the Ovelay Texts objects currently active in the scene.
		private static Dictionary<string, OverlayText> ot_List = new Dictionary<string, OverlayText>();
		public static Dictionary<string, OverlayText> allOverlayTexts { get { return ot_List; } }

		protected override void Awake ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
			#endif

			base.Awake ();

			if (!ot_List.ContainsKey(gameObject.name))
				ot_List.Add(gameObject.name, this);
		}

		protected override void OnDestroy ()
		{
			ot_List.Remove(gameObject.name);
			base.OnDestroy();
		}
	}
}
