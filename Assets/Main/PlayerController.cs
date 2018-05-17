using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public float maxSpeed = 10.0f;
	public float deceleration = 5.0f;
	public float jumpHeight = 1.0f;

	float horizontal;
	float vertical;
	float forwardVelocity = 0.0f;

	bool isMoving = false;
	bool isGrounded = false;
	bool isTurning180 = false;
	float lastHorizontalInput;

	Animator anim;
	Rigidbody body;

	void Start(){
		anim = GetComponentInChildren<Animator>();
		body = GetComponent<Rigidbody>();

	}


////UPDATE

	void Update(){
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		
		//Facing
		if(horizontal > 0)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, 0.0f + 90.0f, transform.rotation.z);
		}
		else if(horizontal < 0)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f + 90.0f, transform.rotation.z);
		}
		
		//If player inputs to run opposite direction
			//And current forwardVelocity is maxspeed
				//Play turn anim


			//TODO - FIX 180 TURNING
		// if(horizontal == -1 && forwardVelocity > 0.0f){
		// 	anim.SetTrigger("isTurning180");
		// 	Debug.Log("Turn!");
		// 	isTurning180 = true;
		// }
		// else if(horizontal == 1 && forwardVelocity < 0.0f){
		// 	anim.SetTrigger("isTurning180");
		// 	Debug.Log("Turn!");
		// 	isTurning180 = true;
		// }


		//Animation
		anim.SetBool("isMoving", isMoving);
		anim.SetFloat("forwardVelocity", Mathf.Abs(horizontal));

		if(Mathf.Abs(horizontal) > 0 && !isTurning180)
		{
			isMoving = true;
			// isTurning180 = false; //TODO FIX
			anim.SetFloat("horizontal", horizontal);
			
		}
		else
		{
			isMoving = false;
			anim.SetFloat("horizontal", 0.0f);
		}


		lastHorizontalInput = horizontal;
	}

	void FixedUpdate(){
		//Velocity
		///Set Horizontal movement
		if(horizontal > 0)
		{
			forwardVelocity += speed * Time.deltaTime;
		}
		else if(horizontal < 0)
		{
			forwardVelocity -= speed * Time.deltaTime;
		}
		else
		{
			forwardVelocity = Mathf.Lerp(forwardVelocity, 0.0f, deceleration * Time.deltaTime);
		}

		///Set Vertical movement
		if(vertical > 0.0f && isGrounded)
		{
			//Jump!
			body.velocity += JumpVelocity();
			isGrounded = false;
		}
		
		forwardVelocity = Mathf.Clamp(forwardVelocity, -maxSpeed, maxSpeed);
		body.velocity = new Vector3(forwardVelocity, body.velocity.y, body.velocity.z);

	}



////COLLISION

	void OnCollisionEnter(Collision c){
		if(Vector3.Dot(transform.up, c.gameObject.transform.up) == 1.0f){
			isGrounded = true;
		}
	}


////FUNCTIONS

	Vector3 JumpVelocity(){
		return -Physics.gravity.normalized * Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
	}
}
