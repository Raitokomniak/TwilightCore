using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Phaser
{
	void Awake(){
		bossIndex = 1;
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
		VectorLib lib = Game.control.vectorLib;
		difficulty = Game.control.stageHandler.difficultyMultiplier;
		ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);

		Pattern p = null;
		

		switch (phase) {
			case 2:
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Daffodil Hypnosis");
                StartPhaseTimer(30);


				//daffodil center
				p = new P_Circle(10 * difficulty);
				//p.BMP = new BMP_Explode(p, 2);
                p.BMP = new BMP_WaitAndExplode(p, 2, 0);
				p.loopCircles =  100 * difficulty;
				p.SetSprite ("Circle", "Glow", "Orange", "Small");
				p.BMP.accelMax = 2;
				patterns.Add(p);

				//daffodil petals
				p = new P_Maelstrom(2 * difficulty, 0.3f, 1);
				p.BMP = new BMP_Explode (p, 6f);
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP.accelMax = 30;
				patterns.Add(p);

				p = new P_Maelstrom(2*difficulty, 0.3f, -1);
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP = new BMP_Explode (p, 6f);
				patterns.Add(p);

				//daffodil leaves
				p = new P_Maelstrom(3 * difficulty, 0.2f, 1);
				p.BMP = new BMP_Explode(p, 10);
				p.startingRotation = 40;
				p.maelStromRotationMultiplier = 0.03f;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
				p.coolDown = 0.3f;
				patterns.Add(p);

				p = new P_Maelstrom(3 * difficulty, 0.2f, -1);
				p.BMP = new BMP_Explode(p, 10);
				p.maelStromRotationMultiplier = 0.03f;
				p.startingRotation = 40;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
				p.coolDown = 0.3f;
				patterns.Add(p);

				//spear

				p = new P_SingleHoming();
				p.BMP = new BMP_TurnToSpears(p, 5f);
				p.bulletCount = 1;
				p.SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				p.BMP.accelMax = 30;
				patterns.Add(p);

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].speed = 7f;
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 4)});

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[0]);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[3]);
					shooter.BossShoot (patterns[4]);
					
					yield return new WaitForSeconds(1f);
					
					
				
					yield return new WaitForSeconds(3f);
					patterns[3].Stop();
					patterns[4].Stop();
					patterns[1].Stop();
					patterns[2].Stop();

					movement.pattern.UpdateDirection("C3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[5]);

					movement.pattern.UpdateDirection("I3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[5]);

					
				}
				
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Hoodwink: Ninetailed Spear");
				Game.control.stageUI.WORLD.SetTopLayerSpeed(2f);

				StartPhaseTimer(30);

				p = new P_Curtain();
				p.bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
				p.SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				p.BMP = new BMP_TurnToSpears(p, 6f);
				p.BMP.accelMax = 30;
                patterns.Add(p);

				p = new P_Maelstrom(2 * difficulty, 1);
				p.BMP = new BMP_Explode(p, 6f);
				p.SetSprite ("Circle", "Glow", "Green", "Medium");
                patterns.Add(p);

				p = new P_Maelstrom(2 * difficulty, -1);
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP = new BMP_Explode(p, 4f);
                patterns.Add(p);

				movement.pattern.force = false;
				movement.pattern.UpdateDirection("B3");
				yield return new WaitForSeconds(1f);
				
				while (!endOfPhase) {
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
					movement.pattern.UpdateDirection("I3");
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
                    yield return new WaitForSeconds(1f);
                    shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].Stop();
					patterns[2].Stop();
					yield return new WaitForSeconds(1f);
					movement.pattern.UpdateDirection("B3");
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].Stop();
					patterns[2].Stop();
					yield return new WaitForSeconds(1f);
				}

				break;
			
			case 0:
				movement.pattern.force = true;

				StartPhaseTimer(30);

				Game.control.stageUI.BOSS.ShowActivatedPhase ("Double Daffodil Hypnosis");

                //0
				p = new P_Spiral(20 * difficulty, 1);
				//patterns[0].BMP = new BMP_WaitAndExplode (patterns[0], 25f);
                p.BMP = new BMP_Explode (p, 25f);
				p.loopCircles =  288 * difficulty;
				p.SetSprite ("Circle", "Glow", "Green", "Medium");
				p.BMP.accelMax = 30;
                patterns.Add(p);

                //1
				p = new P_Maelstrom(2 * difficulty, 1);
				p.BMP = new BMP_Explode (p, 6f);
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP.accelMax = 30;
                patterns.Add(p);

                //2
				p = new P_Maelstrom(2 * difficulty, -1);
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP = new BMP_Explode (p, 6f);
                patterns.Add(p);

                //3
				p = new P_Spiral(10 * difficulty, 1);
				p.BMP = new BMP_Explode(p, 2f);
				p.loopCircles =  288 * difficulty;
				p.SetSprite ("Circle", "Glow", "Orange", "Small");
				p.BMP.accelMax = 30;
                patterns.Add(p);

                //4		
				p = new P_Maelstrom(3 * difficulty, 0.2f, 1);
				p.BMP = new BMP_Explode(p, 10);
				p.startingRotation = 40;
				p.maelStromRotationMultiplier = 0.01f;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
				p.coolDown = 0.2f;
                patterns.Add(p);

                //5
				p = new P_Maelstrom(3 * difficulty, 0.2f, -1);
				p.BMP = new BMP_Explode(p, 10);
				p.maelStromRotationMultiplier = 0.01f;
				p.startingRotation = 40;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
                patterns.Add(p);

                //6
				p = new P_SingleHoming();
                p.infinite = false;
				p.BMP = new BMP_TurnToSpears(p, 5f);
				p.SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				p.BMP.accelMax = 30;
                patterns.Add(p);


                //7
				p = new P_Maelstrom();
				p.BMP = new BMP_Explode (p, 6f);
				p.rotationDirection =  1;
				p.bulletCount =  2 * difficulty;
				p.SetSprite ("Circle", "Glow", "Orange", "Medium");
				p.BMP.accelMax = 30;
                patterns.Add(p);

                //8
				p = new P_Maelstrom(2 * difficulty, -1);
				p.SetSprite ("Circle", "Glow", "Orange", "Medium");
				p.BMP = new BMP_Explode (p, 6f);
                patterns.Add(p);

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].speed = 7f;
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 4)});

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[4]);
					shooter.BossShoot (patterns[5]);
					shooter.BossShoot (patterns[7]);
					shooter.BossShoot (patterns[8]);
					yield return new WaitForSeconds(1f);
					
					shooter.BossShoot (patterns[3]);
					shooter.BossShoot (patterns[6]);

					patterns[4].Stop();
					patterns[5].Stop();
					patterns[7].Stop();
					patterns[8].Stop();
				
					yield return new WaitForSeconds(3f);
					shooter.BossShoot (patterns[6]);
					patterns[1].Stop();
					patterns[2].Stop();

					movement.pattern.UpdateDirection("C3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[6]);

					yield return new WaitForSeconds(.5f);

					shooter.BossShoot (patterns[6]);

					movement.pattern.UpdateDirection("I3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[6]);

					yield return new WaitForSeconds(.5f);

					shooter.BossShoot (patterns[6]);

				}
				
				
				break;
			case 3:
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Hoodwink: Fox Fires");
				StartPhaseTimer(30);
				
				p = new P_FoxFires(difficulty, 40 * difficulty);
				p.SetSprite ("Fireball", "Glow", "Orange", "Small");
                patterns.Add(p);

				p = new P_Maelstrom();
				p.BMP = new BMP_Explode(p, 6f);
				p.rotationDirection = 1;
                p.infinite = true;
				p.SetSprite ("BigCircle", "Big", "Red", "Huge");
				p.bulletCount =  Mathf.CeilToInt(1.2f * difficulty);
				p.coolDown = 2.5f / difficulty;
                patterns.Add(p);

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I3", 5, 1), new WayPoint("XY", 5)});
				movementPatterns[0].centerPoint = vectorLib.GetVector("X3");
				
				while (!endOfPhase) {
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					movementPatterns[0].force = false;
					movementPatterns[0].speed = 17f;
					movement.SmoothAcceleration(50);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil(() => movement.pattern.rotateOnAxis == false);
                    movement.CorrectRotation();
					movement.moving = false;
                    patterns[0].Stop();
					movementPatterns[0].force = true;
					movementPatterns[0].speed = 7f;
					movement.moving = true;
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    yield return new WaitForSeconds(.5f);
                    patterns[1].BMP = new BMP_Explode(p, 6f);
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(4f);
					patterns[1].Stop();
				}
				break;
		}
		NextPhase();
		phaser.routineOver = true;
	}
}
