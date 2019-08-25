using UnityEngine;
using System.Collections;

[AddComponentMenu("Muu-U.F.O./Player/Camera")]
public class CameraController : MonoBehaviour
{
	public static CameraController instance;

	public float damping = 2f; //Damping speed of the camera 'level complete' movement

	public GameObject camera { get { return transform.GetChild (0).gameObject; } }

//	private GameObject m_Target;
	private Transform m_LookAt;

	private Vector3 m_PlayPos = new Vector3 (0f, 13f, -11f);
	private Quaternion m_PlayRot = Quaternion.Euler (55f, 0f, 0f);

	//---------- Pos & Rot to characters screenshot
	// Pos =  0f, 2.34f, -3.2f
	// Rot = 30f, 0f,     0f
	//----------

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	void Start ()
	{
//		m_Target = GameObject.FindGameObjectWithTag("Player");
	}

	void Update ()
	{
		if (LoseKinematic.instance.isPlaying)
			return;

		Vector3 m_TargetPos = Vector3.zero;

		switch (GameManager.instance.gameMode) 
		{
		case GameMode.Menu:
			m_LookAt = PlanetManager.instance.GetPlanet ().prefab.transform;

			Vector3 center = PlanetManager.instance.GetPlanet ().prefab.transform.position;
			Vector3 radius = Vector3.up * PlanetManager.instance.GetPlanet ().planet.radius;

			m_TargetPos = center + (transform.rotation * new Vector3 (0, radius.y + 10f, 0));
			break;
		case GameMode.Playing:
			m_LookAt = GameObject.FindGameObjectWithTag ("Player").transform;
			m_TargetPos = m_LookAt.position;
			break;
		case GameMode.Paused:
			// Nothing yet.
			break;
		}

		// Movement
		transform.position = Vector3.Slerp(transform.position, m_TargetPos, damping * Time.deltaTime);

		// Rotation
		Vector3 gravityUp = (m_TargetPos - transform.position).normalized;
		Vector3 bodyUp = m_LookAt.up;

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * m_LookAt.rotation;
		transform.rotation = Quaternion.Slerp (m_LookAt.rotation, targetRotation, 0f);
	}
}

