using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	EnemyShoot shooter;
	public float maxHealth;
	public float currentHealth;
	public float superThreshold;
	public int healthBars;
	//int remainingHealthBars;

	bool invulnerable;

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
		Game.control.ui.UpdateBossHealthBars (healthBars);
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
		Game.control.ui.ToggleInvulnerable(invulnerable);
	}

	public bool GetInvulnerableState(){
		return invulnerable;
	}


	public void TakeHit(float damage)
	{
		Game.control.player.GainScore(Mathf.RoundToInt(damage));
		
		if(!invulnerable && !Game.control.stageHandler.gameOver){
			if(GetComponent<Phaser>() != null){
				if (GetComponent<Phaser>().superPhase) {
					currentHealth -= damage / 4;
				}
				else {
					currentHealth -= damage;
				}
			}
			if(tag == "Boss" || tag == "MidBoss") Game.control.ui.UpdateBossHealth(currentHealth);
		}


		if(tag == "Boss" || tag == "MidBoss"){
			if (currentHealth <= superThreshold && !shooter.phaser.superPhase && !invulnerable) {
				invulnerable = true;
				shooter.phaser.superPhase = true;
				shooter.phaser.NextBossPhase ();
				Game.control.enemySpawner.DestroyAllProjectiles ();
			}
		}


		if(currentHealth <= 0){
			healthBars-= 1;
			if (healthBars <= 0) {
				Die ();
			} else {
				currentHealth = maxHealth;
				Game.control.ui.UpdateBossHealth (currentHealth);
				Game.control.ui.UpdateBossHealthBars (healthBars);
				invulnerable = true;
				shooter.phaser.superPhase = false;			
				shooter.phaser.NextBossPhase();
				Game.control.enemySpawner.DestroyAllProjectiles ();
			}
		}
	}



	public void Die() {
		
		
		Destroy(this.gameObject);
		Game.control.sound.PlaySound ("Enemy", "Die", true);
		//Instantiate(Resources.Load("expPoint"), transform.position, transform.rotation);
		if(Random.Range(0, 2) == 0)
			Instantiate(Resources.Load("Prefabs/nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
		else 
			Instantiate(Resources.Load("Prefabs/dayCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
		//if(shooter.bulletsShot.Count != 0) GameController.gameControl.enemySpawner.DestroyEnemyProjectiles (shooter.bulletsShot);
		if(tag == "Boss" || tag == "MidBoss"){
			Game.control.enemySpawner.DestroyAllProjectiles();
			for(int i = 0; i < 9; i++){
				Instantiate(Resources.Load("Prefabs/expPoint"), transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5)), transform.rotation);
				if(Random.Range(0, 2) == 0)
					Instantiate(Resources.Load("Prefabs/nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
			}
				
			Game.control.ui.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
			Game.control.ui.ToggleBossHealthSlider(false, 0, "");
			Game.control.ui.HideBossTimer();
			if (tag == "Boss") {
				Game.control.stageHandler.EndHandler ("StageComplete");
			}
			shooter.wave.dead = true;
		}


	}

	public void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == "NullField") {
			if (tag != "Boss" && tag != "MidBoss") {
				Die ();
			} else
				TakeHit (Game.control.player.stats.damage);
		}
	}
}
