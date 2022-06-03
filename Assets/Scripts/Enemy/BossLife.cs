using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLife : EnemyLife
{
    Phaser bossScript;
    public float superThreshold;
	public int healthBars;

    public override void SetHealth(int setMaxHealth, int _healthBars, Phaser _bossScript){
        bossScript = _bossScript;
		healthBars = _healthBars;
		maxHealth = setMaxHealth;
		currentHealth = maxHealth;
		superThreshold = maxHealth * .20f;
		//Game.control.ui.UpdateBossHealthBars (healthBars);
	}

	bool CanTakeHit(){
		if(invulnerable) return false;
		if(Game.control.stageHandler.gameOver) return false;
		return true;
	}

	public void BossTakeHit()
	{	
		float damage = Game.control.stageHandler.stats.damage;
		GetComponent<MiniToast>().PlayScoreToast(Mathf.RoundToInt(Game.control.player.GainScore(Mathf.RoundToInt(damage))));
		
		if(CanTakeHit()){
			if (GetComponent<Phaser>().superPhase) 
				 currentHealth -= damage / 4;
			else currentHealth -= damage;

			Game.control.ui.BOSS.UpdateBossHealth(currentHealth);
		}

		if (currentHealth <= superThreshold && !bossScript.superPhase && !invulnerable) {
			invulnerable = true;
			bossScript.NextPhase();
			Game.control.enemySpawner.DestroyAllProjectiles ();
		}

		if(currentHealth <= 0){
			healthBars-= 1;
			if (healthBars <= 0) Die ();
			else {
				bossScript.NextPhase();
				NextHealthBar();
			}
		}
	}
    public override void Die() {
		dead = true;
		Game.control.sound.PlaySound ("Enemy", "Die", true);
		IEnumerator animateDeathRoutine = AnimateDeath();
		StartCoroutine(animateDeathRoutine);
	}

    public override IEnumerator AnimateDeath(){
		DropLoot("Core");
		BossDeath();
		yield return new WaitForSeconds(3f);
		Destroy(this.gameObject);
	}

	void BossDeath(){
        bossScript.StopPats();
		bossScript.StopCoro();
		Game.control.enemySpawner.DestroyAllProjectiles();
		DropLoot("Exp");
		Game.control.ui.BOSS.HideBossTimer();
		Game.control.ui.BOSS.ToggleBossHealthSlider (false, 0, "");
		//DOESN'T BELONG HERE
		Game.control.ui.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
		GetComponent<EnemyShoot> ().wave.dead = true;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<EnemyShoot>().enabled = false;
		//USE THIS FOR DEBUGGING STAGE
		//if(wave.isBoss) Game.control.stageHandler.EndHandler ("StageComplete");
	}


	public void NextHealthBar(){
		currentHealth = maxHealth;
		Game.control.ui.BOSS.UpdateBossHealth (currentHealth);
		//Game.control.ui.UpdateBossHealthBars (healthBars);
		invulnerable = true;
		bossScript.superPhase = false;			
		bossScript.endOfPhase = true;
		Game.control.enemySpawner.DestroyAllProjectiles ();
	}
   
   void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") {
			BossTakeHit();
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "PlayerProjectile") {
            Destroy(c.gameObject);
			BossTakeHit();
		}
	}
}
