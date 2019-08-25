using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[AddComponentMenu("Muu-U.F.O./AI/Meteor")]
public class MeteorScript : MonoBehaviour 
{
	[SerializeField]
	private AudioClip m_Clip;
	public AudioClip clip { get { return m_Clip; } set { m_Clip = value; } }

	[SerializeField]
	private AudioClip m_FireClip;
	public AudioClip fireClip { get { return m_FireClip; } set { m_FireClip = value; } }

	void OnCollisionEnter (Collision collision)
	{
		if (collision.rigidbody) 
		{
			if (collision.gameObject.tag == "Player") 
			{
				PlayerController player = collision.gameObject.GetComponent<PlayerController> ();
				player.AddSpeed ();

				if (!player.audio.isPlaying) 
				{
					player.audio.clip = player.cryClip;
					player.audio.Play ();
				}
				AudioSource.PlayClipAtPoint (fireClip, transform.position);
			}
		}

//		if (collision.gameObject.tag == "Planet") 
//		{
		GameObject explosion = PoolSystem.instance.PoolOut (Catalog.Explosion);
		if (explosion != null) 
		{
			explosion.transform.position = transform.position;
			explosion.transform.rotation = transform.rotation;
			explosion.SetActive (true);

			AudioSource.PlayClipAtPoint (clip, transform.position);
		}
		StartCoroutine (DestroyDelay ());
//		}
	}

	private IEnumerator DestroyDelay ()
	{
		yield return new WaitForSeconds (1.5f);

		PoolSystem.instance.PoolIn (gameObject);
	}
}
