using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : ScriptableObject 
{
	[System.Serializable]
	public class Data
	{
		[SerializeField]
		private GameObject m_Prefab;
		public GameObject prefab { get { return m_Prefab; } set { m_Prefab = value; } }

		[SerializeField]
		private Sprite m_Sprite;
		public Sprite sprite { get { return m_Sprite; } set { m_Sprite = value; } }

		[SerializeField]
		private int m_Price;
		public int price { get { return m_Price; } set { m_Price = value; } }

		[SerializeField]
		private bool m_Bought;
		public bool bought { get { return m_Bought; } set { m_Bought = value; } }

		public Data () {}
	}

	[System.Serializable]
	public class DataList
	{
		[SerializeField]
		private List<Data> m_List;
		public List<Data> list { get { return m_List; } set { m_List = value; } }

		public DataList () { list = new List<Data>(); }
	}

	[SerializeField]
	private int m_CoinPurse;
	public int coinPurse { get { return m_CoinPurse; } set { m_CoinPurse = Mathf.Clamp (value, 0, 999); } }

	[Space]

	[SerializeField]
	private DataList m_Planets;
	public List<Data> planets { get { return m_Planets.list; } set { m_Planets.list = value; } }

	[Space]

	[SerializeField]
	private DataList m_Characters;
	public List<Data> characters { get { return m_Characters.list; } set { m_Characters.list = value; } }

	public bool[] Extract (List<Data> list)
	{
		bool[] booleans = new bool[list.Count];

		for (int i = 0; i < list.Count; i++)
			booleans [i] = list [i].bought;

		return booleans;
	}

	[System.Serializable]
	public class Extractor
	{
		[SerializeField]
		private int m_CoinPurse;
		public int coinPurse { get { return m_CoinPurse; } set { m_CoinPurse = value; } }

		[SerializeField]
		private bool[] m_Planets;
		public bool[] planets { get { return m_Planets; } set { m_Planets = value; } }

		[SerializeField]
		private bool[] m_Characters;
		public bool[] characters { get { return m_Characters; } set { m_Characters = value; } }

		public Extractor () {}
		public Extractor (int coinPurse, bool[] planets, bool[] characters)
		{
			this.coinPurse = coinPurse;
			this.planets = planets;
			this.characters = characters;
		}
	}
}
