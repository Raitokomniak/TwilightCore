using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4 : Phaser
{
    float accumulatedDamage = 1;

	void Awake(){
		bossIndex = 4;
		numberOfPhases = 10;
		Game.control.stageHandler.bossOn = true;
		Game.control.stageHandler.bossScript = this;
        bossBonus = true;
	}

    public override void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
	}

    public override void KeepTrackOfDamage(float d){
        accumulatedDamage += d;
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
                life.SetInvulnerable(true);
                Game.control.stageUI.BOSS.HideUI();
                movement.pattern.UpdateDirection("X1");
                while (Game.control.stageHandler.stageTimer < 14f) yield return null;
                NextPhase();
            break;
            case 1:
                life.SetInvulnerable(true);
                NextPhase();
                break;
			case 2:
                life.SetInvulnerable(false);
                life.SetPhaseHealth(2500);
                Game.control.stageUI.BOSS.RevealUI();
                Game.control.stageUI.BOSS.ShowActivatedPhase ("A Stroke of Genius");
                StartPhaseTimer(13);

                p = new P_MusicalNotes(30);
                p.BMP = new BMP_Explode(p, 6);
                p.infinite = true;
               // p.infinite = true;
                patterns.Add(p);

                
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                while(!endOfPhase){
                    shooter.BossShoot(patterns[0]);
                    yield return new WaitForSeconds(1);
                }
                patterns[0].Stop();
              break;
              case 3:

                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Vidya");
                StartPhaseTimer(13);

                p = new P_MusicalNotes(30);
                p.BMP  = new BMP_Explode(p, 6);
               // p.infinite = true;
                patterns.Add(p);

                yield return new WaitForSeconds(1);
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                while(!endOfPhase && phaseTimer > 2f){
                    shooter.BossShoot(patterns[0]);
                    yield return new WaitForSeconds(.5f);
                }
                movement.pattern.UpdateDirection("X1");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                life.SetInvulnerable(true);
				break;
              
              case 4:
                life.SetInvulnerable(false);
                life.SetPhaseHealth(2500);
                StartPhaseTimer(23);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("A Dream of Avarice");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

               

                while(!endOfPhase){
                    yield return new WaitForSeconds(5);
                }

                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
            
              break;
              case 5:
                StartPhaseTimer(10);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Gayatri");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);


                while(!endOfPhase && phaseTimer > 2f){
                    yield return new WaitForSeconds(.5f);
                }
                movement.pattern.UpdateDirection("X1");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

              break;
			case 6:
                life.SetPhaseHealth(2500);
                StartPhaseTimer(17);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("A Gift of Benevolence");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                while(!endOfPhase){
                    yield return new WaitForSeconds(5);
                }
				break;
			case 7:
                StartPhaseTimer(17);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Mahakali");
                while(!endOfPhase && phaseTimer > 2f){
                    yield return new WaitForSeconds(5);
                }
                movement.pattern.UpdateDirection("X1");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

				break;
            case 8:
                StartPhaseTimer(60);
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Three Cities, Three Paths");
/*                Debug.Log("accumulated damage " + accumulatedDamage);
                Debug.Log("percentage " + accumulatedDamage / 7500);
                Debug.Log("health should be at " + 4000 * (1 - (accumulatedDamage / 7500)) + "hp");*/
                life.SetPhaseHealth(4000);
                life.SetHealthToPercentage(1 - (accumulatedDamage / 7500));

                movement.pattern.UpdateDirection("X3");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                  while(!endOfPhase && phaseTimer > 2f){
                    yield return new WaitForSeconds(5);
                }

				break;
            case 9:
                StartPhaseTimer(30);
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Tripura Sundari");

                  while(!endOfPhase && phaseTimer > 2f){
                    yield return new WaitForSeconds(5);
                }

				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
