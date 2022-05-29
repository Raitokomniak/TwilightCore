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
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
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
				//THIS IS HERE SO THE TOP LAYER ISNT UPDATED RIGHT AWWAY
				NextPhase();
				break;
			case 1:
				
				Game.control.ui.WORLD.UpdateTopPlayer (1f);

			    GetComponent<EnemyLife>().SetInvulnerable (true);

				patterns.Add(new P_Maelstrom());
				patterns[0].bulletMovement = new BMP_Explode(patterns[0], 6f, false);
				patterns[0].bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2f));
				patterns[0].rotationDirection =  1;
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_Maelstrom());
				patterns[1].SetSprite ("Circle", "Glow", "Yellow");
				patterns[1].bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2f));
				patterns[1].bulletMovement = new BMP_Explode(patterns[0], 6f, false);
				patterns[1].rotationDirection =  -1;

				movementPatterns.Add(enemy.wave.movementPattern);
				enemyMove.SetUpPatternAndMove (movementPatterns[0]);

				while(Game.control.stageHandler.stageTimer < enemy.wave.spawnTime + 12f)
				{
					yield return new WaitForSeconds (2f);
					enemy.BossShoot (patterns[0]);
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds (2.2f);
					patterns[0].StopPattern();
					patterns[1].StopPattern();
				}
				patterns[0].StopPattern();
				patterns[1].StopPattern();
				NextPhase();
				break;
			case 2:
				Game.control.ui.WORLD.UpdateTopPlayer (1f);
				Game.control.ui.BOSS.ShowActivatedPhase ("Twilight Core: Depulsio");
				GetComponent<EnemyLife>().SetInvulnerable (true);

				patterns.Add(new P_Spiral(Mathf.CeilToInt(4 * (difficultyMultiplier / 2f))));
				patterns[0].SetSprite ("Circle", "Glow", "BlackPurple");
				patterns[0].bulletMovement = new BMP_WaitAndExplode(patterns[0], 3f);
				patterns[0].bulletMovement.accelSpeed = 20f;
				patterns[0].loopCircles = 288 * difficultyMultiplier;
				patterns[0].bulletCount = 5 * difficultyMultiplier;

				patterns.Add(new P_Spiral(Mathf.CeilToInt(4 * (difficultyMultiplier / 2f))));
				patterns[1].SetSprite ("Circle", "Glow", "BlackLilac");
				patterns[1].bulletMovement = new BMP_WaitAndExplode(patterns[0], 3f);
				patterns[1].bulletMovement.accelSpeed = 20f;
				patterns[1].loopCircles = 288 * difficultyMultiplier;
				patterns[1].bulletCount = 5 * difficultyMultiplier;
				patterns[1].rotationDirection = -1;

				while(Game.control.stageHandler.stageTimer < enemy.wave.spawnTime + enemy.wave.movementPattern.stayTime)
				{
					yield return new WaitForSeconds (1f);
					enemy.BossShoot (patterns[0]);
					yield return new WaitForSeconds (1f);
					enemy.BossShoot (patterns[1]);
				}

				patterns[0].StopPattern();
				patterns[1].StopPattern();
				enemyMove.movementPattern.ForceLeave();
				//enemyMove.movementPattern.UpdateDirection(lib.GetVector("XY").x, lib.GetVector("XY").y);
				yield return new WaitForSeconds(2f);
				GetComponent<EnemyLife>().Die ();
				break;
            }

			yield return null;
	}
    
}
