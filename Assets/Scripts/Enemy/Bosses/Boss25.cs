using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss25 : Phaser
{
    void Awake(){
		bossIndex = 2.5f;
		numberOfPhases = 3;
		ignoreDialog = true;
        topLayerWaitTime = 2;
        Game.control.stageHandler.midBossOn = true;
        Game.control.stageHandler.bossScript = this;
        bossBonus = true;
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
		Pattern p = null;

		 switch(phase)
			{
			case 0:
				Game.control.stageUI.WORLD.UpdateTopPlayer (1f); ////////
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Crown Jewels");
                topLayerWaitTime = 0;

                StartPhaseTimer(30);

				
                p = new P_Shape(4 * difficulty, "Earring", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_WaitAndExplode(p, 4, 0);
                p.BMP.unFold = true;
                p.SetSprite ("Coin", "Small");	 
				patterns.Add(p);

                p = new P_Circle(4 * difficulty);
                p.BMP = new BMP_Explode(p, 5f, true, true, false);
                p.BMP.axisRotateSpeed = 2;
                p.SetSprite ("Pearl", "Big");	 
				patterns.Add(p);

                p = new P_Shape(4 * difficulty, "Ring", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_WaitToHome(p, 2, false);
                p.BMP.forceSprite = false;
                p.SetSprite ("Coin", "Small");	 
				patterns.Add(p);

				movementPatterns.Add(shooter.wave.movementPattern);
                movement.pattern.speed = 4f;
				movement.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds (2f);
                shooter.BossShoot(patterns[1]);

				while(Game.control.stageHandler.stageTimer < shooter.wave.spawnTime + movement.pattern.stayTime)
				{
                    movement.pattern.UpdateDirection("C3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[0]);

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    
                    if(difficulty > 2){
                        shooter.BossShoot(patterns[0]);
                        shooter.BossShoot(patterns[1]);
                        yield return new WaitForSeconds(1);
                        shooter.BossShoot(patterns[1]);
                    }

                    movement.pattern.UpdateDirection("I3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[0]);

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[1]);
                    yield return new WaitForSeconds(1);
                    shooter.BossShoot(patterns[1]);

                    movement.pattern.UpdateDirection("D4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[2]);
                    movement.pattern.UpdateDirection("E4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[2]);
                    movement.pattern.UpdateDirection("F4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot(patterns[2]);


					yield return null;
				}
				patterns[0].Stop();
				patterns[1].Stop();
				NextPhase();
				break;
            case 1:
                movement.SetUpPatternAndMove(movement.pattern);
				movement.pattern.UpdateDirection("XU");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
				GetComponent<BossLife>().FakeDeath();
                Game.control.stageHandler.midBossOn = false;
                break;
            }

			yield return null;
	}
}
