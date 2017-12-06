using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	public Camera playerCamera;

	void Start ()
	{
		if (!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
		}
		else
		{			
			GetComponent<Player>().SetupPlayer();
		}
		
	}
	
	
	public override void OnStartClient()
	{
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player player = GetComponent<Player>();
		
		GameManager.RegisterPlayer(_netID, player);
	}
	

	void AssignRemoteLayer ()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents ()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}
		
	void OnDisable()
	{
		if(isLocalPlayer)
		{
			GameManager.gameManager.SetSceneCameraActive(true);
			Cardboard.SDK.gameObject.SetActive (false);
		}
		
		GameManager.UnRegisterPlayer(transform.name);
	}
}
