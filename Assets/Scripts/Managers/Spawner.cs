using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./Managers/Spawner")]
public class Spawner : MonoBehaviour 
{
	public static Spawner instance;

	public float meteorDelay = 1f;
	public float ufoDelay = 3f;
	public float moocoinsDelay = 10f;

	[Space]

	public float distance = 10f;

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	public void GoPlay () 
	{
		//Random.Range(0, PoolSystem.instance.prefabs.Count)
		StartCoroutine (Spawn(Catalog.Meteor, 0, meteorDelay));
		StartCoroutine (Spawn(Catalog.UFO, 0, ufoDelay, ufoDelay * 0.5f));
		StartCoroutine (Spawn(Catalog.Moocoin, 10, moocoinsDelay, moocoinsDelay));
	}

	private void SpawnObject (Catalog type)
	{
		Vector3 pos = Random.onUnitSphere * (distance + PlanetManager.instance.GetPlanet().planet.radius);

		GameObject meteor = PoolSystem.instance.PoolOut(type);

		if (meteor != null) 
		{
			meteor.transform.position = pos;
			meteor.transform.rotation = Quaternion.identity;
			meteor.SetActive (true);
		}
	}

	/// <summary>
	/// Spawn the specified type, max, delay and startDelay.
	/// </summary>
	/// <param name="type">Object type.</param>
	/// <param name="max">If max is 0 then will be infinity.</param>
	/// <param name="delay">Delay between spawn.</param>
	/// <param name="startDelay">Start delay.</param>
	IEnumerator Spawn (Catalog type, int max, float delay, float startDelay = 0f) 
	{
		yield return new WaitForSeconds (startDelay);

		SpawnObject (type);

		yield return new WaitForSeconds (delay);

		if (max != 0) 
		{
			int counter = 0;
			while (counter < max - 1) 
			{
				counter++;
				SpawnObject (type);
				yield return new WaitForSeconds (delay);
			}
		}
		else
			StartCoroutine (Spawn(type, max, delay, startDelay));
	}
}
