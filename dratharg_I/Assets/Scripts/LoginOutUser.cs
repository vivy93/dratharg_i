using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginOutUser : MonoBehaviour {
	public Text usernameText;
	
	// Use this for initialization
	void Start () {
		Debug.Log(LoadMenu.IsLoggedIn);
		if (LoadMenu.IsLoggedIn)
		{
			usernameText.text="Logged in as "+LoadMenu.playerUsername;
		}
	}
	
	public void Logout()
	{
		if (LoadMenu.IsLoggedIn)
		{
			LoadMenu.Instance.LoggedIn_LogoutButtonPressed();
		}	
	}
}
