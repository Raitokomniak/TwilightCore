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
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateMaxLives(Game.control.stageHandler.stats.maxLives);
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);
		GetComponent<SpriteRenderer> ().enabled = true;
		invulnerable = false;
		dead = false;
	}

	void Update(){
		//DevGodMode(); ///////////////////////////////////////////////////////////////
		if(!invulnerable)
			GetComponent<SpriteRenderer> ().enabled = true;
	}

	void DevGodMode(){
		invulnerable = true;
	}
	public void TakeHit()
	{
        if(invulnerable || Game.control.stageHandler.gameOver) return;

		Game.control.pause.PlayerHitPause();
		Game.control.sound.PlaySound ("Player", "TakeHit", true);
			
        if(lives > 0) LoseLife();
		else if(lives <= 0) {  
            IEnumerator deathRoutine = Die();
            StartCoroutine(deathRoutine);  
        }

	}

	void LoseLife(){
		lives -= 1;
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);


		invulnerabilityRoutine = AnimateInvulnerabilityRoutine();
		StartCoroutine(invulnerabilityRoutine);


		if(GetComponent<PlayerMovement>().focusMode) 
			 Game.control.player.special.DepleteCore ("Night", false);
		else Game.control.player.special.DepleteCore ("Day", false);
		Game.control.stageHandler.DenyBossBonus();
	}

	public void GainLife(){
		lives += 1;
		//Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(lives);
		Game.control.ui.PlayToast("New Life!");
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateMaxLives(lives);
		Game.control.sound.PlaySound("Player", "ExtraLife", false);
	}

	IEnumerator Die(){
		Game.control.sound.PlaySound ("Player", "Die", true);
		invulnerable = true;
		GetComponent<PlayerShoot>().DisableWeapons();
		GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
		GetComponentInChildren<MagneticRange>().gameObject.SetActive(false);
		GetComponent<SpriteRenderer>().enabled = false;
		Game.control.stageHandler.EndHandler("GameOver");
		GetComponent<PlayerMovement>().FocusMode(false);
		dead = true;
        yield return new WaitForSecondsRealtime(1);
        GetComponent<SpriteRenderer>().enabled = false;
	}


	//Animates the invulnerable state. Phase time 0.2 seconds
	IEnumerator AnimateInvulnerabilityRoutine()
	{
		invulnerable = true;
		while(invulnerable)
		{
			for (int i = 0; i < 3; i++) {
				GetComponent<SpriteRenderer> ().enabled = false;
				yield return new WaitForSecondsRealtime (0.2f);
				GetComponent<SpriteRenderer> ().enabled = true;
				yield return new WaitForSecondsRealtime (0.2f);
			}
			invulnerable = false;
		}
	}
}
