using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[AddComponentMenu("Muu-U.F.O./Player/Player Controller")]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour 
{
	[SerializeField]
	private PlayerType m_Type;
	public PlayerType type { get { return m_Type; } set { m_Type = value; } }

	[Space]
	
	[SerializeField]
	private float m_MoveSpeed = 15f;
	public float moveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

	[SerializeField]
	private float m_BornSpeed = 10f;
	public float bornSpeed { get { return m_BornSpeed; } set { m_BornSpeed = value; } }

	[SerializeField]
	private float m_JumpForce = 220f;
	public float jumpForce { get { return m_JumpForce; } set { m_JumpForce = value; } }

	[SerializeField]
	private float m_MouseSensitivity = 250f;
	public float mouseSensitivity { get { return m_MouseSensitivity; } set { m_MouseSensitivity = value; } }

	[SerializeField]
	private LayerMask m_GroundMask;
	public LayerMask groundMask { get { return m_GroundMask; } set { m_GroundMask = value; } }

	[Space]

	[SerializeField]
	private ParticleSystem m_Fire;
	public ParticleSystem fire { get { return m_Fire; } set { m_Fire = value; } }

	[SerializeField]
	private ParticleSystem m_Dust;
	public ParticleSystem dust { get { return m_Dust; } set { m_Dust = value; } }

	[SerializeField]
	private Animator m_Anim;
	public Animator anim { get { return m_Anim; } set { m_Anim = value; } }

	[Space]

	[SerializeField]
	private AudioClip m_CryClip;
	public AudioClip cryClip { get { return m_CryClip; } set { m_CryClip = value; } }

	[SerializeField]
	private AudioClip m_StepsClip;
	public AudioClip stepsClip { get { return m_StepsClip; } set { m_StepsClip = value; } }

	[SerializeField]
	private AudioClip m_JumpClip;
	public AudioClip jumpClip { get { return m_JumpClip; } set { m_JumpClip = value; } }

	private Vector3 m_MoveDir;
	private Vector3 m_StartDir = new Vector3 (0, 0.5f, 0);
	private Vector3 m_EndDir = new Vector3 (0, -0.3f, 0);

	public Rigidbody ridgibody { get { return GetComponent<Rigidbody> (); } }
	public AudioSource audio { get { return GetComponent<AudioSource> (); } }

	private bool m_Playable;
	public bool playable { get { return m_Playable; } set { m_Playable = value; } }

	private float m_AddedSpeed;
	private float m_StepsTimeLeft;

//	private bool m_IsGrounded;
//	public bool isGrounded { get { return m_IsGrounded; } set { m_IsGrounded = value; } }

	void Start ()
	{
		playable = true;
		GetComponent<GravityBody> ().enabled = true;
		transform.position = PlanetManager.instance.GetPlanet ().planet.spawnPoint.position;
	}

	void Update () 
	{
		if (GameManager.instance.isPaused)
			return;

		if (LoseKinematic.instance.isPlaying) 
		{
			GetComponent<GravityBody> ().enabled = false;
		}

		if (!playable)
			return;

		float verticalAxis = (m_AddedSpeed > 0) ? 1 : (Input.GetAxisRaw ("Vertical") + CrossPlatformInputManager.GetAxisRaw ("Vertical") );
		float horizontalAxis = Input.GetAxis ("Horizontal") + CrossPlatformInputManager.GetAxis ("Horizontal");

		// Movement
		m_MoveDir = new Vector3 (0, 0, verticalAxis).normalized;
		anim.SetFloat("Movement", Mathf.Abs(verticalAxis));

		// Rotate
		transform.Rotate (Vector3.up * horizontalAxis * Time.deltaTime * mouseSensitivity);

		// Jump
		Vector3 start = transform.position + (transform.rotation * m_StartDir);
		Vector3 end   = transform.position + (transform.rotation * m_EndDir);
		bool jump = Input.GetButtonDown ("Jump") || CrossPlatformInputManager.GetButtonDown ("Jump");
		bool isGrounded = Physics.Linecast (start, end, groundMask);

		if (jump && isGrounded) 
		{
			anim.SetTrigger ("Jump");
			ridgibody.AddForce(transform.up * jumpForce);
			audio.PlayOneShot (jumpClip);
		}

//		m_IsGrounded = false;
//		Ray ray = new Ray (transform.position, -transform.up);
//		RaycastHit hit;
//
//		if (Physics.Raycast (ray, out hit, 1 + .1f, groundMask)) 
//		{
//			m_IsGrounded = true;
//		}

		// Dust effect
		if (verticalAxis > 0.1f || verticalAxis < -0.1f) 
		{
			if (!dust.isPlaying) 
			{
				dust.Play ();
			}

			// Step Sounds
			if (m_StepsTimeLeft >= (m_AddedSpeed > 0 ? 0.1f : 0.2f) && isGrounded) 
			{
				AudioSource.PlayClipAtPoint (stepsClip, transform.position, 0.5f);
				m_StepsTimeLeft = 0;
			}
			m_StepsTimeLeft += Time.deltaTime;
		} 
		else 
		{
			if (!dust.isStopped) 
			{
				dust.Stop ();
			}
		}
	}

	void FixedUpdate ()
	{
		if (GameManager.instance.isPaused)
			return;
		
		if (!playable)
			return;
		
		// Movement
		ridgibody.MovePosition (ridgibody.position + transform.TransformDirection(m_MoveDir) * (moveSpeed + m_AddedSpeed) * Time.deltaTime);
	}

	void OnDrawGizmos ()
	{
		Vector3 start = transform.position + (transform.rotation * m_StartDir);
		Vector3 end   = transform.position + (transform.rotation * m_EndDir);

		Debug.DrawLine (start, end);
	}

	public void AddSpeed ()
	{
		m_AddedSpeed = bornSpeed;

		fire.Play ();

		StartCoroutine (AddSpeedDelay());
	}

	private IEnumerator AddSpeedDelay ()
	{
		yield return new WaitForSeconds (4f);

		m_AddedSpeed = 0f;

		fire.Stop ();
	}
}
