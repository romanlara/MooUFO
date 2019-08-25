// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace UIBundle
{
	[AddComponentMenu("UI/UIBundle/Localizer")]
//	[ExecuteInEditMode]
	public class Localizer : MonoBehaviour 
	{
//		[System.Serializable]
//		public class LanguageData
//		{
//			[SerializeField]
//			private Language m_Language;
//			public Language language { get { return m_Language; } set { m_Language = value; } }
//
//			[SerializeField]
//			private string m_Group;
//			public string group { get { return m_Group; } set { m_Group = value; } }
//
//			public LanguageData () {}
//			public LanguageData (Language language, string group) : this() 
//			{
//				this.language = language;
//				this.group = group;
//			}
//		}
//
//		[System.Serializable]
//		public class LanguageDataList
//		{
//			[SerializeField]
//			private List<LanguageData> m_Languages;
//			public List<LanguageData> languages { get { return m_Languages; } set { m_Languages = value; } }
//
//			public LanguageDataList () { languages = new List<LanguageData>(); }
//		}

		[SerializeField]
		private string m_Current;
		public string current { get { return m_Current; } set { m_Current = value; } }

		[SerializeField]
		private int m_SelectedIndex;
		public int selectedIndex 
		{ 
			get { return m_SelectedIndex; } 
			set 
			{ 
				m_SelectedIndex = Mathf.Clamp(value, 0, languageNames.Length);
				LoadLanguage(languageNames[m_SelectedIndex]);
			} 
		}

		[Space]

//		[SerializeField]
//		private LanguageDataList m_Languages = new LanguageDataList();
//		public List<LanguageData> languages { get { return m_Languages.languages; } set { m_Languages.languages = value; } }

		private Language m_Language;
		public Language language 
		{ 
			get 
			{ 
				if (m_Language == null)
					LoadLanguage(languageNames[selectedIndex]);
				return m_Language; 
			} 
			set { m_Language = value; } 
		}

		public static Localizer instance;

		public string[] languageNames
		{
			get
			{
//				List<string> lans = new List<string>();
//
//				foreach (LanguageData data in languages)
//					if (data.language != null && !string.IsNullOrEmpty(data.language.label) && !lans.Contains(data.language.label))
//						lans.Add(data.language.label);
//
//				string[] _lans = new string[lans.Count];
//				lans.CopyTo(_lans);
//
//				return _lans;

				TextAsset[] xmlLangs = Resources.LoadAll<TextAsset>("Localizer");

				string[] names = new string[xmlLangs.Length];

				for (int i = 0; i < xmlLangs.Length; i++)
					names[i] = xmlLangs[i].name;

				return names;
			}
		}

//		public string[] groups
//		{
//			get
//			{
//				List<string> gps = new List<string>();
//
//				foreach (LanguageData data in languages)
//					if (!string.IsNullOrEmpty(data.group) && !gps.Contains(data.group))
//						gps.Add(data.group);
//
//				string[] _gps = new string[gps.Count];
//				gps.CopyTo(_gps);
//
//				return _gps;
//			}
//		}

		void Awake () 
		{
			if (!instance)
				instance = this;
			else 
			{
				Debug.LogWarning ("There needs to be one active Localizer script.");
				if (Application.isPlaying)
					Destroy (this);
			}
		}

		public void LoadLanguage (string languageName)
		{
			TextAsset xmlLang = Resources.Load<TextAsset>("Localizer/" + languageName);
			XmlLocalizer xmlLoc = new XmlLocalizer();
			language = xmlLoc.Read(xmlLang.text);
		}

//		public Language GetCurrent (string group)
//		{
//			foreach (LanguageData data in languages)
//				if (data != null && data.language != null && data.language.label == current && data.group == group)
//					return data.language;
//
//			return null;
//		}
//
//		public Language GetLanguage (string label, string group)
//		{
//			foreach (LanguageData data in languages)
//				if (data != null && data.language != null && data.language.label == label && data.group == group)
//					return data.language;
//			
//			return null;
//		}
	}
}
