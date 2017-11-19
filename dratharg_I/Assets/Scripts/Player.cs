﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	
	[SerializeField]
    private GameObject deathEffect;
	
	[SerializeField]
	private GameObject[] disableGameObjectsOnDeath;

    public void Setup ()
    {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

        SetDefaults();
    }
	
	//for testing health and explosion
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(99999);
		}
	}

	[ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
		if (isDead)
			return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0)
		{
			Die();
		}
    }

	private void Die()
	{
		isDead = true;
		//Disable the components
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}
		//Disable the gameobjects
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath[i].SetActive(false);
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;

		GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(_gfxIns, 3f);
		
		if (isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(true);
			Cardboard.SDK.gameObject.SetActive (false);
			
		}
		
		Debug.Log(transform.name + " is DEAD!");

		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn ()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		Debug.Log(transform.name + " respawned.");
	}

    public void SetDefaults ()
    {
		isDead = false;

        currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}
		
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath[i].SetActive(true);
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;
		
		if (isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(false);
			Cardboard.SDK.gameObject.SetActive (true);
			
		}
    }

}