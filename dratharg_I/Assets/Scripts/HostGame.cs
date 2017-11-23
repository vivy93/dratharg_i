using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize = 4;
	private string roomName;
	private NetworkManager _networkManager;
	
	// Use this for initialization
	void Start () {
		_networkManager = NetworkManager.singleton;
		if (_networkManager.matchMaker ==null)
		{
			_networkManager.StartMatchMaker();
		}
	}

	public void SetRoomName(string _name)
	{
		roomName = _name;

	}
	
	public void CreateRoom()
	{
		if(roomName != "" && roomName != null)
		{
			Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
			//Create room
			_networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, _networkManager.OnMatchCreate);
		}
		
	}
}
