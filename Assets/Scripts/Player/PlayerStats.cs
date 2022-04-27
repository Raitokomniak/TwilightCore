using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public PlayerShoot playerShoot;
	public PlayerMovement playerMovement;
	public PlayerLife playerLife;

	public int lives;
	public int xp;
	public int xpCap;
	public int level;
	public int upgradePoints;

	public int dayCorePoints;
	public int dayCoreLevel;
	public int nightCorePoints;
	public int nightCoreLevel;
	public int coreCap;
	public int dayCoreThreshold;
	public int nightCoreThreshold;

	public float movementSpeed;
	public float damage;
	float damageMin;
	float bulletScaleMin;
	public float shootSpeed;
	public float bulletScale;
	public int shootLevel;

	public int damagePoints;
	public int speedPoints;
	public int scalePoints;

	public int genPoints;

	public int powerCap;
	public int speedCap;
	public int scaleCap;

	long score;
	long hiScore;

	public void Init(){
		damageMin = 1f;

		lives = 3;
		xpCap = 5;
		xp = 0;
		level = 1;
		damage = damageMin;
		bulletScaleMin = 1f;
		bulletScale = bulletScaleMin;


		movementSpeed = 0.05f;
		shootSpeed = .1f;
		shootLevel = 0;

		upgradePoints = 0;
		dayCoreLevel = 0;
		dayCorePoints = 0;
		nightCoreLevel = 0;
		nightCorePoints = 0;

		coreCap = 100;
		dayCoreThreshold = 20;
		nightCoreThreshold = 20;

		playerLife.InitLife ();
		playerShoot.InitShoot ();
	}


	public void GainCoreCharge(string core, int gainedCharge)
	{
		int multiplier = 0;
		if (core == "Day")
			multiplier = dayCoreLevel + 1;
		else if (core == "Night")
			multiplier = nightCoreLevel + 1;


		GainScore (100 * multiplier);

		int corePoints = 0;

		if (core == "Day") {
			if (dayCorePoints < 100)
				dayCorePoints += gainedCharge;
			if (dayCorePoints > dayCoreThreshold && dayCorePoints <= 100) {
				if (dayCorePoints != 100)
					dayCoreThreshold += 20;

				PowerUpdate ("Day", true);
			} 

			corePoints = dayCorePoints;

		} else if (core == "Night") {
			if (nightCorePoints < 100)
				nightCorePoints += gainedCharge;

			if (nightCorePoints > nightCoreThreshold && nightCorePoints <= 100) {
				if (nightCorePoints != 100)
					nightCoreThreshold += 20;

				PowerUpdate ("Night", true);

			}
			corePoints = nightCorePoints;
		}

		GameController.gameControl.ui.UpdateCoreCharge (core, corePoints);
	}

	public void PowerUpdate(string core, bool up){

		if (core == "Day") {
			if (up) {
				dayCoreLevel++;
				if (dayCoreLevel > nightCoreLevel) {
					shootLevel = dayCoreLevel;
					GameController.gameControl.ui.PlayToast ("PowerUp");
				}
			} else {
				if (dayCoreLevel != 0)
					dayCoreLevel--;
				if (dayCoreLevel > nightCoreLevel)
					shootLevel = dayCoreLevel;
			}
		} else if (core == "Night") {
			if (up) {
				nightCoreLevel++;
				if (dayCoreLevel <= nightCoreLevel) {
					shootLevel = nightCoreLevel;
					GameController.gameControl.ui.PlayToast ("PowerUp");
				}
			} else {
				if (nightCoreLevel != 0)
					nightCoreLevel--;
				if (nightCoreLevel > dayCoreLevel)
					shootLevel = nightCoreLevel;
			}
		}

		if (!up) {
			GameController.gameControl.ui.PlayToast ("PowerDown");
		}
			

		playerShoot.UpdateShootLevel ();
	}


	public void DepleteCore(bool special){
		int points = 0;
		int limit;
		int threshold = 0;
		string core = "";

		if (!playerMovement.focusMode) {
			threshold = dayCoreThreshold;
			points = dayCorePoints;
			core = "Day";
		} else {
			threshold = nightCoreThreshold;
			points = nightCorePoints;
			core = "Night";
		}

		if (!special) {
			if (threshold != 20) {
				threshold -= 20;
				limit = points - 20;
			} else {
				limit = 0;
			}
		} else {
			if (threshold != 20) {
				threshold -= 20;
				limit = points - 20;
			} else {
				limit = 0;
			}
		}

		if (special) {

			StartCoroutine (GameController.gameControl.ui.DepleteCoreCharge (core, 4f, points, limit));
			points = limit;
		}

		if (!playerMovement.focusMode) {
			dayCorePoints = limit;
			dayCoreThreshold = threshold;
		} else {
			nightCorePoints = limit;
			nightCoreThreshold = threshold;
		}

		GameController.gameControl.ui.UpdateCoreCharge (core, points);
		PowerUpdate (core, false);
	}


	public void GainScore(int gained){
		score += gained;
		if (score >= hiScore) {
			hiScore = score;
			GameController.gameControl.ui.UpdateHiScore (hiScore);
		}
		GameController.gameControl.ui.UpdateScore (score);
	}

	//////////////////////////////////////////////////
	//////////////////////////////////////////////////
	//NOT USED
	////////////////////////////////////////////////
	public void GainXP(int gainedXP)
	{
		xp += gainedXP;

		if(xp >= xpCap) {
			xp = xp - xpCap;
			xpCap += 5;
			LevelUp();
		}

		GameController.gameControl.ui.UpdateStatPanel("XP", xp);
	}

	void LevelUp(){
		level++;
		upgradePoints++;
		GameController.gameControl.ui.UpdateStatPanel ("UpgradePoints", upgradePoints);
	}



	void UpgradeSpeed(){
		if(shootSpeed > 0.15f)		shootSpeed -= .06f;
	}

}
