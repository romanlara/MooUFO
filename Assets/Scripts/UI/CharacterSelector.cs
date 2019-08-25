using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour 
{
	public static CharacterSelector instance;

	[SerializeField]
	private RectTransform m_AvatarTemplate;
	public RectTransform avatarTemplate { get { return m_AvatarTemplate; } set { m_AvatarTemplate = value; } }

	[SerializeField]
	private CharaAvatar m_CharacterSelected;
	public CharaAvatar characterSelected { get { return m_CharacterSelected; } set { m_CharacterSelected = value; } }

	[Space]

	[SerializeField]
	private Animator m_Anim;
	public Animator anim { get { return m_Anim; } set { m_Anim = value; } }

	void Awake ()
	{
		if (instance == null)
			instance = this;
	}

	void Start ()
	{
		foreach (DataBase.Data data in GameManager.instance.information.characters) 
		{
			GameObject avatar = CreateAvatar(m_AvatarTemplate.gameObject);
			avatar.SetActive(true);

			RectTransform avatarRectTransform = avatar.transform as RectTransform;
			avatarRectTransform.SetParent(m_AvatarTemplate.parent, false);

			CharaAvatar chara = avatar.GetComponent<CharaAvatar> ();
			chara.type = data.prefab.GetComponent<PlayerController> ().type;
			chara.imageAvatar.sprite = data.sprite;
			chara.price = data.price;
			chara.padlock.SetActive (!data.bought);
		}
	}

	private GameObject CreateAvatar (GameObject template)
	{
		return (GameObject) Instantiate(template);
	}

	public GameObject GetCharacter ()
	{
		GameObject chara = null;

		foreach (DataBase.Data data in GameManager.instance.information.characters) 
		{
			if (data.prefab.GetComponent<PlayerController>().type == characterSelected.type) 
			{
				chara = data.prefab;
				break;
			}
		}

		return chara;
	}
}
