using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Muu-U.F.O./AI/Moocoin")]
public class MoocoinScript : MonoBehaviour 
{
	[SerializeField]
	private float m_DestroyTime = 5f;
	public float destroyTime { get { return m_DestroyTime; } set { m_DestroyTime = value; } }

	[SerializeField]
	private AudioClip m_Clip;
	public AudioClip clip { get { return m_Clip; } set { m_Clip = value; } }

	void Start ()
	{
		StartCoroutine (DestroyDelay (destroyTime));
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.rigidbody) 
		{
			if (collision.gameObject.tag == "Player") 
			{
				GameObject sprite = PoolSystem.instance.PoolOut (Catalog.Sprite);
				sprite.transform.position = transform.position;
				sprite.SetActive (true);

				GameManager.instance.information.coinPurse += 1;
				PoolSystem.instance.PoolIn (gameObject);
				AudioSource.PlayClipAtPoint (clip, transform.position);
			}
		}
	}

	private IEnumerator DestroyDelay (float seconds)
	{
		yield return new WaitForSeconds (seconds);
		PoolSystem.instance.PoolIn (gameObject);
	}
}
