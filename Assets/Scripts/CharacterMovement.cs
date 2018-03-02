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

	private SphereCollider sphere;
	private GameObject character;
	private Rigidbody rb = null;
	private bool canJump = true;
	private int jumped = 0;
	private float fallingSpeed = 0.0f;
	private float gravity = -0.1f;

	public float hoverTime = 1.0f; 
	public float hoverFallSpeed = -0.5f;
	public float turnSpeed = 2.0f;
	public int JumpNum = 2;  
	public bool forces = true;

	public float groundMove = 0.2f;
	public float airMove = 0.14f;
	public float jumpSpeed = 1.0f;

	public float jumpPower = 3.0f;
	public float groundForce = 0.10f;
	public float airForce = 0.07f;



	// Use this for initialization
	void Start () {
		character = this.gameObject;
		rb = GetComponent<Rigidbody> (); 
		sphere = GetComponent<SphereCollider> ();
	}

	void FixedUpdate() {
		// can also use Physics.CheckSphere with some work to determine groundedness of the character but this is simpler
		if (Physics.Raycast(transform.position, - Vector3.up, sphere.bounds.extents.y + 0.01f)) {
			canJump = true;
			jumped = 0;
		}
		bool jump = Input.GetButtonDown ("Jump");

		// since I switched to fixed update I no longer need to access Time.deltaTime to keep things accurate
		//float x = Input.GetAxis("Horizontal") * Time.deltaTime;
		//float z = Input.GetAxis("Vertical") * Time.deltaTime;
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");


		character.transform.Rotate(0, turnSpeed * x, 0);
		if (forces) {
			// section for forces based control
			if (jump && (jumped< JumpNum)) {
				jumped++;
				rb.AddForce (new Vector3 (0, jumpPower, 0), ForceMode.Impulse);
				canJump = false;
			}
			if (canJump) {
				rb.AddForce (new Vector3 (0, 0, groundForce * z), ForceMode.VelocityChange);
			} else {
				rb.AddForce (new Vector3 (0, 0, airForce * z), ForceMode.VelocityChange);
			}
		} else {
			// section for transform based control 
			// this if statement sets the falling speed
			if (jump && (jumped < JumpNum)) {
				jumped++;
				canJump = false;
				fallingSpeed = jumpSpeed;
				//Debug.Log ("jumped is " + jumped.ToString());
			}
			if (Input.GetButtonDown ("Fire1") && !canJump) {
				fallingSpeed = hoverFallSpeed*Time.deltaTime;
			} else {
				fallingSpeed += gravity * Time.deltaTime;
			}

			if (canJump) {
				fallingSpeed = 0.0f;
				character.transform.Translate (0, 0, groundMove * z);
			} else {
				fallingSpeed += -1.0f*Time.deltaTime;
				character.transform.Translate (0, fallingSpeed, airMove * z);
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
