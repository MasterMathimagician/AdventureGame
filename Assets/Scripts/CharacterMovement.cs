using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class: CharacterMovement
 * Purpose: This is to control the movement of an unanimated character and produce something a little more refined than that
 * which is seen in the online tutorials
 * 
 * 
 * To add:
 * Jumping
 * double jumps
 * hover
 * variable speed movement (try to connect a dualshock 4 controller)
 * Then move on to animated character controllers
 * 
*/

public class CharacterMovement : MonoBehaviour {


	public float jumpPower = 3.0f;
	public float groundMove = 10.0f;
	public float airMove = 7.0f;

	public float groundForce = 1.0f;
	public float airForce = 0.7f;

	public float hoverFallSpeed = 0.5f;
	public float turnSpeed = 50.0f;
	public int JumpNum = 2;  
	public bool forces = true;

	private SphereCollider sphere;
	private GameObject character;
	private Rigidbody rb = null;
	private bool canJump = true;
	private int jumped = 0;


	// Use this for initialization
	void Start () {
		character = this.gameObject;
		rb = GetComponent<Rigidbody> (); 
		sphere = GetComponent<SphereCollider> ();
	}

	void FixedUpdate() {
		if (Physics.Raycast(transform.position, - Vector3.up, sphere.bounds.extents.y + 0.1f)) {
			canJump = true;
			jumped = 0;
		}
		bool jump = Input.GetButtonDown ("Jump");
		if (jump && (jumped< JumpNum)) {
			jumped++;
			//Vector3 force = new Vector3 (0, jumpPower, 0);
			//rb.AddForce (force, ForceMode.Impulse);

			rb.AddForce (new Vector3 (0, jumpPower, 0), ForceMode.Impulse);
			canJump = false;
		}
		float x = Input.GetAxis("Horizontal") * Time.deltaTime;
		float z = Input.GetAxis("Vertical") * Time.deltaTime;

		character.transform.Rotate(0, turnSpeed * x, 0);
		if (forces) {
			if (canJump) {
				rb.AddForce (new Vector3 (0, 0, groundForce * z), ForceMode.VelocityChange);

			} else {
				rb.AddForce (new Vector3 (0, 0, airForce * z), ForceMode.VelocityChange);
			}
		} else {
			if (canJump) {
				character.transform.Translate (0, 0, groundMove * z);
			} else {
				character.transform.Translate (0, 0, airMove * z);
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
