using System.Collections;
using System.Xml;
using System.IO;

namespace UIBundle
{
	public class XmlLocalizer 
	{
		public Language Read (string data)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(new StringReader(data));

			string xmlPathPatternLanguade = "//localization/language";
			string xmlPathPatternGroups = "//localization/group";

			XmlNode nodeLanguage = xmlDoc.SelectSingleNode(xmlPathPatternLanguade);
			XmlNodeList nodesGroups = xmlDoc.SelectNodes(xmlPathPatternGroups);

			Language lang = new Language();
			lang.langName = nodeLanguage.InnerXml;

			Language.GroupDataList groupData = new Language.GroupDataList();
			foreach (XmlNode nodeGroup in nodesGroups)
			{
				string keyGroup = ((XmlElement)nodeGroup).GetAttribute("value");
				groupData.groups.Add(keyGroup, ExtractTexts(nodeGroup.ChildNodes));
			}

			lang.translations = groupData;
			return lang;
		}

		private Language.TextData ExtractTexts (XmlNodeList nodes)
		{
			Language.TextData textData = new Language.TextData();
			foreach (XmlNode node in nodes)
				textData.texts.Add(((XmlElement)node).GetAttribute("key"), node.InnerText);
			return textData;
		}
	}
}
