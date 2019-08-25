using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialLink : MonoBehaviour 
{
	public enum Social
	{
		Facebook,
		Twitter
	}

	public Social link;

	public void OnClicked ()
	{
		switch (link) 
		{
		case Social.Facebook:
			Application.OpenURL ("https://m.facebook.com/Sliverbroom/");
			break;
		case Social.Twitter:
			Application.OpenURL ("https://twitter.com/Sliverbroomstu");
			break;
		}
	}
}
