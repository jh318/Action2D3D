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

	bool Moving = false;
	bool Ground = false;
	float lastHorizontalInput;

	Animator anim;
	Rigidbody body;
	public List<GameObject> hitboxes = new List<GameObject>();

	public delegate void EnemyHit(int damage);
	public static event EnemyHit enemyHit = delegate{};

	enum AttackTransition {None,Z,X,C};
	enum StateTransition {Base, Override, Additive};
	AttackTransition attackTransition;
	StateTransition stateTransition;

	class AttackType{
		float damage = 1;
		float knockback = 1;
	}


	void Start(){
		anim = GetComponentInChildren<Animator>();
		body = GetComponent<Rigidbody>();
		attackTransition = AttackTransition.None;
		stateTransition = StateTransition.Base;		
		//InitializeHitboxes();
	}


////UPDATE

	void Update(){
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		
		//Facing
		if(horizontal > 0) //TODO: Replace with smooth turn animation
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, 0.0f + 90.0f, transform.rotation.z);
		}
		else if(horizontal < 0)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f + 90.0f, transform.rotation.z);
		}

		
		//Animation

		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Base"))
		{
			if(Input.GetKeyDown(KeyCode.Z)){
				anim.SetInteger("State", (int)StateTransition.Override);
			}
		}
		
		if(anim.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
		{
			anim.SetInteger("State", (int)StateTransition.Base);
			
			if(Input.GetKeyDown(KeyCode.Z)){
				anim.SetBool("ComboSuccess", true);
			}
		}
		
		anim.SetFloat("forwardVelocity", Mathf.Abs(horizontal));
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
		if(vertical > 0.0f && Ground)
		{
			//Jump!
			body.velocity += JumpVelocity();
			Ground = false;
		}
		
		forwardVelocity = Mathf.Clamp(forwardVelocity, -maxSpeed, maxSpeed);
		body.velocity = new Vector3(forwardVelocity, body.velocity.y, body.velocity.z);

	}



////COLLISION

	void OnCollisionEnter(Collision c){
		if(Vector3.Dot(transform.up, c.gameObject.transform.up) == 1.0f){
			Ground = true;
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Enemy")
		{
			Debug.Log("Hit!");
			enemyHit(10);
		}

	}

////FUNCTIONS

	Vector3 JumpVelocity(){
		return -Physics.gravity.normalized * Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
	}



	void InitializeHitboxes()
	{ 
		hitboxes.AddRange(GameObject.FindGameObjectsWithTag("Hitbox"));

		for(int i = 0; i < hitboxes.Count; i++){
			hitboxes[i].gameObject.GetComponent<BoxCollider>().enabled = false;
		}
	}

	void GetAttackAnimation(){
		AnimatorClipInfo[] m_CurrentClipInfo;
		m_CurrentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
		if(m_CurrentClipInfo[0].clip.name == "attack1"){

		}
	}


////ATTACKS

	void BnB1(int damage, float knockback)
	{

	}


}
