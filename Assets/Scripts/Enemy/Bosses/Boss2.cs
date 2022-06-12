using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Phaser
{
	void Awake(){
		bossIndex = 2;
		numberOfPhases = 4;
		Game.control.stageHandler.bossOn = true;
		Game.control.stageHandler.bossBonus = true;
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
		difficulty = Game.control.stageHandler.difficultyMultiplier;
        ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);

        switch (phase) {
			case 0:
				//movementPatterns.Add(new EnemyMovementPattern (new Vector3 (2.63f, 7.63f, 0f), false, 0));
				Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				
				patterns.Add(new P_Spiral(2 * difficulty));
				patterns[0].SetSprite ("Spider_Glow", "Small");
				patterns[0].rotationDirection =  1;
				patterns[0].bulletMovement = new BMP_SlowWaving(patterns[0], 4f, true);

				patterns.Add(new P_SpiderWeb());
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].SetSprite ("Circle", "Glow", "Red", "Medium");

				patterns.Add(new P_RepeatedHoming());
				patterns[2].coolDown = 4f / difficulty;
				patterns[2].infinite = false;
				patterns[2].bulletCount = Mathf.CeilToInt(4 * (difficulty / 2));
				patterns[2].bulletMovement = new BMP_WaitToHome(patterns[2], 20f);
				patterns[2].SetSprite ("Arrow", "Glow", "Red", "Small");	

				shooter.BossShoot (patterns[2]);
				shooter.BossShoot (patterns[2]);
				while (!endOfPhase) {
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(3);
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(3);
					movement.pattern.UpdateDirection("H4");
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(3);
					movement.pattern.UpdateDirection("C4");
					shooter.BossShoot (patterns[1]);
					patterns[0].StopPattern();
					yield return new WaitForSeconds(3);
				}
				patterns[2].StopPattern();
				break;
			case 1:
                Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.ui.BOSS.ShowActivatedPhase ("Indra's Net");
				StartPhaseTimer(30);

				patterns.Add(new P_GiantWeb());
			//patterns[0].bulletCount = 6 * difficultyMultiplier;
				if(difficulty == 5) patterns[0].bulletCount = 30; 
				else patterns[0].bulletCount = 2 * difficulty;
				patterns[0].delayBeforeAttack = 2f;
				patterns[0].bulletMovement = new BMP_Explode(patterns[0], 6f, false, false);
				patterns[0].SetSprite("Circle", "Big", "Red", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false, false);
				patterns[1].rotationDirection =  1;
				patterns[1].infinite = true;
				patterns[1].SetSprite ("Diamond", "Glow", "Red", "Small");

				movement.pattern.UpdateDirection("X3");
				yield return new WaitForSeconds(2);
				while (!endOfPhase) {
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds (9);
					patterns[1].StopPattern();
					yield return new WaitForSeconds (1);
				}
				patterns[0].StopPattern();
				patterns[1].StopPattern();
				break;
			case 2:
				

				patterns.Add(new P_SpiderWebLaser());
				patterns[0].bulletCount = 3;
				patterns[0].rotationDirection =  0;

				patterns.Add(new P_Maelstrom());
				//patterns[1].SetSprite ("Spider_Glow");
				patterns[1].coolDown = .5f;
				patterns[1].rotationDirection =  2;
				patterns[1].SetSprite ("Diamond", "Glow", "Red", "Small");
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false, false);

				patterns.Add(new P_RepeatedHoming());
				patterns[2].coolDown = 4f / difficulty;
				patterns[2].infinite = false;
				patterns[2].bulletCount = Mathf.CeilToInt(4 * (difficulty / 2));
				patterns[2].bulletMovement = new BMP_WaitToHome(patterns[2], 20f);
				patterns[2].SetSprite ("Arrow", "Glow", "Red", "Small");	

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[0]);
					

					yield return new WaitForSeconds (2f);
					movement.pattern.UpdateDirection("C4");
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds (2f);
					shooter.BossShoot(patterns[1]);
					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds (2f);
					movement.pattern.UpdateDirection("H4");

					yield return new WaitForSeconds(2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
				}
				break;
			case 3:
				Game.control.ui.BOSS.ShowActivatedPhase ("Void Dance");
				StartPhaseTimer(30);

				P_VoidPortal portal = new P_VoidPortal(20f, 15, 40);
				portal.dontDestroyAnimation = true;
				patterns.Add(portal);
				patterns[0].infinite = false;
				patterns[0].bulletMovement = new BMP_Explode(patterns[0], 0, false, false);
				patterns[0].delayBeforeAttack = 1f;

				patterns.Add(new P_Spiral(6 * difficulty));
				patterns[1].loopCircles =  288 * 3;
				patterns[1].SetSprite ("Arrow", "Glow", "Red", "Small");
				patterns[1].rotationDirection =  1;
				patterns[1].bulletMovement = new BMP_WaitAndExplode(patterns[1], 6f);

				patterns.Add(new P_Spiral(6 * difficulty));
				patterns[2].loopCircles =  288 * 3;
				patterns[2].SetSprite ("Arrow", "Glow", "White", "Small");
				patterns[2].rotationDirection =  -1;
				patterns[2].bulletMovement = new BMP_WaitAndExplode(patterns[2], 6f);

				patterns.Add(new P_Maelstrom());
				patterns[3].bulletCount = Mathf.CeilToInt(.6f * difficulty);
				patterns[3].bulletMovement = new BMP_SlowWaving(patterns[3], 3f, true);
				patterns[3].rotationDirection =  1;
				patterns[3].infinite = true;
				patterns[3].SetSprite ("Diamond", "Glow", "Red", "Small");

				shooter.BossShoot(patterns[0]);
				shooter.BossShoot(patterns[3]);

				yield return new WaitForSeconds (2f);
				while (!endOfPhase) {
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					shooter.BossShoot(patterns[1]);
					yield return new WaitForSeconds (1f);
					shooter.BossShoot(patterns[2]);
					yield return new WaitForSeconds (1f);
				}
				patterns[0].animation.GetComponent<BulletAnimationController>().stop = true;
				patterns[3].StopPattern();
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
