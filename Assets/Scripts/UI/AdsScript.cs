using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UIBundle;

public class AdsScript : MonoBehaviour 
{
	//--------------------------------------------------//
	#if UNITY_IOS
	privete string gameId = "1618507";
	#elif UNITY_ANDROID
	private string gameId = "1618506";
	#endif
	//--------------------------------------------------//

	public static AdsScript instance;

	[SerializeField]
	private string m_PlacementId = "rewardedVideo";
	public string placementId { get { return m_PlacementId; } set { m_PlacementId = value; } }

	[SerializeField]
	private bool m_TestMode = true;
	public bool testMode { get { return m_TestMode; } set { m_TestMode = value; } }

	public bool isReady { get { return Advertisement.IsReady (placementId); } }

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	void Start () 
	{
		if (Advertisement.isSupported)
			Advertisement.Initialize (gameId, testMode);
	}

	void ShowAd ()
	{
		ShowOptions options = new ShowOptions ();
		options.resultCallback = HandleActionResult;

		Advertisement.Show (placementId, options);
	}

	void HandleActionResult (ShowResult result)
	{
		if (result == ShowResult.Finished) 
		{
			FinishedState ();
		} 
		else if (result == ShowResult.Skipped) 
		{
			SkippedState ();
		} 
		else if (result == ShowResult.Failed) 
		{
			FailedState ();
		}
	}

	private int GetMoocoins ()
	{
		int[] range = new int[] { 30, 40, 50, 60, 70 };

		return range[Random.Range(0, range.Length)];
	}

	public void ShowOverwatch ()
	{
		Dialog.DialogDetails details = new Dialog.DialogDetails () {
			title = "Advertisement",
			question = "You prize is ready!!\nDo you want to see this video?"
		};

		Dialog.EventButtonDetails cancelEvent = new Dialog.EventButtonDetails () {
			setAsSelectedGameObject = true,
			title = "No",
			colors = new ColorBlock() {
				colorMultiplier = 1,
				fadeDuration = 0.1f,
				normalColor = new Color(1f, 0f, 0.435f, 0.5f),
				highlightedColor = new Color(1f, 0f, 0.435f, 0.5f),
				pressedColor = new Color(1f, 0f, 0.435f, 0.8f),
				disabledColor = Color.white
			},
			action = new Button.ButtonClickedEvent()
		};
		cancelEvent.action.AddListener (() => {  });
		details.buttonDetails.Add (cancelEvent);

		Dialog.EventButtonDetails acceptEvent = new Dialog.EventButtonDetails () {
			setAsSelectedGameObject = false,
			title = "Yes",
			colors = new ColorBlock() {
				colorMultiplier = 1,
				fadeDuration = 0.1f,
				normalColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				highlightedColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				pressedColor = new Color(0f, 0.8f, 0.8f, 0.8f),
				disabledColor = Color.white
			},
			action = new Button.ButtonClickedEvent()
		};
		acceptEvent.action.AddListener (() => { ShowAd(); });
		details.buttonDetails.Add (acceptEvent);

		Dialog dialog = Dialog.allDialogs ["Dialog - First"];
		if (dialog != null) 
		{
			dialog.details = details;
			dialog.Open ();
		}
	}

	private void FinishedState ()
	{
		int moocoinsWon = GetMoocoins();

		Dialog.DialogDetails details = new Dialog.DialogDetails () {
			title = "",
			question = "You won " + moocoinsWon + " Moocoins."
		};

		Dialog.EventButtonDetails acceptEvent = new Dialog.EventButtonDetails () {
			setAsSelectedGameObject = true,
			title = "Ok",
			colors = new ColorBlock() {
				colorMultiplier = 1,
				fadeDuration = 0.1f,
				normalColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				highlightedColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				pressedColor = new Color(0f, 0.8f, 0.8f, 0.8f),
				disabledColor = Color.white
			},
			action = new Button.ButtonClickedEvent()
		};
		acceptEvent.action.AddListener (() => { 
			GameManager.instance.information.coinPurse += moocoinsWon; 
			GameManager.instance.SaveLib ();
		});
		details.buttonDetails.Add (acceptEvent);

		Dialog dialog = Dialog.allDialogs ["Dialog - Second"];
		if (dialog != null) 
		{
			dialog.details = details;
			dialog.Open ();
		}
	}

	private void SkippedState ()
	{
		Dialog.DialogDetails details = new Dialog.DialogDetails () {
			title = "",
			question = "Oh! I'm sorry, you lost your prize."
		};

		Dialog.EventButtonDetails acceptEvent = new Dialog.EventButtonDetails () {
			setAsSelectedGameObject = true,
			title = "Ok",
			colors = new ColorBlock() {
				colorMultiplier = 1,
				fadeDuration = 0.1f,
				normalColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				highlightedColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				pressedColor = new Color(0f, 0.8f, 0.8f, 0.8f),
				disabledColor = Color.white
			},
			action = new Button.ButtonClickedEvent()
		};
		acceptEvent.action.AddListener (() => {  });
		details.buttonDetails.Add (acceptEvent);

		Dialog dialog = Dialog.allDialogs ["Dialog - Second"];
		if (dialog != null) 
		{
			dialog.details = details;
			dialog.Open ();
		}
	}

	private void FailedState ()
	{
		int moocoinsWon = GetMoocoins();

		Dialog.DialogDetails details = new Dialog.DialogDetails () {
			title = "",
			question = "Oops! A problem occurred.\nBut, don't worry here you have your prize:\n\nYou won " + moocoinsWon + " Moocoins."
		};

		Dialog.EventButtonDetails acceptEvent = new Dialog.EventButtonDetails () {
			setAsSelectedGameObject = true,
			title = "Ok",
			colors = new ColorBlock() {
				colorMultiplier = 1,
				fadeDuration = 0.1f,
				normalColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				highlightedColor = new Color(0f, 0.8f, 0.8f, 0.5f),
				pressedColor = new Color(0f, 0.8f, 0.8f, 0.8f),
				disabledColor = Color.white
			},
			action = new Button.ButtonClickedEvent()
		};
		acceptEvent.action.AddListener (() => { 
			GameManager.instance.information.coinPurse += moocoinsWon; 
			GameManager.instance.SaveLib ();
		});
		details.buttonDetails.Add (acceptEvent);

		Dialog dialog = Dialog.allDialogs ["Dialog - Second"];
		if (dialog != null) 
		{
			dialog.details = details;
			dialog.Open ();
		}
	}
}
