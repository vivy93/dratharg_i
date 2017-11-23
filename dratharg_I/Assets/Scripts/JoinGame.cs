﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {
	
	List<GameObject> roomList = new List<GameObject>();
	private NetworkManager networkManager;
	
	[SerializeField]
	private Text status;

	[SerializeField]
	private Button roomButton;
	
	[SerializeField]
	private GameObject roomListItemPrefab;
	
	[SerializeField]
	private Transform roomListParent;
	
	// Use this for initialization
	void Start () {
		roomButton.enabled = false;
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
		RefreshRoomList();
	}
	
	public void RefreshRoomList()
	{
		ClearRoomList ();
		networkManager.matchMaker.ListMatches(0, 20, "",true, 0, 0, OnMatchList);
		status.text = "Loading...";		
	}
	
	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";
		if (!success || matchList ==null)
		{
			status.text = "Couldn't get room list.";
			return;
		}
		//ClearRoomList();
		foreach(MatchInfoSnapshot match in matchList)
		{
			//GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
			//_roomListItemGO.transform.SetParent(roomListParent);
			//_roomListItemGO.SetActive (true);


			
			RoomListItem _roomListItem = roomButton.GetComponent<RoomListItem>();
			_roomListItem.enabled = true;
			roomButton.enabled = true;

			if (_roomListItem != null)
			{
				_roomListItem.Setup(match, JoinRoom);
			}

			//roomList.Add(_roomListItemGO);
		}
		//if (roomList.Count == 0) {
		//	status.text = "No rooms at the moment...";
		//}
	}
	
	void ClearRoomList()
	{
		for(int i =0; i<roomList.Count; i++)
		{
			Destroy(roomList[i]);
		}
		
		roomList.Clear();
		Debug.Log ("ClearRoomList");
	}


	public void JoinRoom(MatchInfoSnapshot _match)
	{
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "","",0,0, networkManager.OnMatchJoined);
		ClearRoomList ();
		status.text = "JOINING...";
	}
	
	
}
