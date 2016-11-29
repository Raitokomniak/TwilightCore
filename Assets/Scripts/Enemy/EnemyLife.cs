using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	EnemyShoot shooter;
	Wave wave;
	public float maxHealth;
	public float currentHealth;
	public float superThreshold;
	public int healthBars;
	int remainingHealthBars;

	bool invulnerable;

	// Use this for initialization
	void Awake () {
		shooter = GetComponent<EnemyShoot> ();
		invulnerable = false;
	}


	public void SetHealth(int setMaxHealth, int _healthBars, float _superThreshold){
		healthBars = _healthBars;
		maxHealth = setMaxHealth;
		currentHealth = maxHealth;
		if (_superThreshold == 0)
			superThreshold = -10;
		else superThreshold = maxHealth * _superThreshold;
		GameController.gameControl.ui.UpdateBossHealthBars (healthBars);
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
	}

	public bool GetInvulnerableState(){
		return invulnerable;
	}


	public void TakeHit(float damage)
	{
		if(!invulnerable && !GameController.gameControl.gameEnd.gameOver){
			if (shooter.superPhase) {
				currentHealth -= damage / 4;
			}
			else {
				currentHealth -= damage;
			}
			if(tag == "Boss" || tag == "MidBoss") GameController.gameControl.ui.UpdateBossHealth(currentHealth);
		}


		if(tag == "Boss" || tag == "MidBoss"){
			if (currentHealth <= superThreshold && !shooter.superPhase && !invulnerable) {
				invulnerable = true;
				shooter.superPhase = true;

				shooter.NextBossPhase ();
				GameController.gameControl.enemySpawner.DestroyAllProjectiles ();
			}
		}


		if(currentHealth <= 0){
			healthBars-= 1;
			if (healthBars <= 0) {
				Die ();
			} else {
				
				GameController.gameControl.enemySpawner.DestroyAllProjectiles ();
				currentHealth = maxHealth;
				GameController.gameControl.ui.UpdateBossHealth (currentHealth);
				GameController.gameControl.ui.UpdateBossHealthBars (healthBars);
				invulnerable = true;
				shooter.superPhase = false;
				shooter.NextBossPhase();
			}
		}
	}



	public void Die() {
		
		Destroy(this.gameObject);
		GameController.gameControl.sound.PlaySound ("Enemy", "Die", true);
		//Instantiate(Resources.Load("expPoint"), transform.position, transform.rotation);
		if(Random.Range(0, 2) == 0)
			Instantiate(Resources.Load("nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
		else 
			Instantiate(Resources.Load("dayCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
		//if(shooter.bulletsShot.Count != 0) GameController.gameControl.enemySpawner.DestroyEnemyProjectiles (shooter.bulletsShot);
		if(tag == "Boss" || tag == "MidBoss"){
			GameController.gameControl.enemySpawner.DestroyAllProjectiles();
			for(int i = 0; i < 9; i++){
				Instantiate(Resources.Load("expPoint"), transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5)), transform.rotation);
				if(Random.Range(0, 2) == 0)
					Instantiate(Resources.Load("nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
			}
				
			GameController.gameControl.ui.UpdateTopPlayer ("Stage" + GameController.gameControl.stage.currentStage);
			GameController.gameControl.ui.ToggleBossHealthSlider(false, 0, "");

			if (tag == "Boss") {
				GameController.gameControl.gameEnd.EndHandler ("StageComplete");
			}
			wave = shooter.wave;
			wave.dead = true;
		}


	}

	public void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == "NullField") {
			if (tag != "Boss" && tag != "MidBoss") {
				Die ();
			} else
				TakeHit (GameController.gameControl.stats.damage);
		}
	}
}
