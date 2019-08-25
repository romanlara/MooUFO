using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIBundle;

public class PlayScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_PlayImage;
	public GameObject playImage { get { return m_PlayImage; } set { m_PlayImage = value; } }

	[SerializeField]
	private GameObject m_PadlockImage;
	public GameObject padlockImage { get { return m_PadlockImage; } set { m_PadlockImage = value; } }

	void Start ()
	{
		OnChangeValue ();
	}

	public void OnChangeValue ()
	{
		int index = PlanetManager.instance.value;
		List<DataBase.Data> planets = GameManager.instance.information.planets;
		bool activation = planets [index].bought;

		playImage.SetActive (activation);
		padlockImage.SetActive (!activation);
	}

	public void OnClick ()
	{
		int index = PlanetManager.instance.value;
		List<DataBase.Data> planets = GameManager.instance.information.planets;
		bool activation = planets [index].bought;

		if (activation) 
		{
			GameManager.instance.Play ();
		} 
		else 
		{
			Dialog.DialogDetails details = new Dialog.DialogDetails () {
				title = "Purching",
				question = "Do you want to buy this planet?\nMoocoins: " + planets [index].price.ToString()
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
			acceptEvent.action.AddListener (() => { OnAccept(planets [index]); });
			details.buttonDetails.Add (acceptEvent);

			Dialog dialog = Dialog.allDialogs ["Dialog - First"];
			if (dialog != null) 
			{
				dialog.details = details;
				dialog.Open ();
			}
		}
	}

	private void OnAccept (DataBase.Data planet)
	{
		int coinPurse = GameManager.instance.information.coinPurse;

		if (coinPurse >= planet.price)
		{
			foreach (DataBase.Data data in GameManager.instance.information.planets) 
			{
				if (data.prefab.name == planet.prefab.name)
				{
					data.bought = true;
					break;
				}
			}

			GameManager.instance.information.coinPurse = coinPurse - planet.price;
			GameManager.instance.SaveLib ();

			OnChangeValue ();
		}
		else
		{
			Dialog.DialogDetails details = new Dialog.DialogDetails () {
				title = "",
				question = "You don't have enough Moocoins."
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
	}
}
