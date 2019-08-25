using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SardonicMe.Perlib;

[AddComponentMenu("Muu-U.F.O./Managers/Game")]
public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	//------------------------------------------------------------//

	[System.Serializable]
	public class SavingSettings
	{
		private static string PATH = Application.persistentDataPath;
		private static char SLASH = Path.DirectorySeparatorChar;

		public const string FILENAME = "savedata.txt";
		public const string PASSWORD = "paid389HDO729jh23";
		public static string path { get { return PATH + SLASH + FILENAME; } }
	}

	//------------------------------------------------------------//

	[SerializeField]
	private GameMode m_GameMode;
	public GameMode gameMode 
	{ 
		get { return m_GameMode; } 
		set 
		{ 
			m_GameMode = value; 

			switch (m_GameMode) 
			{
			case GameMode.Menu:
				mobileSingleStickControl.alpha = 0;
				canvasUI.SetActive (true);

				if (AdsScript.instance != null && AdsScript.instance.isReady)
					AdsScript.instance.ShowOverwatch ();
				break;
			case GameMode.Playing:
				mobileSingleStickControl.alpha = 1;
				canvasUI.SetActive (false);

				if (Spawner.instance) 
					Spawner.instance.GoPlay ();
				break;
			case GameMode.Paused:
				// Nothing yet
				break;
			}
		} 
	}

	[SerializeField]
	private GameObject m_CanvasUI;
	public GameObject canvasUI { get { return m_CanvasUI; } set { m_CanvasUI = value; } }

	[SerializeField]
	private CanvasGroup m_MobileSingleStickControl;
	public CanvasGroup mobileSingleStickControl { get { return m_MobileSingleStickControl; } set { m_MobileSingleStickControl = value; } }

	[SerializeField]
	private Animator m_PauseAnim;
	public Animator pauseAnim { get { return m_PauseAnim; } set { m_PauseAnim = value; } }

	[Space]

	[SerializeField]
	private DataBase m_Information;
	public DataBase information { get { return m_Information; } set { m_Information = value; } }

	private GameObject m_PlayerObject;
	public GameObject playerObject { get { return m_PlayerObject; } set { m_PlayerObject = value; } }

	private bool m_IsPaused;
	public bool isPaused 
	{ 
		get { return m_IsPaused; } 
		set 
		{ 
			m_IsPaused = value;

			if (OnPause != null)
				OnPause (m_IsPaused);
		} 
	}

	public delegate void PauseEvent (bool isPause);
	public PauseEvent OnPause;

	void Awake ()
	{
		if (instance == null)
			instance = this;

		OpenLib ();
	}

	void Start ()
	{
		gameMode = m_GameMode;
	}

	public void Pause ()
	{
		isPaused = !m_IsPaused;

		if (isPaused) 
		{
			gameMode = GameMode.Paused;

			if (pauseAnim != null)
				pauseAnim.SetTrigger ("Open");
		} 
		else
		{
			gameMode = GameMode.Playing;

			if (pauseAnim != null)
				pauseAnim.SetTrigger ("Close");
		}
	}

	public void Play ()
	{
//		if (m_PlayerObject == null)
//			CharacterSelector.instance.CreateCharacterDefault ();

		DestroyImmediate (m_PlayerObject);

		m_PlayerObject = Instantiate (CharacterSelector.instance.GetCharacter()) as GameObject;

		ChangeMode (GameMode.Playing);
	}

	public void ChangeMode (GameMode mode)
	{
		gameMode = mode;
	}

	//---------------------------------------------------------//
	#region SAVING_SETTINGS

	public void OpenLib ()
	{
		Debug.Log (SavingSettings.path.ToString());
		Perlib savefile = new Perlib (SavingSettings.FILENAME, SavingSettings.PASSWORD);
		savefile.Open ();

		if (savefile.HasKey ("information")) 
		{
			DataBase.Extractor savedInformation = savefile.GetValue<DataBase.Extractor> ("information");

			information.coinPurse = savedInformation.coinPurse;

			for (int i = 0; i < information.planets.Count; i++) 
			{
				DataBase.Data planet = information.planets [i];
				planet.bought = savedInformation.planets [i];
			}

			for (int i = 0; i < information.characters.Count; i++) 
			{
				DataBase.Data character = information.characters [i];
				character.bought = savedInformation.characters [i];
			}
		}
	}

	public void SaveLib ()
	{
		Perlib savefile = new Perlib (SavingSettings.FILENAME, SavingSettings.PASSWORD);
		savefile.Open ();

		DataBase.Extractor extractor = new DataBase.Extractor (
			information.coinPurse, 
			information.Extract(information.planets),
			information.Extract(information.characters)
		);

		savefile.SetValue ("information", extractor);
		savefile.Save ();
	}

	#endregion
	//---------------------------------------------------------//
}
