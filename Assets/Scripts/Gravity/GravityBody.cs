using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./Faux Gravity/Body")]
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour 
{
	public Rigidbody ridgibody { get { return GetComponent<Rigidbody> (); } }

	private GravityAttractor m_Attractor;

	void Start () 
	{
		ridgibody.constraints = RigidbodyConstraints.FreezeRotation;
		ridgibody.useGravity = false;

		m_Attractor = GravityAttractor.instance;
	}

	void OnEnable ()
	{
		ridgibody.constraints = RigidbodyConstraints.None;
		Start ();
	}

	void OnDisable ()
	{
		ridgibody.constraints = RigidbodyConstraints.FreezePosition;
	}

	void Update () 
	{
		if (m_Attractor != null)
			m_Attractor.Attract (transform);
	}
}
