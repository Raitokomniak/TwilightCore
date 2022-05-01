using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.tag == "EnemyProjectile" || c.gameObject.tag == "Enemy")
		{
			if (!Game.control.player.health.invulnerable && !Game.control.stageHandler.gameOver) {
				Game.control.player.health.TakeHit ();
			}
		}

		//6 == pickup point layer
		if(c.gameObject.layer == 6) { 
			if(c.gameObject.tag == "ExpPoint") {
				Game.control.player.GainXP(7);
			}

			if(c.gameObject.tag == "DayCorePoint" && !Game.control.player.special.specialAttack) {
				Game.control.player.special.GainCoreCharge ("Day", 2);
			}

			if(c.gameObject.tag == "NightCorePoint" && !Game.control.player.special.specialAttack) {
				Game.control.player.special.GainCoreCharge ("Night", 2);
			}

			Destroy(c.gameObject);
		}
	}
}
