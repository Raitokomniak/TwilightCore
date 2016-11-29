using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.tag == "EnemyProjectile" || c.gameObject.tag == "Enemy")
		{
			if (!player.GetComponent<PlayerLife>().invulnerable && !GameController.gameControl.gameEnd.gameOver) {
				player.GetComponent<PlayerLife>().TakeHit ();
			}
		}
	}
}
