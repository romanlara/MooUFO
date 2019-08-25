using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./AI/AI Controller")]
//[RequireComponent(typeof(Rigidbody))]
public class AIController : MonoBehaviour 
{
	[SerializeField]
	private float m_Distance = 5f;
	public float distance { get { return m_Distance; } set { m_Distance = value; } }

	[SerializeField]
	private float m_MoveSpeed = 15f;
	public float moveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

	[Space]

	[SerializeField]
	private AudioClip m_CatchClip;
	public AudioClip catchClip { get { return m_CatchClip; } set { m_CatchClip = value; } }

	public Rigidbody ridgibody { get { return GetComponent<Rigidbody> (); } }

	private bool m_Abducing;
	public bool abducing { get { return m_Abducing; } set { m_Abducing = value; } }

	private Vector3 m_TargetPoint;
	private float m_RotateSpeed = 15f;

	public virtual void Start () 
	{
		m_TargetPoint = NextPoint ();
	}

	public virtual void Update ()
	{
		if (GameManager.instance.isPaused)
			return;
		
		if (LoseKinematic.instance.isPlaying)
			return;
		
//		transform.position = Vector3.MoveTowards(transform.position, m_TargetPoint, moveSpeed * Time.deltaTime);
		Vector3 center = PlanetManager.instance.GetPlanet ().prefab.transform.position;
		Vector3 radius = Vector3.up * PlanetManager.instance.GetPlanet ().planet.radius;

		Vector3 sphericalPosition = center + (transform.rotation * new Vector3 (0, radius.y + distance, 0));
		transform.position = Vector3.Lerp (transform.position, sphericalPosition, 5 * Time.deltaTime);
		transform.position = Vector3.MoveTowards (transform.position, m_TargetPoint, moveSpeed * Time.deltaTime);

		Vector3 gravityUp = (transform.position - center).normalized;
		Vector3 bodyUp = transform.up;

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * transform.rotation;
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 50 * Time.deltaTime);

		if (abducing) 
		{
			m_TargetPoint = GameObject.FindGameObjectWithTag ("Player").transform.position;
//			moveSpeed = 8f;
			return;
		}

		// Rotate to the target point.
		Vector3 position = transform.position;//center + (transform.rotation * radius);
//		Vector3 targetDir = m_TargetPoint - position;
//		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, m_RotateSpeed * Time.deltaTime, 0f);
//
//		Debug.DrawRay (position, newDir * 2, Color.red);
//		transform.rotation = Quaternion.LookRotation (newDir, transform.up);

		// Get Distanse between position current and target position to change the target point.
		float angle = Vector3.Angle(position - center, m_TargetPoint - center);
		float arcDistance = 2 * Mathf.PI * radius.y * (angle / 360);

		if (arcDistance <= 1f)
			m_TargetPoint = NextPoint ();
	}

	public virtual void FixedUpdate ()
	{
		if (GameManager.instance.isPaused)
			return;
		
		// Movement
//		Vector3 center = PlanetManager.instance.GetPlanet ().prefab.transform.position;
//		Vector3 sphericalPosition = center + (transform.rotation * new Vector3(0, 15f, 0));
//		transform.position = Vector3.MoveTowards(sphericalPosition, m_TargetPoint, moveSpeed * Time.deltaTime);

//		ridgibody.MovePosition (Vector3.MoveTowards(sphericalPosition, m_TargetPoint, moveSpeed * Time.deltaTime));

//		Vector3 sphericalPosition = center + (ridgibody.rotation * new Vector3(0, 15f, 0));
//		Vector3 direction = m_TargetPoint - ridgibody.position;
//		ridgibody.MovePosition (sphericalPosition + direction * moveSpeed * Time.deltaTime);

//		Vector3 movement = Vector3.MoveTowards (ridgibody.position, m_TargetPoint, moveSpeed * Time.deltaTime);
//		Vector3 center = PlanetManager.instance.GetPlanet ().prefab.transform.position;
//		Vector3 offset = movement - center;
//		ridgibody.MovePosition (center + Vector3.ClampMagnitude(offset, PlanetManager.instance.GetPlanet().planet.radius + 20));
	}

	private Vector3 NextPoint ()
	{
		return Random.onUnitSphere * (PlanetManager.instance.GetPlanet().planet.radius + distance);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (m_TargetPoint, 0.5f);
	}
}
