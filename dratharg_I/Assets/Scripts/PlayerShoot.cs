using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER = "Player";
	private int counter=1;

	private PlayerWeapon currentWeapon;
	private WeaponManager weaponManager;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	GameObject panel;

	void Start ()
	{
		if (cam == null)
		{
			Debug.LogError("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}
		weaponManager = GetComponent<WeaponManager>();
	}

	void Update ()
	{
		currentWeapon = weaponManager.GetCurrentWeapon();

		panel = GameObject.FindGameObjectWithTag("Panel");
		if (panel!=null) {
			if (panel.activeSelf)
				return;
		}
		
		if(Input.GetButtonDown("ChangeWeapon") )
		{
			counter++;
			if(counter%2==0)
			{
				currentWeapon.fireRate = 10f;
				currentWeapon.damage = 3;		
			}
			else
			{
				currentWeapon.fireRate = 0f;
				currentWeapon.damage = 10;
				
			}

		}
		
		if (currentWeapon.fireRate <= 0f)
  		{
			if (Input.GetButtonDown("Fire1"))
			{
				Shooting();
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire1"))
			{
				InvokeRepeating("Shooting", 0f, 1f/currentWeapon.fireRate);
			} 
			else if (Input.GetButtonUp ("Fire1"))
			{
				CancelInvoke("Shooting");
			}	
		}
	}
	

	
	[ClientRpc]
	void RpcShootingEffect ()
	{
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}
	
	[Command]
	void CmdPlayerShot (string _playerID, int _damage)
	{
		Debug.Log(_playerID + " has been shot.");
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);
	}
	
	[ClientRpc]
	void RpcHitingEffect(Vector3 _pos, Vector3 _normal)
	{
		GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy(_hitEffect, 2f);
	}
	[Command]
	void CmdShoot ()
	{
		RpcShootingEffect();
	}

	[Client]
	void Shooting ()
	{
		if (!isLocalPlayer)
 		{
 			return;
 		}
		
		CmdShoot();
		

		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask) )
		{
			if (_hit.collider.tag == PLAYER)
			{
				CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
				Debug.Log(_hit.collider.name + " has been shot.");
			}
			CmdHit(_hit.point, _hit.normal);
		}

	}

	[Command]
	void CmdHit (Vector3 _pos, Vector3 _normal)
	{
		RpcHitingEffect(_pos, _normal);
	}

}