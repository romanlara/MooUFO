// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

namespace UIBundle
{
	[AddComponentMenu("UI/UIBundle/Translator")]
//	[ExecuteInEditMode]
	public class Translator : MonoBehaviour 
	{
		[SerializeField]
		private Text m_Text;
		public  Text text { get { return m_Text; } set { m_Text = value; } }

		[Space]

		[SerializeField]
		private string m_Group;
		public string group { get { return m_Group; } set { m_Group = value; } }

		[SerializeField]
		private int m_GroupIndex;
		public int groupIndex { get { return m_GroupIndex; } set { m_GroupIndex = value; } }

		[SerializeField]
		private bool m_UseKey = true;
		public bool useKey { get { return m_UseKey; } set { m_UseKey = value; } }

		[SerializeField]
		private string m_Key;
		public string key { get { return m_Key; } set { m_Key = value; } }

		[SerializeField]
		private int m_KeyIndex;
		public int keyIndex { get { return m_KeyIndex; } set { m_KeyIndex = value; } }

		private Localizer m_Localizer;
		public Localizer localizer { get { return m_Localizer; } }

		IEnumerator Start () 
		{
			m_Localizer = Localizer.instance;

			if (m_Localizer)
			{
				if (!text) m_Text = GetComponent<Text>();

				if (text) 
				{
					if (useKey)
						text.text = TranslateFromKey(key);
					else
						text.text = TranslateFromText(text.text);
				}
			}

			yield return null;
		}

		void OnEnable ()
		{
			StartCoroutine("Start");
		}

		public string TranslateFromKey (string key)
		{
			if (!localizer) return key;
//			if (!localizer.language) return key;
//			Language language = localizer.GetCurrent(group);

			return localizer.language.GetTranslation(group, key);
		}

		public void TranslateFromKey (TooltipDisplay tooltip)
		{
			tooltip.ShowDescription(TranslateFromKey(key));
		}

		public string TranslateFromText (string text) 
		{
			Language language = localizer.language;//.GetCurrent(group);
			string translatedText = text;

			// https://regexper.com/#%25(%5Ba-zA-Z%5D*)((-%7C_)%3F%5Ba-zA-Z%5D*)*%25
			foreach (Match match in Regex.Matches(text, @"\%([a-zA-Z]*)((-|_)?[a-zA-Z]*)*%"))
			{
				string key = match.Value.Replace("%", string.Empty);
				translatedText = translatedText.Replace(match.Value, language.GetTranslation(group, key.Trim()) );
			}

			return translatedText;
		}
	}
}
