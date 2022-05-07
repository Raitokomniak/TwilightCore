using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Phaser
{
    public override void StopCoro(){
		if(numerator != null) StopCoroutine (numerator);
		routineOver = true;
	}

    public override void ExecutePhase(int phase, Phaser _phaser){
		numerator = Execute (phase, _phaser);
		StartCoroutine (numerator);
    }

    IEnumerator Execute(int phase, Phaser phaser){
        patterns = new List<Pattern>();
		movementPatterns = new List<EnemyMovementPattern>();
		GetComponent<EnemyMovement>().EnableSprite(true);
		lib = Game.control.enemyLib;


        switch (phase) {
			case 0:
			/*
				patterns.Add(lib.singleHoming);
				patterns[0].Customize(new BulletMovementPattern (true, null, 0.5f, patterns[0], 0, 14));
				patterns[0].SetSprite("Circle", "Big", "Red");

				patterns.Add(lib.maelStrom);
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].SetSprite ("Diamond", "Glow", "Red");

				//enemy.BossShoot (patterns[1]);
				while (!endOfPhase) {
					//enemy.BossShoot (patterns[0]);
					yield return new WaitForSeconds (1f);
				}
			*/	
				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (2.63f, 7.63f, 0f), false, 0));

				patterns.Add(new Pattern(lib.spiral));
				patterns[0].SetSprite ("Arrow", "Glow", "Red");
				patterns[0].Customize("BulletCount", 10);
				patterns[0].Customize("RotationDirection", 1);
				patterns[0].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[0], 0, 14));

				patterns.Add(new Pattern (lib.spiderWeb));
				patterns[1].Customize (new BulletMovementPattern (false, "DownAndExplode", 0.5f, patterns[1], 0, 14));
				patterns[1].SetSprite ("Circle", "Glow", "Red");
				while (!phaser.endOfPhase) {	
					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.rocking));
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds(2);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitUntil(() => movementPatterns[0].CheckIfReachedDestination(enemyMove) == true);
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2);
					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.zigZag));
					enemy.BossShoot (patterns[1]);
					patterns[0].Customize (new BulletMovementPattern (true, "Stop", 6f, patterns[0], 0, 14));
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2);
					patterns[0].StopPattern();
				}
				break;
			case 1:
                Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.ShowActivatedPhase ("Boss", "Indra's Net");

				movementPatterns.Add(new EnemyMovementPattern(lib.centerHor));

				patterns.Add(new Pattern(lib.giantWeb));
				patterns[0].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[0], 0, 14));
				patterns[0].SetSprite("Circle", "Big", "Red");

				patterns.Add(new Pattern(lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize("RotationDirection", 1);
				patterns[1].SetSprite ("Diamond", "Glow", "Red");

				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds(2);
				while (!endOfPhase) {
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[0]);
					yield return new WaitForSeconds (10);
				}
				patterns[0].StopPattern();

				endOfPhase = true;
				break;
			case 2:
				yield return new WaitForSeconds (2f);
				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 6f, patterns[0], 0, 14));
				patterns[0].SetSprite ("Diamond", "Glow", "Red");
				patterns.Add(new Pattern (lib.spiderWeb));
				patterns[1].Customize (new BulletMovementPattern (false, "DownAndExplode", 0.5f, patterns[1], 0, 14));
				patterns[1].SetSprite ("Circle", "Glow", "Red");
			
				//patterns.Add(new Pattern (lib.laser));
				//patterns[0].Customize("BulletCount", 4);

				while (!endOfPhase) {	
					for (int i = 0; i < 2; i++) {
						enemy.BossShoot (patterns[0]);
						patterns[0].SetSprite ("Circle", "Glow", "Yellow");
						yield return new WaitForSeconds (2f);

						enemy.BossShoot (patterns[0]);
						patterns[0].SetSprite ("Circle", "Glow", "Green");
					}
					yield return new WaitForSeconds (2f);
					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.rocking));
					for (int i = 0; i < 4; i++) {
						enemy.BossShoot (patterns[1]);
						yield return new WaitForSeconds (1f);
						enemy.BossShoot (patterns[0]);
						yield return new WaitForSeconds (1f);
					}
				}
				break;
			case 3:
				enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.centerHor));
				yield return new WaitForSeconds (2f);
				Game.control.ui.ShowActivatedPhase ("Boss", "Void Dance");

				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize ("BulletCount", 6);
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 6f, patterns[0], 0, 14));
				patterns[0].SetSprite ("Circle", "Glow", "White");
				patterns.Add(new Pattern (lib.giantWeb));
				patterns[1] = lib.maelStrom;
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].SetSprite ("Diamond", "Glow", "Red");

				while (!endOfPhase) {
					for (int i = 0; i < 12; i++) {
						enemy.BossShoot (patterns[0]);
						yield return new WaitForSeconds (.5f);
					}
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds (2f);
				}
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
