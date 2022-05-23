using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {

	PlayerHandler player;

	void OnTriggerEnter2D(Collider2D c)
	{
		player = Game.control.player;

		if(c.gameObject.tag == "EnemyProjectile" || c.gameObject.tag == "Enemy" || c.gameObject.tag == "Boss" || c.gameObject.tag == "MidBoss")
		{
			if (!player.health.invulnerable && !Game.control.stageHandler.gameOver) {
				player.health.TakeHit ();
			}
		}

		//6 == pickup point layer
		if(c.gameObject.layer == 6) { 
			if(c.gameObject.tag == "ExpPoint") {
				player.GainXP(1);
			}

			if(c.gameObject.tag == "DayCorePoint" && !player.special.specialAttack) {
				player.special.GainCoreCharge ("Day", 2);
			}

			if(c.gameObject.tag == "NightCorePoint" && !player.special.specialAttack) {
				player.special.GainCoreCharge ("Night", 2);
			}

			Destroy(c.gameObject);
		}
	}
}
