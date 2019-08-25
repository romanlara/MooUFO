using UnityEngine;
using System.Collections;

[AddComponentMenu("Muu-U.F.O./AI/U.F.O. Behaviour")]
public class UFOBehaviour : AIController
{
//	[SerializeField]
//	private float m_FloatForce = 13f;
//	public float floatForce { get { return m_FloatForce; } set { m_FloatForce = value; } }

	public override void FixedUpdate ()
	{
		base.FixedUpdate ();

		Transform planet = PlanetManager.instance.GetPlanet ().prefab.transform;
		Vector3 radius = Vector3.up * PlanetManager.instance.GetPlanet ().planet.radius;
		Vector3 surface = planet.position + (transform.rotation * radius);

		Debug.DrawLine (transform.position, surface, Color.red);

//		if (Vector3.Distance (transform.position, surface) < 10f)
//			ridgibody.velocity = (transform.up * floatForce);
	}
}

