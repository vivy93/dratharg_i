using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

	[SerializeField]
	private string weaponLName = "Weapon";

	[SerializeField]
	private Transform holder;

	[SerializeField]
	private PlayerWeapon weapon;

	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	void Start ()
	{
		SetWeapon(weapon);
	}

	public PlayerWeapon GetCurrentWeapon ()
	{
		return currentWeapon;
	}
	
	public WeaponGraphics GetCurrentGraphics()
 	{
 		return currentGraphics;
 	}

	void SetWeapon (PlayerWeapon curWeapon)
	{
		currentWeapon = curWeapon;

		GameObject _weapon = (GameObject)Instantiate(curWeapon.graphics, holder.position, holder.rotation);
		_weapon.transform.SetParent(holder);
		
		currentGraphics = _weapon.GetComponent<WeaponGraphics>();
 		if (currentGraphics == null)
 			Debug.LogError("Error: " + _weapon.name);
		if (isLocalPlayer)
			_weapon.layer = LayerMask.NameToLayer(weaponLName);
			

	}

}