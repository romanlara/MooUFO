using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./Managers/Pool System")]
public class PoolSystem : MonoBehaviour 
{
	public static PoolSystem instance;

	[System.Serializable]
	public class PrefabData
	{
		[SerializeField]
		private GameObject m_Prefab;
		public GameObject prefab { get { return m_Prefab; } set { m_Prefab = value; } }

		[SerializeField]
		private int m_Amount;
		public int amount { get { return m_Amount; } set { m_Amount = value; } }

		[SerializeField]
		private Catalog m_Type;
		public Catalog type { get { return m_Type; } set { m_Type = value; } }

		public PrefabData () {}
		public PrefabData (GameObject prefab) { this.prefab = prefab; }
		public PrefabData (GameObject prefab, int amount) : this(prefab) { this.amount = amount; }
	}

	[System.Serializable]
	public class PrefabList
	{
		[SerializeField]
		private List<PrefabData> m_List;
		public List<PrefabData> list { get { return m_List; } set { m_List = value; } }

		public PrefabList () { list = new List<PrefabData>(); }

		public GameObject GetPrefab (Catalog type)
		{
			GameObject prefab = null;

			foreach (PrefabData data in list) 
			{
				if (data.type == type) 
				{
					prefab = data.prefab;
					break;
				}
			}

			return prefab;
		}
	}

//	public GameObject pooledObject;
//	public int pooledAmount = 20;
	public bool willGrow = true;

	[Space]

	[SerializeField]
	private PrefabList m_Prefabs;
	public List<PrefabData> prefabs { get { return m_Prefabs.list; } set { m_Prefabs.list = value; } }

	List<GameObject> pooledObjects;

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	void Start () 
	{
		pooledObjects = new List<GameObject> ();

		foreach (PrefabData pooledObject in prefabs)
		{
			for (int i = 0; i < pooledObject.amount; i++) 
			{
				GameObject obj = Instantiate (pooledObject.prefab) as GameObject;
				obj.transform.SetParent (transform);
				obj.name = pooledObject.prefab.name;
				obj.SetActive (false);
				pooledObjects.Add (obj);
			}
		}
	}

	public GameObject PoolOut (Catalog type)
	{
		GameObject pooledObject = m_Prefabs.GetPrefab (type);

		for (int i = 0; i < pooledObjects.Count; i++) 
		{
			if (!pooledObjects [i].activeInHierarchy && pooledObjects[i].name == pooledObject.name) 
			{
				pooledObjects [i].transform.parent = null;
				return pooledObjects [i];
			}
		}

		if (willGrow) 
		{
			GameObject obj = Instantiate (pooledObject) as GameObject;
			obj.name = pooledObject.name;
			pooledObjects.Add (obj);
			return obj;
		}

		return null;
	}

	public void PoolIn (GameObject pooled)
	{
		pooled.transform.SetParent (transform);
		pooled.SetActive (false);
	}

	public void PoolInAll ()
	{
		foreach (GameObject obj in pooledObjects) 
		{
			obj.transform.SetParent (transform);
			obj.SetActive (false);
		}
	}
}
