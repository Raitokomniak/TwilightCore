using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss05 : Phaser
{

	void Awake(){
		bossIndex = 0.5f;
		ignoreDialog = true;
	}

   public override void StopCoro(){
		
	}


    public override void ExecutePhase(int phase, Phaser _phaser){
		phaseExecuteRoutine = Execute (phase, _phaser);
		StartCoroutine (phaseExecuteRoutine);
    }

    IEnumerator Execute(int phase, Phaser phaser){
		difficultyMultiplier = Game.control.stageHandler.difficultyMultiplier;
		ResetLists();

		 switch(phase)
			{
			case 0:
				Game.control.ui.UpdateTopPlayer (1f);

			    GetComponent<EnemyLife>().SetInvulnerable (true);

				patterns.Add(new P_Maelstrom());
				patterns[0].bulletMovement = new BulletMovementPattern (true, "Explode", 6f, patterns[0], 0, 14);
				patterns[0].bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2f));
				patterns[0].rotationDirection =  1;
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_Maelstrom());
				patterns[1].SetSprite ("Circle", "Glow", "Yellow");
				patterns[1].bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2f));
				patterns[1].bulletMovement = new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14);
				patterns[1].rotationDirection =  -1;


				movementPatterns.Add(new EnemyMovementPattern (lib.centerHor));
				movementPatterns[0].Customize ("Speed", 7f);

				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				for (int i = 0; i < 5; i++) {
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (patterns[0]);
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds (2.2f);
					patterns[0].StopPattern();
					patterns[1].StopPattern();
				}

				movementPatterns[0] = new EnemyMovementPattern ("Leaving", new Vector3 (lib.centerX, 13, 0), false, 0);
				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds(2f);
				GetComponent<EnemyLife>().Die ();


				patterns[0].StopPattern();
				patterns[1].StopPattern();
				break;
			case 1:
				


				break;
            }
	}
    
}
