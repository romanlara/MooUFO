using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./Faux Gravity/Attractor")]
public class GravityAttractor : MonoBehaviour 
{
	public static GravityAttractor instance;

	[SerializeField]
	private float m_Gravity = -10f;
	public float gravity { get { return m_Gravity; } set { m_Gravity = value; } }

	private const float SPEED = 50f;

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	public void Attract (Transform body)
	{
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 bodyUp = body.up;

		Rigidbody rigidbody = body.GetComponent<Rigidbody> ();
		rigidbody.AddForce (gravityUp * gravity);

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * body.rotation;
		body.rotation = Quaternion.Slerp (body.rotation, targetRotation, SPEED * Time.deltaTime);
	}
}
