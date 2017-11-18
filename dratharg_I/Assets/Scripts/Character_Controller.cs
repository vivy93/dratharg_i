using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour {

	public float speed = 10f;
	public float damage = 10f;
	public float range = 100f;
	public float health = 100f;
	public float jumpSpeed = 50f;
	public float gravity = 20f;

	private Vector3 jumpMovement;
	private Vector3 jumpMovement_=Vector3.zero;

	public Camera m_Camera;
	CharacterController cc;
	private bool falling;

	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
		cc = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		//float translation = Input.GetAxis ("Vertical") * speed;
		//float straffe = Input.GetAxis ("Horizontal") * speed;
		//translation *= Time.deltaTime;
		//straffe *= Time.deltaTime;

		//transform.Translate (straffe, 0, translation);

		//ha ezt a 2 sort bent hagyom akkor együtt mozognak xDD
		//var rot = Cardboard.SDK.HeadPose.Orientation;
		//transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, rot.eulerAngles.y, transform.localEulerAngles.z);
		jumpMovement = m_Camera.transform.TransformDirection(Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal"));
		cc.SimpleMove(speed*jumpMovement);
		
		jumpMovement_.x = Input.GetAxis("Horizontal") * speed;
        jumpMovement_.z = Input.GetAxis("Vertical") * speed;
		Debug.Log (jumpMovement_);

		if (Input.GetButtonDown("Fire1")) {
			Shoot ();
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit(); 
		}
		if (Input.GetButtonDown("Jump")) {
			float i = 1f;
			while (jumpSpeed>= i) {
				jumpMovement_.y =+ i;
				i+=0.2f;
			}

			jumpMovement_.y -= gravity * Time.deltaTime;
			cc.Move(jumpMovement_ * Time.deltaTime);
		}
	}

	void Shoot ()
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

	}
}
