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

    public int dayCorePoints;
	public int dayCoreLevel;
	public int nightCorePoints;
	public int nightCoreLevel;

	public long score;
	public long hiScore;

	public PlayerStats(){
		damageMin = 1.3f;

		maxLives = 10;
		lives = 5;
		xpCap = 30;
		xp = 0;
		level = 1;
		damage = damageMin;
		bulletScaleMin = 1f;
		bulletScale = bulletScaleMin;

        dayCorePoints = 0;
	    dayCoreLevel = 0;
	    nightCorePoints = 0;
	    nightCoreLevel = 0;

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
        special.StageInit();
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateScore (Game.control.stageHandler.stats.score);
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateLives(Game.control.stageHandler.stats.lives);
		Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateXP(Game.control.stageHandler.stats.xp, Game.control.stageHandler.stats.xpCap);
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




	/////DEBUG

	public void DebugFillCores(){
		//POWERUP DOESNT TAKE INTO ACCOUNT BIG AMOUNTS OF CORE POINT GAINS
		for(int i = 0; i < special.coreCap / 2; i++){
			special.GainCoreCharge("Day", i);
			special.GainCoreCharge("Night", i);
		}
		
	}
}
