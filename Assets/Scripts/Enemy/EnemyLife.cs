using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	bool dead;
	Wave wave;
	EnemyShoot shooter;
	public float maxHealth;
	public float currentHealth;
	public float superThreshold;
	public int healthBars;
	bool invulnerable;

	void Awake () {
		shooter = GetComponent<EnemyShoot> ();
		invulnerable = false;
	}

	public void SetHealth(int setMaxHealth, int _healthBars, float _superThreshold, Wave _wave){
		wave = _wave;
		healthBars = _healthBars;
		maxHealth = setMaxHealth;
		currentHealth = maxHealth;
		if (_superThreshold == 0)
			superThreshold = -10;
		else superThreshold = maxHealth * _superThreshold;
		//Game.control.ui.UpdateBossHealthBars (healthBars);
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
		Game.control.ui.BOSS.ToggleInvulnerable(invulnerable);
	}

	public bool GetInvulnerableState(){
		return invulnerable;
	}


	public void TakeHit(float damage)
	{
		
		long gainedScore = Game.control.player.GainScore(Mathf.RoundToInt(damage));
		GetComponent<MiniToast>().PlayScoreToast(Mathf.RoundToInt(gainedScore));
		
		if(!invulnerable && !Game.control.stageHandler.gameOver){
			if(GetComponent<Phaser>() != null)
			{
				if (GetComponent<Phaser>().superPhase) 
					currentHealth -= damage / 4;
				else 
					currentHealth -= damage;
			}
			if(tag == "Boss" || tag == "MidBoss") 
				Game.control.ui.BOSS.UpdateBossHealth(currentHealth);
		}


		if(tag == "Boss" || tag == "MidBoss"){
			if (currentHealth <= superThreshold && !wave.bossScript.superPhase && !invulnerable) {
				invulnerable = true;
				wave.bossScript.superPhase = true;
				wave.bossScript.NextPhase ();
				Game.control.enemySpawner.DestroyAllProjectiles ();
			}
		}


		if(currentHealth <= 0){
			healthBars-= 1;
			if (healthBars <= 0) {
				Die ();
			} else {
				currentHealth = maxHealth;
				Game.control.ui.BOSS.UpdateBossHealth (currentHealth);
				//Game.control.ui.UpdateBossHealthBars (healthBars);
				invulnerable = true;
				wave.bossScript.superPhase = false;			
				wave.bossScript.NextPhase();
				Game.control.enemySpawner.DestroyAllProjectiles ();
			}
		}
	}



	public void Die() {
		if(!dead){
			dead = true;
			//Destroy(this.gameObject);
			Game.control.sound.PlaySound ("Enemy", "Die", true);

			IEnumerator animateDeathRoutine = AnimateDeath();
			StartCoroutine(animateDeathRoutine);
		}
	}

	public IEnumerator AnimateDeath(){
		DisableEnemy();
		DropLoot();
		if(tag == "Boss" || tag == "MidBoss") 
			BossDeath();
		yield return new WaitForSeconds(3f);
		Destroy(this.gameObject);
	}

	void DisableEnemy(){
		if(wave.isBoss || wave.isMidBoss) wave.bossScript.StopPats();
		else GetComponent<EnemyShoot>().StopPattern();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<EnemyShoot>().enabled = false;
	}

	void DropLoot(){
		if(Random.Range(0, 2) == 0)
			Instantiate(Resources.Load("Prefabs/nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
		else 
			Instantiate(Resources.Load("Prefabs/dayCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
	}

	void BossDeath(){
		Game.control.enemySpawner.DestroyAllProjectiles();
			for(int i = 0; i < 9; i++){
				Instantiate(Resources.Load("Prefabs/expPoint"), transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5)), transform.rotation);
				if(Random.Range(0, 2) == 0)
					Instantiate(Resources.Load("Prefabs/nightCorePoint"), transform.position + new Vector3(Random.Range(-5, 5), 2f, 0), Quaternion.Euler(0,0,0));
			}
				
			Game.control.ui.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
			shooter.wave.dead = true;
	}

	public void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") {
			if (tag != "Boss" && tag != "MidBoss") {
				Die ();
			} else
				TakeHit (Game.control.stageHandler.stats.damage);
		}
	}
}
