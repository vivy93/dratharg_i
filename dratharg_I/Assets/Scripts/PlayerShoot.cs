using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";
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
		//panel = GameObject.FindGameObjectWithTag("Panel");
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
			counter++;
		}
		
		if (currentWeapon.fireRate <= 0f)
  		{
			if (Input.GetButtonDown("Fire1"))
			{
				Shoot();
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire1"))
			{
				InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
			} 
			else if (Input.GetButtonUp ("Fire1"))
			{
				CancelInvoke("Shoot");
			}	
		}
	}
	
	[Command]
	void CmdOnShoot ()
	{
		RpcDoShootEffect();
    }
	
	[ClientRpc]
	void RpcDoShootEffect ()
	{
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}
	
	[Command]
	void CmdOnHit (Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect(_pos, _normal);
    }
	
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy(_hitEffect, 2f);
	}

	[Client]
	void Shoot ()
	{
		if (!isLocalPlayer)
 		{
 			return;
 		}
		
		CmdOnShoot();
		

		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask) )
		{
			if (_hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
				Debug.Log(_hit.collider.name + " has been shot.");
			}
			CmdOnHit(_hit.point, _hit.normal);
		}

	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage)
	{
		Debug.Log(_playerID + " has been shot.");
		Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
	}

}