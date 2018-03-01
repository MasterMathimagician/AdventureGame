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


	public float jumpPower = 10.0f;
	public float moveSpeed = 1.5f;
	public float hoverFallSpeed = 0.5f;
	public int JumpNum = 2;  

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
	}

	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		bool jump = Input.GetButtonDown ("Jump");
		if (jump && (jumped< JumpNum)) {
			jumped++;
			Vector3 force = new Vector3 (0, jumpPower, 0);
			rb.AddForce (force, ForceMode.Impulse);
		}

		character.transform.Rotate(0, x, 0);
		character.transform.Translate(0, 0, z);
	}
}
