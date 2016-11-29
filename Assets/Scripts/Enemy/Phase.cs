using System;
using System.Collections;
using UnityEngine;

public class Phase : MonoBehaviour {

	public ArrayList movementPatterns;
	public bool endOfPhase;

	ArrayList patterns;

	Pattern p1;
	Pattern p2;
	Pattern p3;
	Pattern p4;
	Pattern p5;

	EnemyMovementPattern mp1;
	EnemyMovementPattern mp2;
	EnemyMovementPattern mp3;
	EnemyMovementPattern mp4;
	EnemyMovementPattern mp5;

	IEnumerator numerator;

	void Awake(){
		endOfPhase = true;
	}

	void Update(){
		
	}

	public Phase(){
		movementPatterns = new ArrayList ();
		patterns = new ArrayList ();
	}

	public IEnumerator Execute(string boss, int phase, EnemyShoot enemy, EnemyMovement enemyMove){
		endOfPhase = false;
		EnemyLib lib = GameController.gameControl.enemyLib;

		patterns.Clear ();

		p1 = null;
		p2 = null;
		p3 = null;
		p4 = null;
		p5 = null;

		mp1 = null;
		mp2 = null;
		mp3 = null;
		mp4 = null;
		mp5 = null;
		GameController.gameControl.ui.UpdateTopPlayer ("Boss" + enemy.wave.bossIndex + "_" + (phase));

//		Debug.Log ("executing phase " + phase);
		switch(boss)
		{
		case "Boss0.5":
			switch(phase)
			{
			case 0:
				enemy.NextBossPhase ();
				break;
			case 1:
				GameController.gameControl.ui.UpdateTopPlayer (1f);

				enemy.enemyLife.SetInvulnerable (true);
				p1 = new Pattern (lib.spiral);
				p1.Customize (new BulletMovementPattern (true, "WaitAndExplode", 5f, p1, 0, 14));
				p1.Customize ("LoopCircles", 1440);
				p1.Customize ("BulletCount", 100);
				p1.SetSprite ("Circle", "Glow", "Green");

				p2 = new Pattern (lib.maelStrom);
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.Customize ("RotationDirection", 1);
				p2.SetSprite ("Circle", "Glow", "Green");

				p3 = new Pattern (lib.maelStrom);
				p3.SetSprite ("Circle", "Glow", "Yellow");
				p3.Customize (new BulletMovementPattern (true, "Explode", 6f, p3, 0, 14));
				p3.Customize ("RotationDirection", -1);

				mp1 = new EnemyMovementPattern (lib.centerHor);
				mp1.Customize ("Speed", 7f);

				enemyMove.SetUpPatternAndMove (mp1);
				for (int i = 0; i < 5; i++) {
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (p2);
					enemy.BossShoot (p3);
					yield return new WaitForSeconds (2.2f);
					p2.stop = true;
					p3.stop = true;
				}

				mp1 = new EnemyMovementPattern ("Leaving", new Vector3 (lib.centerX, 13, 0), false, 0);
				enemyMove.SetUpPatternAndMove (mp1);
				yield return new WaitForSeconds(2f);
				enemy.enemyLife.Die ();


				p2.stop = true;
				p3.stop = true;


				break;
			}
			break;
		//BOSS 1 - FOREST GUARDIAN
		case "Boss1":
			switch (phase) {
			case 0:
				p1 = new Pattern (lib.spiral);
				p1.Customize (new BulletMovementPattern (true, "WaitAndExplode", 5f, p1, 0, 14));
				p1.Customize ("LoopCircles", 1440);
				p1.Customize ("BulletCount", 100);
				p1.SetSprite ("Circle", "Glow", "Green");

				p2 = new Pattern (lib.maelStrom);
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.Customize ("RotationDirection", 1);
				p2.SetSprite ("Circle", "Glow", "Green");

				p3 = new Pattern (lib.maelStrom);
				p3.SetSprite ("Circle", "Glow", "Yellow");
				p3.Customize (new BulletMovementPattern (true, "Explode", 6f, p3, 0, 14));
				p3.Customize ("RotationDirection", -1);

				mp1 = new EnemyMovementPattern (lib.centerHor);
				mp1.Customize ("Speed", 7f);
				mp2 = new EnemyMovementPattern (lib.rocking);
				mp2.Customize ("Speed", 7f);

				while (!endOfPhase) {

					enemyMove.SetUpPatternAndMove (mp1);
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (p2);
					enemy.BossShoot (p3);
					yield return new WaitForSeconds(2.2f);
					p2.stop = true;
					p3.stop = true;
					if (endOfPhase)
						break;
					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.rocking));
					yield return new WaitForSeconds(1f);
					enemy.BossShoot (p1);
					yield return new WaitForSeconds(2.2f);
					enemy.BossShoot (p1);
					yield return new WaitForSeconds(3f);
				}
				p2.stop = true;
				p3.stop = true;
				break;

			case 1:
				GameController.gameControl.sound.PlaySpellSound ("Enemy");
				GameController.gameControl.ui.ShowActivatedPhase ("Boss", "Justice Seal: Ninetailed Spear");

				p1 = new Pattern (lib.curtain);
				p1.Customize ("BulletCount", 9);
				p1.SetSprite ("Circle", "Bevel", "Lilac");
				p1.Customize (new BulletMovementPattern (false, "TurnToSpears", 6f, p1, 0, 14));

				p2 = new Pattern (lib.maelStrom);
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.Customize ("RotationDirection", 1);
				p2.SetSprite ("Circle", "Glow", "Green");

				p3 = new Pattern (lib.maelStrom);
				p3.SetSprite ("Circle", "Glow", "Yellow");
				p3.Customize (new BulletMovementPattern (true, "Explode", 4f, p3, 0, 14));
				p3.Customize ("RotationDirection", -1);

				mp1 = new EnemyMovementPattern ("", new Vector3 (-15, 6f, 0f), false, 0);
				mp1.Customize ("Speed", 7f);

				mp2 = new EnemyMovementPattern ("", new Vector3 (1, 6, 0), false, 0);
				mp2.Customize ("Speed", 7f);



				enemyMove.SetUpPatternAndMove (mp1);
				yield return new WaitForSeconds (2.2f);
				while (!endOfPhase) {
					yield return new WaitUntil (() => mp1.CheckIfReachedDestination (enemyMove) == true);
					enemyMove.SetUpPatternAndMove (mp2);
					enemy.BossShoot (p1);
					yield return new WaitUntil (() => mp1.CheckIfReachedDestination (enemyMove) == true);
					yield return new WaitForSeconds (1f);
					enemy.BossShoot (p2);
					enemy.BossShoot (p3);
					yield return new WaitForSeconds (2.2f);
					p2.stop = true;
					p3.stop = true;
					yield return new WaitForSeconds (1f);
					enemyMove.SetUpPatternAndMove (mp1);
					enemy.BossShoot (p1);
					yield return new WaitUntil (() => mp1.CheckIfReachedDestination (enemyMove) == true);
					yield return new WaitForSeconds (1f);
					enemy.BossShoot (p2);
					enemy.BossShoot (p3);
					yield return new WaitForSeconds (2.2f);
					p2.stop = true;
					p3.stop = true;
					yield return new WaitForSeconds (1f);
				}

				break;

			case 2:
				mp1 = new EnemyMovementPattern ("", new Vector3 (lib.centerX + 4f, enemy.transform.position.y, 0), false, 0);
				mp1.Customize ("Teleport", 1);
				mp2 = new EnemyMovementPattern ("", new Vector3 (lib.centerX - 4f, enemy.transform.position.y, 0), false, 0);
				mp2.Customize ("Teleport", 1);

				while (!endOfPhase) {
					enemyMove.SetUpPatternAndMove (mp1);
					yield return new WaitForSeconds (5f);	
					enemyMove.SetUpPatternAndMove (mp2);
					yield return new WaitForSeconds (5f);
				}
				break;
			case 3:
				/*p2 = new Pattern (lib.maelStrom);
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.Customize ("RotationDirection", 1);
				p2.SetSprite ("Circle", "Glow", "Green");

				p3 = new Pattern (lib.maelStrom);
				p3.SetSprite ("Circle", "Glow", "Yellow");
				p3.Customize (new BulletMovementPattern (true, "Explode", 6f, p3, 0, 14));
				p3.Customize ("RotationDirection", -1);*/
				GameController.gameControl.ui.ShowActivatedPhase ("Boss", "Guardian Seal: Fox Fires");

				p1 = new Pattern ("Cluster", true, 150, 0, 0.01f, 0, 1f);
				p1.SetSprite ("Fireball", "Glow", "Orange");

				p2 = new Pattern (lib.maelStrom);
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.Customize ("RotationDirection", 1);
				p2.SetSprite ("Circle", "Big", "Red");
				p2.Customize ("BulletCount", 6);
				p2.Customize ("CoolDown", .5f);

				mp1 = new EnemyMovementPattern ("Swing", new Vector3 (-13, enemy.transform.position.y, 0), false, 0);
				mp1.Customize ("Speed", 5f);
				mp1.Customize ("Direction", 1);

				mp2 = new EnemyMovementPattern ("", new Vector3 (lib.centerX, lib.centerY, 0), false, 0);

				while (!endOfPhase) {
					enemyMove.SetUpPatternAndMove (mp1);
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (p1);
					yield return new WaitForSeconds (4f);
					mp1.Customize ("Speed", 7f);

					enemyMove.SetUpPatternAndMove (mp2);
					yield return new WaitForSeconds (1f);
					enemy.BossShoot (p2);

					yield return new WaitForSeconds (4f);
					p2.stop = true;
				}
				//mp1.Customize ("Speed", 1f);
				break;
			}
			break;


		//BOSS 2 - SPIDER BOSS
		case "Boss2":
			switch (phase) {
			case 0:
				p1 = lib.singleHoming;
				p1.Customize(new BulletMovementPattern (true, null, 0.5f, p1, 0, 14));
				p1.SetSprite("Circle", "Big", "Red");

				p2 = lib.maelStrom;
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.SetSprite ("Diamond", "Glow", "Red");

				enemy.BossShoot (p2);
				while (!endOfPhase) {
					if (endOfPhase)
						break;
					
					enemy.BossShoot (p1);
					yield return new WaitForSeconds (1f);

				}
				break;
			case 1:
				enemyMove.SetUpPatternAndMove(GameController.gameControl.enemyLib.centerHor);
				GameController.gameControl.ui.ShowActivatedPhase ("Boss", "Dark Core: Web of Lies");

				p1 = new Pattern(lib.giantWeb);
				p2 = lib.maelStrom;
				p2.Customize (new BulletMovementPattern (true, "Explode", 6f, p2, 0, 14));
				p2.SetSprite ("Diamond", "Glow", "Red");

				enemy.BossShoot (p2);
				while (!endOfPhase) {
					enemy.BossShoot (p1);
					yield return new WaitForSeconds (10);
				}
				endOfPhase = true;
				break;
			case 2:
				
				yield return new WaitForSeconds (2f);


				p1 = new Pattern (lib.spiral);
				p1.Customize (new BulletMovementPattern (true, "WaitAndExplode", 6f, p1, 0, 14));
				p1.SetSprite ("Diamond", "Glow", "Red");
				p2 = lib.spiderWeb;
				p2.Customize (new BulletMovementPattern (false, "DownAndExplode", 0.5f, p2, 0, 14));
				p2.SetSprite ("Circle", "Glow", "Red");
				while (!endOfPhase) {	
					for (int i = 0; i < 2; i++) {
						enemy.BossShoot (p1);
						p1.SetSprite ("Circle", "Glow", "Yellow");
						yield return new WaitForSeconds (2f);

						enemy.BossShoot (p1);
						p1.SetSprite ("Circle", "Glow", "Green");
					}
					yield return new WaitForSeconds (2f);
					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (GameController.gameControl.enemyLib.rocking));
					for (int i = 0; i < 4; i++) {
						enemy.BossShoot (p2);
						yield return new WaitForSeconds (1f);
						enemy.BossShoot (p1);
						yield return new WaitForSeconds (1f);
					}
				}
				break;
			case 3:
				enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (GameController.gameControl.enemyLib.centerHor));
				yield return new WaitForSeconds (2f);
				GameController.gameControl.ui.ShowActivatedPhase ("Boss", "Dark Core: Weave Misfortune");

				p1 = lib.laser;
				p1.Customize ("BulletCount", 6);
				p1.SetSprite ("Circle", "Glow", "White");
				p2 = lib.giantWeb;

				while (!endOfPhase) {
					for (int i = 0; i < 12; i++) {
						enemy.BossShoot (p1);
						yield return new WaitForSeconds (1f);
					}
					enemy.BossShoot (p2);
					yield return new WaitForSeconds (2f);
				}
				break;
			}
			break;

		}

	}

	public void StartPhase(string boss, int phase, EnemyShoot enemy, EnemyMovement enemyMove){
		numerator = Execute (boss, phase, enemy, enemyMove);
		StartCoroutine (numerator);
	}

	public void EndPhase(){
//		Debug.Log ("end of phase called");
		if(p1 != null) p1.stop = true;
		if(p2 != null) p2.stop = true;
		if(p3 != null) p3.stop = true;
		if(p4 != null) p4.stop = true;
		if(p5 != null) p5.stop = true;
		endOfPhase = true;
		StopCoroutine (numerator);
	}
}
