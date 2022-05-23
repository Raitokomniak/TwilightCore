using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour {
	
	public int lives; 
	public bool dead;
	public bool invulnerable;
	IEnumerator invulnerabilityRoutine;

	public void Init(){
		lives = Game.control.stageHandler.stats.lives;
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);
		GetComponent<SpriteRenderer> ().enabled = true;
		invulnerable = false;
		dead = false;
	}


	void Update(){
		if(!invulnerable)
			GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void TakeHit()
	{
		Game.control.sound.PlaySound ("Player", "TakeHit", true);
		Game.control.enemySpawner.DestroyAllProjectiles ();

		if(lives > 0) {  LoseLife(); }
		else if(lives <= 0) {  Die();  }
	}

	void LoseLife(){
		lives -= 1;
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);
		invulnerabilityRoutine = AnimateInvulnerabilityRoutine();
		StartCoroutine(invulnerabilityRoutine);
		Game.control.player.special.DepleteCore (false);
		if(Game.control.stageHandler.bossOn) Game.control.stageHandler.bossBonus = false;
	}

	public void GainLife(){
		lives += 1;
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);
	}

	void Die(){
		Game.control.sound.PlaySound ("Player", "Die", true);
		invulnerable = true;
		GetComponent<PlayerShoot>().DisableWeapons();
		GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
		GetComponent<SpriteRenderer>().enabled = false;
		Game.control.stageHandler.EndHandler("GameOver");
		GetComponent<PlayerMovement>().FocusMode(false);
		dead = true;
	}


	//Animates the invulnerable state. Phase time 0.2 seconds
	IEnumerator AnimateInvulnerabilityRoutine()
	{
		invulnerable = true;
		while(invulnerable)
		{
			for (int i = 0; i < 3; i++) {
				GetComponent<SpriteRenderer> ().enabled = false;
				yield return new WaitForSeconds (0.2f);
				GetComponent<SpriteRenderer> ().enabled = true;
				yield return new WaitForSeconds (0.2f);
			}
			invulnerable = false;
		}
	}
}
