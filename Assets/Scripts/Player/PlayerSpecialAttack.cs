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
		if(Game.control.menu.menuOn) return false;
		if(specialAttack) return false;
		if(Game.control.dialog.handlingDialog) return false;
		return true;
	}

	IEnumerator SpecialAttack(string core)
	{
		specialAttackTime = 4f;
		specialAttack = true;

		DepleteCore (true);

		if (core == "Day") {
			Game.control.ui.ShowActivatedPlayerPhase ("Day Core: StarLight Special");
			daySpecial.SetActive (true);
			dayAnimatedSprite.GetComponent<AnimationController> ().Scale (1, .5f, true, false);
			dayAnimatedSprite.GetComponent<AnimationController> ().rotating = true;
			yield return new WaitForSeconds (1.5f);
			daySpecial.SetActive (false);
			ArrayList bombs = new ArrayList ();

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
			}
			Game.control.enemySpawner.DestroyAllProjectiles();
			daySpecial.SetActive (false);
		
		} else if(core == "Night") {
			Game.control.ui.ShowActivatedPlayerPhase ("Night Core: Trick or Treat");

			nightSpecial.SetActive (true);
			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (1, 2.5f, true, true);

			yield return new WaitForSeconds (specialAttackTime);

			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (-1, 2.5f, true, true);
			nightSpecial.SetActive (false);
		}

		specialAttack = false;
	}

	public void DepleteCore(bool special){
		int points = 0;
		int limit;
		int threshold = 0;
		string core = "";

		if (!movement.focusMode) {
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
			Game.control.ui.LEFT_SIDE_PANEL.DepleteCoreCharge(core, 4f, points, limit);
			points = limit;
		}

		if (!movement.focusMode) {
			dayCorePoints = limit;
			dayCoreThreshold = threshold;
		} else {
			nightCorePoints = limit;
			nightCoreThreshold = threshold;
		}

		Game.control.ui.LEFT_SIDE_PANEL.UpdateCoreCharge (core, points);
		PowerUpdate (core, false);
	}

	public void GainCoreCharge(string core, int gainedCharge)
	{
		int multiplier = 0;
		if (core == "Day")
			multiplier = dayCoreLevel + 1;
		else if (core == "Night")
			multiplier = nightCoreLevel + 1;


		Game.control.player.GainScore (100 * multiplier);

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

		GetComponent<MiniToast>().PlayCorePointToast(gainedCharge, core);
		Game.control.ui.LEFT_SIDE_PANEL.UpdateCoreCharge (core, corePoints);
	}

	public void PowerUpdate(string core, bool up){
		if (core == "Day") {
			if (up) {
				dayCoreLevel++;
				if (dayCoreLevel > nightCoreLevel) {
					shoot.shootLevel = dayCoreLevel;
					Game.control.ui.PlayToast ("Power Up!");
				}
			} else {
				if (dayCoreLevel != 0)
					dayCoreLevel--;
				if (dayCoreLevel > nightCoreLevel)
					shoot.shootLevel = dayCoreLevel;
			}

			Game.control.ui.LEFT_SIDE_PANEL.UpdatePower("Day", dayCoreLevel);
		} else if (core == "Night") {
			if (up) {
				nightCoreLevel++;
				if (dayCoreLevel <= nightCoreLevel) {
					shoot.shootLevel = nightCoreLevel;
					Game.control.ui.PlayToast ("Power Up!");
				}
			} else {
				if (nightCoreLevel != 0)
					nightCoreLevel--;
				if (nightCoreLevel > dayCoreLevel)
					shoot.shootLevel = nightCoreLevel;
			}

			Game.control.ui.LEFT_SIDE_PANEL.UpdatePower("Night", nightCoreLevel);
		}

		if (!up) {
			Game.control.ui.PlayToast ("Power Down");
		}
			
		shoot.UpdateShootLevel ();
	}

}
