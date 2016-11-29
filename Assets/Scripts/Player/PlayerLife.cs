using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour {

	public int lives;
	public bool invulnerable;

	public void InitLife(){
		lives = GameController.gameControl.stats.lives;
		GameController.gameControl.ui.UpdateStatPanel("Lives", lives);
		GetComponent<SpriteRenderer> ().enabled = true;
		invulnerable = false;
	}
		

	//Handles collision with enemy and exp points

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.tag == "ExpPoint") {
			GameController.gameControl.stats.GainXP(7);
			Destroy(c.gameObject);
		}

		if(c.gameObject.tag == "DayCorePoint" && !GetComponent<PlayerSpecialAttack>().specialAttack) {
			GameController.gameControl.stats.GainCoreCharge ("Day", 2);
			Destroy(c.gameObject);
		}
		else if(c.gameObject.tag == "NightCorePoint" && !GetComponent<PlayerSpecialAttack>().specialAttack) {
			GameController.gameControl.stats.GainCoreCharge ("Night", 2);
			Destroy(c.gameObject);
		}
	}



	public void TakeHit()
	{
		GameController.gameControl.sound.PlaySound ("Player", "TakeHit", true);
		GameController.gameControl.enemySpawner.DestroyAllProjectiles ();

		if(lives > 0) {
			lives -= 1;
			GameController.gameControl.ui.UpdateStatPanel("Lives", lives);
			StartCoroutine(AnimateInvulnerability());
			//GameController.gameControl.stats.DepleteCore (false);
		}

		else if(lives <= 0) {
			GameController.gameControl.sound.PlaySound ("Player", "Die", true);
			Debug.Log ("gameover");
			invulnerable = true;
			GetComponent<SpriteRenderer>().enabled = false;
			GameController.gameControl.gameEnd.EndHandler("GameOver");
		}
	}


	//Animates the invulnerable state. Phase time 0.2 seconds
	IEnumerator AnimateInvulnerability()
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
