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
	private float lastJumped = 0.0f;
	private float jumpInterval = 0.05f;

	private float moveAngle;
	private float moveForward;
	private bool stillJumping=false;

	public float jumpHold = 0.9f;
	public float jumpDuration = 2.0f;
	public float hoverTime = 1.0f; 
	public float hoverFallSpeed = 0.0f;
	public float turnSpeed = 70.0f;
	public int JumpNum = 2;  
	public bool forces = true;

	public float groundMove = 10.0f;
	public float airMove = 7.0f;
	public float jumpSpeed = 0.3f;

	public float jumpPower = 3.0f;
	public float groundForce = 0.10f;
	public float airForce = 0.07f;



	// Use this for initialization
	void Start () {
		character = this.gameObject;
		rb = GetComponent<Rigidbody> (); 
		sphere = GetComponent<SphereCollider> ();
	}

	void Update() {
		canJump =Physics.Raycast(transform.position, - Vector3.up, sphere.bounds.extents.y + 0.1f);
		// can also use Physics.CheckSphere with some work to determine groundedness of the character but this is simpler
		bool jump = Input.GetButtonDown ("Jump");
		bool jumping = Input.GetButton ("Jump");
		bool doneJumping = Input.GetButtonUp ("Jump");
		bool hover = Input.GetButton ("Fire1");
		moveAngle = Input.GetAxis("Horizontal") * Time.deltaTime;
		moveForward = Input.GetAxis("Vertical") * Time.deltaTime;

		if (forces) {
			// section for forces based control
			if (jump && (jumped< JumpNum)) {
				jumped++;
				rb.AddForce (new Vector3 (0, jumpPower, 0), ForceMode.Impulse);
				canJump = false;
			}
			if (canJump) {
				rb.AddForce (new Vector3 (0, 0, groundForce * moveForward), ForceMode.VelocityChange);
			} else {
				rb.AddForce (new Vector3 (0, 0, airForce * moveForward), ForceMode.VelocityChange);
			}
		} else {
			// section for transform based control 
			// this if statement sets the falling speed and stops downward motion when in contact with the ground
			if (canJump) {
				fallingSpeed = Mathf.Max(0.0f, fallingSpeed);
			} else {
				fallingSpeed += gravity * Time.deltaTime;
			}

			float fs = hoverFallSpeed*Time.deltaTime;
			if (hover && !canJump && (fs >fallingSpeed)) {
				fallingSpeed = fs;
			} 

			float timeSinceLastJumped = Time.unscaledTime-lastJumped;
			if (jump && (jumped < JumpNum)) {
				float last = Time.unscaledTime-lastJumped;
				if (last > jumpInterval) {
					lastJumped = Time.unscaledTime;
					jumped++;
					canJump = false;
					fallingSpeed = jumpSpeed;
				}
			} else if (jumping&&stillJumping) {
				fallingSpeed *= jumpHold * Time.deltaTime;
			} else if (doneJumping||hover||(timeSinceLastJumped>jumpDuration)){
				fallingSpeed = (Mathf.Min(0.0f, fallingSpeed)+fallingSpeed)/2;
				stillJumping = false;
			}
		}
		character.transform.Rotate(0, turnSpeed * moveAngle, 0);
		if (canJump) {
			jumped = 0;
			character.transform.Translate (0, 0, groundMove * moveForward);
		} else {
			//Debug.Log ("fallingSpeed is " + fallingSpeed);
			character.transform.Translate (0, fallingSpeed, airMove * moveForward);
		}
	}

	void FixedUpdate(){


	}
}
