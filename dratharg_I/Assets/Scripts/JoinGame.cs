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
	
	[SerializeField]
	private GameObject roomListItemPrefab;
	
	[SerializeField]
	private Transform roomListParent;

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
	
	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";
		if (!success || matchList ==null)
		{
			status.text = "Couldn't get room list.";
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
			status.text = "No rooms at the moment...";
		}
	}
	
	void ClearRoomList()
	{
		for (int i = 0; i < rooms.Length; i++) {
			rooms [i].SetActive (false);
		}
	}
		
	public void JoinRoom(MatchInfoSnapshot _match)
	{
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "","",0,0, networkManager.OnMatchJoined);
		StartCoroutine(WaitForJoin());
	}
	
	IEnumerator WaitForJoin ()
	{
		ClearRoomList();

		int countdown = 10;
		while (countdown > 0)
		{
			status.text = "JOINING... (" + countdown + ")";

			yield return new WaitForSeconds(1);

			countdown--;
		}

		// Failed to connect
		status.text = "Failed to connect.";
		yield return new WaitForSeconds(1);

		MatchInfo matchInfo = networkManager.matchInfo;
		if (matchInfo != null)
		{
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost();
		}

		RefreshRoomList();

	}
	
}
