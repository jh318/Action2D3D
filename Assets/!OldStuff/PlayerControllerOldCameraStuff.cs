using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOldCameraStuff : MonoBehaviour {

	public float speed = 1;
	public float maxSpeed = 5.0f;
	public float turningSpeed = 60.0f;


	Animator anim;
	Rigidbody body;

	float right = 0.0f;
	float forward = 0.0f;

	void Start(){
		anim = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
		
	}

	void Update(){
		MovementAnimation(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		float horizontal =  Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
		transform.Rotate(0,horizontal,0);
		float vertical =  Input.GetAxis("Vertical") * speed * Time.deltaTime;
		transform.Translate(0,0,vertical);

	}

	void FixedUpdate(){
		//Drive(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			
	}

	void MovementAnimation(float horizontal, float vertical){
		if (Mathf.Abs(horizontal) > 0.5){
			anim.SetFloat("horizontal", horizontal);
		}
		else{
			anim.SetFloat("horizontal", 0);
		}

		if(Mathf.Abs(vertical) > 0.5){
			anim.SetFloat("vertical", vertical);
		}
		else{
			anim.SetFloat("vertical",0);
		}

		if(horizontal == 0.0f && vertical == 0.0f){
			anim.SetBool("isMoving", false);
		}
		else{
			anim.SetBool("isMoving", true);
		}
	}

	void Drive(float horizontal, float vertical){
		if(vertical > 0.0f){
			forward = 1.0f;
		}
		else if(vertical < 0.0f){
			forward = -1.0f;
		}
		else{
			forward = 0.0f;
		}
	

		if(horizontal > 0.0f){
			right = 1.0f;
		}
		else if(horizontal < 0.0f){
			right = -1.0f;
		}
		else{
			right = 0.0f;
		}


		Vector3 movement = new Vector3(right, 0.0f, forward) * Time.deltaTime * speed;
		body.velocity += movement;	
		body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);

	}
}
