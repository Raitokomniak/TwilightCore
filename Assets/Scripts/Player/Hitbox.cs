using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {

	PlayerHandler player;
	
	void CheckHazard(string t){
		if(t == "EnemyProjectile" || t == "Enemy" || t == "Boss" || t == "MidBoss" || t == "EnvironmentalHazard"){
			player.health.TakeHit ();
		}
	}

	void CheckPickUp(GameObject o){
		if(o.layer == 6) { //6 == pickup point layer
			if(o.tag == "ExpPoint") 	  player.GainXP(1);
			if(o.tag == "DayCorePoint")   player.special.GainCoreCharge ("Day", 2);
			if(o.tag == "NightCorePoint") player.special.GainCoreCharge ("Night", 2);
			Destroy(o);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		player = Game.control.player;
		CheckHazard(c.gameObject.tag);
		CheckPickUp(c.gameObject); 
	}
}
