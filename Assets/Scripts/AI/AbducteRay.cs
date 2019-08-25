using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbducteRay : MonoBehaviour 
{
	public void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			PlayerController player = other.GetComponent<PlayerController> ();
			player.playable = false;

			AIController ai = transform.parent.GetComponent<AIController> ();
			ai.abducing = true;

			LoseKinematic.instance.audio.clip = ai.catchClip;
			LoseKinematic.instance.audio.Play ();

			StartCoroutine (PlayLoseKinematic ());
		}
	}

	public IEnumerator PlayLoseKinematic ()
	{
		yield return new WaitForSeconds (0.5f);

		LoseKinematic.instance.Play ();
	}
}
