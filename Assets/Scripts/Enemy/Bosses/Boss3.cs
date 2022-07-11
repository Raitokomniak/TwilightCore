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
        Pattern p;

        switch (phase) {
			case 0:
               	Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: Stages of Grief");
                StartPhaseTimer(30);

				movement.pattern.speed = 4f;

				
                p = new P_Shape(Mathf.CeilToInt(5 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 2, true, true, false);
                p.BMP.axisRotateSpeed = 5;
                p.SetSprite ("Fireball", "Glow", "Turquoise", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(4 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 4, true, true, false);
                p.BMP.axisRotateSpeed = 4;
                p.SetSprite ("Fireball", "Glow", "Blue", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 5, true, true, false);
                p.BMP.axisRotateSpeed = 3;
                p.SetSprite ("Fireball", "Glow", "Lilac", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 6, true, true, false);
                p.BMP.axisRotateSpeed = 2;
                p.SetSprite ("Fireball", "Glow", "Purple", "Small");	 
                patterns.Add(p);
                
                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 7, true, true, false);
                p.BMP.axisRotateSpeed = 1;
                p.SetSprite ("Fireball", "Glow", "Orange", "Small");	 
                patterns.Add(p);

				while(!endOfPhase){
					movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds(1);

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(1);

                    movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1);

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[3]);
					yield return new WaitForSeconds(1);

                    movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[4]);
					yield return new WaitForSeconds(1);
                    
				}
				break;
			case 1:
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Mother's Fear: Death");
              break;
			case 2:
				Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: It Rains, It Pours");
                StartPhaseTimer(30);
				movement.pattern.speed = 2f;

				p = new P_Shower();
                p.SetSprite ("Circle", "Glow", "Blue", "Small");
                patterns.Add(p);

                p = new P_Rain(100);
                p.BMP = new BMP_RainDrop(p, 2);
                p.SetSprite ("Diamond", "Glow", "Turquoise", "Medium");
				patterns.Add(p);

                p = new P_Rain(100);
                p.BMP = new BMP_RainDrop(p, 2);
                p.SetSprite ("Diamond", "Glow", "Blue", "Medium");
				patterns.Add(p);


				Game.control.sound.PlaySFXLoop("Rain");

				while(!endOfPhase){
					movement.pattern.UpdateDirection("F3");
					shooter.BossShoot (patterns[0]);
					shooter.BossShoot (patterns[1]);
                    shooter.BossShoot (patterns[2]);
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
                    patterns[2].StopPattern();
				}
				break;
			case 3:
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Mother's Fear: Loss");
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
