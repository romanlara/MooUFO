using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager instance;

	private AudioSource m_Audio;
	public AudioSource audio { get { return m_Audio; } set { m_Audio = value; } }

	void Awake ()
	{
		if (instance == null)
			instance = this;

		m_Audio = GetComponent<AudioSource> ();
	}
}
