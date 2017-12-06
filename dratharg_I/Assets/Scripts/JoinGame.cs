using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {
	
	private NetworkManager networkManager;
	
	[SerializeField]
	private Text status;

	public GameObject[] rooms;
	
	// Use this for initialization
	void Start () {
		rooms = GameObject.FindGameObjectsWithTag ("Room");
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].SetActive (false);
		}
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
		RefreshRoomList();
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";
		if (!success || matchList ==null)
		{
			status.text = "Something went wrong! Check your internet connection.";
			return;
		}

		int i=0;
		foreach(MatchInfoSnapshot match in matchList)
		{
			RoomListItem _roomListItem = rooms[i].GetComponent<RoomListItem>();
			_roomListItem.enabled = true;
			rooms[i].gameObject.SetActive (true);

			if (_roomListItem != null)
			{
				_roomListItem.Setup(match, JoinRoom);
			}
			i++;
		}
		int j = 0;
		for (int k = 0; k < rooms.Length; k++) {
			if (rooms[k].gameObject.activeSelf) {
				j++;
			}
		}
		if (j==0) {
			status.text = "No available rooms...";
		}
	}

	public void JoinRoom(MatchInfoSnapshot matchInfo)
	{
		networkManager.matchMaker.JoinMatch(matchInfo.networkId, "", "","",0 ,0 , networkManager.OnMatchJoined );
		StartCoroutine(WaitingForExistingRoom());
	}
	
	IEnumerator WaitingForExistingRoom ()
	{
		ClearRoomList();

		int countdown = 10;
		while (countdown > 0)
		{
			status.text = "JOINING... (" + countdown + ")";

			yield return new WaitForSeconds(1);

			countdown--;
		}

		status.text = "The room not exist!.";
		yield return new WaitForSeconds(1);

		MatchInfo matchInfo = networkManager.matchInfo;
		if (matchInfo != null)
		{
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost();
		}

		RefreshRoomList();
	}

	public void RefreshRoomList()
	{
		ClearRoomList ();
		//networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
		networkManager.matchMaker.ListMatches(0, 20, "",true, 0, 0, OnMatchList);
		status.text = "Loading...";
	}
	void ClearRoomList()
	{
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].SetActive (false);
		}
	}
}
