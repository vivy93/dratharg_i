using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {
	public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

	public JoinRoomDelegate joinRoomCallback;

	[SerializeField]
	private Button roomButton;

	[SerializeField]
	private Text roomNameText;

	private MatchInfoSnapshot match;

	// Use this for initialization
	public void Setup (MatchInfoSnapshot _match,  JoinRoomDelegate _joinRoomCallback) {
		
		match = _match;
		joinRoomCallback =_joinRoomCallback;
		//roomNameText.text = match.name + " (" + match.currentSize + " )";
		roomButton.GetComponentInChildren<Text>().text = match.name + " (" + match.currentSize + " )";
		Debug.Log ("RoomListItem////Setup");
	}
	
	public void JoinGameMethod () {
		Debug.Log ("JoinRoom");
		joinRoomCallback.Invoke(match);
	}
	public void Test () {
		roomButton.enabled = true;
		this.GetComponentInChildren<Text>().text = "CSŐŐ";
	}

}
