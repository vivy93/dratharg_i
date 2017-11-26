using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBar : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBarFill;

	[SerializeField]
	private Player player;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Camera m_camera = Camera.main;
		transform.LookAt (transform.position + m_camera.transform.rotation * Vector3.forward, m_camera.transform.rotation * Vector3.up);
		healthBarFill.localScale = new Vector3(player.GetHealth(),1f,1f);
	}

}
