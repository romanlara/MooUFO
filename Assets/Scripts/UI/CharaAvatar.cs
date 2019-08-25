using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIBundle;

public class CharaAvatar : MonoBehaviour 
{
	[SerializeField]
	private Image m_ImageAvatar;
	public Image imageAvatar { get { return m_ImageAvatar; } set { m_ImageAvatar = value; } }

	[SerializeField]
	private Image m_Mark;
	public Image mark { get { return m_Mark; } set { m_Mark = value; } }

	[SerializeField]
	private GameObject m_Padlock;
	public GameObject padlock { get { return m_Padlock; } set { m_Padlock = value; } }

	[SerializeField]
	private PlayerType m_Type;
	public PlayerType type { get { return m_Type; } set { m_Type = value; } }

	private int m_Price;
	public int price { get { return m_Price; } set { m_Price = value; } }

	private void SetAction ()
	{
		DestroyImmediate (GameManager.instance.playerObject);

		CharacterSelector selector = CharacterSelector.instance;
		selector.characterSelected.imageAvatar.sprite = imageAvatar.sprite;
		selector.characterSelected.type = type;
		selector.anim.SetTrigger ("Close");
	}

	public void OnClick ()
	{
		if (!padlock.activeInHierarchy) 
		{
			SetAction ();
		} 
		else 
		{
			Dialog.DialogDetails details = new Dialog.DialogDetails () {
				title = "Purching",
				question = "Do you want to buy this character?\nMoocoins: " + price.ToString()
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
			acceptEvent.action.AddListener (() => { OnAccept(); });
			details.buttonDetails.Add (acceptEvent);

			Dialog dialog = Dialog.allDialogs ["Dialog - First"];
			if (dialog != null) 
			{
				dialog.details = details;
				dialog.Open ();
			}
		}
	}

	private void OnAccept ()
	{
		int coinPurse = GameManager.instance.information.coinPurse;

		if (coinPurse >= price)
		{
			foreach (DataBase.Data data in GameManager.instance.information.characters) 
			{
				if (data.prefab.GetComponent<PlayerController> ().type == type)
				{
					data.bought = true;
					break;
				}
			}

			padlock.SetActive (false);
			GameManager.instance.information.coinPurse = coinPurse - price;
			GameManager.instance.SaveLib ();

			SetAction ();
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
