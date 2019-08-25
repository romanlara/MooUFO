// -----------------------------------------------------
//                        uiBundle
//                           by 
//                      Lara Brothers
//                     Copyrights 2016
//    Roman Lara (programmer) & Humberto Lara (Artist)
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIBundle
{
	public class Language/* : ScriptableObject */
	{
		[System.Serializable]
		public class TextData
		{
//			[SerializeField]
//			private string m_Key;
//			public string key { get { return m_Key; } set { m_Key = value; } }
//
//			[SerializeField]
//			[TextArea(3, 10)]
//			private string m_Value;
//			public string value { get { return m_Value; } set { m_Value = value; } }

			[SerializeField]
			private Dictionary<string, string> m_Texts;
			public Dictionary<string, string> texts { get { return m_Texts; } set { m_Texts = value; } }

			public TextData () { this.texts = new Dictionary<string, string>(); }
			public TextData (string key, string value) : this() { this.texts.Add(key, value); }
			public TextData (Dictionary<string, string> texts) : this() { this.texts = texts; }
		}

		[System.Serializable]
		public class GroupDataList
		{
			[SerializeField]
			private Dictionary<string, TextData> m_Groups;
			public Dictionary<string, TextData> groups { get { return m_Groups; } set { m_Groups = value; } }

			public GroupDataList () { this.groups = new Dictionary<string, TextData>(); }
		}

		[SerializeField]
		private string m_LangName;
		public string langName { get { return m_LangName; } set { m_LangName = value; } }

		[Space]

		[SerializeField]
		private GroupDataList m_Translations;
		public GroupDataList translations { get { return m_Translations; } set { m_Translations = value; } }

//		public string[] allKeys 
//		{
//			get 
//			{
//				List<string> keys = new List<string>();
//
//				foreach (TranslationData translation in translations)
//					keys.Add(translation.key);
//
//				string[] _keys = new string[keys.Count];
//				keys.CopyTo(_keys);
//
//				return _keys;
//				return null;
//			}
//		}

		public string[] AllGroups ()
		{
			string[] groups = new string[translations.groups.Count];
			translations.groups.Keys.CopyTo(groups, 0);
			return groups;
		}

		public string[] AllKeys (string group)
		{
			string[] keys = new string[2];
			if (translations.groups.ContainsKey(group))
			{
				keys = new string[translations.groups[group].texts.Count];
				translations.groups[group].texts.Keys.CopyTo(keys, 0);
			}
			return keys;
		}
		

		public bool HasKey (string group, string key)
		{
//			foreach (TranslationData translation in translations)
//				if (translation.key == key)
//					return true;
//
//			return false;
			if (translations == null) return false;

			bool b;
			if (translations.groups.ContainsKey(group))
				b =  translations.groups[group].texts.ContainsKey(key);
			else
				b = false;
			return b;
		}

//		private TranslationData _HasKey (string key)
//		{
//			foreach (TranslationData translation in translations)
//				if (translation.key == key)
//					return translation;
//
//			return null;
//		}

		public string GetTranslation (string group, string key)
		{
//			TranslationData translation = _HasKey(key);
//
//			if (translation != null)
//				return translation.value;
//			else
//				return key;

			if (HasKey(group, key))
				return translations.groups[group].texts[key];
			else
				return key;
		}
	}
}
