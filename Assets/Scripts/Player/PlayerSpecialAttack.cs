using UnityEngine;
using System.Collections;

public class PlayerSpecialAttack : MonoBehaviour {

	PlayerMovement movement;
	PlayerShoot shoot;
	public bool specialAttack;
	public float specialAttackTime;

	public GameObject nightSpecial;
	public GameObject daySpecial;

	public GameObject nightAnimatedSprite;
	public GameObject dayAnimatedSprite;

	GameObject starLightBomb;

	public int dayCorePoints;
	public int dayCoreLevel;
	public int nightCorePoints;
	public int nightCoreLevel;
	public int coreCap;
	public int dayCoreThreshold;
	public int nightCoreThreshold;



	void Awake(){
		movement = GetComponent<PlayerMovement>();
		shoot = GetComponent<PlayerShoot>();

		starLightBomb = Resources.Load ("Prefabs/StarLightBomb") as GameObject;

		dayCoreLevel = 0;
		dayCorePoints = 0;
		nightCoreLevel = 0;
		nightCorePoints = 0;

		coreCap = 100;
		dayCoreThreshold = 20;
		nightCoreThreshold = 20;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.X) && CanUseSpecial())
		{
			if (!movement.focusMode && dayCorePoints >= 20)
				StartCoroutine(SpecialAttack ("Day"));
			else if (movement.focusMode && nightCorePoints >= 20)
				StartCoroutine(SpecialAttack ("Night"));
			else
				Game.control.ui.PlayToast("Not enough points");
		}
	}

	bool CanUseSpecial(){
		if(Game.control.stageHandler.loading) return false;
		if(Game.control.menu.menuOn) return false;
		if(specialAttack) return false;
		if(Game.control.dialog.handlingDialog) return false;
		return true;
	}

	IEnumerator SpecialAttack(string core)
	{
		specialAttackTime = 3f;
		specialAttack = true;

		DepleteCore (core, true);

		if (core == "Day") {
			Game.control.ui.ShowActivatedPlayerPhase ("Day Core: Dawnbreaker");
			daySpecial.SetActive (true);
			dayAnimatedSprite.GetComponent<AnimationController> ().Scale (1, 1.5f, true, false);
			dayAnimatedSprite.GetComponent<AnimationController> ().rotating = true;
			yield return new WaitForSeconds (1.5f);
			daySpecial.SetActive (false);
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
			yield return new WaitForSeconds (specialAttackTime);
			Game.control.enemySpawner.DestroyAllProjectiles();
			daySpecial.SetActive (false);
		
		} else if(core == "Night") {
			Game.control.ui.ShowActivatedPlayerPhase ("Night Core: Trick or Treat");

			nightSpecial.SetActive (true);
			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (1, 2, true, true);

			yield return new WaitForSeconds (specialAttackTime);

			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (-1, 2f, true, true);
			nightSpecial.SetActive (false);
		}

		specialAttack = false;
	}

	public void DepleteCore(string core, bool special){
		int points = 0;
		int limit;

		if (core == "Day") {
			points = dayCorePoints;
		} else {
			points = nightCorePoints;
		}

		limit = points - 20;
		if(limit < 0) limit = 0;
		
		if (special) {
			Game.control.ui.LEFT_SIDE_PANEL.DepleteCoreCharge(core, 4f, points, limit);
			points = limit;
		}

		if (!movement.focusMode) {
			dayCorePoints = limit;
		} else {
			nightCorePoints = limit;
		}

		Game.control.ui.LEFT_SIDE_PANEL.DepleteCoreCharge(core, 4f, points, limit);
		//Game.control.ui.LEFT_SIDE_PANEL.UpdateCoreCharge (core, points);
		PowerUpdate (core, false);
	}

	public void GainCoreCharge(string core, int gainedCharge)
	{
		int multiplier = 0;
		if (core == "Day") 		  multiplier = dayCoreLevel + 1;
		else if (core == "Night") multiplier = nightCoreLevel + 1;


		Game.control.player.GainScore (100 * multiplier);

		int corePoints = 0;

		if (core == "Day") {
			if (dayCorePoints < 100) dayCorePoints += gainedCharge;
			if (dayCorePoints > 100) dayCorePoints = 100;
			
			if (dayCorePoints > dayCoreThreshold * multiplier && dayCorePoints <= 100) {
				PowerUpdate ("Day", true);
			} 

			corePoints = dayCorePoints;

		} else if (core == "Night") {
			if (nightCorePoints < 100) nightCorePoints += gainedCharge;
			if (nightCorePoints > 100) nightCorePoints = 100;

			if (nightCorePoints > nightCoreThreshold * multiplier && nightCorePoints <= 100) {
				PowerUpdate ("Night", true);
			}

			corePoints = nightCorePoints;
		}

		GetComponent<MiniToast>().PlayCorePointToast(gainedCharge, core);
		Game.control.ui.LEFT_SIDE_PANEL.UpdateCoreCharge (core, corePoints);
	}

	public void PowerUpdate(string core, bool up){
		if (core == "Day") {
			if (up) dayCoreLevel++;
			else
				if (dayCoreLevel != 0) dayCoreLevel--;
		} 
		else if (core == "Night") {
			if (up) nightCoreLevel++;
			else 
				if (nightCoreLevel != 0) nightCoreLevel--;
		}

		Game.control.ui.LEFT_SIDE_PANEL.UpdatePower("Day", dayCoreLevel);
		Game.control.ui.LEFT_SIDE_PANEL.UpdatePower("Night", nightCoreLevel);

		//Debug.Log("day core level " + dayCoreLevel);
		//Debug.Log("night core level " + nightCoreLevel);
		shoot.UpdateShootLevel (dayCoreLevel, nightCoreLevel);
	}

}
