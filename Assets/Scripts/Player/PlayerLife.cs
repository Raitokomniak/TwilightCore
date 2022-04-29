using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour {

	public int lives;
	public bool invulnerable;
	IEnumerator invulnerabilityRoutine;

	public void Init(){
		lives = Game.control.player.stats.maxLives;
		Game.control.ui.UpdateStatPanel("Lives", lives);
		GetComponent<SpriteRenderer> ().enabled = true;
		invulnerable = false;
	}

	public void TakeHit()
	{
		Game.control.sound.PlaySound ("Player", "TakeHit", true);
		Game.control.enemySpawner.DestroyAllProjectiles ();

		if(lives > 0) {
			lives -= 1;
			Game.control.ui.UpdateStatPanel("Lives", lives);
			invulnerabilityRoutine = AnimateInvulnerabilityRoutine();
			StartCoroutine(invulnerabilityRoutine);
			Game.control.player.special.DepleteCore (false);
		}

		else if(lives <= 0) {
			Game.control.sound.PlaySound ("Player", "Die", true);
			invulnerable = true;
			GetComponent<PlayerShoot>().DisableWeapons();
			GetComponent<SpriteRenderer>().enabled = false;
			Game.control.stage.EndHandler("GameOver");
		}
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
