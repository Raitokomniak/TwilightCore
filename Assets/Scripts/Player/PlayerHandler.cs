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
	public float dayShootSpeed;
    public float nightShootSpeed;
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

		movementSpeed = 15f;
		dayShootSpeed = .1f;
        nightShootSpeed = .06f;

		upgradePoints = 0;
	}
}

public class PlayerHandler : MonoBehaviour {

	public PlayerShoot combat;
	public PlayerMovement movement;
	public PlayerLife health;
	public PlayerSpecialAttack special;
    public MiniToast miniToaster;

	void Awake(){
		combat = GetComponent<PlayerShoot>();
		movement = GetComponent<PlayerMovement>();
		health = GetComponent<PlayerLife>();
		special = GetComponent<PlayerSpecialAttack>();
        miniToaster = GetComponentInChildren<MiniToast>();
	}

	public void Init(){
		transform.position = Game.control.vectorLib.GetVector("X8");
		health.Init ();
		combat.Init ();
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateScore (Game.control.stageHandler.stats.score);
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateLives(Game.control.stageHandler.stats.lives);
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateXP(Game.control.stageHandler.stats.xp);
	}

	public long GainScore(int gained){
		long gainedScore = gained * Game.control.stageHandler.difficultyMultiplier;
		Game.control.stageHandler.stats.score += gainedScore;
		if (Game.control.stageHandler.stats.score >= Game.control.stageHandler.stats.hiScore) {
			Game.control.stageHandler.stats.hiScore = Game.control.stageHandler.stats.score;
			Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateHiScore (Game.control.stageHandler.stats.hiScore);
		}
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateScore (Game.control.stageHandler.stats.score);

		return gainedScore;
	}


	public void GainXP(int gainedXP)
	{
		PlayerStats stats = Game.control.stageHandler.stats;
		stats.xp += gainedXP;
		miniToaster.PlayToast("XP");

		if(stats.xp >= stats.xpCap) {
			stats.xp = stats.xp - stats.xpCap;
			stats.xpCap += 50;
			stats.lives += 1;
			health.GainLife();
		}

		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateXP(Game.control.stageHandler.stats.xp);
	}


	/////DEBUG

	public void DebugFillCores(){
		//POWERUP DOESNT TAKE INTO ACCOUNT BIG AMOUNTS OF CORE POINT GAINS
		for(int i = 0; i < special.coreCap; i++){
			special.GainCoreCharge("Day", i);
			special.GainCoreCharge("Night", i);
		}
		
	}
}
