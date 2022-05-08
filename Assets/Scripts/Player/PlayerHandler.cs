using UnityEngine;
using System.Collections;

public class PlayerStats {

	public int maxLives;
	public int xp;
	public int xpCap;
	public int level;
	public int upgradePoints;

	public float movementSpeed;
	public float damage;
	public float damageMin;
	public float bulletScaleMin;
	public float shootSpeed;
	public float bulletScale;

	public long score;
	public long hiScore;

	public PlayerStats(){
		damageMin = 1.3f;

		maxLives = 5;
		xpCap = 5;
		xp = 0;
		level = 1;
		damage = damageMin;
		bulletScaleMin = 1f;
		bulletScale = bulletScaleMin;

		movementSpeed = 12f;
		shootSpeed = .1f;

		upgradePoints = 0;
	}
}

public class PlayerHandler : MonoBehaviour {

	public PlayerStats stats;
	public PlayerShoot combat;
	public PlayerMovement movement;
	public PlayerLife health;
	public PlayerSpecialAttack special;
	public GameObject hitBox;


	void Awake(){
		combat = GetComponent<PlayerShoot>();
		movement = GetComponent<PlayerMovement>();
		health = GetComponent<PlayerLife>();
		special = GetComponent<PlayerSpecialAttack>();
	}

	public void Init(){
		if(stats == null) stats = new PlayerStats();

		health.Init ();
		combat.Init ();
	}


	public void GainScore(int gained){
		stats.score += gained * Game.control.stageHandler.difficultyMultiplier;
		if (stats.score >= stats.hiScore) {
			stats.hiScore = stats.score;
			Game.control.ui.UpdateHiScore (stats.hiScore);
		}
		Game.control.ui.UpdateScore (stats.score);
	}



	//////////////////////////////////////////////////
	//////////////////////////////////////////////////
	//NOT USED
	////////////////////////////////////////////////
	public void GainXP(int gainedXP)
	{
		stats.xp += gainedXP;

		if(stats.xp >= stats.xpCap) {
			stats.xp = stats.xp - stats.xpCap;
			stats.xpCap += 5;
			LevelUp();
		}

		Game.control.ui.UpdateStatPanel("XP", stats.xp);
	}

	void LevelUp(){
		stats.level++;
		stats.upgradePoints++;
		Game.control.ui.UpdateStatPanel ("UpgradePoints", stats.upgradePoints);
	}

}
