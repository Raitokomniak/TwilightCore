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
				yield return new WaitForSeconds (2f);
				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 6f, patterns[0], 0, 14));
				patterns[0].SetSprite ("Diamond", "Glow", "Red");
				patterns.Add(new Pattern (lib.spiderWeb));
				patterns[1].Customize (new BulletMovementPattern (false, "DownAndExplode", 0.5f, patterns[1], 0, 14));
				patterns[1].SetSprite ("Circle", "Glow", "Red");
				while (!endOfPhase) {	
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds(4);
				}
				break;
			case 1:
				enemyMove.SetUpPatternAndMove(lib.centerHor);
                Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.ShowActivatedPhase ("Boss", "Indra's Net");

				patterns.Add(new Pattern(lib.giantWeb));
				patterns[0].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[0], 0, 14));
				patterns[0].SetSprite("Circle", "Big", "Red");

				patterns.Add(new Pattern(lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize("RotationDirection", 1);
				patterns[1].SetSprite ("Diamond", "Glow", "Red");

				enemy.BossShoot (patterns[1]);
				while (!endOfPhase) {
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
