using UnityEditor;
using UnityEngine;

public class DataBaseAsset : MonoBehaviour 
{
	[MenuItem("Assets/Create/Data Base")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<DataBase>();
	}
}
