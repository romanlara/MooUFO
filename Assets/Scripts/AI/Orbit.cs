using UnityEngine;
using System.Collections;

[AddComponentMenu("Muu-U.F.O./AI/Orbit")]
public class Orbit : MonoBehaviour
{
	[SerializeField]
	private Vector2 m_Distance = new Vector2(10f, 15f);
	public Vector2 distance { get { return m_Distance; } set { m_Distance = value; } }

	[SerializeField]
	private float m_MoveSpeed = 5f;
	public float moveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

	[SerializeField]
	private float m_RotateSpeed = 5f;
	public float rotateSpeed { get { return m_RotateSpeed; } set { m_RotateSpeed = value; } }

	private Vector3 m_TargetPoint;
	private float m_NewDistance;

	void Start ()
	{
		m_TargetPoint = NextPoint ();
	}

	void Update ()
	{
		Vector3 center = PlanetManager.instance.GetPlanet ().prefab.transform.position;
		Vector3 radius = Vector3.up * PlanetManager.instance.GetPlanet ().planet.radius;

		Vector3 sphericalPosition = center + (transform.rotation * new Vector3 (0, radius.y + m_NewDistance, 0));
		transform.position = Vector3.Lerp (transform.position, sphericalPosition, 5 * Time.deltaTime);
		transform.position = Vector3.MoveTowards (transform.position, m_TargetPoint, moveSpeed * Time.deltaTime);

		// Adjust the orientation
		Vector3 gravityUp = (transform.position - center).normalized;
		Vector3 bodyUp = transform.up;

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * transform.rotation;
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 50 * Time.deltaTime);

		// Rotate to the target point.
		Vector3 position = transform.position;

		// Get Distanse between position current and target position to change the target point.
		float angle = Vector3.Angle(position - center, m_TargetPoint - center);
		float arcDistance = 2 * Mathf.PI * radius.y * (angle / 360);

		if (arcDistance <= 1f)
			m_TargetPoint = NextPoint ();
	}

	private Vector3 NextPoint ()
	{
		m_NewDistance = Random.Range (distance.x, distance.y);

		return Random.onUnitSphere * (PlanetManager.instance.GetPlanet().planet.radius + m_NewDistance);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawSphere (m_TargetPoint, 0.5f);
	}
}

