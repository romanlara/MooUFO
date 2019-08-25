using UnityEngine;
using System.Collections;

[AddComponentMenu("Muu-U.F.O./AI/Planet")]
public class PlanetScript : MonoBehaviour
{
	[SerializeField]
	[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
	private Color m_AmbientColor;
	public Color ambientColor { get { return m_AmbientColor; } set { m_AmbientColor = value; } }

	[SerializeField]
	private Color m_TintColor;
	public Color tintColor { get { return m_TintColor; } set { m_TintColor = value; } }

	[SerializeField]
	private Color m_ThresholdColor;
	public Color thresholdColor { get { return m_ThresholdColor; } set { m_ThresholdColor = value; } }

	[SerializeField]
	private Color m_ShaftsColor;
	public Color shaftsColor { get { return m_ShaftsColor; } set { m_ShaftsColor = value; } }

	[Space]

	[SerializeField]
	private Transform m_SpawnPoint;
	public Transform spawnPoint { get { return m_SpawnPoint; } set { m_SpawnPoint = value; } }

	[Space]

	[SerializeField]
	private AudioClip m_AmbienceClip;
	public AudioClip ambienceClip { get { return m_AmbienceClip; } set { m_AmbienceClip = value; } }

	public float radius { get { return transform.localScale.x /* 0.5f*/; } }
}

