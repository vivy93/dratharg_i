using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize = 4;
	private string room;
	private NetworkManager netManager;
	
	// Use this for initialization
	void Start () {
		netManager = NetworkManager.singleton;
		if (netManager.matchMaker ==null)
		{
			netManager.StartMatchMaker();
		}
	}
	public void CreateRoom()
	{
		if(room != "" && room != null)
		{
			Debug.Log("Room name: " + room + ", Player number: " + roomSize);
			netManager.matchMaker.CreateMatch(room, roomSize, true, "", "", "", 0, 0, netManager.OnMatchCreate);
		}

	}
	public void SetRoomName(string _name)
	{
		room = _name;

	}
	

}
