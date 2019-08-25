using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./Conventional/InfiniRotate")]
public class InfiniRotate : MonoBehaviour 
{
	public enum pivot { X, Y, Z }

	[SerializeField]
	private float m_Speed = 20f;
	public float speed { get { return m_Speed; } set { m_Speed = value; } }

	[SerializeField]
	private pivot m_Axis;
	public Vector3 axis 
	{ 
		get 
		{ 
			switch (m_Axis) 
			{
			default:
			case pivot.Y: return Vector3.up;
			case pivot.X: return Vector3.right;
			case pivot.Z: return Vector3.forward;
			} 
		} 
	}

	[SerializeField]
	public bool m_IsRandom;
	private bool isRandom { get { return m_IsRandom; } set { m_IsRandom = value; } }

	private float m_duration = 2f;
	private float m_leftTime;
	private bool m_Switch;

	void Update () 
	{
		if (isRandom) 
		{
			if (m_leftTime >= m_duration)
			{
				m_Switch = !m_Switch;
				speed = (m_Switch) ? -speed : Mathf.Abs (speed);

				m_duration = Random.Range (5, 10);
				m_leftTime = 0f;
			}

			m_leftTime += Time.deltaTime;
		}

		transform.Rotate (axis * speed * Time.deltaTime);
	}
}
