using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character_Controller : MonoBehaviour {
	public static bool panelIsActive;
	public float speed = 3f;
	//public float damage = 10f;
	//public float range = 100f;
	//public float health = 100f;
	public float jumpSpeed = 10f; 
	public float verticalVelocity = 0f;
	//public float gravity = 20f;
	//public Vector3 gravity = Vector3.zero;
	//protected Vector3 move = Vector3.zero;

	private Vector3 jumpMovement;
	//private Vector3 jumpMovement_=Vector3.zero;

	public Camera m_Camera;
	CharacterController cc;
	GameObject panel;

	// Use this for initialization
	void Start () {
		cc = gameObject.GetComponent<CharacterController> ();

	}
	
	// Update is called once per frame
	void Update () {
		panel = GameObject.FindGameObjectWithTag("Panel");
		if (panel!=null) {
			if (panel.activeSelf)
				return;
		}
			
		if (cc.isGrounded)
		{
			verticalVelocity = 0;
			if (Input.GetButtonDown("Jump")) {
				verticalVelocity = jumpSpeed;				
			}
		}
		
		jumpMovement = m_Camera.transform.TransformDirection(Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal"));
		
		jumpMovement *= speed;
	
		verticalVelocity += (Physics.gravity.y);
		jumpMovement.y = verticalVelocity;
		cc.Move(jumpMovement*Time.deltaTime);
		
	}

	/*void Shoot ()
	{
		GameObject bullet = Instantiate (bulletPrefab, m_Camera.transform.position + m_Camera.transform.TransformDirection (Vector3.forward * 0.5f - Vector3.up * 0.09f), m_Camera.transform.rotation) as GameObject;
		bullet.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * 20f, ForceMode.VelocityChange);

		RaycastHit hit;
		if (Physics.Raycast (m_Camera.transform.position, m_Camera.transform.forward, out hit, range)) {
			Target target = hit.transform.GetComponent<Target> ();
			if (target != null) {
				target.TakeDemage (damage);
			}
		}
		Destroy (bullet, 4f);

	}*/
}
