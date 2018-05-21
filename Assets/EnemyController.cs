using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public Collider hurtBox;

	float health;
	float prevHealth;
	public float maxHealth;

	public Image healthBar;

	//public delegate void OnHealthChanged(float health, float prevHealth, float maxHealth);
	//public event OnHealthChanged onHealthChanged = delegate{};
	

	void OnEnable(){
		PlayerController.enemyHit += HitStun;
	}

	void Start(){
		health = maxHealth;
		healthBar.fillAmount = health/maxHealth;
	}

	void HitStun(int damage){
		health -= damage;
		healthBar.fillAmount = health/maxHealth;
	}
	
	
}
