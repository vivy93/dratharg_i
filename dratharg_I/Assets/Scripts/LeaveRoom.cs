using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class LeaveRoom : MonoBehaviour {
	GameObject panel;

	[SerializeField]
	private string scenesName;

	private NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
		panel = GameObject.FindGameObjectWithTag("Panel");
		panel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("CIAAAA     " + networkManager.matchMaker);
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Back")) {
			//GameObject _leaveRoom = Instantiate(leaveRoomPrefab, canvas.transform.position, canvas.transform.rotation);
			//_leaveRoom.transform.SetParent (canvas.transform);
			//_leaveRoom.SetActive(_leaveRoom.activeSelf);
			panel.SetActive(!panel.activeSelf);
		}

		if (panel.activeSelf && (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Cancel"))) {
			//networkManager.StopHost();
			networkManager.matchMaker.ListMatches(0, 20, "",true, 0, 0, OnMatchList);
			//MatchInfo matchInfo = networkManager.matchInfo;
			//Debug.Log ("CICA");
			//networkManager.matchMaker.DropConnection(
			//if (matchInfo != null) {
				//networkManager.matchMaker.DestroyMatch (NodeId., 0, networkManager.OnDestroyMatch); //.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDestroyMatch);

			//}
		}
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		foreach (MatchInfoSnapshot match in matchList) {
			//networkManager.matchMaker.DestroyMatch(match.networkId, 0, OnMatchDestroyed);
			MatchInfo matchInfo = networkManager.matchInfo;
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDestroyMatch);
			networkManager.StopHost ();
		}
	}

	public void OnMatchDestroyed(bool succes, string extendedInfo)
	{
		if (succes) {
			Debug.Log ("Host destroyed the game!");

		} else {
			Debug.Log ("Client leave the game!");
		}
		SceneManager.LoadScene (scenesName);
	}

}
