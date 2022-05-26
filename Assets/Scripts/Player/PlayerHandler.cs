using UnityEngine;
using System.Collections;

public class PlayerStats {

	public int maxLives;
	public int lives;
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
		lives = maxLives;
		xpCap = 30;
		xp = 0;
		level = 1;
		damage = damageMin;
		bulletScaleMin = 1f;
		bulletScale = bulletScaleMin;

		movementSpeed = 12f;
		shootSpeed = .1f;

		upgradePoints = 0;
	}

/*
	public PlayerStats(PlayerStats ps){
		damageMin = ps.damageMin;

		maxLives = ps.maxLives;
		lives = ps.lives;
		xpCap = ps.xpCap;
		xp = ps.xp;
		level = ps.level;
		damage = ps.damage;
		bulletScaleMin = ps.bulletScaleMin;
		bulletScale = ps.bulletScale;

		movementSpeed = ps.movementSpeed;
		shootSpeed = ps.shootSpeed;

		upgradePoints = ps.upgradePoints;
	}*/
}

public class PlayerHandler : MonoBehaviour {

	public PlayerShoot combat;
	public PlayerMovement movement;
	public PlayerLife health;
	public PlayerSpecialAttack special;


	void Awake(){
		combat = GetComponent<PlayerShoot>();
		movement = GetComponent<PlayerMovement>();
		health = GetComponent<PlayerLife>();
		special = GetComponent<PlayerSpecialAttack>();
	}

	public void Init(){
		health.Init ();
		combat.Init ();
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateScore (Game.control.stageHandler.stats.score);
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateLives(Game.control.stageHandler.stats.lives);
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateXP(Game.control.stageHandler.stats.xp);
	}

	public long GainScore(int gained){
		long gainedScore = gained * Game.control.stageHandler.difficultyMultiplier;
		Game.control.stageHandler.stats.score += gainedScore;
		if (Game.control.stageHandler.stats.score >= Game.control.stageHandler.stats.hiScore) {
			Game.control.stageHandler.stats.hiScore = Game.control.stageHandler.stats.score;
			Game.control.ui.RIGHT_SIDE_PANEL.UpdateHiScore (Game.control.stageHandler.stats.hiScore);
		}
		Game.control.ui.RIGHT_SIDE_PANEL.UpdateScore (Game.control.stageHandler.stats.score);

		return gainedScore;
	}


	public void GainXP(int gainedXP)
	{
		PlayerStats stats = Game.control.stageHandler.stats;
		stats.xp += gainedXP;
		GetComponent<MiniToast>().PlayXPToast(gainedXP);

		if(stats.xp >= stats.xpCap) {
			stats.xp = stats.xp - stats.xpCap;
			stats.xpCap += 50;
			stats.lives += 1;
			GetComponent<PlayerLife>().GainLife();
		}

		Game.control.ui.RIGHT_SIDE_PANEL.UpdateXP(Game.control.stageHandler.stats.xp);
	}
}
