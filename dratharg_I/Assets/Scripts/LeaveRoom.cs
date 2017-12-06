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
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Back")) {
			panel.SetActive(!panel.activeSelf);
		}

		if (panel.activeSelf && (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Cancel"))) {
			networkManager.matchMaker.ListMatches(0, 20, "",true, 0, 0, OnMatchList);
		}
	}
	public void OnMatchDestroyed(bool succes, string extendedInfo)
	{
		if (succes) {
			Debug.Log ("Host closed the game!");

		} else {
			Debug.Log ("Client exit!");
		}
		SceneManager.LoadScene (scenesName);
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		foreach (MatchInfoSnapshot match in matchList) {
			MatchInfo matchInfo = networkManager.matchInfo;
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDestroyMatch);
			networkManager.StopHost ();
		}
	}



}
