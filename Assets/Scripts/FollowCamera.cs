using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: FollowCamera
 * Purpose: to act as a 3rd person camera controller that will work with 
 * 
 * 
*/

public class FollowCamera : MonoBehaviour {

	public Transform lookTo;
	public float distance = 4.0f;
	public float height = 1.5f;
	public float damping = 5.0f;
	public bool smoothRotation = true;
	public bool followBehind = true;
	public float rotationDamp = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 wantedPosition;
		wantedPosition = lookTo.TransformPoint (0, height, -distance);

		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);

		if (smoothRotation) {
			Quaternion wantedRotation = Quaternion.LookRotation (lookTo.position - transform.position, lookTo.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamp);
		} else {
			transform.LookAt (lookTo, lookTo.up);
		}
	}
}
