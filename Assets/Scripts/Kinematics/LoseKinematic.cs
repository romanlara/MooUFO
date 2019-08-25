using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.ImageEffects;

public class LoseKinematic : MonoBehaviour 
{
	public static LoseKinematic instance;

	[SerializeField]
	private Transform m_PlayerActor;
	public Transform playerActor { get { return m_PlayerActor; } set { m_PlayerActor = value; } }

	[SerializeField]
	private AudioClip m_Clip;
	public AudioClip clip { get { return m_Clip; } set { m_Clip = value; } }

	public Animator anim { get { return GetComponent<Animator> (); } }
	public AudioSource audio { get { return GetComponent<AudioSource> (); } }

	private bool m_IsPlaying;
	public bool isPlaying { get { return m_IsPlaying; } set { m_IsPlaying = value; } }

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	public void Play () 
	{
		GameManager.instance.SaveLib ();

		// Ambient Color
//		PlanetManager.PlanetData planet = PlanetManager.instance.GetPlanet ();
//		SunShafts sunShaftsLC = PlanetManager.instance.cameras.mainCamera.GetComponent<SunShafts> ();
//		sunShaftsLC.sunThreshold = planet.planet.thresholdColor;
//		sunShaftsLC.sunColor = planet.planet.shaftsColor;

		// To locate the kinematic on the actived planet.
		Vector3 platetPos = PlanetManager.instance.GetPlanet ().prefab.transform.position;
		Vector3 radius = Vector3.up * PlanetManager.instance.GetPlanet ().planet.radius;

		transform.position = platetPos + radius; // The kinematic is located.

		// Starts the animation.
		m_IsPlaying = true;
		anim.SetTrigger ("PlayLose");
		StartCoroutine (PlayAudioAbducte());

		// The player is located in the actor point.
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.transform.SetParent (playerActor);
		player.transform.position = playerActor.position;
		player.transform.rotation = playerActor.rotation;

		// The spawner and the poolSystem are stoped.
		Spawner.instance.StopAllCoroutines ();
		PoolSystem.instance.PoolInAll ();
	}

	private IEnumerator PlayAudioAbducte ()
	{
		yield return new WaitForSeconds (0.4f);
		audio.PlayOneShot (clip);
		// Main Camera
		CameraController.instance.camera.SetActive(false);
	}
}
