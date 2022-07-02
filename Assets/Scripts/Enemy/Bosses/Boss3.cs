using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Phaser
{
	void Awake(){
		bossIndex = 3;
		numberOfPhases = 4;
		Game.control.stageHandler.bossOn = true;
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
		GetComponent<EnemyMovement>().EnableSprite(true);
		Game.control.sound.StopLoopingEffects();

        switch (phase) {
			case 0:
               	Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: It Rains, It Pours");

				movement.pattern.speed = 2f;

				patterns.Add(new P_Shower());
				patterns.Add(new P_Rain(100));

				Game.control.sound.PlaySFXLoop("Rain");

				while(!endOfPhase){
					movement.pattern.UpdateDirection("F3");
					shooter.BossShoot (patterns[0]);
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(4);
					movement.SmoothAcceleration(3);
					for(int i = 0; i < 2; i++){
						movement.pattern.UpdateDirection("C3");
						yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
						movement.SmoothAcceleration(3);
						movement.pattern.UpdateDirection("I3");
						yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
						movement.SmoothAcceleration(3);
					}
					patterns[0].StopPattern();
					yield return new WaitForSeconds(1);
					patterns[1].StopPattern();
				}
				break;
			case 1:
              break;
			case 2:
				
				break;
			case 3:
		
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
