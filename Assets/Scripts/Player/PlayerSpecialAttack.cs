using UnityEngine;
using System.Collections;

public class PlayerSpecialAttack : MonoBehaviour {

    PlayerHandler playerHandler;
	PlayerMovement movement;
	PlayerShoot shoot;
	public bool specialAttack;
	public float specialAttackTime;

	public GameObject nightSpecial;
	public GameObject daySpecial;

	public GameObject nightAnimatedSprite;
	public GameObject dayAnimatedSprite;

    int dayCoreLevel;
    int dayCorePoints;
    int nightCoreLevel;
    int nightCorePoints;

	public int coreCap;
	public int dayCoreThreshold;
	public int nightCoreThreshold;
    
    public float specialScale;


	void Awake(){
		movement = GetComponent<PlayerMovement>();
		shoot = GetComponent<PlayerShoot>();
        playerHandler = GetComponent<PlayerHandler>();

		//starLightBomb = Resources.Load ("Prefabs/StarLightBomb") as GameObject;

		coreCap = 150;
		dayCoreThreshold = coreCap / 5;
		nightCoreThreshold = coreCap / 5;
	}

    public void GameInit(){
        dayCoreLevel = 0;
		dayCorePoints = 0;
		nightCoreLevel = 0;
		nightCorePoints = 0;

        SaveToStats();
    }

    public void StageInit(){
        dayCoreLevel = Game.control.stageHandler.stats.dayCoreLevel;
        dayCorePoints = Game.control.stageHandler.stats.dayCorePoints;
        nightCoreLevel = Game.control.stageHandler.stats.nightCoreLevel;
        nightCorePoints = Game.control.stageHandler.stats.nightCorePoints;

        Game.control.stageUI.LEFT_SIDE_PANEL.UpdateCoreCharge ("Day", dayCorePoints);
        Game.control.stageUI.LEFT_SIDE_PANEL.UpdateCoreCharge ("Night", nightCorePoints);
        Game.control.stageUI.LEFT_SIDE_PANEL.UpdatePower("Day", dayCoreLevel);
        Game.control.stageUI.LEFT_SIDE_PANEL.UpdatePower("Night", nightCoreLevel);
    }

    public void SaveToStats(){
        Game.control.stageHandler.stats.dayCoreLevel = dayCoreLevel;
        Game.control.stageHandler.stats.dayCorePoints = dayCorePoints;
        Game.control.stageHandler.stats.nightCoreLevel = nightCoreLevel;
        Game.control.stageHandler.stats.nightCorePoints = nightCorePoints;
    }

    public void CheckUsedSpecial(){
	if (!movement.focusMode && dayCorePoints >= 20)
				StartCoroutine(SpecialAttack ("Day"));
			else if (movement.focusMode && nightCorePoints >= 20)
				StartCoroutine(SpecialAttack ("Night"));
			else
				Game.control.stageUI.PlayToast("Not enough points for special attack");
    }

	public bool CanUseSpecial(){
        if(Game.control.pause.playerHitTimerOn) return false;
		if(Game.control.loading) return false;
		if(Game.control.menu.menuOn) return false;
		if(specialAttack) return false;
		if(Game.control.dialog.handlingDialog) return false;
        if(!Game.control.stageHandler.stageOn) return false;
		return true;
	}

	IEnumerator SpecialAttack(string core)
	{
        Game.control.stageHandler.DenyBossSurvivalBonus();
        
        GetComponent<PlayerLife>().invulnerable = true;
		specialAttackTime = 3f;
		specialAttack = true;

		DepleteCore (core, true);
        PowerUpdate (core, false);

		if (core == "Day") {
			Game.control.stageUI.WORLD.ShowFXLayer("Light");
			Game.control.sound.PlaySpellSound("Player", "DayCore1");
			Game.control.stageUI.RIGHT_SIDE_PANEL.PlayerSpecialToast (true, "Day Core: Dawnbreaker");
			daySpecial.SetActive (true);
            specialScale = .3f * (dayCoreLevel + 1);
			dayAnimatedSprite.GetComponent<AnimationController> ().Scale (true, specialScale, true, false);
			dayAnimatedSprite.GetComponent<AnimationController> ().StartRotating(4f);
            

			yield return new WaitForSeconds (specialAttackTime);
            dayAnimatedSprite.GetComponent<AnimationController> ().Scale (false, specialScale, true, false);
			daySpecial.SetActive (false);
			Game.control.stageUI.WORLD.HideFXLayer();


			//UNUSED STARBOMB SPECIAL, NOT SURE IF WANNA SCRAP YET

/*			ArrayList bombs = new ArrayList ();

			for (int i = 0; i < 10; i++) {
				GameObject bomb = (GameObject)Instantiate (starLightBomb, new Vector3(-6 + Random.Range(-12, 12), 0 + Random.Range(-10, 10), 0), transform.rotation);
				bomb.SetActive (true);
				bomb.tag = "NullField";
				bomb.GetComponent<AnimationController> ().rotating = true;
				yield return new WaitForSeconds (.1f);
				bomb.GetComponent<AnimationController> ().Scale (1, 3, true, false);
				bombs.Add (bomb);
			}
			yield return new WaitForSeconds (specialAttackTime);

			foreach (GameObject bomb in bombs) {
				bomb.GetComponent<AnimationController> ().Scale (-1, 3, false, false);
				yield return new WaitForSeconds (.1f);
				Destroy (bomb);
			}*/

		} else if(core == "Night") {
			Game.control.stageUI.WORLD.ShowFXLayer("Night");
			//Game.control.ui.EffectOverlay("NightCore", true, 2);
			Game.control.sound.PlaySpellSound("Player", "NightCore1");
			Game.control.stageUI.RIGHT_SIDE_PANEL.PlayerSpecialToast (false, "Night Core: Trick or Treat");

            specialScale = .4f * (nightCoreLevel + 1);
			nightSpecial.SetActive (true);
			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (true, specialScale, true, true);

			yield return new WaitForSeconds (specialAttackTime);

			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (false, specialScale, true, true);
			//Game.control.ui.EffectOverlay("NightCore", false, 2);
			nightSpecial.SetActive (false);
			Game.control.stageUI.WORLD.HideFXLayer();
			
		}
        
        GetComponent<PlayerLife>().invulnerable = false;
		specialAttack = false;
	}

	public void DepleteCore(string core, bool special){
		int points = 0;
		int limit;
		int threshold;

		if (core == "Day") {
			points = dayCorePoints;
			threshold = dayCoreThreshold;
		} else {
			points = nightCorePoints;
			threshold = nightCoreThreshold;
		}

		limit = points - threshold;
		if(limit < 0) limit = 0;
		
		Game.control.stageUI.LEFT_SIDE_PANEL.DepleteCoreCharge(core, 4f, points, limit);
		
		if (core == "Day") dayCorePoints = limit;
		else nightCorePoints = limit;

		if(!special) PowerUpdate (core, false);
	}



	public void GainCoreCharge(string core, int gainedCharge)
	{
		int multiplier = 1;
		int corePoints = 0;
		int threshold = 0;

		if (core == "Day")   {
			multiplier = dayCoreLevel + 1;
			corePoints = dayCorePoints;
			threshold = dayCoreThreshold * multiplier;
		}
		else if (core == "Night") {
			multiplier = nightCoreLevel + 1;
			corePoints = nightCorePoints;
			threshold = nightCoreThreshold * multiplier;
		}

		if 		(corePoints < coreCap) corePoints += gainedCharge;
		else if (corePoints > coreCap) corePoints = coreCap;

		if(corePoints > threshold && corePoints < coreCap) PowerUpdate (core, true);
		
		if(core == "Day") 	 dayCorePoints = corePoints;
		if (core == "Night") nightCorePoints = corePoints;

	    playerHandler.miniToaster.PlayToast(core + "Core");
		Game.control.stageUI.LEFT_SIDE_PANEL.UpdateCoreCharge (core, gainedCharge);

        if(dayCoreLevel == 5 && nightCoreLevel == 5) multiplier *= 10; //extra score for full power
		Game.control.player.GainScore (coreCap * multiplier);
	}

	public void PowerUpdate(string core, bool up){
		if (core == "Day") {
			if (up) dayCoreLevel++;
			else if (dayCoreLevel != 0) dayCoreLevel--;
		} 
		else if (core == "Night") {
			if (up) nightCoreLevel++;
			else if (nightCoreLevel != 0) nightCoreLevel--;
		}

		Game.control.stageUI.LEFT_SIDE_PANEL.UpdatePower("Day", dayCoreLevel);
		Game.control.stageUI.LEFT_SIDE_PANEL.UpdatePower("Night", nightCoreLevel);
        
		shoot.UpdateShootLevel (dayCoreLevel, nightCoreLevel);
	}

}
