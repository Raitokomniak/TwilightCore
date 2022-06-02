using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	public bool dead;
	public float maxHealth;
	public float currentHealth;
	public bool invulnerable = false;

	public virtual void SetHealth(int setMaxHealth, int _healthBars, Phaser _bossScript){}

	public void SetHealth(int _maxHealth){
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
		Game.control.ui.BOSS.ToggleInvulnerable(invulnerable);
	}

	public bool GetInvulnerableState(){
		return invulnerable;
	}

	public virtual void Die() {
		dead = true;
		Game.control.sound.PlaySound ("Enemy", "Die", true);
		IEnumerator animateDeathRoutine = AnimateDeath();
		StartCoroutine(animateDeathRoutine);
	}

	public virtual IEnumerator AnimateDeath(){
		DropLoot("Core");
		DisableEnemy();
		yield return new WaitForSeconds(3f);
		Destroy(this.gameObject);
	}

	public virtual void DisableEnemy(){
		GetComponent<EnemyShoot>().StopPattern();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<EnemyShoot>().enabled = false;
	}

	public void DropLoot(string type){
		if(type == "Core"){
			for(int i = 0; i < 2; i++){ ////////// MANAGE AMOUNT OF LOOT
				if(Random.Range(0, 2) == 0)
					 Instantiate(Resources.Load("Prefabs/nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
				else Instantiate(Resources.Load("Prefabs/dayCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
			}
		}
		if(type == "Exp")
			for(int i = 0; i < 9; i++)
				Instantiate(Resources.Load("Prefabs/expPoint"), transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5)), transform.rotation);
	}

	void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") {
			if(!dead) Die ();
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "PlayerProjectile") {
				if(!dead) Die ();
				Destroy(c.gameObject);
		}
	}
}
