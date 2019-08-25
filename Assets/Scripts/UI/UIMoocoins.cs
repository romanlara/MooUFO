using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoocoins : MonoBehaviour 
{
	[SerializeField]
	private Text m_Text;
	public Text text { get { return m_Text; } set { m_Text = value; } }

	void Update () 
	{
		text.text = GameManager.instance.information.coinPurse.ToString ();
	}
}
