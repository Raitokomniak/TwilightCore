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
        bossSurvivalBonus = true;
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
                //while (Game.control.stageHandler.stageTimer < 13f) yield return null;
                while (Game.control.stageHandler.stageTimer < 1f) yield return null;
                NextPhase();
            break;
            case 1:
                life.SetInvulnerable(true);
                NextPhase();
                break;
			case 3:
                life.SetInvulnerable(false);
                life.SetPhaseHealth(2500);
                Game.control.stageUI.BOSS.RevealUI();
                Game.control.stageUI.BOSS.ShowActivatedPhase ("A Stroke of Genius");
                StartPhaseTimer(11);

                p = new P_MusicalNotes(3 * difficulty);
                p.BMP = new BMP_SlowWaving(p, 4, true);
                p.coolDown = 1f;
                p.infinite = true;
                patterns.Add(p);

                p = new P_Cluster(2 * difficulty);
                p.coolDown = .2f / difficulty;
                p.BMP = new BMP_WaitAndExplode(p, 6, 2);
                p.SetSprite("Circle", "Glow", "Yellow", "Medium");
                patterns.Add(p);

                p = new P_Cluster(2 * difficulty);
                p.coolDown = .2f / difficulty;
                p.BMP = new BMP_WaitAndExplode(p, 6, 2);
                p.SetSprite("Circle", "Glow", "White", "Medium");
                patterns.Add(p);

                p = new P_GiantLotus();
                //p.BMP = new BMP_StopAndRotate(p, 1, 2);
                p.infinite = false;
                p.bulletCount = 6 * difficulty;
                p.SetSprite("Lotus", "Glow", "White", "Huge");
                p.originMagnitude = 6;
                patterns.Add(p);

                p = new P_GiantLotus();
                //p.BMP = new BMP_StopAndRotate(p, 1, 2);
                p.infinite = false;
                p.bulletCount = 6 * difficulty;
                p.SetSprite("Lotus", "Glow", "White", "Small");
                p.originMagnitude = 2;
                patterns.Add(p);
                
                

                movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X5", 1), new WayPoint("C4", 1, 1)});
				movementPatterns[0].centerPoint = vectorLib.GetVector("X3");

                movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[1].SetWayPoints(new List<WayPoint>(){new WayPoint("X5", 1), new WayPoint("I4", 1, -1)});
				movementPatterns[1].centerPoint = vectorLib.GetVector("X3");
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                
                

                while(!endOfPhase){
                    
                    shooter.BossShoot(patterns[3]);
                    Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");
                    yield return new WaitForSeconds(2);

                    for(int i = 0; i < 2; i++){

                        //stroke
                        movement.SetUpPatternAndMove(movementPatterns[1]);
                        movement.pattern.speed = 6f;
                        yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                        yield return new WaitForSeconds(.5f);
                        yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                        movement.pattern.UpdateDirection("X5");
                        shooter.BossShoot(patterns[1]);
                        shooter.BossShoot(patterns[2]);
                        
                        yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                        //stroke
                        movement.SetUpPatternAndMove(movementPatterns[0]);
                        movement.pattern.speed = 6f;
                        yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                        yield return new WaitForSeconds(.5f);
                        yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                        movement.pattern.UpdateDirection("X5");
                        shooter.BossShoot(patterns[1]);
                        shooter.BossShoot(patterns[2]);
                        
                        patterns[0].Stop();
                        shooter.BossShoot(patterns[0]);
                        
                    }
                    patterns[0].Stop();
                    movement.CorrectRotation();
                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);



                    
                yield return null;
                }
                patterns[0].Stop();
              break;
              case 2:

                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Vidya");
                StartPhaseTimer(15);

                p = new P_MusicalNotes(30);
                p.BMP  = new BMP_Explode(p, 6);
               // p.infinite = true;
                patterns.Add(p);

                p = new P_Shape(40, "Vidya", 3);
                p.BMP = new BMP_WaitAndExplode(p, 1 * difficulty, 1);
                p.infinite = false;
                p.SetSprite("Circle", "Glow", "Blue", "Medium");
                patterns.Add(p);

                p = new P_Shape(40, "VidyaDrop", 3);
                p.BMP = new BMP_WaitAndExplode(p, 1 * difficulty, 1);
                p.infinite = false;
                p.SetSprite("Circle", "Glow", "Turquoise", "Medium");
                patterns.Add(p);

                p = new P_GiantLotus();
                //p.BMP = new BMP_StopAndRotate(p, 1, 2);
                p.infinite = false;
                p.bulletCount = 6 * difficulty;
                p.SetSprite("Lotus", "Glow", "White", "Small");
                p.originMagnitude = 2;
                patterns.Add(p);

                Game.control.sound.PlaySpellSound("Enemy", "Lotus");

                yield return new WaitForSeconds(1);
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                shooter.BossShoot(patterns[1]);
                yield return new WaitForSeconds(2f);
              //  shooter.BossShoot(patterns[3]);
                yield return new WaitForSeconds(1f);
                shooter.BossShoot(patterns[2]);

                movement.pattern.UpdateDirection("C6");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot(patterns[3]);
                Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");

                movement.pattern.UpdateDirection("E4");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot(patterns[3]);
                Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");

                movement.pattern.UpdateDirection("F2");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot(patterns[3]);
                Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");

                movement.pattern.UpdateDirection("H4");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot(patterns[3]);
                Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");

                movement.pattern.UpdateDirection("J6");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot(patterns[3]);
                Game.control.sound.PlaySpellSound("Enemy", "SmallLotus");


                movement.pattern.UpdateDirection("B4");

                yield return new WaitForSeconds(2f);
                shooter.BossShoot(patterns[1]);
                yield return new WaitForSeconds(1f);
             //   shooter.BossShoot(patterns[3]);

                movement.pattern.UpdateDirection("J4");

                

                while(!endOfPhase && phaseTimer > 2f){
                
                    yield return null;
                }
                patterns[0].Stop();
                movement.pattern.UpdateDirection("X1");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                life.SetInvulnerable(true);
				break;
              
              case 4:
                life.SetInvulnerable(false);
                life.SetPhaseHealth(2500);
                StartPhaseTimer(20);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("A Dream of Avarice");

                patterns.Add(new P_Circle(5));

                p = new P_GiantLotus();
                //p.BMP = new BMP_StopAndRotate(p, 1, 2);
                p.infinite = false;
                p.SetSprite("Lotus", "Glow", "Red", "Medium");
                p.originMagnitude = 2;
                patterns.Add(p);

                p = new P_Rain(10 * difficulty);
                p.BMP = new BMP_3DRotation(p, true, true);
                p.SetSprite("Coin", "Medium");
                p.BMP.accelMax = 2f;
                patterns.Add(p);

                
               yield return new WaitForSeconds(1);
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

               

                while(!endOfPhase){

                    movement.pattern.UpdateDirection("X2");
                    
                    yield return new WaitForSeconds(5f);
                    patterns[2].Stop();


                    yield return null;
                }

                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
            
              break;
              case 5:
                StartPhaseTimer(20);
                movement.pattern.UpdateDirection("X3");
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Avatar: Gayatri");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

                p = new P_Shape(40, "Gayatri", 3);
                p.BMP = new BMP_WaitAndExplode(p, 2, 2);
               // p.BMP = new BMP_3DRotation(p, true, false);
                p.infinite = false;
                p.SetSprite("Circle", "Glow", "Red", "Medium");
                patterns.Add(p);

                p = new P_SingleHoming();
                p.BMP = new BMP_3DRotation(p, false, true);
                p.SetSprite("Circle", "Glow", "Red", "Huge");
                patterns.Add(p);

                shooter.BossShoot(patterns[0]);

                while(!endOfPhase && phaseTimer > 2f){
                   // shooter.BossShoot(patterns[1]);
                    yield return new WaitForSeconds(2f);
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
