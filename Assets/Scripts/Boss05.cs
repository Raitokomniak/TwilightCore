using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss05 : Phaser
{
   public override void StopCoro(){
		if(numerator != null) StopCoroutine (numerator);
		routineOver = true;
	}

    public override void ExecutePhase(int phase, Phaser _phaser){
		numerator = Execute (phase, _phaser);
		StartCoroutine (numerator);
    }

	void Update(){
		
	}
    IEnumerator Execute(int phase, Phaser phaser){
		patterns.Clear();
		movementPatterns.Clear();
		 switch(phase)
			{
			case 0:
				phaser.NextBossPhase ();
				break;
			case 1:
				Game.control.ui.UpdateTopPlayer (1f);

			    GetComponent<EnemyLife>().SetInvulnerable (true);

				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 5f, patterns[0], 0, 14));
				patterns[0].Customize ("LoopCircles", 1440);
				patterns[0].Customize ("BulletCount", 100);
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize ("RotationDirection", 1);
				patterns[1].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[2].SetSprite ("Circle", "Glow", "Yellow");
				patterns[2].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[2], 0, 14));
				patterns[2].Customize ("RotationDirection", -1);

				movementPatterns.Add(new EnemyMovementPattern (lib.centerHor));
				movementPatterns[0].Customize ("Speed", 7f);

				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				for (int i = 0; i < 5; i++) {
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);
					yield return new WaitForSeconds (2.2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
				}

				movementPatterns[0] = new EnemyMovementPattern ("Leaving", new Vector3 (lib.centerX, 13, 0), false, 0);
				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds(2f);
				GetComponent<EnemyLife>().Die ();


				patterns[1].StopPattern();
				patterns[2].StopPattern();


				break;
            }
	}
    
}
