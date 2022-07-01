using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLife : EnemyLife
{
    Phaser bossScript;
    public float superThreshold;
	public int healthBars;
    bool deathFlag = false;

    public override void SetHealth(int setMaxHealth, int _healthBars, Phaser _bossScript){
        bossScript = _bossScript;
		healthBars = _healthBars;
		maxHealth = setMaxHealth;
		currentHealth = maxHealth;
		superThreshold = maxHealth * .20f;
		//Game.control.ui.UpdateBossHealthBars (healthBars);
	}

	public void SetHealthToThreshold(){
		currentHealth = superThreshold;
		Game.control.enemySpawner.DestroyAllProjectiles ();
		Game.control.ui.BOSS.UpdateBossHealth(currentHealth);
	}

	bool CanTakeHit(){
		if(invulnerable) return false;
		if(Game.control.stageHandler.gameOver) return false;
		return true;
	}

	public void BossTakeHit()
	{	
        if(dead) return;

        Game.control.sound.PlaySound("Enemy", "BossHit", false);
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
			if (healthBars <= 0 && !GetComponent<EnemyShoot> ().wave.dead)  Die(false);
			else {
				bossScript.NextPhase();
				NextHealthBar();
			}
		}
        
       // IEnumerator animateHitRoutine = AnimateHit();
       // StartCoroutine(animateHitRoutine);
	}

    IEnumerator AnimateHit(){
        GetComponent<EnemyMovement>().enemySprite.color = new Color(1, 0.5f, 0.5f, 1);
        GameObject hitFX = Instantiate(Resources.Load("Prefabs/HitFX"), transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(0.1f);
        GetComponent<EnemyMovement>().enemySprite.color = new Color(1, 1, 1, 1);
    }

    public void FakeDeath(){
        deathFlag = true;
        dead = true;
        GetComponent<EnemyShoot>().enabled = false;
        Game.control.ui.BOSS.HideUI();
    }

    public override void Die(bool silent) {
        if(deathFlag) return;
		IEnumerator animateDeathRoutine = AnimateDeath(silent);
		StartCoroutine(animateDeathRoutine);
	}

    public override IEnumerator AnimateDeath(bool silent){
        deathFlag = true;
        bossScript.StopPats();
		bossScript.StopCoro();
        Game.control.enemySpawner.DestroyAllProjectiles();
        yield return new WaitForSeconds(2f);

        if(!silent) Game.control.sound.PlaySound("Enemy", "BossDie", true);
		DropLoot("Core");
		DropLoot("ExpPoint");
		Game.control.ui.BOSS.HideUI();
		Game.control.ui.BOSS.ToggleBossHealthSlider (false, 0, "");
		Game.control.ui.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage); //DOESN'T BELONG HERE

		GetComponentInChildren<SpriteRenderer>().enabled = false;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EnemyShoot> ().wave.dead = true;
		GetComponent<EnemyShoot>().enabled = false;
        dead = true;
        
		yield return new WaitForSeconds(.3f);
        
		Destroy(this.gameObject);
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
