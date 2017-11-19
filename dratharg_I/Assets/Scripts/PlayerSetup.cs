using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	public Camera playerCamera;
	// Camera sceneCamera;

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
		}
		// else
		// {
			// // We are the local player: Disable the scene camera
			// sceneCamera = Camera.main;
			// if (sceneCamera != null)
			// {
				// Cardboard.SDK.gameObject.SetActive (true);
				// sceneCamera.gameObject.SetActive(false);
            // }
		// }
		//Game manager handle this function.
		//RegisterPlayer();
		GetComponent<Player>().Setup();
	}
	
	public override void OnStartClient()
	{
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player player = GetComponent<Player>();
		
		GameManager.RegisterPlayer(_netID, player);
	}
	
	//Game manager handle this function.
	/*void RegisterPlayer ()
	{
		string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;
	}*/

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
		GameManager.instance.SetSceneCameraActive(true);
		Cardboard.SDK.gameObject.SetActive (false);
		// if (sceneCamera != null) {
			
			// sceneCamera.gameObject.SetActive (true);
			// Cardboard.SDK.gameObject.SetActive (false);
			// //playerCamera.gameObject.SetActive(false);
		// }
		GameManager.UnRegisterPlayer(transform.name);
	}
}
