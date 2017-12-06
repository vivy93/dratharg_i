using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public static GameManager gameManager;

	public MatchSetting matchSettings;
	
	[SerializeField]
	private GameObject s_cam;

	private const string PLAYER = "Player ";

	private static Dictionary<string, Player> playerList = new Dictionary<string, Player>();


	void Awake ()
	{
		if (gameManager != null)
		{
			Debug.LogError("Warning! Please use just one GameManager.");
		} else
		{
			gameManager = this;
		}
	}
	
	public void SetSceneCameraActive(bool isActive)
	{
		if(s_cam==null) return;
		
		s_cam.SetActive(isActive);
		
	}
		

	public static void UnRegisterPlayer (string _pID)
	{
		playerList.Remove(_pID);
	}

    public static void RegisterPlayer (string _net, Player player)
    {
		string playerID = PLAYER + _net;
        playerList.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static Player GetPlayer (string _pID)
    {
        return playerList[_pID];
    }

}
