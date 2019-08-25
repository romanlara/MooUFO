using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Muu-U.F.O./Managers/Planet Manager")]
public class PlanetManager : MonoBehaviour
{
	public static PlanetManager instance;

	[System.Serializable]
	public class PlanetData
	{
		[SerializeField]
		private GameObject m_Prefab;
		public GameObject prefab { get { return m_Prefab; } set { m_Prefab = value; } }

		public bool active { get { return m_Prefab.activeInHierarchy; } }

		public PlanetScript planet { get { return m_Prefab.GetComponent<PlanetScript> (); } } 

		public PlanetData () {}
		public PlanetData (GameObject prefab) { this.prefab = prefab; }
	}

	[System.Serializable]
	public class PlanetList
	{
		[SerializeField]
		private List<PlanetData> m_List;
		public List<PlanetData> list { get { return m_List; } set { m_List = value; } }

		public PlanetList () { list = new List<PlanetData>(); }

		public PlanetData GetPlanet ()
		{
			PlanetData planet = null;

			foreach (PlanetData data in list) 
			{
				if (data.active) 
				{
					planet = data;
					break;
				}
			}

			return planet;
		}
	}

	[System.Serializable]
	public class CameraData
	{
		[SerializeField]
		private Camera m_MainCamera;
		public Camera mainCamera { get { return m_MainCamera; } set { m_MainCamera = value; } }

		[SerializeField]
		private Camera m_LoseCamera;
		public Camera loseCamera { get { return m_LoseCamera; } set { m_LoseCamera = value; } }

		public CameraData () {}
	}

	[SerializeField]
	private CameraData m_Cameras;
	public CameraData cameras { get { return m_Cameras; } set { m_Cameras = value; } }

	[Space]

	[SerializeField]
	private float m_SpeedColor;
	public float speedColor { get { return m_SpeedColor; } set { m_SpeedColor = value; } }

	[Space]

	[SerializeField]
	private int m_Value;
	public int value { get { return m_Value; } set { m_Value = Mathf.Clamp (value, 0, planets.Count - 1); } }

	[SerializeField]
	private PlanetList m_Planets;
	public List<PlanetData> planets { get { return m_Planets.list; } set { m_Planets.list = value; } }

	void Awake ()
	{
		if (instance == null)
			instance = this;

		for (int i = 0; i < planets.Count; i++) 
		{
			GameObject prefab = planets [i].prefab;
			GameObject planet = (GameObject) Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
			planet.name = prefab.name;
			planet.SetActive (false);
			planets [i].prefab = planet;
		}

		planets [0].prefab.SetActive (true);
	}

	void Start ()
	{
		PlayAmbienceAudio ();
	}

	public PlanetData GetPlanet ()
	{
		return m_Planets.GetPlanet ();
	}

	void Update ()
	{
		PlanetData planet = m_Planets.GetPlanet ();

		if (planet != null) 
		{
			float speed = speedColor * Time.deltaTime;

			cameras.mainCamera.backgroundColor = Color.Lerp (cameras.mainCamera.backgroundColor, planet.planet.tintColor, speed);
			cameras.loseCamera.backgroundColor = Color.Lerp (cameras.loseCamera.backgroundColor, planet.planet.tintColor, speed);

			SunShafts sunShaftsMC = cameras.mainCamera.GetComponent<SunShafts> ();
			sunShaftsMC.sunThreshold = Color.Lerp (sunShaftsMC.sunThreshold, planet.planet.thresholdColor, speed);
			sunShaftsMC.sunColor = Color.Lerp (sunShaftsMC.sunColor, planet.planet.shaftsColor, speed);

			SunShafts sunShaftsLC = cameras.loseCamera.GetComponent<SunShafts> ();
			sunShaftsLC.sunThreshold = Color.Lerp (sunShaftsLC.sunThreshold, planet.planet.thresholdColor, speed);
			sunShaftsLC.sunColor = Color.Lerp (sunShaftsLC.sunColor, planet.planet.shaftsColor, speed);

//			RenderSettings.ambientSkyColor = Color.Lerp (RenderSettings.ambientSkyColor, planet.planet.ambientColor, speed);
		}
	}

	public void OnForward ()
	{
		planets [value].prefab.SetActive (false);

		value += 1;

		planets [value].prefab.SetActive (true);

		PlayAmbienceAudio ();
	}

	public void OnBackward ()
	{
		planets [value].prefab.SetActive (false);

		value -= 1;

		planets [value].prefab.SetActive (true);

		PlayAmbienceAudio ();
	}

	private void PlayAmbienceAudio ()
	{
		AudioManager.instance.audio.clip = planets [value].planet.ambienceClip;
		AudioManager.instance.audio.Play ();
	}
}

