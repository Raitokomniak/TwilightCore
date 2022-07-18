using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_2 : Phaser
{
	void Awake(){
		bossIndex = 4.2f;
		numberOfPhases = 2;
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
        Pattern p;

        switch (phase) {
			case 0:
                Game.control.stageUI.BOSS.ShowActivatedPhase ("सरस्: Purifying Knowledge");
                StartPhaseTimer(30);

                p = new P_PacMan(2);
               // p.infinite = true;
                p.SetSprite ("Tear", "Glow", "Blue", "Small");
                patterns.Add(p);

                yield return new WaitForSeconds(1);
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                while(!endOfPhase){
                    yield return new WaitForSeconds(5);
                }
                movement.pattern.UpdateDirection("X1");

              break;
              case 1:
        
				break;
              case 2:
            
              break;
              case 3:
              

              break;
			case 4:
			
				break;
			case 5:
             
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
