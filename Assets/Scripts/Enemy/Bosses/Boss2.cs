using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Phaser
{
	void Awake(){
		bossIndex = 2;
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
		GetComponent<EnemyMovement>().EnableSprite(true);
		

        switch (phase) {
			case 0:
				//movementPatterns.Add(new EnemyMovementPattern (new Vector3 (2.63f, 7.63f, 0f), false, 0));
				
				patterns.Add(new P_Spiral());
				patterns[0].SetSprite ("Arrow", "Glow", "Red");
				patterns[0].bulletCount =  10;
				patterns[0].rotationDirection =  1;
				patterns[0].bulletMovement = new BMP_WaitAndExplode(patterns[0], 6f);

				patterns.Add(new P_SpiderWeb());
				patterns[1].SetSprite ("Circle", "Glow", "Red");

				while (!phaser.endOfPhase) {
					enemyMove.SetUpPatternAndMove (new EMP_Rock());
					enemy.BossShoot (patterns[1]);
					yield return new WaitForSeconds(2);
					enemyMove.movementPattern.UpdateDirection(2.63f, 7.63f);
					yield return new WaitUntil(() => enemyMove.movementPattern.CheckIfReachedDestination(enemyMove) == true);
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2);
					
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2);
					patterns[0].StopPattern();
				}
				
				break;
			case 1:
                Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.BOSS.ShowActivatedPhase ("Indra's Net");

				movementPatterns.Add(new EMP_EnterFromTop());

				patterns.Add(new P_GiantWeb());
				patterns[0].bulletMovement = new BMP_Explode(patterns[0], 6f, false);
				patterns[0].SetSprite("Circle", "Big", "Red");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false);
				patterns[1].rotationDirection =  1;
				patterns[1].SetSprite ("Diamond", "Glow", "Red");

				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds(2);
				while (!phaser.endOfPhase) {
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[0]);
					yield return new WaitForSeconds (9);
					patterns[1].StopPattern();
					yield return new WaitForSeconds (1);
				}
				patterns[0].StopPattern();
				patterns[1].StopPattern();

				endOfPhase = true;
				break;
			case 2:
				movementPatterns.Add(new EMP_EnterFromTop());
				movementPatterns.Add(new EMP_ZigZag());
				movementPatterns[1].movementDirection = 2;


				patterns.Add(new P_SpiderWebLaser());
				patterns[0].bulletCount = 2 * difficultyMultiplier;
				patterns[0].rotationDirection =  0;

				patterns.Add(new P_RepeatedHoming());
				patterns[1].SetSprite("Spider_Glow");
				patterns[1].coolDown = 3f;
				patterns[1].bulletMovement = new BMP_SlowWaving(patterns[1], 3f);

				while (!phaser.endOfPhase) {	
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitUntil(() => movementPatterns[0].CheckIfReachedDestination(enemyMove) == true);
					enemy.BossShoot (patterns[0]);
					enemy.BossShoot(patterns[1]);

					yield return new WaitForSeconds (4f);
					enemyMove.movementPattern.UpdateDirection(-15, 4f);
					yield return new WaitForSeconds (4f);
					movementPatterns[1].movementDirection = 1;
					enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					yield return new WaitUntil(() => movementPatterns[1].CheckIfReachedDestination(enemyMove) == true);
					enemy.BossShoot (patterns[0]);

					yield return new WaitForSeconds (4f);

					movementPatterns[1].movementDirection = -1;
					enemyMove.SetUpPatternAndMove (movementPatterns[1]);

					yield return new WaitForSeconds(4f);

				}
				break;
			case 3:
				enemyMove.SetUpPatternAndMove(new EMP_EnterFromTop());
				yield return new WaitForSeconds (2f);
				Game.control.ui.BOSS.ShowActivatedPhase ("Void Dance");

				patterns.Add(new P_Spiral());
				patterns[0].bulletCount = 6;
				patterns[0].bulletMovement = new BMP_WaitAndExplode(patterns[0], 6f);
				patterns[0].SetSprite ("Circle", "Glow", "White");
				patterns.Add(new P_GiantWeb());
				patterns[1] = new P_Maelstrom();
				patterns[1].bulletMovement = new BMP_Explode(patterns[0], 6f, false);
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
